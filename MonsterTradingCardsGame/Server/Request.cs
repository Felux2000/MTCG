﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using MonsterTradingCardsGame.Server;

namespace MonsterTradingCardsGame.Server
{
    public enum Method
    {
        GET,
        POST,
        PUT,
        DELETE
    }


    internal class Request
    {
        private Dictionary<string, Method> stringToMethod = new Dictionary<string, Method>
        {
            {"GET" , Method.GET},
            {"POST" ,Method.POST},
            {"PUT" ,Method.PUT},
            {"DELETE" ,Method.DELETE}
        };
        public Method? HttpMethod { get; set; }
        public string? Path { get; set; }
        public string? Params { get; set; }
        public string? ContentType { get; set; }
        public int? ContentLength { get; set; }
        public string? Body { get; set; }
        public string? AuthToken { get; set; }
        public bool HasParams { get; set; }

        private const string PHContentType = "Content-Type: ";
        private const string PHContentLength = "Content-Length: ";
        private const string PHAuthorization = "Authorization";

        public Request(Stream inputStream)
        {
            BuildRequest(inputStream);
        }
        private void BuildRequest(Stream inputStream)
        {
            try
            {
                string? line;
                using (StreamReader reader = new(inputStream))
                {
                    if ((line = reader.ReadLine()) != null)
                    {
                        string[] splitLine = line.Split(' ');
                        HasParams = splitLine[1].Contains('?');
                        HttpMethod = GetMethodFromLine(splitLine);
                        Path = GetPathFromLine(splitLine);
                        Params = GetParamsFromLine(splitLine);

                        while ((line = reader.ReadLine()) != null)
                        {

                            if (line.StartsWith(PHContentLength))
                            {
                                ContentLength = GetContentLengthFromLine(line);
                            }
                            if (line.StartsWith(PHContentType))
                            {
                                ContentType = GetContentTypeFromLine(line);
                            }
                            if (line.StartsWith(PHAuthorization))
                            {
                                AuthToken = GetTokenFromLine(line);
                            }
                        }
                    }

                    if (HttpMethod == Method.POST || HttpMethod == Method.PUT)
                    {
                        if (ContentLength != null)
                        {
                            int readByte;
                            for (int i = 0; i < ContentLength; i++)
                            {
                                readByte = inputStream.ReadByte();
                                Body = $"{Body}{readByte}";
                            }
                        }
                    }

                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private Method GetMethodFromLine(string[] splitLine)
        {
            return stringToMethod[splitLine[0].ToUpper()];
        }

        private string GetPathFromLine(string[] splitLine)
        {
            if (HasParams)
            {
                return splitLine[1].Split("?")[0];
            }
            return splitLine[1];
        }

        private string GetParamsFromLine(string[] splitLine)
        {
            if (HasParams)
            {
                return splitLine[1].Split("?")[1];
            }
            return "";
        }

        private static int GetContentLengthFromLine(string line)
        {
            return int.Parse(line[PHContentLength.Length..]);
        }

        private static string GetContentTypeFromLine(string line)
        {
            return line[PHContentType.Length..];
        }

        private static string GetTokenFromLine(string line)
        {
            string[] splitline = line.Split(' ');
            string token = "";
            for (int i = 0; i < splitline.Length; i++)
            {
                if (splitline[i] == "Bearer")
                {
                    token = splitline[i + 1];
                    break;
                }
            }
            return token;
        }
    }
}
