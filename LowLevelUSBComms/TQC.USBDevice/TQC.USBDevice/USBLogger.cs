using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;
using TQC.IF.USBLogger;
using TQC.USBDevice.Exceptions;
using TQC.USBDevice.Utils;

namespace TQC.USBDevice
{
    public enum ProbeType
    {
        Gloss = 0,
        Temperature = 1,
        Humidity = 2,
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
        ThermocoupleSimulator = 9,
    }
    public class USBLogger : IDisposable
    {
        const int InvalidHandle = -1;
        int m_Handle;
        
        private ILog m_Log = LogManager.GetLogger("TQC.USBDevice.USBLogger");

        public enum USBProductId
        {
            Glossmeter = 0x1b4cFF00,
            USB_PRODUCT2 = 0x15CD0011,
            USB_CURVEX_3 = 0x2047FFFE,
            USB_CURVEX_3a = 0x20470827,
            GRADIENT_OVEN = 0x04037B60,
            USB_THERMOCOUPLE_SIMULATOR = 0x20470828,

        }
        USBProductId m_ProductId;
        private USBGeneric m_Logger;
        private USBCommunication m_UsbCommunications;
        bool m_bUseTQCCommunications;

        const string c_COMObjectFileName = "USBGenericLogger.dll";

        internal USBLogger(IUsbInterfaceForm mainWinForm)
        {
            Configuration config = new Configuration();
            m_Handle = InvalidHandle;
            m_Logger = new USBGeneric();
            m_UsbCommunications = new USBCommunication(mainWinForm, this);
            m_bUseTQCCommunications = config.UseNativeCommunication;

            m_Log.Info(m_bUseTQCCommunications ?
                "Using COM wrapper to speak to device" :
                "Using .NET wrapper to speak to device");

            var compatibleVersion = new Version(6, 0, 49, 0);
            if (COMObjectVersion < compatibleVersion)
            {
                throw new ApplicationException(string.Format("Underlying COM object {0} too old (Found Version {1}). Update Ideal Finish Analysis or Calibration software to {2}", c_COMObjectFileName, COMObjectVersion, compatibleVersion));
            }
        }
        

        ~USBLogger()
        {
            Dispose(false);
        }


        public void OnWindowsMessage(ref System.Windows.Forms.Message m)
        {
            m_UsbCommunications.OnWindowsMessage(ref m);
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

        /// <summary>
        /// 
        /// Get or sets the number of days that the debug output is stored for.
        /// The default is 2 days. 
        /// </summary>
        public int DebugPurgePolicy
        {
            get
            {
                int resultVal = -1;
                object result = null;
                if (m_bUseTQCCommunications)
                {
                    m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 70205, ref result);
                }
                string val = result as string;
                int.TryParse(val, out resultVal);
                return resultVal;
            }
            set
            {
                if (m_bUseTQCCommunications)
                {
                    object result = value.ToString();
                    m_Logger.GetCalibrationDataByHandle(m_Handle, -1001, 70205, ref result);
                }

            }
        }

        /// <summary>
        /// gets/sets the base of file name that the debug information is stored in.
        /// </summary>
        public string DebugFileNameBase
        {
            get
            {
                object result = null;
                if (m_bUseTQCCommunications)
                {
                    m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 70204, ref result);
                }
                string val = result as string;
                return val;
            }
            set
            {
                object result = value;
                if (m_bUseTQCCommunications)
                {
                    m_Logger.GetCalibrationDataByHandle(m_Handle, -1001, 70204, ref result);
                }
            }
        }

        /// <summary>
        /// gets the file name that the debug information is stored in.
        /// </summary>
        public string DebugFileName
        {
            get
            {
                object result = null;
                if (m_bUseTQCCommunications)
                {
                    m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 70206, ref result);
                }
                string val = result as string;
                return val;
            }
        }

        /// <summary>
        /// Opens the debug file 
        /// Nb. the logger needs to be connected and open.
        /// </summary>
        /// <param name="fileName">The base name of the file. If no file is given then the default is used</param>
        /// <returns>If the debug file is open (true => open)</returns>
        public bool DebugOpen(string fileNameRoot = null)
        {
            m_Log.Info("DebugOpen");
            IsDebugOutputOpen = false;
            if (!String.IsNullOrEmpty(fileNameRoot))
            {
                DebugFileNameBase = fileNameRoot;
            }
            IsDebugOutputOpen = true;
            return IsDebugOutputOpen;
        }

        /// <summary>
        /// Closes the debug file.
        /// </summary>
        /// <returns>if the debug file is open or closed (false => Closed)</returns>
        public bool DebugClose()
        {
            m_Log.Info("DebugClose");
            IsDebugOutputOpen = false;
            return IsDebugOutputOpen;
        }
        

        public bool IsDebugOutputOpen
        {
            get
            {
                object result = null;
                if (m_bUseTQCCommunications)
                {
                    m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 70201, ref result);
                    string val = result as string;
                    switch (val)
                    {
                        case "TRUE":
                            return true;
                        case "FALSE":
                            return false;
                        default:
                            throw new Exception("Not valid response");
                    }
                }
                return false;
            }
            private set
            {
                object result = null;
                if (m_bUseTQCCommunications)
                {
                    m_Logger.GetCalibrationDataByHandle(m_Handle, 0, value ? 70203 : 70202, ref result);
                }
            }
        }

        public string DebugOutputFromPreviousCommand
        {
            get
            {
                object result = null;
                if (m_bUseTQCCommunications)
                {
                    m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 70200, ref result);
                }
                return result as string;
            }
        }

        public bool OpenWithMinumumRequests(USBProductId id)
        {
            return Open(id, true);
        }

        public bool Open(USBProductId id, bool minimumCommunications = false)
        {
            m_ProductId = id;
            m_Log.Info(string.Format("Open {0} {1}", id, minimumCommunications));
            if (m_bUseTQCCommunications)
            {
                return OpenCom(id, minimumCommunications, null);
            }
            else
            {
                return m_UsbCommunications.Open(id, minimumCommunications);
            }
        }

        private bool OpenCom(USBProductId id, bool minimumCommunications, string portName)
        {
            m_Handle = InvalidHandle;
            
            try
            {
                m_Handle = m_Logger.OpenAndReturnHandle(minimumCommunications ? 2 : 0, 0, 0, portName, (uint)id);
                ClearCachedData();
            }
            catch (COMException ex)
            {
                m_Log.Error(string.Format("Failed to open logger {0} ", id.ToString()), ex);
            }
            return m_Handle >= 0;
        }



        public bool Open(USBProductId id, string portName, bool minimumCommunications = false)
        {
            m_ProductId = id;
            if (m_bUseTQCCommunications)
            {
                return OpenCom(id, minimumCommunications, portName);
            }
            else
            {
                return m_UsbCommunications.Open(id, minimumCommunications, portName);
            }            
        }

        public bool CanLoggerBeConfigured
        {
            get
            {
                if (Environment.UserName == "Windows7" || (Environment.UserDomainName.ToLower() == "tqc"))
                {
                    return true;
                }
                return false;
            }
        }

        public void Close()
        {
            m_Log.Info("Close");
            ClearCachedData();
            if (m_bUseTQCCommunications)
            {
                if (m_Handle != InvalidHandle)
                {
                    m_Logger.CloseByHandle(m_Handle);
                }
                m_Handle = InvalidHandle;
            }
            else
            {
                m_UsbCommunications.Close();
            }
        }

        internal enum Commands
        {
            ReadDeviceInfo = 0x01,
            WriteDeviceInfo = 0x11,
            ReadCalibrationDetails = 0x02,
            WriteCalibrationDetails = 0x12,
            ReadLoggedInfo = 0x03,
            OffloadData = 0x04,
            ReadCurrentProbeVals = 0x05,
            ReadCurrentRawProbeVals = 0x06,
            WriteSetup = 0x17,
            LoggerSpecificCommand=0x43,
            GROSetCommand = 0x60,
            GROReadCommand = 0x61,
            NotValidCommand = 0x7F,
            BounceCommand=0xFF,
        }

        public enum USBCommandResponseCode
        {
            /// <summary>
            /// Errors given back by the logger itself
            /// </summary>
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

            /// <summary>
            /// Errors generated by the PC code in response to a general error in reading the response packet...
            /// </summary>
            ResponsePacketErrorCRC,
            ResponsePacketErrorTimeout,
            ResponsePacketErrorBadLength,
            ResponsePacketErrorBadCommand,
            ResponseUsbDeviceRemoved,
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
        const int ERROR_CRC = (int)23L;
        const int ERROR_BAD_LENGTH = (int)24L;
        const int ERROR_BAD_COMMAND = (int)22L;
        const int ERROR_TIMEOUT = unchecked((int)(0x8003001EL));
        const int USB_DISCONNECTED = unchecked((int)(0x80040004L));



        private static DeviceResponseUnexpectedException UsbErrorToException(COMException ex)
        {
            switch (ex.ErrorCode)
            {
                case DISP_E_MEMBERNOTFOUND:
                    return new DeviceUnknownErrorException();

                case STG_E_DOCFILECORRUPT:
                    return new CommandCorruptException();

                case STG_E_BADBASEADDRESS:
                    return new CommandOutOfSequenceException();

                case RPC_E_UNEXPECTED:
                    return new CommandUnexpectedException();

                case STG_E_INUSE:
                    return new DeviceBusyException();

                case CO_E_INCOMPATIBLESTREAMVERSION:
                    return new CommandNotSuportedException();

                case SEC_E_UNSUPPORTED_FUNCTION:
                    return new EnumerationNotSuportedException();

                case SPAPI_E_MACHINE_UNAVAILABLE:
                    return new BatchNotAvailableException();

                case OSS_OUT_OF_RANGE:
                    return new DataOutOfRangeException();

                case CO_E_FAILEDTOOPENPROCESSTOKEN:
                    return new CommandModeNotSupportedException();

                case ERROR_CRC:
                    return new ResponsePacketErrorCRCException();

                case ERROR_BAD_LENGTH:
                    return new ResponsePacketErrorBadLengthException();

                case ERROR_BAD_COMMAND:
                    return new ResponsePacketErrorBadCommandException();

                case ERROR_TIMEOUT:
                    return new ResponsePacketErrorTimeoutException();
                case USB_DISCONNECTED:
                    return new UsbDisconnectedException();

                default:
                    return null;
            }
        }
        const int MAX_RETRY_ATTEMPTS = 5;
        internal byte[] Request(Commands command, byte[] request, byte conversationId = 0)
        {
            bool retry;
            int attempts = MAX_RETRY_ATTEMPTS;
            if (request != null && request.Length >= 2)
            {
                var id = BitConverter.ToInt16(request, 0);
                m_Log.Info(string.Format("Request {0}/{1} to {2}", command, id, conversationId));
            }
            else
            {
                m_Log.Info(string.Format("Request {0} to {1}", command, conversationId));
            }
            do
            {
                retry = false;
                try
                {
                    return IssueRequest(command, request, conversationId);
                }
                catch (COMException ex)
                {
                    var newException = UsbErrorToException(ex);

                    if (newException != null)
                    {
                        if (attempts != 0 && (newException.GetType() == typeof(ResponsePacketErrorTimeoutException)))
                        {
                            attempts = attempts - 1;
                            retry = true;
                        }
                        else
                        {
                            m_Log.Info("Known COM exception in Request", newException);
                            throw newException;
                        }
                    }
                    else
                    {
                        m_Log.Error("Unknown COM exception in Request", ex);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    m_Log.Error("Unknown exception in Request", ex);
                    throw;
                }
            }
            while (retry);
            return null;
        }



        private byte[] IssueRequest(Commands command, byte[] request, byte conversationId)
        {
            if (m_bUseTQCCommunications)
            {
                return m_Logger.GenericCommandByHandle(m_Handle, conversationId, (byte)command, request) as byte[];
            }
            else
            {
                return m_UsbCommunications.IssueRequest(command, request, conversationId);
            }
        }


        virtual internal Version _SoftwareVersion(byte deviceId)
        {
            return new Version();
        }


        public string Version
        {
            get
            {
                if (m_bUseTQCCommunications)
                {
                    return m_Logger.strVersionByHandle(m_Handle);
                }
                else
                {
                    return _SoftwareVersion(0).ToString() ;
                }

            }
        }

        virtual internal Int32 _SerialNumber(byte deviceId)
        {
            return -1;
        }

        public string LoggerSerialNumber
        {
            get
            {
                if (m_bUseTQCCommunications)
                {
                    return m_Logger.LoggerIdByHandle(m_Handle);
                }
                else
                {
                    return _SerialNumber(0).ToString();
                }
            }
        }

        internal virtual DeviceType _DeviceType(byte deviceId)
        {
            return DeviceType.Unknown;
        }

        public DeviceType LoggerType
        {
            get
            {
                if (m_bUseTQCCommunications)
                {
                    return LoggerTypeToDeviceType(m_Logger.LoggerTypeByHandle(m_Handle));
                }
                else
                {
                    return _DeviceType(0);
                }

            }
        }
        public static string DeviceTypeToString(DeviceType type)
        {
            switch (type)
            {
                case DeviceType.PolyGlossmeter:
                    return "PolyGloss";
                case DeviceType.SoloGlossmeter:
                    return "SoloGloss";
                case DeviceType.DuoGlossmeter:
                    return "DuoGloss";
                case DeviceType.CurveX3_Basic:
                case DeviceType.CurveX3_High:
                case DeviceType.CurveX3_Mid:
                    return "CurveX3";
                case DeviceType.GRO:
                    return "GRO";
                case DeviceType.ThermocoupleSimulator:
                    return "TCT";
                case DeviceType.Unknown:
                    return "****";
                default:
                    return "????";
            }
        }

        protected static DeviceType LoggerTypeToDeviceType(Int16 deviceType)
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
                case 9: return DeviceType.ThermocoupleSimulator;
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
                m_UsbCommunications.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        virtual internal String _CalibrationCompany(byte deviceId)
        {
            return null;
        }

        public string CalibrationCompany
        {
            get
            {
                if (m_bUseTQCCommunications)
                {
                    object result = null;
                    m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 0, ref result);
                    return result as string;
                }
                else
                {
                    return _CalibrationCompany(0);
                }

            }
            set
            {
                if (CanLoggerBeConfigured)
                {
                    if (m_bUseTQCCommunications)
                    {
                        object result = value;
                        m_Logger.GetCalibrationDataByHandle(m_Handle, -1001, 0, ref result);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new LoggerCannotBeConfiguredException("Unable to set Calibration Company");
                }
            }
        }

        internal virtual String _CalibrationUserName(byte deviceId)
        {
            return null;
        }

        public string CalibrationUser
        {
            get
            {
                if (m_bUseTQCCommunications)
                {
                    object result = null;
                    m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 1, ref result);
                    return result as string;
                }
                else
                {
                    return _CalibrationUserName(0);
                }
            }
            set
            {
                if (CanLoggerBeConfigured)
                {
                    if (m_bUseTQCCommunications)
                    {
                        object result = value;
                        m_Logger.GetCalibrationDataByHandle(m_Handle, -1001, 1, ref result);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new LoggerCannotBeConfiguredException("Unable to set Calibration User");
                }
            }
        }

        internal virtual DateTime _Calibration(byte deviceId)
        {
            return DateTime.MinValue;
        }

        public DateTime CalibrationDate
        {
            get
            {
                if (m_bUseTQCCommunications)
                {
                    object result = null;
                    m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 2, ref result);
                    return DateTime.FromOADate((double)result);
                }
                else
                {
                    return _Calibration(0);
                }
            }
            set
            {
                if (CanLoggerBeConfigured)
                {
                    if (m_bUseTQCCommunications)
                    {
                        object result = value.ToOADate();
                        m_Logger.GetCalibrationDataByHandle(m_Handle, -1001, 2, ref result);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new LoggerCannotBeConfiguredException("Unable to set Calibration Date");
                }
            }
        }

        public Version DotNetDLLVersion
        {
            get
            {                
                var ass = Assembly.GetAssembly(typeof(USBGeneric));
                var version = ass.GetName().Version;                
                return version;
            }
        }

        public Version COMObjectVersion
        {
            get
            {
                
                var myProcess = Process.GetCurrentProcess();
                foreach (ProcessModule processModule in myProcess.Modules)
                {
                    if (processModule.FileName.EndsWith(c_COMObjectFileName))
                    {
                        return FileVersionToVersion(processModule.FileVersionInfo);
                    }
                    
                }
                //Just incase somethings gone wrong!
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(@"C:\Program Files (x86)\Common Files\Ideal Finish\", c_COMObjectFileName));
                return FileVersionToVersion(myFileVersionInfo);

            }
        }

        private static Version FileVersionToVersion(FileVersionInfo myFileVersionInfo)
        {
            return new Version(myFileVersionInfo.FileMajorPart, myFileVersionInfo.FileMinorPart, myFileVersionInfo.FileBuildPart, myFileVersionInfo.FilePrivatePart);
        }

        internal bool IsCurvex3
        {
            get
            {
                return (m_ProductId == USBProductId.USB_CURVEX_3) || (m_ProductId == USBProductId.USB_CURVEX_3a); 
            }
        }

        internal bool IsThermocoupleSimulator
        {
            get
            {
                return (m_ProductId == USBProductId.USB_THERMOCOUPLE_SIMULATOR);
            }
        }

        public bool IsGlossmeter
        {
            get
            {
                return (m_ProductId == USBProductId.Glossmeter);
            }
        }

        public bool IsGRO
        {
            get
            {
                return (m_ProductId == USBProductId.GRADIENT_OVEN);
            }
        }

    }
}
