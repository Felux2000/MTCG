using MonsterTradingCardsGame.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server
{
    internal class RequestHandler
    {
        private AppController AppController;
        private Socket ClientSocket;

        public RequestHandler(AppController appController, Socket clientSocket)
        {
            AppController = appController;
            ClientSocket = clientSocket;
        }

        public void run()
        {
            try
            {

            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                closeRequest();
            }
        }

        public void sendResponse()
        {

        }

        public void closeRequest()
        {

        }
    }
}
