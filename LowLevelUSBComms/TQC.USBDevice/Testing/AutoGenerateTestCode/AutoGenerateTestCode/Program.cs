using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TQC.USBDevice.AutoGenerateTestCode
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var fileName in args)
            {
                AutoGenerateTestCode(fileName);
            }
            
        }

        private static void AutoGenerateTestCode(string fileName)
        {
            var commands = UsbCommands.ParseFile("CurveX3","UserSetup",fileName);

            UsbTestWriter writer = new UsbTestWriter(commands);
            writer.GenerateOutput(Console.Out);
        }
    }
}
