using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.USBDevice
{
    //Extend here with request responses that are not covered by the standard COM calls.
    public class TQCUsbLogger : USBLogger, ISimpleTQCDevice
    {
        Dictionary<byte, int> ProbesPerDevice = new Dictionary<byte, int>();

        private int GetInt8(byte deviceId, Commands command, int commandId)
        {
            return GetResponse(deviceId, command, commandId)[0];
        }

        private int GetInt32(byte deviceId, Commands command, int commandId)
        {
            return BitConverter.ToInt32(GetResponse(deviceId, command, commandId), 0);
        }

        private byte[] GetResponse(byte deviceId, Commands command, int commandId)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes((short)commandId));
            var response = Request(command, request.ToArray(), deviceId);
            return response;
        }
        private byte[] GetProbeValues(byte deviceId, byte mode, byte setid)
        {
            List<byte> request = new List<byte>();
            request.Add(mode);
            request.Add(setid);
            var response = Request(Commands.ReadCurrentProbeVals, request.ToArray(), deviceId);
            return response;
        }

        protected override void ClearCachedData()
        {
            ProbesPerDevice = new Dictionary<byte, int>();
        }

        public Int32 SerialNumber
        {
            get
            {
                return _SerialNumber(0);
            }
        }

        internal Int32 _SerialNumber(byte deviceId)
        {
            return GetInt32(deviceId, Commands.ReadDeviceInfo, 0);
        }

        internal Version _SoftwareVersion(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadDeviceInfo, 1);
            return new Version(result[0], result[1], result[2], result[3]);
        }

        public Version HardwareVersion
        {
            get
            {
                return _HardwareVersion(0);
            }
        }

        internal Version _HardwareVersion(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadDeviceInfo, 2);
            return new Version(result[0], result[1], result[2], result[3]);
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
            return new Version(result[0], result[1]);
        }

        internal string _DeviceName(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadDeviceInfo, 3);
            return Encoding.UTF8.GetString(result, 0, result.Length);            
        }

        public string DeviceName
        {
            get
            {
                return _DeviceName(0);
            }
        }

        internal string _ManufactureName(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadDeviceInfo, 4);
            return Encoding.UTF8.GetString(result, 0, result.Length);
        }

        public string ManufactureName
        {
            get
            {
                return _ManufactureName(0);
            }
        }


        internal DateTime _ManufactureDate(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadDeviceInfo, 5);
            double dateTime = BitConverter.ToDouble(result, 0);

            return DateTime.FromOADate(dateTime);
        }

        public DateTime ManufactureDate
        {
            get
            {
                return _ManufactureDate(0);
            }
        }


        public Version SoftwareVersion
        {
            get
            {
                return _SoftwareVersion(0);
            }
        }
        public Int32 NumberOfProbes
        {
            get
            {
                return _NumberOfProbes(0);
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

        internal string _ProbeName(byte deviceId, int probeId)
        {
            if (probeId >= 0 && probeId < _NumberOfProbes(deviceId))
            {
                var result = GetResponse(deviceId, Commands.ReadCalibrationDetails, 200 + probeId);
                return Encoding.UTF8.GetString(result, 0, result.Length);
            }
            else
            {
                throw new IndexOutOfRangeException("probeId");
            }
        }

        public string ProbeName(int probeId)
        {
            return _ProbeName(0, probeId);            
        }

        internal ProbeType _ProbeType(byte deviceId, int probeId)
        {
            if (probeId >= 0 && probeId < _NumberOfProbes(deviceId))
            {
                var result = GetResponse(deviceId, Commands.ReadCalibrationDetails, 100 + probeId);
                return (ProbeType)result[0];
            }
            else
            {
                throw new IndexOutOfRangeException("ProbeId");
            }
        }
        
        public ProbeType ProbeType(int probeId)
        {
            //get
            {
                return _ProbeType(0, probeId);
            }
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

        internal DateTime _Calibration(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadCalibrationDetails, 0);
            double dateTime = BitConverter.ToDouble(result, 0);
            return DateTime.FromOADate(dateTime);
        }

        /// <summary>
        /// This is not supported by the GRO!
        /// </summary>
        public String CalibrationCompany
        {
            get
            {
                return _CalibrationCompany(0);
            }
        }

        internal String _CalibrationCompany(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadCalibrationDetails, 1);
            return Encoding.UTF8.GetString(result, 0, result.Length);
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

        internal String _CalibrationUserName(byte deviceId)
        {
            var result = GetResponse(deviceId, Commands.ReadCalibrationDetails, 2);            
            return Encoding.UTF8.GetString(result, 0, result.Length);
        }

        internal DeviceType _DeviceType(byte deviceId)
        {
            return LoggerTypeToDeviceType(GetInt32(deviceId, Commands.ReadDeviceInfo, 102));
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
            int maxNumberOfSets = (int)(totalNumberOfProbes / 6 + 0.999);
            for (int setId = 1; setId < maxNumberOfSets; setId++)
            {
                var result = GetProbeValues(deviceId, 0x06, 1);
                const int lengthOfData = sizeof(float);
                for (int i = 0; i < result.Length / lengthOfData; i++)
                {
                    double value = BitConverter.ToSingle(result, i * lengthOfData);
                    data.Add(value);
                }
            }
            return data;                        
        }

        /// <summary>
        /// This is not valid for GRO
        /// </summary>
        public IEnumerable<double> ProbeValues
        {
            get
            {
                return _ProbeValues(0);
            }
        }

        internal IEnumerable<double> _ProbeValues(byte deviceId)
        {
            List<double> data = new List<double>();
            int totalNumberOfProbes = _NumberOfProbes(deviceId);
            int maxNumberOfSets = (int)(totalNumberOfProbes / 8 + 0.999);
            for (int setId = 1; setId < maxNumberOfSets; setId++)
            {
                var result = GetProbeValues(deviceId, 0x05, 1);
                const int lengthOfData = 2 ;
                for (int i = 0; i < result.Length / lengthOfData; i++)
                {
                    Int16 value = BitConverter.ToInt16(result, i * lengthOfData);

                    //Assume temperature. (this will be wrong)


                    data.Add(value/10.0);
                }
            }
            return data;
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
    }
}
