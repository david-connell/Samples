using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TQC.USBDevice;

namespace TQC.DemoApp
{
    public partial class Form1 : Form , IUsbInterfaceForm
    {
        protected TQC.USBDevice.TQCUsbLogger m_Logger;
        bool m_Cancel; 
        AsyncOperation BackgroundWorker { get; set; }
        
        public Form1()
        {
            InitializeComponent();
            Configuration config = new Configuration();
            config.UseNativeCommunication = false;
        }

        private void m_Close_Click(object sender, EventArgs e)
        {
            m_Cancel = true;
        }

        private void m_Start_Click(object sender, EventArgs e)
        {
            BackgroundWorker = AsyncOperationManager.CreateOperation(null);
            ThreadPool.QueueUserWorkItem((o) => ThreadWorkerClamp());
        }
        static int OK;
        static int Error;

        private void ThreadWorkerClamp()
        {
            while (!m_Cancel)
            {
                try
                {
                    var result = m_Logger.Version;
                    OK++;
                }
                catch (Exception )
                {
                    Error++;
                }
                finally
                {

                }
            }
            return;
        }
        bool m_IsInitialized = false;
        protected override void WndProc(ref Message aMessage)
        {
            if (m_IsInitialized)
            {
                m_Logger.OnWindowsMessage(ref aMessage);
            }
            base.WndProc(ref aMessage);
        }

        private void m_Open_Click(object sender, EventArgs e)
        {
            
            //m_Logger.Close();
            m_Logger.OpenWithMinumumRequests(USBLogger.USBProductId.USB_CURVEX_3a);
            m_IsInitialized = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_Logger = new USBDevice.TQCUsbLogger(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_CancelLabel.Text = Error.ToString();
            mk_OKLabel.Text = OK.ToString();
        }

        private void m_DeviceArrived_Click(object sender, EventArgs e)
        {
            m_Logger.CheckDevicePresent();
        }
    }
}
