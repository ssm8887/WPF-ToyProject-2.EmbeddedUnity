using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPCommunication
{
    public class CommandEnum
    {
        public enum CommandId : byte
        {
            None = 0,
            Command_01 = 1,
            Command_02 = 2,
            Command_03 = 3
        }
    }
}
