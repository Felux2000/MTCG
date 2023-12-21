using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using static MonsterTradingCardsGame.Server.ProtocolSpecs;

namespace MonsterTradingCardsGame.Server.Requests
{
    internal class Request
    {
        public Method? HttpMethod { get; set; }
        public string? Path { get; set; }
        public string? Params { get; set; }
        public string? ContentType { get; set; }
        public int? ContentLength { get; set; }
        public string? Body { get; set; }
        public string? AuthToken { get; set; }
        public bool HasParams { get; set; }

        public Request(Stream inputStream)
        {
            BuildRequest(inputStream);
        }
        private void BuildRequest(Stream inputStream)
        {
            try
            {
                string? line;
                using (StreamReader reader = new(inputStream, leaveOpen: true))
                {
                    line = reader.ReadLine();
                    if (line != string.Empty && line != null)
                    {
                        string[] splitLine = line.Split(PSRequestBlockSeperator);
                        HasParams = splitLine[1].Contains(PSRequestParamSeperator);
                        HttpMethod = GetMethodFromLine(splitLine);
                        Path = GetPathFromLine(splitLine);
                        Params = GetParamsFromLine(splitLine);

                        while (line != string.Empty && line != null)
                        {
                            if (line.StartsWith(PSContentLength))
                            {
                                ContentLength = GetContentLengthFromLine(line);
                            }
                            if (line.StartsWith(PSContentType))
                            {
                                ContentType = GetContentTypeFromLine(line);
                            }
                            if (line.StartsWith(PSAuthorization))
                            {
                                AuthToken = GetTokenFromLine(line);
                            }
                            line = reader.ReadLine();
                        }
                    }


                    if (HttpMethod == Method.POST || HttpMethod == Method.PUT)
                    {
                        if (ContentLength != null)
                        {
                            int readByte;
                            for (int i = 0; i < ContentLength; i++)
                            {
                                readByte = reader.Read();
                                if (readByte == -1)
                                {
                                    break;
                                }
                                Body = $"{Body}{(char)readByte}";

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

        private static Method GetMethodFromLine(string[] splitLine)
        {
            return stringToMethod[splitLine[0].ToUpper()];
        }

        private string GetPathFromLine(string[] splitLine)
        {
            if (HasParams)
            {
                return splitLine[1].Split(PSRequestParamSeperator)[0];
            }
            return splitLine[1];
        }

        private string GetParamsFromLine(string[] splitLine)
        {
            if (HasParams)
            {
                return splitLine[1].Split(PSRequestParamSeperator)[1];
            }
            return string.Empty;
        }

        private static int GetContentLengthFromLine(string line)
        {
            return int.Parse(line[PSContentLength.Length..]);
        }

        private static string GetContentTypeFromLine(string line)
        {
            return line[PSContentType.Length..];
        }

        private static string GetTokenFromLine(string line)
        {
            string[] splitline = line.Split(PSRequestBlockSeperator);
            string token = string.Empty;
            for (int i = 0; i < splitline.Length; i++)
            {
                if (splitline[i] == PSAuthorizationPrefix)
                {
                    token = splitline[i + 1];
                    break;
                }
            }
            return token;
        }
    }
}

