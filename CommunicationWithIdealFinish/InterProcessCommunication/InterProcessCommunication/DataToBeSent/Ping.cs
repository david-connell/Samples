using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.GOC.InterProcessCommunication.DataToBeSent
{
    class Ping : IDataToBeSent
    {
        public void Send(NamedPipeServerData namedPipeServerData, Version protocolVersion)
        {
            if ((protocolVersion.Major == 1) && (protocolVersion.Major == 0))
            {
                string message = "@2";
                byte[] buf = Encoding.ASCII.GetBytes(message);
                namedPipeServerData.PipeServer.Write(buf, 0, buf.Length);
            }
        }
        public override string ToString()
        {
            return "Ping";
        }

    }
}
