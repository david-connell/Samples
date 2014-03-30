using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TQC.GOC.InterProcessCommunication;
using TQC.GOC.InterProcessCommunication.Model;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        ConsoleTextWriter m_TextWriter = new ConsoleTextWriter();
        GOCServerImplementation m_Server = new GOCServerImplementation();
        public Form1()
        {
            InitializeComponent();

            
            m_Server.Connect += m_Server_Connect;
            m_Server.Disconnect += m_Server_Disconnect;
            m_Server.ExceptionThrown += m_Server_ExceptionThrown;
            m_Server.GOCServerStatus += m_Server_GOCServerStatus;

            //Hook this up afterwards to make sure that we don't miss anything...
            m_Server.CreateServer(m_TextWriter);
        }

        void m_Server_GOCServerStatus(object sender, GOCServerStatusEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke((MethodInvoker)(() => m_Server_GOCServerStatus(sender, e)));
                return;
            }
            switch (e.Status)
            {
                case GOCServerStatus.DataFolderRecieved:
                    {
                        m_Version.Text = m_Server.IdealFinishAnalysisVersion.ToString();
                        m_Path.Text = m_Server.DataFolder;
                    }
                    break;
                case GOCServerStatus.PingIng:
                    //Ignore
                    break;
                case GOCServerStatus.SendingDataHeader:
                case GOCServerStatus.SendingDataSamples:
                case GOCServerStatus.SendEndOfData:
                    TransmittedData(1);
                    break;


            }
        }

        DateTime m_CurrentPoint = DateTime.Now;
        int m_PacketsPerSecond = 0;

        private void TransmittedData(int packets)
        {
            DateTime currentTime = DateTime.Now;
            if ((currentTime - m_CurrentPoint).TotalSeconds > 1)
            {
                m_CurrentPoint = currentTime;
                m_PacketsPerSecond = packets;
            }
            else
            {
                m_PacketsPerSecond += packets;
            }
            m_DataRate.Text = m_PacketsPerSecond.ToString();

        }

        void m_Server_ExceptionThrown(object sender, ExceptionEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke((MethodInvoker)(() => m_Server_ExceptionThrown(sender, e)));
                return;
            }

            MessageBox.Show(e.Exception.Message);
        }

        void m_Server_Connect(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke((MethodInvoker ) (() =>m_Server_Connect(sender, e)));
                return;
            }
            m_Connected.Text = "CONNECTED";
        }

        void m_Server_Disconnect(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke((MethodInvoker)(() => m_Server_Disconnect(sender, e)));
                return;
            }

            m_Connected.Text = "DISCONNECTED";
            m_Version.Text = "";
            m_Path.Text = "";

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_Debug.Text = m_TextWriter.ToString();
            TransmittedData(0);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Server.Dispose();
        }
        DataRunDetail m_DataRunDetail;
        int m_PointId;
        private void Start(object sender, EventArgs e)
        {
            
            m_PointId = 0;
            
            m_DataRunDetail = new DataRunDetail(
                    "0001-111",
                    new List<Channel>() 
                    { 
                        new Channel("ChannelA", ChannelType.Temperature),
                        new Channel("ChannelB", ChannelType.Temperature),
                        new Channel("ChannelC", ChannelType.Temperature),
                        new Channel("ChannelD", ChannelType.Temperature),
                        new Channel("ChannelE", ChannelType.Temperature),
                        new Channel("ChannelF", ChannelType.Temperature),

                    },
                    DateTime.Now,
                    1,
                    Environment.UserName
                    );
            m_Server.DataRunStart(m_DataRunDetail);
            
        }

        private void SendSample(object sender, EventArgs e)
        {
            ++m_PointId;
            double dataPoint = m_PointId;
            DateTime sampleTime = m_DataRunDetail.StartOfRun.AddSeconds(m_PointId * m_DataRunDetail.SampleRate);
            List<double> vals = new List<double>();
            for (int channelId = 0; channelId < m_DataRunDetail.NumberOfChannels; channelId++)
            {
                vals.Add(dataPoint + channelId * 2);
            }
            m_Server.Data(
                new SamplePoint(sampleTime, vals.ToArray()));
        }

        private void Stop(object sender, EventArgs e)
        {
            m_Server.DataRunStop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label4.Text = (DateTime.Now - m_Server.LastPing).ToString();
        }
    }

    public class ConsoleTextWriter : TextWriter
    {
        StringBuilder m_output = new StringBuilder();
        public override void Write(char value)
        {
            m_output.Append(value);
            
        }
        public override string ToString()
        {
            return m_output.ToString();
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}
