using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace MonsterTradingCardsGame.Server
{
    internal class Response
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }

        public Response(HttpStatusCode httpStatus, string content)
        {
            StatusCode = (int)httpStatus;
            StatusMessage = Regex.Replace(httpStatus.ToString(), "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
            ContentType = "application/json";
            Content = content;
        }

        public string Build()
        {
            return ($"HTTP/2 {StatusCode} {StatusMessage}\r\nContent-Type: {ContentType}\r\nContent-Length: {Content.Length}\r\n\r\n{Content}");
        }
    }
}
