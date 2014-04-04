using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.GOC.InterProcessCommunication.DataToBeSent
{
    internal class StopOfRunData : IDataToBeSent
    {

        public StopOfRunData()
        {

        }

        public bool Send(NamedPipeServerData namedPipeServerData, Version protocolVersion)
        {
            bool status = false;
            if ((protocolVersion.Major == 1) && (protocolVersion.Minor == 0))
            {
                status = SendStopRunDataV1(namedPipeServerData);
            }
            return status;
        }

        private static bool SendStopRunDataV1(NamedPipeServerData namedPipeServerData)
        {
            string message = "@6";
            byte[] buf = Encoding.ASCII.GetBytes(message);
            namedPipeServerData.PipeServer.Write(buf, 0, buf.Length);
            return true;
        }

        public override string ToString()
        {
            return "DataRunFinish";
        }
    }

}
