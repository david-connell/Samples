using System;
using System.Collections.Generic;
using System.Text;
using TQC.GOC.InterProcessCommunication.Model;

namespace TQC.GOC.InterProcessCommunication.DataToBeSent
{
    internal class SampleData : IDataToBeSent
    {
        SamplePoint m_SamplePoint;
        public SampleData(SamplePoint runDetail)
        {
            m_SamplePoint = runDetail;
        }

        SamplePoint SamplePoint { get { return m_SamplePoint; } }

        public void Send(NamedPipeServerData namedPipeServerData, Version protocolVersion)
        {
            if ((protocolVersion.Major == 1) && (protocolVersion.Major == 0))
            {
                SendSampleDataV1(namedPipeServerData);
            }
        }

        private void SendSampleDataV1(NamedPipeServerData namedPipeServerData)
        {
            StringBuilder message = new StringBuilder("@5*");
            List<byte> request = new List<byte>();
            request.AddRange(Encoding.ASCII.GetBytes(message.ToString()));
            request.AddRange(BitConverter.GetBytes(m_SamplePoint.SampleTime.ToOADate()));
            foreach (var item in m_SamplePoint.Samples)
            {
                request.AddRange(BitConverter.GetBytes(item));
            }
            byte[] buf = request.ToArray();

            namedPipeServerData.PipeServer.Write(buf, 0, buf.Length);
        }

        public override string ToString()
        {
            return string.Format("Sample '{0}'", m_SamplePoint.Samples[0]);
        }

    }
}
