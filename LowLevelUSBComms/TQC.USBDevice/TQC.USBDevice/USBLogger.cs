using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TQC.USBDevice
{
    

    public class USBLogger : IDisposable
    {
        public enum DeviceType
        {
            Unknown = -1,
            SoloGlossmeter,
            DuoGlossmeter,
            PolyGlossmeter,
            CurveX3_Basic,
            CurveX3_Mid,
            CurveX3_High,
            GRO,
        }
        
        public enum USBProductId
        {
            Glossmeter = 0x1b4cFF00,
            USB_PRODUCT2 = 0x15CD0011,
            USB_CURVEX_3 = 0x2047FFFE,
            USB_CURVEX_3a = 0x20470827,
            GRADIENT_OVEN = 0x04037B60,

        }
        private TQC.IF.USBLogger.USBGeneric m_Logger;

        public USBLogger()
        {
            m_Logger = new IF.USBLogger.USBGeneric();
        }
        ~USBLogger()
        {
            Dispose(false);
        }


        public bool Open(USBProductId id)
        {
            int result = -1;
            try
            {
                result = m_Logger.Open(0, 0, 0, null, (uint)id);
            }
            catch (COMException ex)
            {
                Console.WriteLine("Failed to open logger {0} - '{1}'", id.ToString(), ex.Message);
            }
            return result == 1;
        }

        public void Close()
        {
            m_Logger.Close();
            
        }
        internal enum Commands
        {
            ReadDeviceInfo = 0x01,
            WriteDeviceInfo= 0x11,
            ReadCalibrationDetails = 0x02,
            WriteCalibrationDetails = 0x12,
            ReadLoggedInfo = 0x03,
            OffloadData = 0x04,
            ReadCurrentProbeVals = 0x05,
            ReadCurrentRawProbeVals = 0x05,
            GROSetCommand = 0x60,
            GROReadCommand = 0x61,
        }

        internal byte[] Request(Commands command, byte[] request)
        {
            return m_Logger.GenericCommand(0, (byte) command, request);
        }
        public string Version
        {
            get
            {
                return m_Logger.strVersion;
            }
        }

        public string LoggerSerialNumber
        {
            get
            {
                return m_Logger.LoggerId;
            }
        }

        public DeviceType LoggerType
        {
            get
            {
                switch (m_Logger.LoggerType)
                {
                    case 1: return DeviceType.SoloGlossmeter;
                    case 2: return DeviceType.DuoGlossmeter;
                    case 3: return DeviceType.PolyGlossmeter;
                    case 4: return DeviceType.CurveX3_Basic;
                    case 5: return DeviceType.CurveX3_Mid;
                    case 6: return DeviceType.CurveX3_High;
                    case 7: return DeviceType.GRO;
                }
                return DeviceType.Unknown;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool inDispose)
        {
            if (inDispose)
            {
                Close();
                GC.SuppressFinalize(this);
            }
        }

        public string CalibrationCompany 
        {
            get
            {
                object result = null;
                m_Logger.GetCalibrationData(0, 0, ref result);
                return result as string;
            }
        }

        public string CalibrationUser
        {
            get
            {
                object result = null;
                m_Logger.GetCalibrationData(0, 1, ref result);
                return result as string;
            }
        }

        public DateTime CalibrationDate
        {
            get
            {
                object result = null;
                m_Logger.GetCalibrationData(0, 1, ref result);
                return DateTime.FromOADate((double)result);
            }
        }
    }
}
