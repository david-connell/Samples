using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TQC.USBDevice.GlossMeter
{
    public class GlossMeterLogger : TQCUsbLogger
    {

        public UInt16 Button { get; set; }
        public UInt32 SpecialStatus { get; set; }

        public bool IsScanButtonPressed { get { return (Button & 0x08) == 0x08; } }
        public bool IsUpPressed { get { return (Button & 0x01) == 0x01; } }
        public bool IsDownPressed { get { return (Button & 0x02) == 0x02; } }
        public bool IsOKPressed { get { return (Button & 0x04) == 0x04; } }
        public bool IsInCraddle { get { return (SpecialStatus & 0x01) == 0x01; } }


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

        public string CalibrationCertificate
        {
            get
            {
                return ReadDeviceInfoAsString(0, 199);
            }
            set
            {
                WriteDeviceInfo(0, 199, value, 40);
            }
        }

        public bool ReadButtonStatus()
        {
            bool status = false;
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes((short)0x30));
            var response = Request(Commands.ReadCurrentProbeVals, request.ToArray());
            if (response != null && response.Length >= 4)
            {
                Button = response[2];
                status = true;
                if (response.Length >= 5)
                {
                    SpecialStatus = BitConverter.ToUInt32(response, 3);
                }
                else
                {
                    SpecialStatus = response[3];
                }
            }
            return status;
        }

        public IList<double> InternalGloss
        {
            get
            {
                List<double> result = new List<double>();
                List<byte> request = new List<byte>();
                request.AddRange(BitConverter.GetBytes((short)11));
                var response = Request(Commands.ReadCalibrationDetails, request.ToArray());
                if (response.Length == 3 * 4)
                {
                    result.Add(BitConverter.ToSingle(response, 0));
                    result.Add(BitConverter.ToSingle(response, 4));
                    result.Add(BitConverter.ToSingle(response, 8));
                }
                return result;
            }
            set
            {
                List<byte> request = new List<byte>();
                request.AddRange(BitConverter.GetBytes((short)11));
                request.AddRange(BitConverter.GetBytes((float)value[0]));
                request.AddRange(BitConverter.GetBytes((float)value[1]));
                request.AddRange(BitConverter.GetBytes((float)value[2]));
                var response = Request(Commands.WriteCalibrationDetails, request.ToArray());

                Thread.Sleep(10000);
            }
        }

        public IList<double> get_CalibrationCoeffients(int channelId)
        {
            List<double> result = new List<double>();
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes((short)(20 + channelId)));
            var response = Request(Commands.ReadCalibrationDetails, request.ToArray());
            int size = sizeof(float);
            int arraySize = response.Length / size;
            for (int id = 2; id < arraySize; id++)
            {
                result.Add(BitConverter.ToSingle(response, id * size));
            }
            return result;
        }

        public void set_CalibrationCoeffients(int channelId, IList<double> values)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes((short)(20 + channelId)));
            
            request.AddRange(BitConverter.GetBytes((float)0.0f));
            request.AddRange(BitConverter.GetBytes((float)0.0f));
            foreach (var value in values)
            {
                request.AddRange(BitConverter.GetBytes((float)value));
            }
            var response = Request(Commands.WriteCalibrationDetails, request.ToArray());

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
