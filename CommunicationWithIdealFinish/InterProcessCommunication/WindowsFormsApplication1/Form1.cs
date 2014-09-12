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
            m_Server.CreateServer(m_TextWriter, components, Icon);
            m_EnableOutput.Checked = m_TextWriter.IsEnabled;
        }

        void m_Server_GOCServerStatus(object sender, GOCServerStatusEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke((MethodInvoker)(() => m_Server_GOCServerStatus(sender, e)));
                return;
            }
            if (!e.ProtocolStatus)
            {
                
            }
            switch (e.Status)
            {
                case GOCServerStatus.DataFolderRecieved:
                    {
                    }
                    break;
                case GOCServerStatus.PingIng:
                    //Ignore
                    break;
                case GOCServerStatus.SendingDataHeader:
                case GOCServerStatus.SendingDataSamples:
                case GOCServerStatus.SendEndOfData:
                    
                    break;


            }
        }

        

        bool m_InShow;

        void m_Server_ExceptionThrown(object sender, ExceptionEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke((MethodInvoker)(() => m_Server_ExceptionThrown(sender, e)));
                return;
            }
            if (!m_InShow)
            {
                m_InShow = true;
                MessageBox.Show(e.Exception.Message);
                m_InShow = false;
            }
        }

        void m_Server_Connect(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke((MethodInvoker ) (() =>m_Server_Connect(sender, e)));
                return;
            }            
        }

        void m_Server_Disconnect(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke((MethodInvoker)(() => m_Server_Disconnect(sender, e)));
                return;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_Debug.Text = m_TextWriter.ToString();            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Server.Dispose();
        }
        DataRunDetail m_DataRunDetail;
        int m_PointId;
        private void Start(object sender, EventArgs e)
        {
            m_SendData.Enabled = true;
            m_PointId = 0;
            double sampleRate = 1.0; //Once a second
            m_DataRunDetail = new DataRunDetail(
                    "0001-111",
                    new List<Channel>() 
                    { 
                        new Channel("ChannellA", ChannelType.Temperature),
                        new Channel("Channel1B", ChannelType.Temperature),
                        new Channel("Channel1C", ChannelType.Temperature),
                        new Channel("Channel1D", ChannelType.Temperature),
                        new Channel("Channel1E", ChannelType.Temperature),
                        new Channel("Channel1F", ChannelType.Temperature),
                        new Channel("Channel1G", ChannelType.Temperature),
                        new Channel("Channel1H", ChannelType.Temperature),

                        new Channel("Channel2A", ChannelType.Temperature),
                        new Channel("Channel2B", ChannelType.Temperature),
                        new Channel("Channel2C", ChannelType.Temperature),
                        new Channel("Channel2D", ChannelType.Temperature),
                        new Channel("Channel2E", ChannelType.Temperature),
                        new Channel("Channel2F", ChannelType.Temperature),
                        new Channel("Channel2G", ChannelType.Temperature),
                        new Channel("Channel2H", ChannelType.Temperature),

                        new Channel("Channel3A", ChannelType.Temperature),
                        new Channel("Channel3B", ChannelType.Temperature),
                        new Channel("Channel3C", ChannelType.Temperature),
                        new Channel("Channel3D", ChannelType.Temperature),
                        new Channel("Channel3E", ChannelType.Temperature),
                        new Channel("Channel3F", ChannelType.Temperature),
                        new Channel("Channel3G", ChannelType.Temperature),
                        new Channel("Channel3H", ChannelType.Temperature),

                        new Channel("Channel4A", ChannelType.Temperature),
                        new Channel("Channel4B", ChannelType.Temperature),
                        new Channel("Channel4C", ChannelType.Temperature),
                        new Channel("Channel4D", ChannelType.Temperature),
                        new Channel("Channel4E", ChannelType.Temperature),
                        new Channel("Channel4F", ChannelType.Temperature),
                        new Channel("Channel4G", ChannelType.Temperature),
                        new Channel("Channel4H", ChannelType.Temperature),

                    },
                    DateTime.Now,
                    sampleRate,
                    "Name" + Environment.UserName
                    );
            m_Server.DataRunStart(m_DataRunDetail);
            
        }

        private void SendSample(object sender, EventArgs e)
        {
            if (m_DataRunDetail == null)
                return;
            ++m_PointId;
            double dataPoint = m_PointId;
            DateTime sampleTime = m_DataRunDetail.StartOfRun.AddSeconds(m_PointId * m_DataRunDetail.SampleRate);
            List<double> vals = new List<double>();
            for (int channelId = 0; channelId < m_DataRunDetail.NumberOfChannels; channelId++)
            {
                if (channelId == 0)
                {
                    vals.Add((dataPoint % 100)/ 10);
                }
                else
                {
                    vals.Add(Math.Sin((dataPoint+channelId) / 20.0 ) * channelId +50);
                }
            }
            m_Server.Data(
                new SamplePoint(sampleTime, vals.ToArray()));
        }

        private void Stop(object sender, EventArgs e)
        {
            m_Server.DataRunStop();
            if (SendSampleTimer.Enabled)
            {
                button3_Click(sender, e);
            }
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            bool sendingSamples = SendSampleTimer.Enabled;
            SendSampleTimer.Enabled = !sendingSamples;
            m_SendSamples.Text = string.Format("{0} sending samples", SendSampleTimer.Enabled ? "Stop" : "Start");
            m_SingleSample.Enabled = sendingSamples;
        }

        private void SendSampleTimer_Tick(object sender, EventArgs e)
        {
            SendSample(sender, e);
        }

        private void m_EnableOutput_CheckedChanged(object sender, EventArgs e)
        {
            m_TextWriter.IsEnabled = m_EnableOutput.Checked;
        }
    }

    public class ConsoleTextWriter : TextWriter
    {
        StringBuilder m_output = new StringBuilder();
        public bool IsEnabled { get; set; }
        public override void Write(char value)
        {
            if (IsEnabled)
            {
                lock (m_output)
                {
                    m_output.Append(value);
                }
            }
            
        }
        public override string ToString()
        {
            lock (m_output)
            {
                return m_output.ToString();
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}
