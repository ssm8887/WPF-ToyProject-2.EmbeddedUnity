using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TCPCommunication;

namespace TCPTest
{
    class ServerTest
    {
        static void Main(string[] args)
        {

            var tcpServer = new TCPServer(IPAddress.Any.ToString(), 2000);

            tcpServer.receiveAction += ConsoleWrite;

            tcpServer.Connect();
        }

        private static void ConsoleWrite(byte[] buff)
        {

            Console.WriteLine(buff[0]);
        }
    }
}
