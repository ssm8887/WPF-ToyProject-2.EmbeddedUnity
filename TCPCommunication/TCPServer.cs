using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPCommunication
{
    public class TCPServer
    {
        private string ipAddress;
        private int port;

        byte[] buff = new byte[1024];

        public event Action<byte[]> receiveAction;

        public TCPServer(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void Connect()
        {
            AsyncServer().Wait();
        }

        private async Task AsyncServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            listener.Start();

            while (true)
            {
                TcpClient tc = await listener.AcceptTcpClientAsync().ConfigureAwait(false);

                await Task.Run(() => AsyncTCPProcess(tc));
            }
        }

        private async void AsyncTCPProcess(Object o)
        {
            TcpClient tc = o as TcpClient;

            var stream = tc.GetStream();

            int recvBytes = 0;

            while (true)
            {
                try
                {
                    recvBytes = await stream.ReadAsync(buff, 0, buff.Length);
                }
                catch
                {
                    Console.WriteLine("ReadAsync Error");
                }

                if (recvBytes == 0)
                {
                    stream.Close();
                }
                else
                {
                    receiveAction?.Invoke(buff);
                }

            }
        }

    }
}
