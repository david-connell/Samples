using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.USBDevice.GradientOven
{
    [FlagsAttribute]
    public enum ButtonStatus : byte
    {
        NothingPressed = 0x00,
        CancelPressed = 0x01,
        OKPressed = 0x02,
    }
}
