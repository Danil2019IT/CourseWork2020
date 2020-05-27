namespace ConsoleServer
{
    internal static class Program
    {
        private const int Port = 8888;

        private static void Main()
        {
            var server = new Server("127.0.0.1", Port);
            server.StartListener();
        }
    }
}