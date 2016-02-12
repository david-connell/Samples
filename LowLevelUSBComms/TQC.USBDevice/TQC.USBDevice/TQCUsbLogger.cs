using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TQC.USBDevice
{
    public enum StateOfLogger : byte
    {
        Idle = 0,
        Logging=1,
        Calibration=3,
    }
    //Extend here with request responses that are not covered by the standard COM calls.
    public class TQCUsbLogger : USBLogger, ISimpleTQCDevice
    {
        Dictionary<byte, int> ProbesPerDevice = new Dictionary<byte, int>();
        Dictionary<byte, CachedData> m_CachedData = new Dictionary<byte, CachedData>();

        public TQCUsbLogger(IUsbInterfaceForm mainWinForm)
            : base(mainWinForm)
        {
        }
        private string CommandDescription(Commands command, int commandId)
        {
            return string.Format("{0} sub command {1}", command.ToString(), commandId);
        }

        
        private int GetInt8(byte deviceId, Commands command, int commandId)
        {                            
            var response = GetResponse(deviceId, command, commandId);
            if (response == null)
            {
                throw new NoDataReceivedException(CommandDescription(command, commandId));
            }
            if (response.Length < sizeof(byte))
            {
                throw new TooLittleDataReceivedException(CommandDescription(command, commandId), response.Length, sizeof(byte));
            }
            return response[0];
        }

        private int GetInt32(byte deviceId, Commands command, int commandId)
        {
            var response = GetResponse(deviceId, command, commandId);
            if (response == null)
            {
                throw new NoDataReceivedException(CommandDescription(command, commandId));
            }
            if (response.Length < sizeof(Int32))
            {
                throw new TooLittleDataReceivedException(CommandDescription(command, commandId), response.Length, sizeof(Int32));
            }
            return BitConverter.ToInt32(response, 0);
        }

        private Int16 GetInt16(byte deviceId, Commands command, int commandId)
        {
            var response = GetResponse(deviceId, command, commandId);
            if (response == null)
            {
                throw new NoDataReceivedException(CommandDescription(command, commandId));
            }
            if (response.Length < sizeof(Int16))
            {
                throw new TooLittleDataReceivedException(CommandDescription(command, commandId), response.Length, sizeof(Int16));
            }

            return BitConverter.ToInt16(response, 0);
        }


        internal byte[] GetResponse(byte deviceId, Commands command, int commandId)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes((short)commandId));
            var response = Request(command, request.ToArray(), deviceId);
            return response;
        }

        internal byte[] GetResponse(byte deviceId, Commands command, int commandId, List<byte> request)
        {
            List<byte> requestFull = new List<byte>();
            requestFull.AddRange(BitConverter.GetBytes((short)commandId));
            if (request != null)
            {
                requestFull.AddRange(request);
            }
            var response = Request(command, requestFull.ToArray(), deviceId);
            return response;
        }


        protected byte[] GetProbeValues(byte deviceId, byte mode, byte setid)
        {
            List<byte> request = new List<byte>();
            request.Add(mode);
            request.Add(setid);
            var response = Request(Commands.ReadCurrentProbeVals, request.ToArray(), deviceId);

            if (response == null)
            {
                throw new NoDataReceivedException(string.Format("GetProbeValues {0} {1} {2}", deviceId, mode, setid));
            }
            return response;
        }

        protected override void ClearCachedData()
        {
            ProbesPerDevice = new Dictionary<byte, int>();
            m_CachedData = new Dictionary<byte, CachedData>();
        }

        public Int32 SerialNumber
        {
            get
            {
                return _SerialNumber(0);
            }
            set
            {     
                if (CanLoggerBeConfigured)           
                    _SetSerialNumber(0, value);
            }
        }

        public class BoardStatus
        {
            private readonly IList<byte> m_AdditionalVals = new List<byte>();

            public UInt32 Status { get; private set;}



            public byte AdditionalValues(int id)
            {
                byte result = 0;
                if (id >= 0 && id <= m_AdditionalVals.Count)
                {
                    result = m_AdditionalVals[id];
                }
                return result;
            }
            internal BoardStatus(UInt32 status, byte[] data, int offset)
            {
                Status = status;
                for (; offset < data.Length; offset++)
                {
                    m_AdditionalVals.Add(data[offset]);
                }
            }
        }
        internal BoardStatus _GetStatus(byte deviceId)
        {
            var response = GetResponse(deviceId, Commands.GROReadCommand, 0x09);
            if (response == null)
            {
                throw new NoDataReceivedException("getHarwareStatus");
            }
            if (response.Length < sizeof(UInt32))
            {
                throw new TooLittleDataReceivedException("getHarwareStatus", response.Length, sizeof(UInt32));
            }
            UInt32 status = BitConverter.ToUInt32(response, 0);
            return new BoardStatus(status, response, 4);
        }


        override internal Int32 _SerialNumber(byte deviceId)
        {
            return GetInt32(deviceId, Commands.ReadDeviceInfo, 0);
        }

        internal void WriteDeviceInfo(byte deviceId, int enumerationId, string value, int maxLength)
        {
            if (CanLoggerBeConfigured)
            {
                List<byte> request = new List<byte>();
                string result = value;
                if (value.Length > maxLength)
                    result = value.Substring(0, maxLength);
                //request.AddRange(BitConverter.GetBytes((byte)value.Length));
                request.AddRange(System.Text.ASCIIEncoding.Default.GetBytes(result));
                for (int counter = result.Length; counter < maxLength; counter++)
                    request.Add(0);
                GetResponse(deviceId, Commands.WriteDeviceInfo, enumerationId, request);
            }
        }

        internal void WriteDeviceInfo(byte deviceId, int enumerationId, Int32 value)
        {
            if (CanLoggerBeConfigured)
            {
                List<byte> request = new List<byte>();                
                request.AddRange(BitConverter.GetBytes(value));
                GetResponse(deviceId, Commands.WriteDeviceInfo, enumerationId, request);
            }
        }

        internal void WriteDeviceInfo(byte deviceId, int enumerationId, Version version)
        {
            if (CanLoggerBeConfigured)
            {
                List<byte> request = new List<byte>();                
                request.Add((byte)version.Revision);
                request.Add((byte)version.Build);
                request.Add((byte)version.Minor);
                request.Add((byte)version.Major);
                //request.AddRange(BitConverter.GetBytes(version));
                GetResponse(deviceId, Commands.WriteDeviceInfo, enumerationId, request);
            }
        }

        internal void _SetSerialNumber(byte deviceId, Int32 serialNumber)
        {
            if (CanLoggerBeConfigured)
            {
                List<byte> request = new List<byte>();
                request.AddRange(BitConverter.GetBytes(serialNumber));
                var result = GetResponse(deviceId, Commands.WriteDeviceInfo, 0, request);
            }
            else
            {
                throw new UnauthorizedAccessException("Cannot set serial number");
            }
        }


        override internal Version _SoftwareVersion(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadDeviceInfo, 1);
            if (result.Length < 4)
            {
                throw new TooLittleDataReceivedException("Read Software Version", result.Length, 4);
            }
            return new Version(result[3], result[2], result[1], result[0]);
        }

        

        public Version HardwareVersion
        {
            get
            {
                return _HardwareVersion(0);
            }
            set
            {                
                WriteDeviceInfo(0, 2, value);
            }
        }

        internal Version _HardwareVersion(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadDeviceInfo, 2);
            if (result.Length < 4)
            {
                throw new TooLittleDataReceivedException("Read Hardware Version", result.Length, 4);
            }
            return new Version(result[3], result[2], result[1], result[0]);
        }



        public Version ProtocolVersion
        {
            get
            {
                return _ProtocolVersion(0);
            }
        }

        internal Version _ProtocolVersion(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadDeviceInfo, 100);
            if (result.Length < 2)
            {
                throw new TooLittleDataReceivedException("Read Protocol Version", result.Length, 2);
            }

            return new Version(result[0], result[1]);
        }

        internal string ReadDeviceInfoAsString(byte deviceId, int enumerationId)
        {
            try
            {
                var result = GetResponse(deviceId, Commands.ReadDeviceInfo, enumerationId);
                return DecodeString(result);
            }
            catch
            {
                // ignored
            }
            return "";
        }

        internal string _DeviceName(byte deviceId)
        {
            return ReadDeviceInfoAsString(deviceId, 3);
        }

        public string DeviceName
        {
            get
            {
                return _DeviceName(0);
            }
            set
            {
                WriteDeviceInfo(0, 3, value, 16);
            }
        }

        public byte[] BounceCommand(byte deviceId, int command, byte [] bytes)
        {
            var result = GetResponse(deviceId, Commands.BounceCommand, command, new List<byte>(bytes));

            return result;
        }
        internal string _ManufactureName(byte deviceId)
        {
            return ReadDeviceInfoAsString(deviceId, 4);            
        }

        internal static string DecodeString(byte[] result)
        {
            var textResult = Encoding.UTF8.GetString(result, 0, result.Length);
            int location = textResult.IndexOf('\0');
            if (location >= 0)
            {
                textResult = textResult.Substring(0, location);
            }
            return textResult.Replace('�', ' ').Trim();
        }

        public string ManufactureName
        {
            get
            {
                return _ManufactureName(0);
            }
            set
            {
                WriteDeviceInfo(0, 4, value, 16);
            }
        }


        internal DateTime _ManufactureDate(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadDeviceInfo, 5);
            return ResultToDateTime("Read Manufacturing Date", result, 0);            
        }

        private static DateTime ResultToDateTime(string description, byte[] result, int offset)
        {
            if (result.Length < offset+sizeof(UInt32) )
            {
                throw new TooLittleDataReceivedException(description, result.Length, offset + sizeof(UInt32));
            }

            var unixTimeStamp = BitConverter.ToInt32(result, offset);

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public DateTime ManufactureDate
        {
            get
            {
                return _ManufactureDate(0);
            }
            set
            {
                Int32 numberOfSecs;
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                var span = value - dtDateTime;
                numberOfSecs = (Int32) span.TotalSeconds;
                WriteDeviceInfo(0, 5, numberOfSecs);
            }
        }


        public Version SoftwareVersion
        {
            get
            {
                return _SoftwareVersion(0);
            }
            set
            {
                WriteDeviceInfo(0, 1, value);
            }
        }

        public virtual Int32 NumberOfProbes
        {
            get
            {
                return _NumberOfProbes(0);
            }
        }
        public virtual Int32 NumberOfBatches
        {
            get
            {
                var result = GetResponse(0, Commands.ReadDeviceInfo, 7);
                return result[0];
            }
        }

        internal Int32 _NumberOfProbes(byte deviceId)
        {
            if (!ProbesPerDevice.ContainsKey(deviceId))
            {
                ProbesPerDevice[deviceId] = GetInt8(deviceId, Commands.ReadDeviceInfo, 11);
            }
            return ProbesPerDevice[deviceId];
        }

        internal LinearCalibrationDetails _Calibration(byte deviceId, int probeId)
        {
            if (probeId >= 0 && probeId < _NumberOfProbes(deviceId))
            {
                var result = GetResponse(deviceId, Commands.ReadCalibrationDetails, 20 + probeId);
                if (result == null)
                {
                    throw new NoDataReceivedException("getCalibration");
                }
                if (result.Length < sizeof(Single)*2)
                {
                    throw new TooLittleDataReceivedException("getCalibration", result.Length, sizeof(Single) * 2);
                }

                return new LinearCalibrationDetails(BitConverter.ToSingle(result, 0), BitConverter.ToSingle(result,4));
            }
            else
            {
                throw new IndexOutOfRangeException("probeId");
            }
        }

        internal void _SetCalibration(byte deviceId, int probeId, LinearCalibrationDetails newValue)
        {
            if (probeId >= 0 && probeId < _NumberOfProbes(deviceId))
            {
                if (CanLoggerBeConfigured)
                {
                    List<byte> request = new List<byte>();
                    request.AddRange(BitConverter.GetBytes((float)newValue.M));
                    request.AddRange(BitConverter.GetBytes((float)newValue.C));
                                                            
                    var result = GetResponse(deviceId, Commands.WriteCalibrationDetails, 20 + probeId, request);
                    if (result != null)
                    {
                        throw new NoDataReceivedException("setCalibration");
                    }
                }

                return; 
            }
            else
            {
                throw new IndexOutOfRangeException("probeId");
            }
        }

        public virtual void SetCalibrationDetails(int probeId, LinearCalibrationDetails details)
        {
            _SetCalibration(0, probeId, details);
        }

        public virtual LinearCalibrationDetails CalibrationDetails(int probeId)
        {
            return _Calibration(0, probeId);
        }

        internal string _ProbeName(byte deviceId, int probeId)
        {
            if (probeId >= 0 && probeId < _NumberOfProbes(deviceId))
            {
                var result = GetResponse(deviceId, Commands.ReadCalibrationDetails, 200 + probeId);
                var probeName =  result == null ? "***" : DecodeString(result);
                                
                return probeName;
            }
            else
            {
                throw new IndexOutOfRangeException("probeId");
            }
        }

        public virtual string ProbeName(int probeId)
        {
            return _ProbeName(0, probeId);            
        }

        internal ProbeType _ProbeType(byte deviceId, int probeId)
        {
            if (probeId >= 0 && probeId < _NumberOfProbes(deviceId))
            {
                var result = GetResponse(deviceId, Commands.ReadCalibrationDetails, 100 + probeId);
                if (result == null)
                {
                    throw new NoDataReceivedException("_ProbeType");
                }
                if (result.Length < sizeof(byte) )
                {
                    throw new TooLittleDataReceivedException("_ProbeType", result.Length, sizeof(byte) );
                }
                return (ProbeType)result[0];
            }
            else
            {
                throw new IndexOutOfRangeException("ProbeId");
            }
        }
        
        public virtual ProbeType ProbeType(int probeId)
        {            
            return _ProbeType(0, probeId);
        }
        
        public DeviceType DeviceType
        {
            get
            {
                return _DeviceType(0);
            }
        }

        /// <summary>
        /// This is not supported by the GRO!
        /// </summary>
        public DateTime Calibration
        {
            get
            {
                return _Calibration(0);
            }
        }

        internal virtual DateTime _Calibration(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadCalibrationDetails, 0);
            return ResultToDateTime("Calibration Date", result, 0);
        }

        /// <summary>
        /// This is not supported by the GRO!
        /// </summary>
        public new String CalibrationCompany
        {
            get
            {
                return _CalibrationCompany(0);
            }
        }

        internal override String _CalibrationCompany(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadCalibrationDetails, 1);
            return result == null ? "" : DecodeString(result);
        }
        /// <summary>
        /// This is not supported by the GRO!
        /// </summary>
        public String CalibrationUserName
        {
            get
            {
                return _CalibrationUserName(0);
            }
        }

        internal override String _CalibrationUserName(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadCalibrationDetails, 2);

            return result == null ? "": DecodeString(result);
        }

        internal override DeviceType _DeviceType(byte deviceId)
        {
            return LoggerTypeToDeviceType(GetInt16(deviceId, Commands.ReadDeviceInfo, 102));
        }


        /// <summary>
        /// This is not valid for GRO
        /// </summary>
        public IEnumerable<double> ColdJunctions
        {
            get
            {
                return _ColdJunctions(0);
            }
        }

        internal IEnumerable<double> _ColdJunctions(byte deviceId)
        {
            List<double> data = new List<double>();
            int totalNumberOfProbes = _NumberOfProbes(deviceId);
            int maxNumberOfSets = 1;
            for (int setId = 1; setId <= maxNumberOfSets; setId++)
            {
                var result = GetProbeValues(deviceId, 0x06, (byte)setId);
                
                if (result != null)
                {
                    const int lengthOfData = sizeof(float);
                    const int startOffset = 6;

                    for (int i = 0; i < (result.Length - startOffset) / lengthOfData; i++)
                    {
                        double value = BitConverter.ToSingle(result, startOffset + i * lengthOfData);
                        data.Add(value);
                    }
                }
            }
            return data;                        
        }

        public void UserInterfaceStatus(out byte buttonStatus, out Int32 status)
        {
            buttonStatus = 0;
            status = 0;

            var response = GetProbeValues(0, 0x30, 0);
            if (response == null)
            {
                throw new NoDataReceivedException("UserInterfaceStatus");
            }
            if (response.Length < 7)
            {
                throw new TooLittleDataReceivedException("UserInterfaceStatus", response.Length, 7);
            }
            //if (response != null && response.Length == 7)
            {
                buttonStatus = response[2];
                status = BitConverter.ToInt32(response, 3);
            }
            return;
        }


        public virtual IEnumerable<double> ProbeValues
        {
            get
            {
                return _ProbeValues(0);
            }
        }


        
        class CachedData
        {
            public int NumberOfProbes {get; private set;}
            public DeviceType DeviceType {get; private set;}
            public CachedData(int numberOfProbes, DeviceType devType)
            {
                NumberOfProbes = numberOfProbes;
                DeviceType =devType;
            }
        }
        

        CachedData GetData(byte deviceId)
        {
            lock (m_CachedData)
            {
                if (!m_CachedData.ContainsKey(deviceId))
                {
                    m_CachedData[deviceId] = new CachedData(_NumberOfProbes(deviceId), _DeviceType(deviceId));
                }
            }
            return m_CachedData[deviceId];
        }
        internal IEnumerable<double> _ProbeValues(byte deviceId)
        {
            List<double> data = new List<double>();
            var cachedData = GetData(deviceId);
            int totalNumberOfProbes = cachedData.NumberOfProbes;
            int maxNumberOfSets = 1; // (int)(totalNumberOfProbes / 8 + 0.999);
            for (int setId = 1; setId <= maxNumberOfSets; setId++)
            {
                byte mode = 0x05;
                switch (cachedData.DeviceType)
                {
                    case DeviceType.PolyGlossmeter:
                    case DeviceType.DuoGlossmeter:
                    case DeviceType.SoloGlossmeter:
                        mode = 0x01;
                        break;
                    case USBDevice.DeviceType.ThermocoupleSimulator:
                        throw new NotSupportedException("Thermocouple simultator cannot read temperatures");
                        //break;

                }
                var result = GetProbeValues(deviceId, mode, (byte)setId);
                if (result != null)
                {
                    const int lengthOfData = 2;
                    const int startOffset = 6;

                    for (int i = 0; i < (result.Length - startOffset) / lengthOfData; i++)
                    {
                        Int16 value = BitConverter.ToInt16(result, startOffset + i * lengthOfData);
                        data.Add((double)value / 10.0);
                    }
                }
            }
            return data;
        }

        public StateOfLogger StateOfLogger
        {
            get
            {
                return (StateOfLogger) GetInt8(0, Commands.ReadDeviceInfo, 12);
            }
        }


        public bool CanSendSetup
        {
            get
            {
                try
                {
                    return GetInt8(0, Commands.ReadDeviceInfo, 18) == 0;
                }
                catch
                {
                    return true;
                }
            }
        }

        public bool CanOffload
        {
            get
            {
                try
                {
                    return GetInt8(0, Commands.ReadDeviceInfo, 17) == 0;
                }
                catch
                {
                    return true;
                }
            }
        }

        public bool StartLogging(float sampleRate, int secsToWait = 0)
        {
            try
            {
                if (GetInt8(0, Commands.ReadDeviceInfo, 18) == 0)
                {
                    List<byte> request = new List<byte>();
                    request.AddRange(BitConverter.GetBytes((short)11));
                    request.AddRange(BitConverter.GetBytes((UInt32)(sampleRate*10) ));
                    request.AddRange(BitConverter.GetBytes((UInt32)secsToWait));
                    var response = Request(Commands.WriteSetup, request.ToArray(), 0);                   
                    return true;       
                }                
            }
            catch
            {
                
            }
            return false;
        }

        public void EnterBootloaderMode()
        {
            List<byte> request = new List<byte>();
            request.Add(0xFF);
            request.Add(0xFF);
            var response = Request((Commands)0x40, request.ToArray());
            return;
        }

    }
}
