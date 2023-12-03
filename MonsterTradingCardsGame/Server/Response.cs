using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace MonsterTradingCardsGame.Server
{
    public enum ContentType
    {
        JSON = 0, TEXT = 1
    }
    internal class Response
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }

        public Response(HttpStatusCode httpStatus, ContentType contentType, string content)
        {
            StatusCode = (int)httpStatus;
            StatusMessage = Regex.Replace(httpStatus.ToString(), "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
            Type = (contentType == ContentType.JSON ? "application/json" : "text/plain");
            Content = content;
        }

        public string Build()
        {
            return ($"HTTP/1.1 {StatusCode} {StatusMessage}\r\nContent-Type: {Type}\r\nContent-Length: {Content.Length}\r\n\r\n{Content}");
        }
    }
}
