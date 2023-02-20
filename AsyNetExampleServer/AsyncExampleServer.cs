using System;

namespace HNet
{
    internal class AsyncServerStart
    {
        static void Main(string[] args)
        {
           AsyncNetServer server = new AsyncNetServer();
            server.StartServer("192.168.3.71",17666);

            Console.ReadKey();
        }
    }
}
