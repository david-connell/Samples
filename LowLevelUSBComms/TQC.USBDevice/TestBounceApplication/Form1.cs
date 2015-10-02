using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using log4net;
using TQC.USBDevice;

namespace TestBounceApplication
{
    public partial class Form1 : Form, IUsbInterfaceForm
    {        
        int m_Id = 0;
        bool m_DoTest;
        bool m_IsConnectButtonClose;
        bool m_IsTestButtonStop;
        TQCUsbLogger m_DataLogger;
        int m_Count;
        int m_NumberOfThreads = 10;
        Stopwatch m_StopWatch;
        List<System.ComponentModel.BackgroundWorker> backgroundWorkers = new List<BackgroundWorker>();
        Configuration m_Configuration = new Configuration();
        private ILog m_Log = LogManager.GetLogger("TestBounce");
        USBLogger.USBProductId m_CachedProduct;

        public Form1()
        {            
            InitializeComponent();
            comboBox1.Items.Add(new UserSelection(USBLogger.USBProductId.Glossmeter));
            comboBox1.Items.Add(new UserSelection(USBLogger.USBProductId.GRADIENT_OVEN));
            comboBox1.Items.Add(new UserSelection(USBLogger.USBProductId.USB_CURVEX_3));
            comboBox1.Items.Add(new UserSelection(USBLogger.USBProductId.USB_CURVEX_3a));
            comboBox1.Items.Add(new UserSelection(USBLogger.USBProductId.USB_PRODUCT2));
            comboBox1.Items.Add(new UserSelection(USBLogger.USBProductId.USB_THERMOCOUPLE_SIMULATOR));
            comboBox1.SelectedIndex = 1;
            m_UseIFAComms.Checked = m_Configuration.UseNativeCommunication;
            CreateBackGroundWorkers();
        }
        USBLogger.USBProductId ProductId
        {
            get
            {
                USBLogger.USBProductId id = USBLogger.USBProductId.GRADIENT_OVEN;
                if (comboBox1.SelectedIndex >= 0)
                {
                    var result = comboBox1.Items[comboBox1.SelectedIndex] as UserSelection;
                    if (result != null)
                    {
                        id = result.ProductId;
                    }
                }
                return id;
            }
        }


        class UserSelection
        {
            public USBLogger.USBProductId ProductId { get; set; }
            public UserSelection(USBLogger.USBProductId id)
            {
                ProductId = id;
            }
            
            public override string ToString()
            {
                switch (ProductId)
                {                    
                    case USBLogger.USBProductId.GRADIENT_OVEN: return "GRO";
                    case USBLogger.USBProductId.Glossmeter: return "Glossmeter";
                    case USBLogger.USBProductId.USB_CURVEX_3a: return "CurveX 3 (VID=0x2047, PID=0x0827)";
                    case USBLogger.USBProductId.USB_CURVEX_3: return "CurveX 3 (VID=0x2047, PID=0xFFFE)";                    
                    case USBLogger.USBProductId.USB_THERMOCOUPLE_SIMULATOR: return "Thermocouple Simulator";                  
                }
                return string.Format("VID/PID{0:X}", (int)ProductId);
            }
        }
        bool CancellationPending { get; set; }

        private void CreateBackGroundWorkers()
        {
            for (int i = 0; i < m_NumberOfThreads; i++)
            {
                var backgroundWorker1 = new System.ComponentModel.BackgroundWorker();

                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.WorkerSupportsCancellation = true;
                backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
                backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
                backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);

                backgroundWorker1.RunWorkerAsync();
                backgroundWorkers.Add(backgroundWorker1);
            }
        }

        private void m_Connect_Click(object sender, EventArgs e)
        {
            if (m_IsConnectButtonClose)
            {
                m_IsTestButtonStop = false;
                SetStartStopButton(m_IsTestButtonStop);
                m_Connect.Text = "Open";
                m_StartStop.Enabled = false;
                m_UseIFAComms.Enabled = true;
            }
            else
            {
                m_UseIFAComms.Enabled = false;
                m_DataLogger = OpenLogger();   
                m_StartStop.Enabled = true;
                m_Connect.Text = "Close";
            }
            m_IsConnectButtonClose = !m_IsConnectButtonClose;
        }

        private void m_StartStop_Click(object sender, EventArgs e)
        {
            SetStartStopButton(!m_IsTestButtonStop);
            m_IsTestButtonStop = !m_IsTestButtonStop;
        }

        
        protected TQCUsbLogger OpenLogger(bool miniumum = true)
        {
            var logger = new TQCUsbLogger(this);
            CloseLogger();
            m_CachedProduct = ProductId;
            if (logger.Open(m_CachedProduct, miniumum))
            {
                return logger;
            }
            m_Log.Error("Failed to connect to logger " + new UserSelection(m_CachedProduct));
            throw new Exception("Failed to connect to logger " + new UserSelection(m_CachedProduct));
        }

        void CloseLogger()
        {
            m_DoTest = false;
            if (m_DataLogger != null)
            {
                m_DataLogger.Close();
                m_DataLogger.Dispose();
            }
        }
        private void SetStartStopButton(bool setToStop)
        {
            if (setToStop)
            {                
                m_DoTest = true;
                m_StopWatch = new Stopwatch();
                m_Count = 0;
                m_StopWatch.Start();

                m_StartStop.Text = "Stop";
            }
            else
            {
                m_DoTest = false;
                m_StartStop.Text = "Start";
                
            }            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!CancellationPending)
            {
                if (m_DoTest && m_DataLogger != null)
                {
                    try
                    {
                        switch (m_CachedProduct)
                        {
                            case USBLogger.USBProductId.GRADIENT_OVEN:
                                IssueGroTestCommand();
                                break;
                            case USBLogger.USBProductId.Glossmeter:
                                IssueRandomBounceCommand();
                                break;
                            default:
                                {
                                    var deviceId = (byte)(m_Count % 8);
                                    //m_DataLogger._ProbeValues((byte)(m_Count % 4));
                                    var result = m_DataLogger._SerialNumber(deviceId);
                                    m_Count++;
                                }
                                break;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        m_Log.Error("Issue Command", ex);
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }            
        }

        Random value = new Random(0);


        internal UInt32 _GetStatus(byte deviceId)
        {
            Byte[] response;
            try
            {
                response = m_DataLogger.GetResponse(deviceId, TQC.USBDevice.USBLogger.Commands.GROReadCommand, 0x09);
            }
            catch (EnumerationNotSuportedException )
            {
                return 0;
            }
            if (response == null)
            {                
                throw new NoDataReceivedException("getHarwareStatus");
            }
            if (response.Length < sizeof(UInt32))
            {                
                throw new TooLittleDataReceivedException("getHarwareStatus", response.Length, sizeof(UInt32));
            }
            UInt32 status = BitConverter.ToUInt32(response, 0);
            return status;
        }

        const int NumberOfFansPerBoard = 8;
        const int NumberOfProbesPerBoard = 8;

        byte AbsoluteFanIdToLocalFanId(short fanId)
        {
            return (byte)(fanId % NumberOfFansPerBoard);
        }

        byte AbsoluteFanIdToThermcoupleBoardID(short fanId)
        {
            return (byte)((fanId / NumberOfFansPerBoard) + 1);
        }


        public void SetTempSetting(short channelId, float temperatureSettingInDegreesC)
        {
            if (channelId < 0 || channelId > 32)
            {
                throw new ArgumentOutOfRangeException("slotId", "Valid slots 0->32");
            }
            if ((temperatureSettingInDegreesC < 0.0f) || (temperatureSettingInDegreesC > 500.0f))
            {
                throw new ArgumentOutOfRangeException("temperatureSettingInDegreesC", "Valid Temeprature 0->500C");
            }

            List<byte> request = new List<byte>();

            request.AddRange(BitConverter.GetBytes((short)(200 + AbsoluteFanIdToLocalFanId(channelId))));
            request.AddRange(BitConverter.GetBytes(((UInt16)(temperatureSettingInDegreesC * 10.0f + 0.5))));
            m_DataLogger.Request(TQC.USBDevice.USBLogger.Commands.GROSetCommand, request.ToArray(), AbsoluteFanIdToThermcoupleBoardID(channelId));
        }

        private void IssueGroTestCommand()
        {
            var deviceId = (byte)(m_Count % 2);
            //var result = m_DataLogger._SerialNumber(deviceId);
            
            float temp = (float) (35.0 + ((m_Count % 1000) - 500) / 100.0);
            m_Log.Info(string.Format("Set channel {0} to {1:0.0}", m_Count % 32, temp));
            SetTempSetting((short) (m_Count % 32), temp);

            m_DataLogger._ProbeValues((byte)((m_Count % 4)+1));

            if (deviceId != 0)
            {

            }
            _GetStatus((byte) (deviceId * (m_Count % 5)));
            m_Count++;
        }
        private void IssueRandomBounceCommand()
        {
            byte[] result = null;
            Exception ex = null;

            try
            {
                byte[] data = new byte[0];
                int length = value.Next(50);
                
                if (length > 0)
                {
                    data = new byte[length];
                    value.NextBytes(data);
                }
                Int16 commandId = (Int16)(m_Id % 255);
                result = m_DataLogger.BounceCommand(0, commandId, data);
                if (result == null)
                {
                    //m_Count++
                }
                else
                {
                    Int16 commandReadBack = BitConverter.ToInt16(result, 0);
                    if (commandId != commandReadBack)
                    {
                        Error(commandId, commandReadBack);
                    }
                    //if (data != null)
                    {
                        for (int counter = 0; counter < data.Length; counter++)
                        {
                            if (data[counter] != result[counter + 2])
                            {
                                Error(counter, data[counter], result[counter + 2]);
                            }
                        }
                    }
                    m_Count++;
                }
            }
            catch (Exception ex1)
            {
                ex = ex1;
            }
            if (ex != null)
            {
                
                ProgressState state = new ProgressState
                {
                    RequestId = m_Id++,
                    Result = result,
                    Exception = ex,
                };
                Thread.Sleep(100);
                backgroundWorkers[0].ReportProgress(0, state);
            }
        }

        private void Error(int counter, byte p1, byte p2)
        {
            throw new Exception(string.Format("Element {0} {1} != {2}", counter, p1, p2));
        }

        private void Error(Int16 p1, Int16 p2)
        {
            throw new Exception("Invalid Int16 found");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressState state = e.UserState as ProgressState;
            if (state != null)
            {
                StringBuilder builder = new StringBuilder();

                if (state.Result != null)
                {
                    builder.Append("[");
                    foreach (byte value in state.Result)
                    {
                        builder.Append((int)value);
                        builder.Append(" ");
                    }
                    builder.Append("]");
                }
                m_Errors.Text = state.RequestId.ToString();
                m_ResultBuffer.Text = builder.ToString();
                m_Exception.Text = state.Exception == null ? "" : state.Exception.Message;
            }
            return;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancellationPending = true;
            CloseLogger();
        }

        int m_TickerCounter;
        private void timer1_Tick(object sender, EventArgs e)
        {
            string result = "Not running";
            m_TickerCounter++;
            int count = m_Count;
            if (m_StopWatch != null && m_DoTest && m_StopWatch.ElapsedMilliseconds > 0)
            {
                result = string.Format("{0:.0} packets/sec", count / ((float)m_StopWatch.ElapsedMilliseconds / 1000.0f));
            }
            m_RequestsPerSecond.Text = (m_TickerCounter %2 == 0 ? " " : "*") +result;
            m_RequestId.Text = count.ToString();
        }

        private void m_UseIFAComms_CheckedChanged(object sender, EventArgs e)
        {
            m_Configuration.UseNativeCommunication = m_UseIFAComms.Checked;            
        }

        protected override void WndProc(ref Message m)
        {
            OnMessageEvent(ref m);
            base.WndProc(ref m);	    // pass message on to base form
        }
        protected virtual void OnMessageEvent(ref Message m)
        {
            MessageEventEventHandler handler = MessageEvent;
            if (handler != null)
            {

                handler(this, new MessageEventEventArgs{Message = m } );
            }
        }

        public event MessageEventEventHandler MessageEvent;
    }

    class ProgressState
    {
        public int RequestId { get; set; }
        public Byte[] Result{ get; set; }
        public Exception Exception { get; set; }
    }

}
