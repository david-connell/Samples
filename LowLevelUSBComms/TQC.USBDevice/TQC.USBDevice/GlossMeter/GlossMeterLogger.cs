using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TQC.USBDevice.GlossMeter
{
    public class GlossMeterLogger : TQCUsbLogger
    {
        public void Buzzer(byte freq, byte duration, byte wait, byte iterations)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes((short)0));
            request.Add(freq);
            request.Add(duration);
            request.Add(wait);
            request.Add(iterations);
            Request(Commands.LoggerSpecificCommand, request.ToArray());
        }

        public virtual bool IsTmr
        {
            get
            {
                List<byte> request = new List<byte>();
                request.AddRange(BitConverter.GetBytes((short)5));                
                var response = Request(Commands.LoggerSpecificCommand, request.ToArray());
                if (response.Length == 1)
                {
                    return response[0] != 0;

                }
                return false;
            }
            set
            {
                List<byte> request = new List<byte>();
                request.AddRange(BitConverter.GetBytes((short)4));
                request.Add(value ? (byte)1 : (byte) 0);
                Request(Commands.LoggerSpecificCommand, request.ToArray());
            }
        }

        public void WriteTextString(int lineNo, string text)
        {
            if (lineNo >= 0 && lineNo <= 5)
            {
                List<byte> request = new List<byte>();
                request.AddRange(BitConverter.GetBytes((UInt16)(200 + lineNo)));
                request.AddRange(System.Text.ASCIIEncoding.Default.GetBytes(text));
                request.Add(0);
                Request(Commands.WriteDeviceInfo, request.ToArray());
            }
        }

        public void ResetScreen()
        {
            for (int lineNo = 0; lineNo <= 5; lineNo++)
            {
                WriteTextString(lineNo, "");
            }
        }
    }
}
