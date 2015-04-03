using System;


namespace TQC.USBDevice.GradientOven
{
    [FlagsAttribute]
    public enum MainBoardStatus : uint
    {
        OK = 0x00,
        PowersuplyOffBecauseEmergencyStop = 0x00000001,
        MotorClampIsBlocked     = 0x00000002,
        MotorCarrierIsBlocked   = 0x00000004,
        InternalFanIsBlocked    = 0x00000008,
        ExternalFanIsBlocked    = 0x00000010,
        OverTemperature         = 0x00000020,               
    }
}
