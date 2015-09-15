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
        
        public Form1()
        {
            //ProductId = USBLogger.USBProductId.Glossmeter;
            InitializeComponent();
            comboBox1.SelectedIndex = 0;            
            backgroundWorker1.RunWorkerAsync() ;
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
                m_DataLogger = OpenLogger();
                m_DoTest = true;
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
            while (!backgroundWorker1.CancellationPending)
            {
                //Thread.Sleep(100);
                if (m_DoTest && m_DataLogger != null)
                {
                    IssueCommand();
                }
            }
            CloseLogger();
        }
        
        private void IssueCommand()
        {
            byte[] result = null;
            Exception ex = null;

            try
            {
                result = m_DataLogger.BounceCommand(0, m_Id%255);
            }
            catch (Exception ex1)
            {
                ex = ex1;
            }

            ProgressState state = new ProgressState
            {
                RequestId = m_Id++,
                Result = result,
                Exception = ex,
            };
            

            backgroundWorker1.ReportProgress(0, state);
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
            backgroundWorker1.CancelAsync();
        }
    }

    class ProgressState
    {
        public int RequestId { get; set; }
        public Byte[] Result{ get; set; }
        public Exception Exception { get; set; }
    }

}
