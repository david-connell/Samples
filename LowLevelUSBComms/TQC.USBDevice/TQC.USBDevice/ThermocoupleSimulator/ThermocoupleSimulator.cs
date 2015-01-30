using System;
using System.Collections.Generic;
using System.Linq;


namespace TQC.USBDevice.ThermocoupleSimulator
{
    public class ThermocoupleSimulator : TQCUsbLogger 
    {
        public bool Open(bool minimumCommunications = false)
        {
            return Open(USBProductId.USB_THERMOCOUPLE_SIMULATOR, minimumCommunications);
        }

        public double ColdJunctionTemperature
        {
            get
            {
                var response = Request(Commands.ReadCurrentProbeVals, BitConverter.GetBytes((short)100));

                return System.BitConverter.ToSingle(response, 1);
            }
        }

        public double BoardTemperature
        {
            get
            {
                var response = Request(Commands.ReadCurrentProbeVals, BitConverter.GetBytes((short)101));

                return System.BitConverter.ToSingle(response, 1);
            }
        }

        public string CalibrationCertificate
        {
            get
            {
                return GetReadDeviceInfoAsString(0, 200);
            }
            set
            {
                SetReadDeviceInfo(0, 200, value, 40);
            }
        }

        public void SetTemperatureOutput(double temperature)
        {                        
            List<byte> request = new List<byte>();
            request.Add(0x10);
            request.Add(0x00);
            request.AddRange(System.BitConverter.GetBytes((float)temperature));
            var response = Request(Commands.WriteDeviceInfo, request.ToArray());
            return;            
        }

        public void EnterBootloaderMode()
        {
            List<byte> request = new List<byte>();
            var response = Request((Commands)0x40, request.ToArray());
            return;
        }


    }
}
