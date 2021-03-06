using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using log4net;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.ConstrainedExecution;
using System.Diagnostics;

namespace UsbLibrary
{
    /// <summary>
    /// Generic HID device exception
    /// </summary>
    public class HIDDeviceException : ApplicationException
    {
        public HIDDeviceException(string strMessage) : base(strMessage) { }

        public HIDDeviceException(string strMessage, Exception ex) : base(strMessage, ex) { }

        public static HIDDeviceException GenerateWithWinError(string strMessage)
        {
            return new HIDDeviceException(string.Format("Msg:{0} WinEr:{1:X8}", strMessage, Marshal.GetLastWin32Error()));
        }
        public static HIDDeviceException GenerateWithWinError(string strMessage, Exception ex)
        {
            return new HIDDeviceException(string.Format("Msg:{0} WinEr:{1:X8}", strMessage, Marshal.GetLastWin32Error()), ex);
        }

        public static HIDDeviceException GenerateError(string strMessage)
        {
            return new HIDDeviceException(string.Format("Msg:{0}", strMessage));
        }
    }


    /// <summary>
    /// Abstract HID device : Derive your new device controller class from this
    /// </summary>
    public abstract class HIDDevice : Win32Usb, IDisposable
    {

        static ILog s_Log = LogManager.GetLogger("UsbLibrary.HIDDevice");
        private FileStream m_oFile;         /// <summary>Filestream we can use to read/write from</summary>
        private int m_nInputReportLength;   /// <summary>Length of input report : device gives us this</summary>
        private int m_nOutputReportLength;  /// <summary>Length if output report : device gives us this</summary>        
        private SafeFileHandle m_hHandle;


        /// <summary>
        /// Finds a device given its PID and VID
        /// </summary>
        /// <param name="nVid">Vendor id for device (VID)</param>
        /// <param name="nPid">Product id for device (PID)</param>
        /// <param name="oType">Type of device class to create</param>
        /// <returns>A new device class of the given type or null</returns>
        public static HIDDevice FindDevice(int nVid, int nPid, Type oType)
        {
            string strPath = string.Empty;
            string strSearch = string.Format("vid_{0:x4}&pid_{1:x4}", nVid, nPid); // first, build the path search string
            Guid gHid = HIDGuid;
            IntPtr hInfoSet = SetupDiGetClassDevs(ref gHid, null, IntPtr.Zero, DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);	// this gets a list of all HID devices currently connected to the computer (InfoSet)
            try
            {
                DeviceInterfaceData oInterface = new DeviceInterfaceData();	// build up a device interface data block
                oInterface.Size = Marshal.SizeOf(oInterface);
                // Now iterate through the InfoSet memory block assigned within Windows in the call to SetupDiGetClassDevs
                // to get device details for each device connected
                int nIndex = 0;
                while (SetupDiEnumDeviceInterfaces(hInfoSet, 0, ref gHid, (uint)nIndex, ref oInterface))	// this gets the device interface information for a device at index 'nIndex' in the memory block
                {
                    string strDevicePath = GetDevicePath(hInfoSet, ref oInterface);	// get the device path (see helper method 'GetDevicePath')
                    if (strDevicePath.IndexOf(strSearch) >= 0)	// do a string search, if we find the VID/PID string then we found our device!
                    {
                        s_Log.InfoFormat("Create device {0}", oType == null ? "NULL" : oType.ToString());
                        HIDDevice oNewDevice = (HIDDevice)Activator.CreateInstance(oType);	// create an instance of the class for this device
                        oNewDevice.Initialise(strDevicePath);	// initialise it with the device path
                        return oNewDevice;	// and return it
                    }
                    nIndex++;	// if we get here, we didn't find our device. So move on to the next one.
                }
            }
            catch (Exception ex)
            {
                s_Log.Info("Find device", ex);
                throw HIDDeviceException.GenerateError(ex.ToString());
            }
            finally
            {
                // Before we go, we have to free up the InfoSet memory reserved by SetupDiGetClassDevs
                SetupDiDestroyDeviceInfoList(hInfoSet);
            }
            s_Log.InfoFormat("Failed to find device {0}", oType == null ? "NULL" : oType.ToString());
            return null;
        }

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
            try
            {
                if (bDisposing)	// if we are disposing, need to close the managed resources
                {
                    DisposeFile();
                }
                if (m_hHandle != null && !m_hHandle.IsInvalid)
                {
                    m_hHandle.Close();
                }
                m_hHandle = null;

            }
            catch (Exception ex)
            {
                s_Log.Info("Dispose", ex);
            }
        }

        private void DisposeFile()
        {
            var tempHandle = m_hHandle;
            var tempFile = m_oFile;
            m_hHandle = null;
            m_oFile = null;

            if (tempFile != null)
            {
                try
                {
                    tempFile.Dispose();
                }
                catch (Exception ex)
                {
                    s_Log.Error("Temp file dispose", ex);
                }
            }
            if (tempHandle != null && !tempHandle.IsInvalid)
            {
                if (!tempHandle.IsClosed)
                {
                    try
                    {
                        s_Log.Info("Handle Close");
                        tempHandle.Close();
                    }
                    catch (Exception ex)
                    {
                        s_Log.Error("Close handle", ex);
                    }
                }
                try
                {
                    s_Log.Info("Handle Dispose");
                    tempHandle.Dispose();
                }
                catch (Exception ex)
                {
                    s_Log.Error("Dispose handle", ex);
                }
            }
        }

        /// <summary>
        /// Initialises the device
        /// </summary>
        /// <param name="strPath">Path to the device</param>
        private void Initialise(string strPath)
        {
            s_Log.InfoFormat("Initialize {0}", strPath);
            DisposeFile();

            s_Log.WarnFormat("Open {0}", strPath);

            //m_hHandle = CreateFile(strPath, GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
            m_hHandle = CreateFile(strPath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_WRITE | FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, IntPtr.Zero);

            if (!m_hHandle.IsInvalid)	// if the open worked...
            {
                IntPtr lpData;
                if (HidD_GetPreparsedData(m_hHandle.DangerousGetHandle(), out lpData))	// get windows to read the device data into an internal buffer
                {
                    try
                    {
                        HidCaps oCaps;
                        HidP_GetCaps(lpData, out oCaps);	                    // extract the device capabilities from the internal buffer
                        m_nInputReportLength = oCaps.InputReportByteLength;     // get the input.
                        m_nOutputReportLength = oCaps.OutputReportByteLength;   // and output report lengths

                        m_oFile = new FileStream(m_hHandle, FileAccess.Read | FileAccess.Write, m_nInputReportLength, true);

                        BeginAsyncRead();                                       // kick off the first asynchronous read                              
                    }
                    catch (Exception ex)
                    {
                        throw HIDDeviceException.GenerateWithWinError("Failed to get the detailed data from the hid.", ex);
                    }
                    finally
                    {
                        HidD_FreePreparsedData(ref lpData);                     // before we quit the funtion, we must free the internal buffer reserved in GetPreparsedData
                    }
                }
                else                                                            // GetPreparsedData failed? Chuck an exception
                {
                    throw HIDDeviceException.GenerateWithWinError("GetPreparsedData failed");
                }
            }
            else                                                            // File open failed? Chuck an exception
            {
                throw HIDDeviceException.GenerateWithWinError("Failed to create device file");
            }
        }
        /// <summary>
        /// Kicks off an asynchronous read which completes when data is read or when the device
        /// is disconnected. Uses a callback.
        /// </summary>
        private bool BeginAsyncRead()
        {
            bool error = true;
            byte[] arrInputReport = new byte[m_nInputReportLength];
            // put the buff we used to receive the stuff as the async state then we can get at it when the read completes
            if (m_oFile != null)
            {
                try
                {
                    m_oFile.BeginRead(arrInputReport, 0, m_nInputReportLength, new AsyncCallback(ReadCompleted), arrInputReport);
                    error = false;
                }
                catch (ObjectDisposedException)
                {
                    //We have got a problem here!
                }

            }
            return error;
        }
        /// <summary>
        /// Callback for above. Care with this as it will be called on the background thread from the async read
        /// </summary>
        /// <param name="iResult">Async result parameter</param>
        protected void ReadCompleted(IAsyncResult iResult)
        {
            byte[] arrBuff = (byte[])iResult.AsyncState;    // retrieve the read buffer
            try
            {
                if (m_oFile == null)
                {
                    return;
                }
                m_oFile.EndRead(iResult);                   // call end read : this throws any exceptions that happened during the read
                try
                {
                    InputReport oInRep = CreateInputReport();   // Create the input report for the device
                    oInRep.SetData(arrBuff);                    // and set the data portion - this processes the data received into a more easily understood format depending upon the report type
                    HandleDataReceived(oInRep);                 // pass the new input report on to the higher level handler
                }
                finally
                {
                    BeginAsyncRead();                           // when all that is done, kick off another read for the next report
                }
            }
            catch (IOException)                                 // if we got an IO exception, the device was removed
            {
                ReadCompleteHandedException();
            }
            catch (OperationCanceledException)
            {
                ReadCompleteHandedException();
            }
            catch (System.ObjectDisposedException)
            {
                ReadCompleteHandedException();
            }
            catch (Exception ex)
            {
                s_Log.Error("Failed to Read Complete", ex);
                throw;
            }
        }

        private void ReadCompleteHandedException()
        {
            HandleDeviceRemoved();
            if (OnDeviceRemoved != null)
            {
                OnDeviceRemoved(this, new EventArgs());
            }
            Dispose();
        }


        protected class WriteCompleteTask
        {
            public ManualResetEvent Event;
            public FileStream FileStream;
        }

        protected virtual void WriteCompleted(IAsyncResult ar)
        {
            WriteCompleteTask task = (WriteCompleteTask)ar.AsyncState;
            try
            {
                task.FileStream.Flush();
                task.FileStream.EndWrite(ar);
            }
            catch (Exception ex)
            {
                s_Log.Error("Write failure", ex);
            }
            task.Event.Set();
        }
        /// <summary>
        /// Write an output report to the device.
        /// </summary>
        /// <param name="oOutRep">Output report to write</param>
        protected bool Write(OutputReport oOutRep)
        {
            bool sentData = false;
            try
            {
                const int numberOfMilliSecondsToWait = 100;
                if (m_oFile == null)
                {
                    return false;
                }
                ManualResetEvent manualEvent = new ManualResetEvent(false);

                m_oFile.BeginWrite(oOutRep.Buffer, 0, oOutRep.BufferLength, WriteCompleted, new WriteCompleteTask { FileStream = m_oFile, Event = manualEvent });

                if (!manualEvent.WaitOne(numberOfMilliSecondsToWait, false))
                {
                    throw new IOException(string.Format("Failed to complete < {0}ms", numberOfMilliSecondsToWait));
                }
                sentData = true;
            }
            catch (IOException ex)
            {
                s_Log.Info("Write IOException", ex);
                throw new HIDDeviceException("Probably the device was removed, or an invalid report ID was used");
            }
            catch (Exception exx)
            {
                s_Log.Info("Write general exception", exx);
            }
            return sentData;
        }
        /// <summary>
        /// virtual handler for any action to be taken when data is received. Override to use.
        /// </summary>
        /// <param name="oInRep">The input report that was received</param>
        protected virtual void HandleDataReceived(InputReport oInRep)
        {
        }
        /// <summary>
        /// Virtual handler for any action to be taken when a device is removed. Override to use.
        /// </summary>
        protected virtual void HandleDeviceRemoved()
        {
        }
        /// <summary>
        /// Helper method to return the device path given a DeviceInterfaceData structure and an InfoSet handle.
        /// Used in 'FindDevice' so check that method out to see how to get an InfoSet handle and a DeviceInterfaceData.
        /// </summary>
        /// <param name="hInfoSet">Handle to the InfoSet</param>
        /// <param name="oInterface">DeviceInterfaceData structure</param>
        /// <returns>The device path or null if there was some problem</returns>
        private static string GetDevicePath(IntPtr hInfoSet, ref DeviceInterfaceData oInterface)
        {
            uint nRequiredSize = 0;
            // Get the device interface details
            if (!SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, IntPtr.Zero, 0, ref nRequiredSize, IntPtr.Zero))
            {
                DeviceInterfaceDetailData oDetail = new DeviceInterfaceDetailData();
                oDetail.Size = 5;	// hardcoded to 5! Sorry, but this works and trying more future proof versions by setting the size to the struct sizeof failed miserably. If you manage to sort it, mail me! Thx
                if (SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, ref oDetail, nRequiredSize, ref nRequiredSize, IntPtr.Zero))
                {
                    return oDetail.DevicePath;
                }
            }
            return null;
        }

        
        /// <summary>
        /// Event handler called when device has been removed
        /// </summary>
        public event EventHandler OnDeviceRemoved;
        /// <summary>
        /// Accessor for output report length
        /// </summary>
        public int OutputReportLength
        {
            get
            {
                return m_nOutputReportLength;
            }
        }
        /// <summary>
        /// Accessor for input report length
        /// </summary>
        public int InputReportLength
        {
            get
            {
                return m_nInputReportLength;
            }
        }
        /// <summary>
        /// Virtual method to create an input report for this device. Override to use.
        /// </summary>
        /// <returns>A shiny new input report</returns>
        public virtual InputReport CreateInputReport()
        {
            return null;
        }
        
    }
}
