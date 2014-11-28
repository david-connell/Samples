using System;

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
        DeviceType DeviceType { get; }
        int SerialNumber { get; }
    }
}