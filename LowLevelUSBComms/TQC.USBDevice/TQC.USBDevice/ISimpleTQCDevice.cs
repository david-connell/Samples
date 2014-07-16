using System;
using System.Collections.Generic;

namespace TQC.USBDevice
{
    public interface ICoreCommands
    {
        DateTime ManufactureDate { get; }
        string ManufactureName { get; }
        Version ProtocolVersion { get; }
        Version HardwareVersion { get; }
        Version SoftwareVersion { get; }

        string DeviceName { get; }
        TQC.USBDevice.USBLogger.DeviceType DeviceType { get; }
        int SerialNumber { get; }
    }

    public class LinearCalibrationDetails
    {
        public double M { get; private set; }
        public double C { get; private set; }
        public LinearCalibrationDetails(double m, double c)
        {
            M = m;
            C = c;
        }
    }

    public interface ISimpleTQCDevice : ICoreCommands
    {
        int NumberOfProbes { get; }
        string ProbeName(int probeId);
        USBLogger.ProbeType ProbeType(int probeId);
        LinearCalibrationDetails CalibrationDetails(int probeId);

        DateTime Calibration { get; }
        string CalibrationCompany { get; }
        string CalibrationUserName { get; }

        IEnumerable<double> ColdJunctions { get; }
        IEnumerable<double> ProbeValues { get; }

    }

   
}
