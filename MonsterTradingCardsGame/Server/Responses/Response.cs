using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MonsterTradingCardsGame.Server.ProtocolSpecs;


namespace MonsterTradingCardsGame.Server.Responses
{
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
            Type = contentType == ContentType.JSON ? PSConentTypeJson : PSConentTypeText;
            Content = content;
        }

        public static Response InternalServerError()
        {
            return new Response(HttpStatusCode.InternalServerError, ContentType.TEXT, $"null error: Internal Server Error \n");
        }

        public static Response BadRequest()
        {
            return new Response(HttpStatusCode.BadRequest, ContentType.TEXT, $"null error: Bad Request \n");
        }

        public static Response Unauthorized()
        {
            return new Response(HttpStatusCode.Unauthorized, ContentType.TEXT, $"null error: Access token is missing or invalid \n");
        }

        public static Response MethodNotFound()
        {
            return new Response(HttpStatusCode.NotFound, ContentType.TEXT, $"null error: Method not found \n");
        }

        public string Build()
        {
            return $"{PSHttpVersion} {StatusCode} {StatusMessage}\r\n{PSContentType}{Type}\r\n{PSContentLength}{Content.Length}\r\n\r\n{Content}";
        }
    }
}
