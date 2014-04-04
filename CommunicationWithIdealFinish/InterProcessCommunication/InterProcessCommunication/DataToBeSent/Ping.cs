using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.GOC.InterProcessCommunication.DataToBeSent
{
    class Ping : IDataToBeSent
    {
        public bool Send(NamedPipeServerData namedPipeServerData, Version protocolVersion)
        {
            bool status = false;
            if ((protocolVersion.Major == 1) && (protocolVersion.Minor == 0))
            {
                status = SendPingV1(namedPipeServerData);
            }
            return status;
        }

        private static bool SendPingV1(NamedPipeServerData namedPipeServerData)
        {
            string message = "@2";
            byte[] buf = Encoding.ASCII.GetBytes(message);
            namedPipeServerData.PipeServer.Write(buf, 0, buf.Length);
            return true;
        }
        public override string ToString()
        {
            return "Ping";
        }

    }
}
