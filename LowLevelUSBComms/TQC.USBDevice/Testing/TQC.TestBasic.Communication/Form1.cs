using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TQC.USBDevice;
using TQC.USBDevice.ThermocoupleSimulator;

namespace TQC.TestBasic.Communication
{
    public partial class Form1 : Form, IUsbInterfaceForm
    {
        Configuration m_Config = new Configuration();
        TQCUsbLogger m_Logger;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = m_Config.UseNativeCommunication;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (m_Config.UseNativeCommunication != checkBox1.Checked)
            {
                m_Config.UseNativeCommunication = checkBox1.Checked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UsbDevices devs = new UsbDevices();
            var allDevices = devs.GetUSBDevices();
            m_Devices.Items.Clear();
            foreach (var device in allDevices)
            {
                m_Devices.Items.Add(device);
                AppendText(device.ToFullString());
            }
            if (m_Devices.Items.Count > 0)
            {
                m_Devices.SelectedIndex = 0;
            }
        }

        private void m_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        
        private void m_OpenPort_Click(object sender, EventArgs e)
        {
            if (m_Logger != null)
            {
                AppendText("Aleady Open, Need to close logger first");
            }
            else
            {
                m_Logger = new TQCUsbLogger(this);
                //m_Logger = new ThermocoupleSimulator(this);
                var device = m_Devices.SelectedItem as SerialOrUsbPort;
                
                if (device == null)
                {
                    AppendText("Did not like device selected");
                    m_Logger.OpenWithMinumumRequests(USBLogger.USBProductId.USB_THERMOCOUPLE_SIMULATOR);
                }
                else
                {
                    checkBox1.Enabled = false;
                    USBLogger.USBProductId product = (USBLogger.USBProductId)device.PidVid;
                    AppendText("Open product {0}", product);
                    try
                    {
                        if (m_Logger.OpenWithMinumumRequests(product))
                        {
                            AppendText("Open product {0} successfully", product);
                        }
                        else
                        {
                            AppendText("***Failed to open");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendText(ex.ToString());
                    }

                }
            }
        }
        protected override void WndProc(ref Message aMessage)
        {
            const int WM_QUERYENDSESSION = 0x0011;
            const int WM_ENDSESSION = 0x0016;
            const int WM_POWERBROADCAST = 0x0218;
            //const int PBT_APMSUSPEND = 0x0004;
            const int PBT_APMRESUMEAUTOMATIC = 0x0012;
            const int PBT_APMQUERYSUSPEND = 0x0000;
            const int BROADCAST_QUERY_DENY = 0x424D5144; // Return this value to deny a query.
            if (m_Logger != null)
            {
                m_Logger.OnWindowsMessage(ref aMessage);
            }
            base.WndProc(ref aMessage);

        }

        public void AppendText(string value)
        {
            m_Output.AppendText(value);
            m_Output.AppendText("\r\n");

        }

        public void AppendText(string value, params object[] parmaters)
        {
            m_Output.AppendText(string.Format(value, parmaters));
            m_Output.AppendText("\r\n");

        }

        private void m_ClosePort_Click(object sender, EventArgs e)
        {
            if (m_Logger == null)
            {
                AppendText("Logger not open");
            }
            else
            {
                checkBox1.Enabled = true;
                AppendText("Closing logger");
                try
                {
                    m_Logger.Dispose();
                }
                catch (Exception ex)
                {
                    AppendText(ex.ToString());
                }
                m_Logger = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var serialNumber = m_Logger.LoggerSerialNumber;
                AppendText("Serial Number = {0}", serialNumber);
            }
            catch (Exception ex)
            {
                AppendText(ex.ToString());
            }
            
        }
    }

}
