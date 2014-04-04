using System;
using System.Collections.Generic;
using System.Text;
using TQC.GOC.InterProcessCommunication.Model;

namespace TQC.GOC.InterProcessCommunication.DataToBeSent
{
    internal class StartOfRunData : IDataToBeSent
    {
        DataRunDetail m_RunDetail;
        public StartOfRunData(DataRunDetail runDetail)
        {
            m_RunDetail = runDetail;
        }

        DataRunDetail DataRunDetail { get { return m_RunDetail; } }

        public bool Send(NamedPipeServerData namedPipeServerData, Version protocolVersion)
        {
            bool status = false;
            if ((protocolVersion.Major == 1) && (protocolVersion.Minor == 0))
            {
                status = SendRunDetailsV1(namedPipeServerData);
            }
            return status;
        }

        private bool SendRunDetailsV1(NamedPipeServerData namedPipeServerData)
        {
            StringBuilder message = new StringBuilder("@4*");
            byte[] buf = Encoding.ASCII.GetBytes(message.ToString());
            List<byte> request = new List<byte>(buf);
            request.AddRange(BitConverter.GetBytes(m_RunDetail.BatchId));
            request.AddRange(BitConverter.GetBytes(m_RunDetail.NumberOfChannels));
            request.AddRange(BitConverter.GetBytes(m_RunDetail.SampleRate));
            request.AddRange(BitConverter.GetBytes(m_RunDetail.StartOfRun.ToOADate()));

            request.Add((byte)m_RunDetail.DefaultTemperatureUnits);
            request.Add((byte)m_RunDetail.DefaultThicknessUnits);

            request.AddRange(Encoding.ASCII.GetBytes(m_RunDetail.SerialNumber));
            request.Add((byte)0);
            request.AddRange(Encoding.ASCII.GetBytes(m_RunDetail.OperatorName));
            request.Add((byte)0);
            foreach (var channel in m_RunDetail.Channels)
            {
                request.Add((byte)channel.ChannelType);
                request.AddRange(Encoding.ASCII.GetBytes(channel.ChannelName));
                request.Add((byte)0);
            }
            request.Add((byte)0);
            namedPipeServerData.PipeServer.Write(request.ToArray(), 0, request.Count);
            return true;
        }

        public override string ToString()
        {
            return string.Format("Start '{0}'", m_RunDetail.BatchId);
        }
    }

}
