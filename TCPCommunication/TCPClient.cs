using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPCommunication
{
    class TCPClient
    {
        private string ipAddress;
        private int port;
        private byte[] buff = new byte[1024];
        private TcpClient client;
        private NetworkStream networkStream;

        private CancellationTokenSource tokenSourceExecute = null;
        private CancellationTokenSource tokenSourceReceive = null;

        public event Action<byte[]> receiveAction;


        public TCPClient(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void Connect()
        {
            if (client is null)
            {
                client = new TcpClient();
            }

            tokenSourceExecute = new CancellationTokenSource();

            Task task = null;
            IPAddress ip = null;

            if (IPAddress.TryParse(ipAddress, out var ipAddr))
            {
                ip = ipAddr;
            }
            while (!tokenSourceExecute.Token.IsCancellationRequested)
            {
                try
                {
                    task = client.ConnectAsync(ip, port);
                    task.Wait(5000);

                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        task = null;

                        if (networkStream != null)
                        {
                            DisConnect(ref client);
                        }

                        networkStream = client?.GetStream();
                        Receive();
                        break;
                    }
                    else
                    {
                        task = null;
                    }
                }
                catch
                {
                }
            }
        }

        public async void Receive()
        {
            int rcvBytes = 0;
            this.tokenSourceReceive = new CancellationTokenSource();

            while (!this.tokenSourceReceive.Token.IsCancellationRequested)
            {

                try
                {
                    rcvBytes = await networkStream.ReadAsync(this.buff, 0, this.buff.Length).ConfigureAwait(false);

                    if (rcvBytes == 0)
                    {
                        this.DisConnect(ref client);
                        break;
                    }
                    else if (rcvBytes == 30)
                    {
                        this.receiveAction?.Invoke(buff);
                        rcvBytes = 0;
                    }
                    else
                    {
                        rcvBytes = 0;
                    }
                }
                catch
                {
                    this.DisConnect(ref client);
                    break;
                }

            }
        }

        private void DisConnect(ref TcpClient tempclient)
        {
            if (tempclient != null)
            {
                tempclient.Close();
                tempclient = null;
            }

            if (networkStream != null)
            {
                networkStream.Close();
                networkStream = null;
            }

        }

        public void RequestCancel()
        {
            this.tokenSourceExecute?.Cancel();
            this.tokenSourceReceive?.Cancel();
            this.DisConnect(ref this.client);

            this.client = null;
        }

    }
}
