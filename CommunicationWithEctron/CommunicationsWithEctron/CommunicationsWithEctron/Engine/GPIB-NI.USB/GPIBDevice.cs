using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunicationsWithEctron.Engine.GPIB_Device;

namespace CommunicationsWithEctron.Engine.GPIB_NI.USB
{
    public class GPIBDevice : IGPIBDevice
    {
        int m_BoardId;

        byte m_PAD;
        byte m_SAD;


        internal GPIBDevice(int boardId, byte pad, byte sad)
        {
            BoardId = boardId;
            PAD = pad;
            SAD = sad;
        }


        internal int BoardId
        {
            get { return m_BoardId; }
            private set { m_BoardId = value; }
        }

        internal byte PAD
        {
            get { return m_PAD; }
            private set { m_PAD = value; }
        }

        internal byte SAD
        {
            get { return m_SAD; }
            private set { m_SAD = value; }
        }

        public override string ToString()
        {
            return string.Format("NI USB {0}.{1}", BoardId, PAD);
        }
    }
}
