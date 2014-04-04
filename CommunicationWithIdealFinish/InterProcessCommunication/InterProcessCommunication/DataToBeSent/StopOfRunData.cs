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

        public void Send(NamedPipeServerData namedPipeServerData, Version protocolVersion)
        {
            if ((protocolVersion.Major == 1) && (protocolVersion.Major == 0))
            {
                SendStopRunDataV1(namedPipeServerData);
            }
        }

        private static void SendStopRunDataV1(NamedPipeServerData namedPipeServerData)
        {
            string message = "@6";
            byte[] buf = Encoding.ASCII.GetBytes(message);
            namedPipeServerData.PipeServer.Write(buf, 0, buf.Length);
        }

        public override string ToString()
        {
            return "DataRunFinish";
        }
    }

}
