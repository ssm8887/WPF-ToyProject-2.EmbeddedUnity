using NamedPipeStreamCommunication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static TCPCommunication.CommandEnum;

namespace NamedPipeServerTest
{
    class Program
    {
        private static CommandId commandId;
        public static NamedPipeServer pipeServer;

        static void Main(string[] args)
        {
            pipeServer = new NamedPipeServer();

            pipeServer.receiveAction += Read;

            Thread thread = new Thread(pipeServer.Connect);
            thread.Start();

            while (true)
            {
                var input = Console.ReadLine();
                if (byte.TryParse(input, out byte result))
                {
                    Write((CommandId)result);

                }
            }
        }


        private static void Read(byte[] buff)
        {
            if (buff == null)
            {
                return;
            }

            //using (MemoryStream ms = new MemoryStream(buff))
            //{
            //    using (BinaryReader br = new BinaryReader(ms))
            //    {
            //        var id = (CommandId)br.ReadByte();
            //        Command = id;
            //    }
            //}

            CommandByte = buff;

        }

        public static byte[] Write(CommandId command)
        {
            byte[] result = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)command);
                }

                result = ms.ToArray();
            }

            pipeServer.Send(result);
            return result;
        }

        private static void ExecuteCommand()
        {
            switch (commandId)
            {
                case CommandId.Command_01:
                    CommandString = "Command 01 Receive";
                    break;

                case CommandId.Command_02:
                    CommandString = "Command 02 Receive";
                    break;

                case CommandId.Command_03:
                    CommandString = "Command 03 Receive";
                    break;

                default:
                    CommandString = "None";
                    break;
            }
        }

        private static CommandId Command
        {
            get => commandId;
            set
            {
                commandId = value;
                ExecuteCommand();
                Console.WriteLine(CommandString);
            }
        }

        private static byte[] commandByte;
        private static byte[] CommandByte
        {
            get => commandByte;
            set
            {
                commandByte = value;
                Console.WriteLine(commandByte[0]);
            }
        }

        public static String CommandString { get; set; } = "None";
    }
}
