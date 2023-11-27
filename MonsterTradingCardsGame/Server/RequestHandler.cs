using MonsterTradingCardsGame.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server
{
    internal class RequestHandler
    {
        public AppController AppController { get; set; }
        public Socket ClientSocket { get; set; }
        public Stream? ClientStream { get; set; }

        public RequestHandler(AppController appController, Socket clientSocket)
        {
            AppController = appController;
            ClientSocket = clientSocket;
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
            finally
            {
                CloseRequest();
            }
        }

        public void SendResponse(Request request)
        {
            Response response;
            if (request.Path == null)
            {
                response = new Response(System.Net.HttpStatusCode.BadRequest, "Pathname was not set");
            }
            else
            {
                response = AppController.HandleRequest(request);
            }
            using (StreamWriter writer = new(ClientStream))
            {
                writer.Write(response.Build());
            }
            Console.WriteLine($"Thread {Thread.CurrentThread.Name}: Content: {response.Content}");
        }

        public void CloseRequest()
        {
            try
            {
                if(ClientStream != null)
                {
                    ClientStream.Close();
                    ClientStream.Close();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
