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
        private int StatusCode;
        private string StatusMessage;
        private string ContentType;
        private string Content;

        public Response(HttpStatusCode httpStatus, string content)
        {
            StatusCode = (int)httpStatus;
            StatusMessage = Regex.Replace(httpStatus.ToString(), "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
            ContentType = "application/json";
            Content = content;
        }

        private string build()
        {
            return ($"HTTP/2 {StatusCode} {StatusMessage}\r\nContent-Type: {ContentType}\r\nContent-Length: {Content.Length}\r\n\r\n{Content}");
        }
    }
}
