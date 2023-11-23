using MonsterTradingCardsGame.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server
{
    internal class Server
    {
        private Socket ServerSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
            );
        private int Port;
        private AppController AppController;
        public Server(AppController appController, int port)
        {
            AppController = appController;
            Port = port;
        }

        public void Start()
        {
            ServerSocket.Bind(new IPEndPoint(IPAddress.Loopback, Port));
            ServerSocket.Listen(5);
            Run();
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    Socket clientSocket = ServerSocket.Accept();
                    Thread clientThread = new Thread(() => ThreadFunc(AppController,clientSocket));
                    clientThread.Start();
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
        
        private void ThreadFunc(AppController appController, Socket clientSocket)
        {
            new RequestHandler(appController, clientSocket);
        }

    }
}
