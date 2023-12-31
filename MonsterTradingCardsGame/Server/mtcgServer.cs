﻿using MonsterTradingCardsGame.Server.Requests;
using MonsterTradingCardsGame.Server.Responses;
using Newtonsoft.Json;
using Npgsql;
using System.Net;
using System.Net.Sockets;

namespace MonsterTradingCardsGame.Server
{
    internal class MtcgServer
    {
        private readonly Socket ServerSocket;
        private readonly int Port;
        private readonly ResponseHandler ResponsHandler;
        public MtcgServer(ResponseHandler responseHandler)
        {
            ResponsHandler = responseHandler;
            Port = 10001;
            ServerSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
            );
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
                    Console.WriteLine($"Client connected {clientSocket.RemoteEndPoint}");
                    Thread clientThread = new(() => ThreadFunc(ResponsHandler, clientSocket));
                    clientThread.Start();
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        private static void ThreadFunc(ResponseHandler responseHandler, Socket clientSocket)
        {
            try
            {
                _ = new RequestHandler(responseHandler, clientSocket);

            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
