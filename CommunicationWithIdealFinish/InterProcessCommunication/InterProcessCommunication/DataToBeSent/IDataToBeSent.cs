using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.GOC.InterProcessCommunication.DataToBeSent
{
    internal interface IDataToBeSent
    {
        void Send(NamedPipeServerData namedPipeServerData, Version protocolVersion);
    }
}
