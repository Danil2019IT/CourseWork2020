using System;
using System.Net.Sockets;

namespace ConsoleServer
{
    public class ClientObject
    {
        private readonly TcpClient _client;
        public ClientObject(TcpClient tcpClient)
        {
            _client = tcpClient;
        }
 
        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = _client.GetStream();
                while (true)
                {
                    ClubHandler.ClubInteraction(stream);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                
                stream?.Close();
                _client?.Close();
            }
        }
    }
}