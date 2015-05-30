using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TQC.USBDevice.GlossMeter
{

    public class TmrLogger : GlossMeterLogger
    {
        public bool Open(bool minimumCommunications = false)
        {
            string portName = null;
            var code = new UsbDevices();
            foreach (var item in code.GetUSBDevices())
            {
                if (item.FriendlyName.ToLower().StartsWith("trm "))
                {
                    return Open(USBProductId.Glossmeter, portName, minimumCommunications);
                }
            }
            return false;
        }




    }
}
