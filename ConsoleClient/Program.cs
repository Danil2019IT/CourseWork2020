using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
 
namespace ConsoleClient
{
    internal static class Program
    {
        private const int Port = 8888;
        private const string Address = "127.0.0.1";

        private static void Main()
        {
            Console.WriteLine("Enter something and we will start");
            TcpClient client = null;
            try
            {
                client = new TcpClient(Address, Port);
                var stream = client.GetStream();
 
                while (true)
                {
                    Console.Write("User: ");
                    // enter message
                    var message = Console.ReadLine();
                    message = $"{message}";
                    // convert the message to an array of bytes
                    var data = Encoding.Unicode.GetBytes(message);
                    // sending message
                    stream.Write(data, 0, data.Length);
 
                    // get answer
                    data = new byte[64]; // buffer for received data
                    var builder = new StringBuilder();
                    do
                    {
                        var bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
 
                    message = builder.ToString();
                    Console.WriteLine("Server:\n{0}", message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client?.Close();
            }
        }
    }
}