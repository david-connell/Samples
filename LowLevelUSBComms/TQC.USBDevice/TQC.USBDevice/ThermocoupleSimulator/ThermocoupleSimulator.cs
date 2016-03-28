using System;
using System.Collections.Generic;

namespace TQC.USBDevice.ThermocoupleSimulator
{
    public class ThermocoupleSimulator : TQCUsbLogger 
    {
        public ThermocoupleSimulator(IUsbInterfaceForm mainWinForm)
            : base(mainWinForm, null)
        {
        }

        public bool Open(bool minimumCommunications = false)
        {
            return Open(USBProductId.USB_THERMOCOUPLE_SIMULATOR, minimumCommunications);
        }

        public double ColdJunctionTemperature
        {
            get
            {
                var response = Request(Commands.ReadCurrentProbeVals, BitConverter.GetBytes((short)100));

                return BitConverter.ToSingle(response, 1);
            }
        }

        public double BoardTemperature
        {
            get
            {
                var response = Request(Commands.ReadCurrentProbeVals, BitConverter.GetBytes((short)101));

                return BitConverter.ToSingle(response, 1);
            }
        }

        public string CalibrationCertificate
        {
            get
            {
                return ReadDeviceInfoAsString(0, 200);
            }
            set
            {
                WriteDeviceInfo(0, 200, value, 40);
            }
        }

        public void SetTemperatureOutput(double temperature)
        {
            List<byte> request = new List<byte> {0x10, 0x00};
            request.AddRange(BitConverter.GetBytes((float)temperature));
            Request(Commands.WriteDeviceInfo, request.ToArray());
        }
    }
}
