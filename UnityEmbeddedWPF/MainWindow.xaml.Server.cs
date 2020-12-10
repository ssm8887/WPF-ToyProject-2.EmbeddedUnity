using NamedPipeStreamCommunication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPCommunication;
using static TCPCommunication.CommandEnum;

namespace UnityEmbeddedWPF
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        //private TCPClient tcpClient;
        private CommandId commandId;
        private NamedPipeServer pipeServer;

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateProperty(params string[] properties)
        {
            foreach (var property in properties)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }

        private void Server_Load(object sender, EventArgs e)
        {
            /// TCP Client 연결시
            //tcpClient = new TCPClient("127.0.0.1", 2000);
            //tcpClient.receiveAction += Read;
            //Task.Run(() => tcpClient.Connect());

            /// Named Pipe Server 연결시
            pipeServer = new NamedPipeServer();
            pipeServer.receiveAction += Read;

            Task.Run(() => pipeServer.Connect());
        }

        public void Read(byte[] data)
        {
            if (data == null)
            {
                return;
            }

            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Command = (CommandId)br.ReadByte();
                }
            }
        }

        public byte[] Write(CommandId command)
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

        private void ExecuteCommand()
        {
            switch (commandId)
            {
                case CommandId.Command_01:
                    CommandString = "Command 01 Receive";
                    //UpdateProperty("CommandString");
                    break;

                case CommandId.Command_02:
                    CommandString = "Command 02 Receive";
                    //UpdateProperty("CommandString");
                    break;

                case CommandId.Command_03:
                    CommandString = "Command 03 Receive";
                    //UpdateProperty("CommandString");
                    break;

                default:
                    CommandString = "None Data Receive";
                    //UpdateProperty("CommandString");
                    break;
            }
            UpdateProperty("CommandString");
        }

        private CommandId Command
        {
            get => commandId;
            set
            {
                commandId = value;
                ExecuteCommand();
            }
        }

        public String CommandString { get; set; } = "None";


    }
}
