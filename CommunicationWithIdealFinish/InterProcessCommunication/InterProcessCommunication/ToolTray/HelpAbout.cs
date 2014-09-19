using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TQC.GOC.InterProcessCommunication.ToolTray
{
    public partial class HelpAbout : Form
    {
        public HelpAbout()
        {
            InitializeComponent();
            Assembly assembly = Assembly.GetCallingAssembly();
            var desc = assembly.GetCustomAttribute(typeof(AssemblyDescriptionAttribute))  as AssemblyDescriptionAttribute;
            m_Name.Text = desc.Description;

            var title = assembly.GetCustomAttribute(typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
            Text += " - " + title.Title;

            var copyright = assembly.GetCustomAttribute(typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;
            m_Copyright.Text = copyright.Copyright;

            m_Version.Text = String.Format("V{0}", assembly.GetName().Version.ToString());

        }

        private void m_OK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
