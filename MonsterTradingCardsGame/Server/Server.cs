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
        private Game Game;
        public Server(Game game, int port)
        {
            Game = game;
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
