using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TQC.USBDevice.GradientOven
{
    public interface IGROThermoCoupleBoard : ISimpleTQCDevice
    {
        ThermcoupleBoardStatus Status { get; }
    }

    public interface IGROMainBoard : ICoreCommands
    {
        IGROThermoCoupleBoard GetChildDevice(byte id);
        IEnumerable<byte> ThermocoupleBoardIDs { get;}

        Percentage ExternalFanSpeed { get; set; }
        Percentage InternalFanSpeed { get; set; }
        Percentage Cooling { get; set; }
        PowerState Power { get; set; }
        ClampState Clamp { get; set; }
        MainBoardStatus Status { get; }

        CarrierPosition CarrierPosition { get; set; }
        Percentage GetFanSetting(short fanId);
        void SetFanSetting(short fanId, Percentage fanSetting);
        float GetTempSetting(short slotId);
        void SetTempSetting(short fanId, float temperatureSettingInDegreesC);
    }
}
