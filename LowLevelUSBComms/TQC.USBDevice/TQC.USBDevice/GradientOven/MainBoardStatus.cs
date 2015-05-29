using System;


namespace TQC.USBDevice.GradientOven
{
    [FlagsAttribute]
    public enum MainBoardStatus : uint
    {
        OK = 0x00,
        PowersuplyOffBecauseEmergencyStop = 0x00000001,
        MotorClampIsBlocked     = 0x00000002,
        MotorLifterIsBlocked    = 0x00000004,
        MotorCarrierIsBlocked   = 0x00000008,
        InternalFanIsBlocked    = 0x00000010,
        ExternalFanIsBlocked    = 0x00000020,
        OverTemperature         = 0x00000040,
        Booting                 = 0x00000080,
    }
}
