using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using Microsoft.Win32;

namespace TQC.GOC.InterProcessCommunication.ToolTray
{
    internal partial class LogFormDetails : Form
    {
        ToolTrayUI m_Parent;
        internal LogFormDetails(ToolTrayUI parent)
        {
            InitializeComponent();
            m_Parent = parent;            
            parent.Update += parent_Update;
            Icon = parent.Icon;
            m_LogFile.Text = ApplicationLogFiles[0];
            m_UsbLogfilePath.Text = UsbLogPath;
            m_IdealFinishLogFile.Text = IdealFinishLogFileLocation;
            m_Warning.Visible = false;
        }


        public LogFormDetails()
        {
            InitializeComponent();
            
        }

        private string UsbLogPath
        {
            get
            {
                return Registry.GetValue(@"HKEY_CURRENT_USER\Software\TQC\USBGeneric", "DebugFileName", "") as string;
            }
            set
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\TQC\USBGeneric", "DebugFileName", value) ;
            }

        }

        private string IdealFinishLogFileLocation
        {
            get
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                folder = Path.Combine(folder, "Debug");
                folder = Path.Combine(folder, "TQC");
                return Path.Combine(folder, "IdealFinish.txt");
            }
            set
            {
                
            }

        }


        string[] ApplicationLogFiles
        {
            get
            {
                var rootAppenders = ((Hierarchy)LogManager.GetRepository())
                                         .Root.Appenders.OfType<FileAppender>().ToList();


                var rootRolliongAppenders = ((Hierarchy)LogManager.GetRepository())
                                         .Root.Appenders.OfType<RollingFileAppender>()
                                         .ToList();
                List<string> files = new List<string>();
                files.AddRange(rootAppenders.Select(x=>x.File));
                files.AddRange(rootRolliongAppenders.Select(x => x.File));
                return files.ToArray();

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
                catch (Exception)
                {
                }

            }
            else if (!IsDisposed && !Disposing)
            {
                //m_ConnectionStatus.Text = m_Parent.Status;
                //m_Path.Text = m_Parent.Path;
                //m_Version.Text = m_Parent.Version;

            }
        }

        
        private void m_OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void m_ViewAppLogFile_Click(object sender, EventArgs e)
        {
            LaunchExplorerWithFile(m_LogFile.Text);
        }

        private void LaunchExplorerWithFile(string p)
        {
            var process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = "explorer.exe";
            process.StartInfo.Arguments = string.Format("/e,/select,{0}", p);
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();

        }

        private void LaunchExplorerWithPath(string p)
        {
            var process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = "explorer.exe";
            process.StartInfo.Arguments = string.Format("/e,/root,{0}", p);
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = m_UsbLogfilePath.Text;
            if (!path.EndsWith("\\"))
            {
                path = Path.GetDirectoryName(path);
            }
            LaunchExplorerWithPath(path);
        }

        private void m_UsbLogfilePath_TextChanged(object sender, EventArgs e)
        {
            UsbLogPath = m_UsbLogfilePath.Text;
            m_Warning.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LaunchExplorerWithFile(m_IdealFinishLogFile.Text);
        }

    }
}
