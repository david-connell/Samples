using System;
using System.Collections.Generic;
using System.IO.Pipes;

namespace TQC.GOC.InterProcessCommunication
{
    internal class NamedPipeServerData
    {
        public byte[] Buffer { get; set; }
        public bool CanDoNextCommand { get; set; }
        public bool PipeBroken { get; set; }
        private NamedPipeServerStream m_PipeServer;

        public NamedPipeServerData(NamedPipeServerStream pipeServer)
        {
            m_PipeServer = pipeServer;
            Buffer = new byte[255];
        }

        public int Length
        {
            get
            {
                return Buffer.Length;
            }
        }

        public NamedPipeServerStream PipeServer
        {
            get
            {
                return m_PipeServer;
            }
        }

    }
}
