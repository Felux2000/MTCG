using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;

namespace MonsterTradingCardsGame.Server
{
    internal class RequestHandler
    {
        public ResponseHandler ResponseHandler { get; set; }
        public Socket ClientSocket { get; set; }
        public Stream? ClientStream { get; set; }

        public RequestHandler(ResponseHandler responseHandler, Socket clientSocket)
        {
            ResponseHandler = responseHandler;
            ClientSocket = clientSocket;
            Run();
        }

        public void Run()
        {
            try
            {
                ClientStream = new NetworkStream(ClientSocket);
                SendResponse(new Request(ClientStream));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            CloseRequest();
        }

        public void SendResponse(Request request)
        {
            Response response;
            try
            {

                if (request.Path == null)
                {
                    response = new Response(System.Net.HttpStatusCode.BadRequest, ContentType.JSON, "Pathname was not set");
                }
                else
                {
                    response = ResponseHandler.CreateResponse(request);
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                response = new Response(HttpStatusCode.InternalServerError, ContentType.TEXT, $"null error: Internal Server Error \n");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
                response = new Response(HttpStatusCode.InternalServerError, ContentType.TEXT, $"null error: Internal Server Error \n");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.StackTrace);
                response = new Response(HttpStatusCode.InternalServerError, ContentType.TEXT, $"null error: Internal Server Error \n");
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
                response = new Response(HttpStatusCode.InternalServerError, ContentType.TEXT, $"null error: Internal Server Error \n");
            }
            using (StreamWriter writer = new(ClientStream))
            {
                writer.Write(response.Build());
            }
        }

        public void CloseRequest()
        {
            try
            {
                if (ClientStream != null)
                {
                    ClientStream.Close();
                    ClientSocket.Close();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
