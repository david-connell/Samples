using System;
using System.Collections.Generic;

namespace TQC.USBDevice.GradientOven
{

    internal class GROThermocoupleBoard : IGROThermoCoupleBoard
    {
        GROMainBoard ParentDevice { get; set; }
        byte ChildDeviceID { get; set; }


        internal GROThermocoupleBoard(GROMainBoard parent, byte childDeviceId)
        {
            ParentDevice = parent;
            ChildDeviceID = childDeviceId;
        }

        public Int32 SerialNumber
        {
            get
            {
                return ParentDevice._SerialNumber(ChildDeviceID);
            }
        }

        public Version SoftwareVersion
        {
            get
            {
                return ParentDevice._SoftwareVersion(ChildDeviceID);
            }
        }

        public Version HardwareVersion
        {
            get
            {
                return ParentDevice._HardwareVersion(ChildDeviceID);
            }
        }

        public string DeviceName
        {
            get
            {
                return ParentDevice._DeviceName(ChildDeviceID);
            }
        }

        public string ManufactureName
        {
            get
            {
                return ParentDevice._ManufactureName(ChildDeviceID);
            }
        }

        public DateTime ManufactureDate
        {
            get
            {
                return ParentDevice._ManufactureDate(ChildDeviceID);
            }
        }

        public Int32 NumberOfProbes
        {
            get
            {
                return ParentDevice._NumberOfProbes(ChildDeviceID);
            }
        }

        public string ProbeName (int probeId)
        {                        
            return ParentDevice._ProbeName(ChildDeviceID, probeId);         
        }

        public ProbeType ProbeType(int probeId)
        {
            return ParentDevice._ProbeType(ChildDeviceID, probeId);
        }

        public Version ProtocolVersion
        {
            get
            {
                return ParentDevice._ProtocolVersion(ChildDeviceID);
            }
        }

        public DeviceType DeviceType
        {
            get
            {
                return ParentDevice._DeviceType(ChildDeviceID);
            }
        }

        public DateTime Calibration
        {
            get
            {
                return ParentDevice._Calibration(ChildDeviceID);
            }
        }

        public String CalibrationUserName
        {
            get
            {
                return ParentDevice._CalibrationUserName(ChildDeviceID);
            }
        }

        public LinearCalibrationDetails CalibrationDetails(int probeId)
        {
            return ParentDevice._Calibration(ChildDeviceID, probeId);            
        }
        public String CalibrationCompany
        {
            get
            {
                return ParentDevice._CalibrationCompany(ChildDeviceID);
            }
        }

        public IEnumerable<double> ColdJunctions
        {
            get
            {
                return ParentDevice._ColdJunctions(ChildDeviceID);
            }
        }

        public IEnumerable<double> ProbeValues
        {
            get
            {
                return ParentDevice._ProbeValues(ChildDeviceID);
            }
        }

        public ThermcoupleBoardStatus Status
        {
            get 
            {
                var value = new ThermcoupleBoardStatus(ParentDevice._GetStatus(ChildDeviceID));
                return value;
            }
        }

    }
}
