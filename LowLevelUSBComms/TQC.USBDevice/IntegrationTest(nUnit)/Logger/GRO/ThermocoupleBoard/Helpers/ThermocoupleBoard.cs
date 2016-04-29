using System;
using TQC.USBDevice;
using TQC.USBDevice.GradientOven;

namespace IntegrationTestNUnit.Logger.GRO.ThermocoupleBoard
{
    class ThermocoupleBoard : IDisposable
    {
        GROMainBoard m_Logger = new GROMainBoard(null);
        readonly byte m_ThermocoupleBoard;
        public ThermocoupleBoard(USBLogger.USBProductId productId, byte thermocoupleBoard)
        {
            m_Logger.OpenWithMinumumRequests(productId);
            m_ThermocoupleBoard = thermocoupleBoard;
        }
        public void Dispose()
        {
            Dispose(true);
        }

        public IGROThermoCoupleBoard Board
        {
            get
            {
                return m_Logger.GetChildDevice(m_ThermocoupleBoard);
            }
        }


        private void Dispose(bool inDispose)
        {
            if (inDispose)
            {
                if (m_Logger != null)
                {
                    m_Logger.Close();
                    m_Logger = null;
                }
                GC.SuppressFinalize(this);
            }
        }
    }
}
