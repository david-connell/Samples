using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TQC.USBDevice.GradientOven
{
    [FlagsAttribute]
    public enum ButtonStatus : byte
    {
        NothingPressed = 0x00,
        CancelPressed = 0x01,
        OKPressed = 0x02,

        YellowPressed = 0x04,
        GreenPressed = 0x02,
        RedPressed = 0x01,
    }
}
