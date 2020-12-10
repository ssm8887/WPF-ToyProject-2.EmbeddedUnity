using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NamedPipeStreamCommunication
{
    public class NamedPipeServer
    {
        //private NamedPipeServerStream pipeServer;
        private NamedPipeServerStream pipeServerIn;
        private NamedPipeServerStream pipeServerOut;

        public event Action<byte[]> receiveAction;

        public NamedPipeServer()
        {
            //PipeSecurity ps = new PipeSecurity();
            //ps.AddAccessRule(new PipeAccessRule(@"Everyone", PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance, AccessControlType.Allow));
            //ps.AddAccessRule(new PipeAccessRule(@"Everyone", PipeAccessRights.FullControl, AccessControlType.Allow));
            //ps.AddAccessRule(new PipeAccessRule(@"Everyone", PipeAccessRights.AccessSystemSecurity, AccessControlType.Allow));
            //ps.AddAccessRule(new PipeAccessRule(@"Everyone", PipeAccessRights.FullControl, AccessControlType.Allow));


            pipeServerOut = new NamedPipeServerStream("serverPipe", PipeDirection.Out, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            pipeServerIn = new NamedPipeServerStream("clientPipe", PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            //pipeServer = new NamedPipeServerStream("testPipe", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
        }

        public void Connect()
        {
            //pipeServer.WaitForConnection();
            pipeServerIn.WaitForConnection();
            pipeServerOut.WaitForConnection();

            Receive();
        }

        public void Receive()
        {
            byte[] inBuffer = new byte[128];

            while (true)
            {
                try
                {
                    int len = pipeServerIn.Read(inBuffer, 0, inBuffer.Length);
                    //int len = pipeServer.Read(inBuffer, 0, inBuffer.Length);

                    if (len != 0)
                    {
                        receiveAction?.Invoke(inBuffer);
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                    break;
                }
            }
        }

        public void Send(byte[] msg)
        {
            if (!pipeServerOut.IsConnected)
            //if (!pipeServer.IsConnected)
            {
                return;
            }

            byte[] outBuffer = msg;

            try
            {
                pipeServerOut.Write(outBuffer, 0, outBuffer.Length);
                pipeServerOut.Flush();
                //pipeServer.Write(outBuffer, 0, outBuffer.Length);
                //pipeServer.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }
    }
}
