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
using TQC.USBDevice;

namespace TestBounceApplication
{
    public partial class Form1 : Form
    {
        private USBLogger.USBProductId ProductId = USBLogger.USBProductId.GRADIENT_OVEN;
        int m_Id = 0;
        bool m_DoTest;
        bool m_IsConnectButtonClose;
        bool m_IsTestButtonStop;
        TQCUsbLogger m_DataLogger;
        int m_Count;
        int m_NumberOfThreads = 10;
        Stopwatch m_StopWatch;
        List<System.ComponentModel.BackgroundWorker> backgroundWorkers = new List<BackgroundWorker>();

        public Form1()
        {
            //ProductId = USBLogger.USBProductId.Glossmeter;
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            CreateBackGroundWorkers();
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
            }
            else
            {
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
            var logger = new TQCUsbLogger();
            CloseLogger();
            if (logger.Open(ProductId, miniumum))
            {
                return logger;
            }
            throw new Exception("Failed to connect to logger " + ProductId.ToString());
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
                Thread.Sleep(0);
                if (m_DoTest && m_DataLogger != null)
                {
                    IssueCommand();
                }
            }            
        }
        
        private void IssueCommand()
        {
            byte[] result = null;
            Exception ex = null;

            try
            {
                result = m_DataLogger.BounceCommand(0, m_Id%255);
                m_Count++;
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
                backgroundWorkers[0].ReportProgress(0, state);
            }
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
                m_RequestId.Text = state.RequestId.ToString();
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
            if (m_StopWatch != null && m_DoTest && m_StopWatch.ElapsedMilliseconds > 0)
            {
                result = string.Format("{0:.0} packets/sec", m_Count / ((float)m_StopWatch.ElapsedMilliseconds / 1000.0f));
            }
            m_RequestsPerSecond.Text = (m_TickerCounter %2 == 0 ? " " : "*") +result;
        }
    }

    class ProgressState
    {
        public int RequestId { get; set; }
        public Byte[] Result{ get; set; }
        public Exception Exception { get; set; }
    }

}
