﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TQC.USBDevice
{
    

    public class USBLogger : IDisposable
    {
        public enum ProbeType
        {
            Gloss = 0,
            Temperature=1,
            Humidity=2,
        }

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

        internal USBLogger()
        {
            m_Logger = new IF.USBLogger.USBGeneric();
        }

        ~USBLogger()
        {
            Dispose(false);
        }

        public int GradientOvenComPort
        {
            get
            {
                int comPort;
                m_Logger.GradientOvenComPort(out comPort);                
                return comPort;
            }
        }

        virtual protected void ClearCachedData()
        {

        }
        public bool Open(USBProductId id)
        {
            int result = -1;
            try
            {
                result = m_Logger.Open(0, 0, 0, null, (uint)id);
                ClearCachedData();
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
            ReadCurrentRawProbeVals = 0x06,
            WriteSetup = 0x17,
            GROSetCommand = 0x60,
            GROReadCommand = 0x61,
        }
        public enum USBCommandResponseCode
        {
            OK, 
            UnknownError,
            CommandCorrupt,
            CommandOutOfSequence,
            CommandUnexpected,
            DeviceBusy,
            CommandNotSuported,
            EnumerationNotSuported,
            BatchNotAvailable,
            DataOutOfRange,
            CommandModeNotSupported,
            Unknown,
        }

        
        
        const int DISP_E_MEMBERNOTFOUND = unchecked((int)(0x80020003L));
        const int STG_E_DOCFILECORRUPT = unchecked((int)(0x80030109L));
        const int STG_E_BADBASEADDRESS = unchecked((int)(0x80030110L));
        const int RPC_E_UNEXPECTED = unchecked((int)(0x8001FFFFL));
        const int STG_E_INUSE = unchecked((int)(0x80030100L));
        const int CO_E_INCOMPATIBLESTREAMVERSION = unchecked((int)(0x8001013BL));
        const int SEC_E_UNSUPPORTED_FUNCTION = unchecked((int)(0x80090302L));
        const int SPAPI_E_MACHINE_UNAVAILABLE = unchecked((int)(0x800F0222L));
        const int OSS_OUT_OF_RANGE = unchecked((int)(0x80093021L));
        const int CO_E_FAILEDTOOPENPROCESSTOKEN = unchecked((int)(0x8001013CL));


        private static DeviceResponseUnexpectedException UsbErrorToException(COMException ex)
        {
            switch (ex.HResult)
            {
                case DISP_E_MEMBERNOTFOUND:
                    return new DeviceUnknownErrorException();
                case STG_E_DOCFILECORRUPT:
                    {
                        return new CommandCorruptException();
                    }
                case STG_E_BADBASEADDRESS:
                    {
                        return new CommandOutOfSequenceException();
                    }
                case RPC_E_UNEXPECTED:
                    {
                        return new CommandUnexpectedException();
                    }
                case STG_E_INUSE:
                    {
                        return new DeviceBusyException();                        
                    }
                case CO_E_INCOMPATIBLESTREAMVERSION:
                    {
                        return new CommandNotSuportedException();
                    }
                case SEC_E_UNSUPPORTED_FUNCTION:
                    {
                        return new EnumerationNotSuportedException();
                    }
                case SPAPI_E_MACHINE_UNAVAILABLE:
                    {
                        return new BatchNotAvailableException();
                    }
                case OSS_OUT_OF_RANGE:
                    {
                        return new DataOutOfRangeException();
                    }
                case CO_E_FAILEDTOOPENPROCESSTOKEN:
                    {
                        return new CommandModeNotSupportedException();
                    }
                default:
                    return null;
            }
        }

        internal byte[] Request(Commands command, byte[] request, byte conversationId = 0)
        {
            try
            {
                return m_Logger.GenericCommand(conversationId, (byte)command, request);
            }
            catch (COMException ex)
            {
                var newException = UsbErrorToException(ex);
                if (newException != null)
                {
                    throw newException;
                }
                throw;
            }
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
                return LoggerTypeToDeviceType(m_Logger.LoggerType);
            }
        }

        protected static DeviceType LoggerTypeToDeviceType(int deviceType)
        {
            switch (deviceType)
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
                m_Logger.GetCalibrationData(0, 2, ref result);
                return DateTime.FromOADate((double)result);
            }
        }
    }
}
