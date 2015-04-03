using System;


namespace TQC.USBDevice.GradientOven
{
    [FlagsAttribute]
    public enum ThermcoupleBoardStatus : uint
    {
        OK = 0x00,        
        ThermocoupleSensorOverTemperature    = 0x00010000,
        ThermocoupleSensorDisconnected      = 0x00020000,
    }
}
