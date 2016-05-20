using System;
using System.Collections.Generic;

namespace TQC.USBDevice
{
    internal class SerialOrUsbPort 
    {

        public bool IsSerialPort { get; set; }
        private int m_nComPortId;
        public string DeviceName { get; private set; }
        private string m_ComPortName;
        public string FullName { get; set; }
        public string FriendlyName { get; private set; }
        private bool m_IsOk;
        public int PidVid { get; private set; }

        public SerialOrUsbPort(string deviceName, string fullName, string niceName, int usbPort)
        {
            m_IsOk = true;
            IsSerialPort = false;
            DeviceName = deviceName;
            FullName = fullName;
            FriendlyName = niceName;
            PidVid = usbPort;
        }
        public override string ToString()
        {
            return string.Format("{0} 0x{1:x8}", FriendlyName, PidVid );
        }
        public string ToFullString()
        {
            return string.Format("{0} {1}", FriendlyName, FullName);
        }
    }

    internal class UsbDevices
    {
        private class USBDevice
        {
            public HidDevice.USBProductId VID_PID { get; set; }
            public String Name { get; set; }
            public USBDevice(HidDevice.USBProductId vID_PID, string name)
            {
                VID_PID = vID_PID;
                Name = name;
            }
            public UInt32 VID
            {
                get { return (UInt32) VID_PID >> 16; }
            }
            public UInt32 PID
            {
                get { return (UInt32) VID_PID & 0xFFFF; }

            }
        }

        public IList<SerialOrUsbPort> GetUSBDevices()
        {
            USBDevice[] usbDevices = new USBDevice[] 
            {
                new USBDevice(HidDevice.USBProductId.Glossmeter, "GlossMeter"),
                new USBDevice(HidDevice.USBProductId.USB_PRODUCT2, "GlossMeter2"),
                new USBDevice(HidDevice.USBProductId.USB_CURVEX_3, "CurveX 3 (Old)"),
                new USBDevice(HidDevice.USBProductId.USB_CURVEX_3a, "CurveX 3"),
                new USBDevice(HidDevice.USBProductId.USB_CURVEX_3_STANDARD, "CurveX 3 Standard"),
                new USBDevice(HidDevice.USBProductId.GRADIENT_OVEN, "GRADIENT_OVEN"),
                new USBDevice(HidDevice.USBProductId.USB_THERMOCOUPLE_SIMULATOR, "USB_THERMOCOUPLE_SIMULATOR"),
            };

            var devices = new List<SerialOrUsbPort>();
            foreach (var item in usbDevices)
            {
                var devs = HidDevice.FindDevice((int)item.VID, (int)item.PID);
                foreach (var dev in devs)
                {

                    if (dev.Product != null)
                    {
                        devices.Add(new SerialOrUsbPort(
                        dev.Path,
                        dev.Path,
                        dev.Product,
                        (int)item.VID_PID));
                        dev.Dispose();

                    }

                }


            }
            return devices;
        }
    }
}