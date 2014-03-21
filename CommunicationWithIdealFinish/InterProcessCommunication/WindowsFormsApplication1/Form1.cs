using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TQC.GOC.InterProcessCommunication;

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
            //Hook this up afterwards to make sure that we don't miss anything...
            m_Server.CreateServer(m_TextWriter);
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
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_Debug.Text = m_TextWriter.ToString();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Server.Dispose();
        }

        private void m_SendData_Click(object sender, EventArgs e)
        {

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
