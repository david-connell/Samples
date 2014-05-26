using System;
using System.Windows.Forms;
using System.Drawing;
using TQC.GOC.InterProcessCommunication.ToolTray;


namespace TQC.GOC.InterProcessCommunication
{
    internal partial class DetailsForm : Form
    {
        ToolTrayUI m_Parent;
        public DetailsForm(ToolTrayUI parent)
        {
            InitializeComponent();
            m_Parent = parent;
            dataGridView.CellFormatting += hostsDataGridView_CellFormatting;
            parent.Update += parent_Update;
            parent.DataRateUpdate += parent_DataRateUpdate;

        }

        void parent_DataRateUpdate(object sender, EventArgs e)
        {
            DataRateUpdate();
        }

        private void DataRateUpdate()
        {
            m_DataRate.Text = m_Parent.DataRate.ToString();
        }

        void parent_Update(object sender, EventArgs e)
        {
             Update();
        }

        private void Update()
        {
            m_ConnectionStatus.Text = m_Parent.Status;
            m_Path.Text = m_Parent.Path;
            m_Version.Text = m_Parent.Version;
            
        }
        private void DetailsForm_Load(object sender, EventArgs e)
        {
            Update();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            m_ProtocolErrors.Text = m_Parent.ProtocolErrors.ToString() ;
            label4.Text = (DateTime.Now - m_Parent.LastPing).ToString();
            DataRateUpdate();            
        }

        private void hostsDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
     
        }
        

    }
}
