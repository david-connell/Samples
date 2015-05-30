using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TQC.USBDevice
{

    internal class Win32Usb
    {

        #region Structures

        /// <summary>

        /// An overlapped structure used for overlapped IO operations. The structure is

        /// only used by the OS to keep state on pending operations. You don't need to fill anything in if you

        /// unless you want a Windows event to fire when the operation is complete.

        /// </summary>

        [StructLayout(LayoutKind.Sequential, Pack = 1)]

        protected struct Overlapped
        {

            public uint Internal;

            public uint InternalHigh;

            public uint Offset;

            public uint OffsetHigh;

            public IntPtr Event;

        }

        /// <summary>

        /// Provides details about a single USB device

        /// </summary>

        [StructLayout(LayoutKind.Sequential, Pack = 1)]

        protected struct DeviceInterfaceData
        {

            public int Size;

            public Guid InterfaceClassGuid;

            public int Flags;

            public int Reserved;

        }

        /// <summary>

        /// Provides the capabilities of a HID device

        /// </summary>

        [StructLayout(LayoutKind.Sequential, Pack = 1)]

        protected struct HidCaps
        {

            public short Usage;

            public short UsagePage;

            public short InputReportByteLength;

            public short OutputReportByteLength;

            public short FeatureReportByteLength;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]

            public short[] Reserved;

            public short NumberLinkCollectionNodes;

            public short NumberInputButtonCaps;

            public short NumberInputValueCaps;

            public short NumberInputDataIndices;

            public short NumberOutputButtonCaps;

            public short NumberOutputValueCaps;

            public short NumberOutputDataIndices;

            public short NumberFeatureButtonCaps;

            public short NumberFeatureValueCaps;

            public short NumberFeatureDataIndices;

        }

        /// <summary>

        /// Access to the path for a device

        /// </summary>

        [StructLayout(LayoutKind.Sequential, Pack = 1)]

        public struct DeviceInterfaceDetailData
        {

            public int Size;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]

            public string DevicePath;

        }

        /// <summary>

        /// Used when registering a window to receive messages about devices added or removed from the system.

        /// </summary>

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]

        public class DeviceBroadcastInterface
        {

            public int Size;

            public int DeviceType;

            public int Reserved;

            public Guid ClassGuid;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]

            public string Name;

        }



        // HIDD_ATTRIBUTES

        [StructLayout(LayoutKind.Sequential)]

        public unsafe struct HIDD_ATTRIBUTES
        {

            public int Size; // = sizeof (struct _HIDD_ATTRIBUTES) = 10



            //

            // Vendor ids of this hid device

            //

            public System.UInt16 VendorID;

            public System.UInt16 ProductID;

            public System.UInt16 VersionNumber;



            //

            // Additional fields will be added to the end of this structure.

            //

        }

        #endregion



        #region Constants

        /// <summary>Windows message sent when a device is inserted or removed</summary>

        public const int WM_DEVICECHANGE = 0x0219;

        /// <summary>WParam for above : A device was inserted</summary>

        public const int DEVICE_ARRIVAL = 0x8000;

        /// <summary>WParam for above : A device was removed</summary>

        public const int DEVICE_REMOVECOMPLETE = 0x8004;

        /// <summary>Used in SetupDiClassDevs to get devices present in the system</summary>

        protected const int DIGCF_PRESENT = 0x02;

        /// <summary>Used in SetupDiClassDevs to get device interface details</summary>

        protected const int DIGCF_DEVICEINTERFACE = 0x10;

        /// <summary>Used when registering for device insert/remove messages : specifies the type of device</summary>

        protected const int DEVTYP_DEVICEINTERFACE = 0x05;

        /// <summary>Used when registering for device insert/remove messages : we're giving the API call a window handle</summary>

        protected const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;

        /// <summary>Purges Win32 transmit buffer by aborting the current transmission.</summary>

        protected const uint PURGE_TXABORT = 0x01;

        /// <summary>Purges Win32 receive buffer by aborting the current receive.</summary>

        protected const uint PURGE_RXABORT = 0x02;

        /// <summary>Purges Win32 transmit buffer by clearing it.</summary>

        protected const uint PURGE_TXCLEAR = 0x04;

        /// <summary>Purges Win32 receive buffer by clearing it.</summary>

        protected const uint PURGE_RXCLEAR = 0x08;

        /// <summary>CreateFile : Open file for read</summary>

        protected const uint GENERIC_READ = 0x80000000;

        /// <summary>CreateFile : Open file for write</summary>

        protected const uint GENERIC_WRITE = 0x40000000;

        /// <summary>CreateFile : Open handle for overlapped operations</summary>

        protected const uint FILE_FLAG_OVERLAPPED = 0x40000000;

        /// <summary>CreateFile : Resource to be "created" must exist</summary>

        protected const uint OPEN_EXISTING = 3;

        /// <summary>ReadFile/WriteFile : Overlapped operation is incomplete.</summary>

        protected const uint ERROR_IO_PENDING = 997;

        /// <summary>Infinite timeout</summary>

        protected const uint INFINITE = 0xFFFFFFFF;

        /// <summary>Simple representation of a null handle : a closed stream will get this handle. Note it is public for comparison by higher level classes.</summary>

        public static IntPtr NullHandle = IntPtr.Zero;

        /// <summary>Simple representation of the handle returned when CreateFile fails.</summary>

        protected static IntPtr InvalidHandleValue = new IntPtr(-1);

        #endregion



        #region P/Invoke

        /// <summary>

        /// Gets the GUID that Windows uses to represent HID class devices

        /// </summary>

        /// <param name="gHid">An out parameter to take the Guid</param>

        [DllImport("hid.dll", SetLastError = true)]

        protected static extern void HidD_GetHidGuid(out Guid gHid);

        /// <summary>

        /// Allocates an InfoSet memory block within Windows that contains details of devices.

        /// </summary>

        /// <param name="gClass">Class guid (e.g. HID guid)</param>

        /// <param name="strEnumerator">Not used</param>

        /// <param name="hParent">Not used</param>

        /// <param name="nFlags">Type of device details required (DIGCF_ constants)</param>

        /// <returns>A reference to the InfoSet</returns>

        [DllImport("setupapi.dll", SetLastError = true)]

        protected static extern IntPtr SetupDiGetClassDevs(ref Guid gClass, [MarshalAs(UnmanagedType.LPStr)] string strEnumerator, IntPtr hParent, uint nFlags);

        /// <summary>

        /// Frees InfoSet allocated in call to above.

        /// </summary>

        /// <param name="lpInfoSet">Reference to InfoSet</param>

        /// <returns>true if successful</returns>

        [DllImport("setupapi.dll", SetLastError = true)]

        protected static extern int SetupDiDestroyDeviceInfoList(IntPtr lpInfoSet);

        /// <summary>

        /// Gets the DeviceInterfaceData for a device from an InfoSet.

        /// </summary>

        /// <param name="lpDeviceInfoSet">InfoSet to access</param>

        /// <param name="nDeviceInfoData">Not used</param>

        /// <param name="gClass">Device class guid</param>

        /// <param name="nIndex">Index into InfoSet for device</param>

        /// <param name="oInterfaceData">DeviceInterfaceData to fill with data</param>

        /// <returns>True if successful, false if not (e.g. when index is passed end of InfoSet)</returns>

        [DllImport("setupapi.dll", SetLastError = true)]

        protected static extern bool SetupDiEnumDeviceInterfaces(IntPtr lpDeviceInfoSet, uint nDeviceInfoData, ref Guid gClass, uint nIndex, ref DeviceInterfaceData oInterfaceData);

        /// <summary>

        /// SetupDiGetDeviceInterfaceDetail - two of these, overloaded because they are used together in slightly different

        /// ways and the parameters have different meanings.

        /// Gets the interface detail from a DeviceInterfaceData. This is pretty much the device path.

        /// You call this twice, once to get the size of the struct you need to send (nDeviceInterfaceDetailDataSize=0)

        /// and once again when you've allocated the required space.

        /// </summary>

        /// <param name="lpDeviceInfoSet">InfoSet to access</param>

        /// <param name="oInterfaceData">DeviceInterfaceData to use</param>

        /// <param name="lpDeviceInterfaceDetailData">DeviceInterfaceDetailData to fill with data</param>

        /// <param name="nDeviceInterfaceDetailDataSize">The size of the above</param>

        /// <param name="nRequiredSize">The required size of the above when above is set as zero</param>

        /// <param name="lpDeviceInfoData">Not used</param>

        /// <returns></returns>

        [DllImport("setupapi.dll", SetLastError = true)]

        protected static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr lpDeviceInfoSet, ref DeviceInterfaceData oInterfaceData, IntPtr lpDeviceInterfaceDetailData, uint nDeviceInterfaceDetailDataSize, ref uint nRequiredSize, IntPtr lpDeviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]

        protected static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr lpDeviceInfoSet, ref DeviceInterfaceData oInterfaceData, ref DeviceInterfaceDetailData oDetailData, uint nDeviceInterfaceDetailDataSize, ref uint nRequiredSize, IntPtr lpDeviceInfoData);

        /// <summary>

        /// Registers a window for device insert/remove messages

        /// </summary>

        /// <param name="hwnd">Handle to the window that will receive the messages</param>

        /// <param name="lpInterface">DeviceBroadcastInterrface structure</param>

        /// <param name="nFlags">set to DEVICE_NOTIFY_WINDOW_HANDLE</param>

        /// <returns>A handle used when unregistering</returns>

        [DllImport("user32.dll", SetLastError = true)]

        protected static extern IntPtr RegisterDeviceNotification(IntPtr hwnd, DeviceBroadcastInterface oInterface, uint nFlags);

        /// <summary>

        /// Unregister from above.

        /// </summary>

        /// <param name="hHandle">Handle returned in call to RegisterDeviceNotification</param>

        /// <returns>True if success</returns>

        [DllImport("user32.dll", SetLastError = true)]

        protected static extern bool UnregisterDeviceNotification(IntPtr hHandle);

        /// <summary>

        /// Gets details from an open device. Reserves a block of memory which must be freed.

        /// </summary>

        /// <param name="hFile">Device file handle</param>

        /// <param name="lpData">Reference to the preparsed data block</param>

        /// <returns></returns>

        [DllImport("hid.dll", SetLastError = true)]

        protected static extern bool HidD_GetPreparsedData(IntPtr hFile, out IntPtr lpData);

        /// <summary>

        /// Frees the memory block reserved above.

        /// </summary>

        /// <param name="pData">Reference to preparsed data returned in call to GetPreparsedData</param>

        /// <returns></returns>

        [DllImport("hid.dll", SetLastError = true)]

        protected static extern bool HidD_FreePreparsedData(ref IntPtr pData);

        /// <summary>

        /// Gets a device's capabilities from the preparsed data.

        /// </summary>

        /// <param name="lpData">Preparsed data reference</param>

        /// <param name="oCaps">HidCaps structure to receive the capabilities</param>

        /// <returns>True if successful</returns>

        [DllImport("hid.dll", SetLastError = true)]

        protected static extern int HidP_GetCaps(IntPtr lpData, out HidCaps oCaps);

        /// <summary>

        /// Creates/opens a file, serial port, USB device... etc

        /// </summary>

        /// <param name="strName">Path to object to open</param>

        /// <param name="nAccess">Access mode. e.g. Read, write</param>

        /// <param name="nShareMode">Sharing mode</param>

        /// <param name="lpSecurity">Security details (can be null)</param>

        /// <param name="nCreationFlags">Specifies if the file is created or opened</param>

        /// <param name="nAttributes">Any extra attributes? e.g. open overlapped</param>

        /// <param name="lpTemplate">Not used</param>

        /// <returns></returns>

        [DllImport("kernel32.dll", SetLastError = true)]

        protected static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPStr)] string strName, uint nAccess, uint nShareMode, IntPtr lpSecurity, uint nCreationFlags, uint nAttributes, IntPtr lpTemplate);

        /// <summary>

        /// Closes a window handle. File handles, event handles, mutex handles... etc

        /// </summary>

        /// <param name="hFile">Handle to close</param>

        /// <returns>True if successful.</returns>

        [DllImport("kernel32.dll", SetLastError = true)]

        protected static extern int CloseHandle(IntPtr hFile);



        [DllImport("hid.dll", SetLastError = true)]

        protected static extern bool HidD_GetAttributes(

                IntPtr hFile,                                                           // IN HANDLE  HidDeviceObject,

                out HIDD_ATTRIBUTES attributes);                        // OUT PHIDD_ATTRIBUTES  Attributes



        [DllImport("hid.dll", SetLastError = true)]

        protected static extern bool HidD_GetManufacturerString(

                IntPtr hFile,           // IN HANDLE  HidDeviceObject,

                byte[] lpBuffer,

                int bufferLength

                );



        [DllImport("hid.dll", SetLastError = true)]

        protected static extern bool HidD_GetProductString(

                IntPtr hFile,           // IN HANDLE  HidDeviceObject,

                byte[] lpBuffer,

                int bufferLength

                );



        #endregion



        #region Public methods

        /// <summary>

        /// Registers a window to receive windows messages when a device is inserted/removed. Need to call this

        /// from a form when its handle has been created, not in the form constructor. Use form's OnHandleCreated override.

        /// </summary>

        /// <param name="hWnd">Handle to window that will receive messages</param>

        /// <param name="gClass">Class of devices to get messages for</param>

        /// <returns>A handle used when unregistering</returns>

        public static IntPtr RegisterForUsbEvents(IntPtr hWnd, Guid gClass)
        {

            DeviceBroadcastInterface oInterfaceIn = new DeviceBroadcastInterface();

            oInterfaceIn.Size = Marshal.SizeOf(oInterfaceIn);

            oInterfaceIn.ClassGuid = gClass;

            oInterfaceIn.DeviceType = DEVTYP_DEVICEINTERFACE;

            oInterfaceIn.Reserved = 0;

            return RegisterDeviceNotification(hWnd, oInterfaceIn, DEVICE_NOTIFY_WINDOW_HANDLE);

        }

        /// <summary>

        /// Unregisters notifications. Can be used in form dispose

        /// </summary>

        /// <param name="hHandle">Handle returned from RegisterForUSBEvents</param>

        /// <returns>True if successful</returns>

        public static bool UnregisterForUsbEvents(IntPtr hHandle)
        {

            return UnregisterDeviceNotification(hHandle);

        }

        /// <summary>

        /// Helper to get the HID guid.

        /// </summary>

        public static Guid HIDGuid
        {

            get
            {

                Guid gHid;

                HidD_GetHidGuid(out gHid);

                return gHid;

            }

        }

        #endregion

    }
 


    internal delegate HidDevice HidDeviceCreatorDelegate(string devicePath);


    [Serializable]
    internal class HidDeviceException : Exception
    {
        public static HidDeviceException GenerateWithWinError(string strMessage)
        {

            return new HidDeviceException(string.Format("Msg:{0} WinEr:{1:X8}", strMessage, Marshal.GetLastWin32Error()));

        }


        public HidDeviceException() { }
        public HidDeviceException(string message) : base(message) { }
        public HidDeviceException(string message, Exception inner) : base(message, inner) { }
        protected HidDeviceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    internal class HidDevice : Win32Usb, IDisposable
    {

        #region Static stuff



        /// <summary>

        /// Finds a device given its PID and VID

        /// </summary>

        /// <param name="vendorId">Vendor id for device (VID)</param>

        /// <param name="productId">Product id for device (PID)</param>

        /// <param name="creatorDelegate">A delgate that will be invoked to create the actual device class.</param>

        /// <returns>A new device class of the given type or null</returns>

        public static IList<HidDevice> FindDevice(int vendorId, int productId)
        {

            return HidDevice.FindDevice(

                    vendorId,

                    productId,

                    delegate(string devicePath) { return new HidDevice(devicePath); }

            );

        }



        /// <summary>

        /// Finds a device given its PID and VID

        /// </summary>

        /// <param name="vendorId">Vendor id for device (VID)</param>

        /// <param name="productId">Product id for device (PID)</param>

        /// <param name="creatorDelegate">A delgate that will be invoked to create the actual device class.</param>

        /// <returns>A new device class of the given type or null</returns>

        public static IList<HidDevice> FindDevice(int vendorId, int productId, HidDeviceCreatorDelegate creatorDelegate)
        {

            List<HidDevice> devices = new List<HidDevice>();
            string searchString = string.Format("vid_{0:x4}&pid_{1:x4}", vendorId, productId); // first, build the path search string

            Guid gHid;

            HidD_GetHidGuid(out gHid);      // next, get the GUID from Windows that it uses to represent the HID USB interface

            IntPtr hInfoSet = SetupDiGetClassDevs(ref gHid, null, IntPtr.Zero, DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);      // this gets a list of all HID devices currently connected to the computer (InfoSet)

            try
            {

                DeviceInterfaceData interfaceData = new DeviceInterfaceData();  // build up a device interface data block

                interfaceData.Size = Marshal.SizeOf(interfaceData);

                // Now iterate through the InfoSet memory block assigned within Windows in the call to SetupDiGetClassDevs

                // to get device details for each device connected

                int index = 0;

                while (SetupDiEnumDeviceInterfaces(hInfoSet, 0, ref gHid, (uint)index, ref interfaceData))      // this gets the device interface information for a device at index 'nIndex' in the memory block
                {

                    string devicePath = GetDevicePath(hInfoSet, ref interfaceData); // get the device path (see helper method 'GetDevicePath')

                    if (devicePath.IndexOf(searchString) >= 0)      // do a string search, if we find the VID/PID string then we found our device!
                    {

                        HidDevice newDevice = creatorDelegate(devicePath);      // create an instance of the class for this device

                        devices.Add(newDevice);

                    }

                    index++;        // if we get here, we didn't find our device. So move on to the next one.

                }

            }

            finally
            {

                // Before we go, we have to free up the InfoSet memory reserved by SetupDiGetClassDevs

                SetupDiDestroyDeviceInfoList(hInfoSet);

            }

            return devices;

        }



        /// <summary>

        /// Helper method to return the device path given a DeviceInterfaceData structure and an InfoSet handle.

        /// Used in 'FindDevice' so check that method out to see how to get an InfoSet handle and a DeviceInterfaceData.

        /// </summary>

        /// <param name="hInfoSet">Handle to the InfoSet</param>

        /// <param name="interfaceData">DeviceInterfaceData structure</param>

        /// <returns>The device path or null if there was some problem</returns>

        private static string GetDevicePath(IntPtr hInfoSet, ref DeviceInterfaceData oInterface)
        {

            uint nRequiredSize = 0;

            // Get the device interface details

            if (!SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, IntPtr.Zero, 0, ref nRequiredSize, IntPtr.Zero))
            {

                DeviceInterfaceDetailData oDetail = new DeviceInterfaceDetailData();

                oDetail.Size = 5;       // hardcoded to 5! Sorry, but this works and trying more future proof versions by setting the size to the struct sizeof failed miserably. If you manage to sort it, mail me! Thx

                if (SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, ref oDetail, nRequiredSize, ref nRequiredSize, IntPtr.Zero))
                {

                    return oDetail.DevicePath;

                }

            }

            return null;

        }

        #endregion



        #region Privates variables

        /// <summary>Filestream we can use to read/write from</summary>

        private FileStream m_File;

        /// <summary>Length of input report : device gives us this</summary>

        private int m_InputReportLength;

        /// <summary>Length if output report : device gives us this</summary>

        private int m_OutputReportLength;

        /// <summary>Handle to the device</summary>

        private IntPtr m_hHandle;



        private int m_VendorId;

        private int m_ProductId;

        private int m_VersionNumber;



        private string m_Manufacturer;

        private string m_Product;






        #endregion



        #region Construction related



        /// <summary>

        /// Constructs the device

        /// </summary>

        /// <param name="path">Path to the device</param>

        public string Path { get; set; }
        public HidDevice(string path)
        {

            // Create the file from the device path

            //m_hHandle = CreateFile(path, GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
            m_hHandle = CreateFile(path, 0, 3, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

            Path = path;

            if (m_hHandle != InvalidHandleValue)    // if the open worked...
            {

                InitializeAttributes();

                InitializeManufacturer();

                InitializeProduct();

                InitializeCapabilities();

                CloseHandle(m_hHandle);
                m_hHandle = IntPtr.Zero;

                //m_File = new FileStream(

                //        new SafeFileHandle(m_hHandle, false),

                //        FileAccess.Read | FileAccess.Write,

                //        m_InputReportLength,

                //        true);  // wrap the file handle in a .Net file stream



            }

            else    // File open failed? Chuck an exception
            {

                m_hHandle = IntPtr.Zero;

                // throw HidDeviceException.GenerateWithWinError("Failed to create device file");

            }

        }



        private void InitializeAttributes()
        {

            HIDD_ATTRIBUTES attributes = new HIDD_ATTRIBUTES();
            attributes.Size = Marshal.SizeOf(attributes);
            if (HidD_GetAttributes(m_hHandle, out attributes))
            {
                m_VendorId = attributes.VendorID;
                m_ProductId = attributes.ProductID;
                m_VersionNumber = attributes.VersionNumber;
            }
            else
            {
                throw HidDeviceException.GenerateWithWinError("GetAttributes failed");
            }
        }



        private void InitializeManufacturer()
        {
            int bufferLength = 126;
            byte[] buffer = new byte[bufferLength];
            if (HidD_GetManufacturerString(m_hHandle, buffer, bufferLength))
            {
                m_Manufacturer = GetUnicodeString(buffer);
            }
            else
            {
                m_Manufacturer = "**Unknown***";                
            }
        }



        private void InitializeProduct()
        {
            int bufferLength = 126;
            byte[] buffer = new byte[bufferLength];

            if (HidD_GetProductString(m_hHandle, buffer, bufferLength))
            {
                m_Product = GetUnicodeString(buffer);
            }
            else
            {
                switch ((USBProductId)(m_VendorId << 16 | m_ProductId))
                {
                    case USBProductId.Glossmeter:
                    case USBProductId.USB_PRODUCT2:
                        m_Product = "*Glossmeter";
                        break;

                    case USBProductId.USB_CURVEX_3:
                    case USBProductId.USB_CURVEX_3a:
                        m_Product = "*CurveX 3";
                        break;

                    case USBProductId.GRADIENT_OVEN:
                        m_Product = "*GRO";
                        break;

                    case USBProductId.USB_THERMOCOUPLE_SIMULATOR:
                        m_Product = "*TCT";
                        break;

                    default:
                        m_Product = "**Unknown**";
                        break;
                }
            }

        }

        public enum USBProductId
        {
            Glossmeter = 0x1b4cFF00,
            USB_PRODUCT2 = 0x15CD0011,
            USB_CURVEX_3 = 0x2047FFFE,
            USB_CURVEX_3a = 0x20470827,
            GRADIENT_OVEN = 0x04037B60,
            USB_THERMOCOUPLE_SIMULATOR = 0x20470828,

        }


        private void InitializeCapabilities()
        {

            IntPtr lpData;
            if (HidD_GetPreparsedData(m_hHandle, out lpData))       // get windows to read the device data into an internal buffer
            {
                try
                {

                    HidCaps caps;

                    HidP_GetCaps(lpData, out caps); // extract the device capabilities from the internal buffer

                    m_InputReportLength = caps.InputReportByteLength;       // get the input...

                    m_OutputReportLength = caps.OutputReportByteLength;     // ... and output report lengths

                }

                finally
                {

                    HidD_FreePreparsedData(ref lpData);     // before we quit the funtion, we must free the internal buffer reserved in GetPreparsedData

                }

            }

            else    // GetPreparsedData failed? Chuck an exception
            {

                throw HidDeviceException.GenerateWithWinError("GetPreparsedData failed");

            }

        }



        private string GetUnicodeString(byte[] buffer)
        {

            int contentLength = 0;

            for (int i = 0; i < buffer.Length; i += 2)
            {

                if (buffer[i] == 0 && buffer[i + 1] == 0)
                {

                    contentLength = i;

                    break;

                }

            }

            return new UnicodeEncoding().GetString(buffer, 0, contentLength);

        }



        #endregion



        #region Privates/protected




        #endregion



        #region Publics



        /// <summary>

        /// Event handler called when device has been removed

        /// </summary>

        //public event EventHandler OnDeviceRemoved;



        public int VendorId
        {

            get { return m_VendorId; }

        }



        public int ProductId
        {

            get { return m_ProductId; }

        }



        public int VersionNumber
        {

            get { return m_VersionNumber; }

        }



        public string Manufacturer
        {

            get { return m_Manufacturer; }

        }



        public string Product
        {

            get { return m_Product; }

        }



        /// <summary>

        /// Accessor for output report length

        /// </summary>

        public int OutputReportLength
        {

            get { return m_OutputReportLength; }

        }



        /// <summary>

        /// Accessor for input report length

        /// </summary>

        public int InputReportLength
        {

            get { return m_InputReportLength; }

        }



        #endregion



        #region IDisposable Members

        /// <summary>

        /// Dispose method

        /// </summary>

        public void Dispose()
        {

            Dispose(true);

            GC.SuppressFinalize(this);

        }

        /// <summary>

        /// Disposer called by both dispose and finalise

        /// </summary>

        /// <param name="bDisposing">True if disposing</param>

        protected virtual void Dispose(bool bDisposing)
        {

            if (bDisposing) // if we are disposing, need to close the managed resources
            {

                if (m_File != null)
                {

                    m_File.Close();

                    m_File = null;

                }

            }

            if (m_hHandle != IntPtr.Zero)   // Dispose and finalize, get rid of unmanaged resources
            {

                CloseHandle(m_hHandle);

            }

        }

        #endregion

    }


}
