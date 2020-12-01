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
        TcpClient tcpClient;

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
                tcpClient = await listener.AcceptTcpClientAsync().ConfigureAwait(false);

                await Task.Run(() => AsyncTCPProcess());
            }
        }

        private async void AsyncTCPProcess()
        {
            var stream = tcpClient.GetStream();

            int recvBytes = 0;

            while (true)
            {
                try
                {
                    recvBytes = await stream.ReadAsync(buff, 0, buff.Length);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"ReadAsync Error : {ex.Message}");
                    Console.WriteLine($"Stream Close");
                }

                if (recvBytes == 0)
                {
                    stream.Close();
                    break;
                }
                else
                {
                    receiveAction?.Invoke(buff);
                    recvBytes = 0;
                }

            }
        }

        public void Send(byte[] msg)
        {
            if (tcpClient is null)
            {
                return;
            }

            if (tcpClient.GetStream() != null)
            {
                NetworkStream stream = tcpClient.GetStream();

                stream.Write(msg, 0, msg.Length);
            }
        }

    }
}
