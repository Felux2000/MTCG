using MonsterTradingCardsGame.Server.Responses;
using Newtonsoft.Json;
using Npgsql;
using System.Net;
using System.Net.Sockets;

namespace MonsterTradingCardsGame.Server.Requests
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
                    response = new Response(HttpStatusCode.BadRequest, ContentType.JSON, "Pathname was not set");
                }
                else
                {
                    response = ResponseHandler.CreateResponse(request);
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
                response = Response.InternalServerError();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
                response = Response.InternalServerError();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.StackTrace);
                response = Response.InternalServerError();
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
                response = Response.InternalServerError();
            }
            if (ClientStream != null)
            {
                using (StreamWriter writer = new(ClientStream))
                {
                    writer.Write(response.Build());
                }
            }
            else
            {
                Console.WriteLine("ClientStream null");
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
