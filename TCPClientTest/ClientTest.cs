using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPCommunication;

namespace TCPClientTest
{
    class ClientTest
    {
        static byte[] byteArr = new byte[30];

        static void Main(string[] args)
        {

            var tcpClient = new TCPCommunication.TCPClient("127.0.0.1", 2000);

            tcpClient.Connect();

            while (true)
            {
                var input = Console.ReadLine();

                byteArr[0] = byte.Parse(input);

                tcpClient.Send(byteArr);
            }
        }
        
    }
}
