using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server
{
    internal class mtcgServer
    {
        private Socket ServerSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
            );
        private int Port;
        private Game Game;
        public mtcgServer(Game game)
        {
            Game = game;
            Port = 10001;
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
                    Console.WriteLine("Waiting for client...");
                    Socket clientSocket = ServerSocket.Accept();
                    Console.WriteLine("Client connected" + clientSocket.RemoteEndPoint);
                    Thread clientThread = new(() => ThreadFunc(Game, clientSocket));
                    clientThread.Start();
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        private static void ThreadFunc(Game game, Socket clientSocket)
        {
            _ = new RequestHandler(game, clientSocket);
        }

    }
}
