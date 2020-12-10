using System;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NamedPipeStreamCommunication
{
    public class NamedPipeClient
    {

        private string remoteInfo;
        public NamedPipeClientStream pipeClientIn;
        public NamedPipeClientStream pipeClientOut;
        //public NamedPipeClientStream pipeClient;

        //public event Action<byte[]> receiveAction;
        public event Action<byte[]> receiveAction;

        public NamedPipeClient()
        {
            //pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut);
            //pipeClientIn = new NamedPipeClientStream(".",
            //                 "testpipeIn",
            //                 PipeAccessRights.ReadData | PipeAccessRights.WriteAttributes,
            //                 PipeOptions.None,
            //                 System.Security.Principal.TokenImpersonationLevel.None,
            //                 System.IO.HandleInheritability.None);
            pipeClientIn = new NamedPipeClientStream(".", "serverPipe", PipeDirection.In);
            pipeClientOut = new NamedPipeClientStream(".", "clientPipe", PipeDirection.Out);
            //pipeClient = new NamedPipeClientStream(".", "testPipe", PipeDirection.InOut, PipeOptions.Asynchronous);

        }

        public void Connect()
        {
            pipeClientIn.Connect();
            pipeClientOut.Connect();
            //pipeClient.Connect();

            var thread = new Thread(Receive);
            thread.Start();
        }

        public void Receive()
        {
            byte[] inBuffer = new byte[128];

            while (true)
            {
                try
                {
                    //int len = await pipeClientIn.ReadAsync(inBuffer, 0, inBuffer.Length).ConfigureAwait(false);
                    int len = pipeClientIn.Read(inBuffer, 0, inBuffer.Length);

                    if (len != 0)
                    {
                        receiveAction?.Invoke(inBuffer);
                        remoteInfo = Encoding.Unicode.GetString(inBuffer);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }
            }

            //char[] inBuffer = new char[128];

            //using (var sr = new StreamReader(pipeClient))
            //{
            //    while (true)
            //    {
            //        try
            //        {
            //            int len = sr.Read(inBuffer, 0, inBuffer.Length);

            //            if (len != 0)
            //            {
            //                receiveAction?.Invoke(inBuffer);
            //            }
            //        }
            //        catch
            //        {

            //        }
            //    }
            //}
        }

        public void Send(byte[] msg)
        {

            if (!pipeClientOut.IsConnected)
            //if (!pipeClient.IsConnected)
            {
                return;
            }

            byte[] outBuffer = msg;

            try
            {
                pipeClientOut.Write(outBuffer, 0, outBuffer.Length);
                pipeClientOut.Flush();
                pipeClientOut.WaitForPipeDrain();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

            //if (!pipeClient.IsConnected)
            //{
            //    return;
            //}

            //char[] outBuffer = msg;

            //using (StreamWriter sw = new StreamWriter(pipeClient))
            //{
            //    try
            //    {
            //        sw.AutoFlush = true;
            //        sw.Write(outBuffer, 0, outBuffer.Length);
            //        //pipeServer.WaitForPipeDrain();
            //        //pipeServer.Flush();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message.ToString());
            //    }
            //}

        }

    }
}
