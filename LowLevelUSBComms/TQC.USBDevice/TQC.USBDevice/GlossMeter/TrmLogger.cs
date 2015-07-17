namespace TQC.USBDevice.GlossMeter
{
    public class TrmLogger : GlossMeterLogger
    {
        public bool Open(bool minimumCommunications = false)
        {            
            var code = new UsbDevices();
            foreach (var item in code.GetUSBDevices())
            {
                if (item.FriendlyName.ToLower().StartsWith("trm "))
                {
                    return Open(USBProductId.Glossmeter, item.DeviceName, minimumCommunications);
                }
            }
            return false;
        }
    }
}
