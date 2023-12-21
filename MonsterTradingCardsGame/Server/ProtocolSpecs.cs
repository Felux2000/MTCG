using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server
{
    public enum Method
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 3
    }

    public enum ContentType
    {
        JSON = 0, TEXT = 1
    }

    internal class ProtocolSpecs
    {
        public static readonly Dictionary<string, Method> stringToMethod = new Dictionary<string, Method>
        {
            {"GET" , Method.GET},
            {"POST" ,Method.POST},
            {"PUT" ,Method.PUT},
            {"DELETE" ,Method.DELETE}
        };
        public const char PSAuthTokenSeperator = '-';
        public const char PSRequestBlockSeperator = ' ';
        public const char PSRequestParamSeperator = '?';
        public const char PSRequestPathSeperator = '/';
        public const string PSHttpVersion = "HTTP/1.1";
        public const string PSContentType = "Content-Type: ";
        public const string PSContentLength = "Content-Length: ";
        public const string PSAuthorization = "Authorization";
        public const string PSAuthorizationPrefix = "Bearer";
        public const string PSAuthTokenSuffix = "-mtcgToken";
        public const string PSConentTypeJson = "application/json";
        public const string PSConentTypeText = "text/plain";
        public const string PSParameterContentPlain = "format=plain";
        public const string PSTradingIdPattern = "([A-Za-z0-9]+(-[A-Za-z0-9]+)+)";
        public const string PSUsernamePattern = "[a-zA-Z]+";

    }
}
