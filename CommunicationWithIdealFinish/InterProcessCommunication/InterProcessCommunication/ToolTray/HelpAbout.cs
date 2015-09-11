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
using log4net;
using TQC.GOC.InterProcessCommunication.Installer;

namespace TQC.GOC.InterProcessCommunication.ToolTray
{
    public partial class HelpAbout : Form
    {
        private static ILog s_Log = LogManager.GetLogger("TQC.Help");

        public static void LogApplicationDetails()
        {

            StringBuilder extraInformation = new StringBuilder();

            FormatAssemblyTitleAndVersion(extraInformation, Assembly.GetEntryAssembly());
            s_Log.Info(extraInformation.ToString());
            extraInformation.Clear();
            var copyright = Assembly.GetEntryAssembly().GetCustomAttribute(typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;
            s_Log.Info(string.Format("{0}", copyright.Copyright));
            s_Log.Info("");


            Assembly assembly = Assembly.GetCallingAssembly();
            var desc = assembly.GetCustomAttribute(typeof(AssemblyDescriptionAttribute)) as AssemblyDescriptionAttribute;
            s_Log.Info(string.Format("{0}", desc.Description));
            

            var title = assembly.GetCustomAttribute(typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
            s_Log.Info(string.Format("{0} V{1}", title.Title, assembly.GetName().Version.ToString()));
            
            copyright = assembly.GetCustomAttribute(typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;
            s_Log.Info(string.Format("{0}", copyright.Copyright));
            s_Log.Info("");

            

            
            if (FormatAssemblyTitleAndVersion(extraInformation, "usbgenericlogger.dll"))
            {
                s_Log.Info(extraInformation.ToString());
                s_Log.Info("");
                extraInformation.Clear();
            }

            if (FormatAssemblyTitleAndVersion(extraInformation, "tqc.usbdevice.dll"))
            {
                s_Log.Info(extraInformation.ToString());
                s_Log.Info("");
                extraInformation.Clear();
            }

            var idealFinishInstallations = InstallerInformation.GetProducts(IdealFinishApplication.IdealFinishUpgradeCode);
            foreach (var idealFinishInstallation in idealFinishInstallations)
            {
                s_Log.Info(String.Format("Ideal Finish Analysis installed @ '{0}'", idealFinishInstallation.InstalledPath));
            }  

            
        }
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

            LogApplicationDetails();
            
        }

        private static bool FormatAssemblyTitleAndVersion(StringBuilder extraInformation, string dllName)
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
