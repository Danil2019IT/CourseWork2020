using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleServer
{
    public class Server
    {
        private readonly TcpListener _server;

        public Server(string ip, int port)
        {
            var localAddr = IPAddress.Parse(ip);
            _server = new TcpListener(localAddr, port);
            _server.Start();
            StartListener();
        }

        public void StartListener()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    var client = _server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    var clientObject = new ClientObject(client);
                    var clientThread = new Thread(clientObject.Process);
                    clientThread.Start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                _server.Stop();
            }
        }
    }
}