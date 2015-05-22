using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

            StringBuilder extraInformation = new StringBuilder();
            if (FormatAssemblyTitleAndVersion(extraInformation, "usbgenericlogger.dll"))
            {                    
                extraInformation.AppendLine();
            }
            
            if (FormatAssemblyTitleAndVersion(extraInformation, "tqc.usbdevice.dll"))
            {
                extraInformation.AppendLine();
            }
            FormatAssemblyTitleAndVersion(extraInformation, Assembly.GetEntryAssembly());
            m_VersionInfo.Text = extraInformation.ToString();
            
        }

        private bool FormatAssemblyTitleAndVersion(StringBuilder extraInformation, string dllName)
        {
            var process = Process.GetCurrentProcess();
            foreach (ProcessModule module in process.Modules)
            {
                if (module.FileName.ToLower().EndsWith(dllName))
                {
                    extraInformation.AppendFormat("{0} : v{1}.{2}.{3}", module.FileVersionInfo.FileDescription, module.FileVersionInfo.FileMajorPart, module.FileVersionInfo.FileMinorPart, module.FileVersionInfo.FileBuildPart);
                    return true;
                }
            }
            return false;
        }

        private static bool FormatAssemblyTitleAndVersion(StringBuilder extraInformation, Assembly assembly)
        {
            if (assembly != null)
            {
                var vals = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (vals != null && vals.Length == 1)
                {
                    AssemblyTitleAttribute attrib = vals[0] as AssemblyTitleAttribute;
                    if (attrib != null)
                    {
                        var name = assembly.GetName();
                        if (name != null)
                        {
                            var version = name.Version;
                            if (version != null)
                            {
                                string versionText = version.ToString();
                                if (version.Revision > 200)
                                {
                                    versionText = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
                                }

                                extraInformation.AppendFormat("{0} : v{1}", attrib.Title, versionText);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        private void m_OK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
