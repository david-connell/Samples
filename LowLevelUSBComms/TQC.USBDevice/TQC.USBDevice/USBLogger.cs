using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using TQC.IF.USBLogger;

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

        public enum USBProductId
        {
            Glossmeter = 0x1b4cFF00,
            USB_PRODUCT2 = 0x15CD0011,
            USB_CURVEX_3 = 0x2047FFFE,
            USB_CURVEX_3a = 0x20470827,
            GRADIENT_OVEN = 0x04037B60,
            USB_THERMOCOUPLE_SIMULATOR = 0x20470828,

        }
        private USBGeneric m_Logger;

        const string c_COMObjectFileName = "USBGenericLogger.dll";

        internal USBLogger()
        {
            m_Handle = InvalidHandle;
            m_Logger = new USBGeneric();
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
        /// Get or sets the number of days that the debug output is stored for.
        /// The default is 2 days. 
        /// </summary>
        public int DebugPurgePolicy
        {
            get
            {
                int resultVal = -1;
                object result = null;
                m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 70205, ref result);
                string val = result as string;
                int.TryParse(val, out resultVal);
                return resultVal;
            }
            set
            {
                object result = value.ToString();
                m_Logger.GetCalibrationDataByHandle(m_Handle, -1001, 70205, ref result);

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
                m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 70204, ref result);
                string val = result as string;
                return val;
            }
            set
            {
                object result = value;
                m_Logger.GetCalibrationDataByHandle(m_Handle, -1001, 70204, ref result);                                
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
                m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 70206, ref result);
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
            IsDebugOutputOpen = false;
            return IsDebugOutputOpen;
        }

        public bool IsDebugOutputOpen
        {
            get
            {
                object result = null;
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
            private set
            {
                object result = null;
                m_Logger.GetCalibrationDataByHandle(m_Handle, 0, value ? 70203 : 70202, ref result);
            }
        }

        public string DebugOutputFromPreviousCommand
        {
            get
            {
                object result = null;
                m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 70200, ref result);
                return result as string;
            }
        }

        public bool OpenWithMinumumRequests(USBProductId id)
        {
            return Open(id, true);
        }

        public bool Open(USBProductId id, bool minimumCommunications = false)
        {
            m_Handle = InvalidHandle;
            try
            {
                m_Handle = m_Logger.OpenAndReturnHandle(minimumCommunications ? 2 : 0, 0, 0, null, (uint)id);
                ClearCachedData();
            }
            catch (COMException ex)
            {
                Console.WriteLine("Failed to open logger {0} - '{1}'", id.ToString(), ex.Message);
            }
            return m_Handle >= 0;
        }
        public bool Open(USBProductId id, string portName, bool minimumCommunications = false)
        {
            m_Handle = InvalidHandle;
            try
            {
                m_Handle = m_Logger.OpenAndReturnHandle(minimumCommunications ? 2 : 0, 0, 0, portName, (uint)id);
                ClearCachedData();
            }
            catch (COMException ex)
            {
                Console.WriteLine("Failed to open logger {0} - '{1}'", id.ToString(), ex.Message);
            }
            return m_Handle >= 0;
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
            if (m_Handle != InvalidHandle)
            {
                m_Logger.CloseByHandle(m_Handle);
            }
            m_Handle = InvalidHandle;

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

                default:
                    return null;
            }
        }
        const int MAX_RETRY_ATTEMPTS = 5;
        internal byte[] Request(Commands command, byte[] request, byte conversationId = 0)
        {
            bool retry;
            int attempts = MAX_RETRY_ATTEMPTS;

            do
            {
                retry = false;
                try
                {
                    return m_Logger.GenericCommandByHandle(m_Handle, conversationId, (byte)command, request) as byte[];                    
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
                            throw newException;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            while (retry);
            return null;
        }

        public string Version
        {
            get
            {
                return m_Logger.strVersionByHandle(m_Handle);
            }
        }

        public string LoggerSerialNumber
        {
            get
            {
                return m_Logger.LoggerIdByHandle(m_Handle);
            }
        }

        public DeviceType LoggerType
        {
            get
            {
                return LoggerTypeToDeviceType(m_Logger.LoggerTypeByHandle(m_Handle));
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
                GC.SuppressFinalize(this);
            }
        }

        public string CalibrationCompany
        {
            get
            {
                object result = null;
                m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 0, ref result);
                return result as string;
            }
            set
            {
                if (CanLoggerBeConfigured)
                {
                    object result = value;
                    m_Logger.GetCalibrationDataByHandle(m_Handle, -1001, 0, ref result);
                }

            }

        }


        public string CalibrationUser
        {
            get
            {
                object result = null;
                m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 1, ref result);
                return result as string;
            }
            set
            {
                if (CanLoggerBeConfigured)
                {
                    object result = value;
                    m_Logger.GetCalibrationDataByHandle(m_Handle, -1001, 1, ref result);
                }

            }

        }

        public DateTime CalibrationDate
        {
            get
            {
                object result = null;
                m_Logger.GetCalibrationDataByHandle(m_Handle, 0, 2, ref result);
                return DateTime.FromOADate((double)result);
            }
            set
            {
                if (CanLoggerBeConfigured)
                {
                    object result = value.ToOADate();
                    m_Logger.GetCalibrationDataByHandle(m_Handle, -1001, 2, ref result);
                }
            }
        }

        public Version DotNetDLLVersion
        {
            get
            {                
                var ass = Assembly.GetAssembly(typeof(USBGeneric));
                var version = ass.GetName().Version;
                
                //foreach(var module in ass.GetLoadedModules(false))
                //{
                //    //Console.WriteLine("*** {0}", module.Name);
                //    version = module.Assembly.GetName().Version;
                //}
                return version;
                //m_Logger.
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
    }
}
