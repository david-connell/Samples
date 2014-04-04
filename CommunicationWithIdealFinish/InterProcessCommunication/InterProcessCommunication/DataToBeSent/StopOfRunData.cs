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

        public void Send(NamedPipeServerData namedPipeServerData)
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
