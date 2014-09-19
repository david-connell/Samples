using System;
using System.Windows.Forms;
using System.Drawing;
using TQC.GOC.InterProcessCommunication.ToolTray;
using System.Windows.Forms.DataVisualization.Charting;
using TQC.GOC.InterProcessCommunication.Model;


namespace TQC.GOC.InterProcessCommunication
{
    internal partial class DetailsForm : Form
    {
        ToolTrayUI m_Parent;
        public DetailsForm(ToolTrayUI parent)
        {
            InitializeComponent();
            m_Parent = parent;            
            parent.Update += parent_Update;
            parent.DataRateUpdate += parent_DataRateUpdate;

        }

        void parent_DataRateUpdate(object sender, EventArgs e)
        {
            DataRateUpdate();
        }

        private void DataRateUpdate()
        {
            if (InvokeRequired && !IsDisposed && !Disposing)
            {
                try
                {
                    Invoke(((MethodInvoker)(() => DataRateUpdate())));
                }
                catch (Exception)
                {
                }
            }
            else if (!IsDisposed && !Disposing)
            {
                m_DataRate.Text = m_Parent.DataRate.ToString();
            }
        }

        void parent_Update(object sender, EventArgs e)
        {
             UpdateUI();
        }

        private void UpdateUI()
        {
            if (InvokeRequired && !IsDisposed && !Disposing)
            {
                try
                {
                    Invoke((MethodInvoker)(() => UpdateUI()));
                }
                catch (Exception )
                {
                }

            }
            else if (!IsDisposed && !Disposing)
            {
                m_ConnectionStatus.Text = m_Parent.Status;
                m_Path.Text = m_Parent.Path;
                m_Version.Text = m_Parent.Version;

            }
        }
        private void DetailsForm_Load(object sender, EventArgs e)
        {
            UpdateUI();
        }
        
        private void timer2_Tick(object sender, EventArgs e)
        {
            m_ProtocolErrors.Text = m_Parent.ProtocolErrors.ToString() ;
            m_TimeSinceLastPing.Text = (DateTime.Now - m_Parent.LastPing).ToString();
            m_ServerState.Text = m_Parent.ServerState;
            m_QueueSize.Text = m_Parent.QueueSize.ToString();
            m_NumberOfSamples.Text = m_Parent.NumberOfSamples.ToString();
            var series = chart1.Series["CurrentTemperatures"];
            series.Points.Clear();
            SamplePoint readings = m_Parent.CurrentReadings;
            if (readings != null)
            {
                for (int i = 0; i < readings.Samples.Length; i++)
                {                    
                    series.Points.AddXY(i + 1, readings.Samples[i]);                    
                }
            }
            //series.ChartType = SeriesChartType.FastLine;
            //series.Color = Color.Red;


            DataRateUpdate();            
        }

        
        

    }
}
