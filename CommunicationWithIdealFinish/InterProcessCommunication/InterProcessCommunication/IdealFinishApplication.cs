using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TQC.GOC.InterProcessCommunication.Installer;

namespace TQC.GOC.InterProcessCommunication
{
    internal class IdealFinishApplication
    {
        public static void StartUp()
        {
            string programName = "TQC.IdealFinish.Analysis.exe";
            if (!IsProgramRunning(programName))
            {
                Thread myNewThread = new Thread(() => RunFirstInstalledProgram(new Guid("{C97015C6-5C71-47DA-B8EB-418115005B5D}"), programName));
                myNewThread.Start();
                
            }
        }

        private static bool IsProgramRunning(string programName)
        {
            programName = programName.ToLowerInvariant();
            bool isRunning = false;            
            try
            {         
                
                foreach (var process in Process.GetProcesses())
                {
                    try
                    {
                        if (process.MainModule.FileName.ToLowerInvariant().EndsWith(programName))
                        {
                            isRunning = true;
                        }

                    }
                    catch (System.Exception)
                    {

                    }
                }
                // This code assumes the process you are starting will terminate itself.  
                // Given that is is started without a window so you cannot terminate it  
                // on the desktop, it must terminate itself or you can do it programmatically 
                // from this application using the Kill method.
            }
            catch (Exception )
            {
                
            }
            return isRunning;
        }

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);



        private static void RunFirstInstalledProgram(Guid upgradeInstallCode, string programName)
        {
            var idealFinishInstallations = InstallerInformation.GetProducts(upgradeInstallCode);
            foreach (var idealFinishInstallation in idealFinishInstallations)
            {
                if (Directory.Exists(idealFinishInstallation.InstalledPath))
                {
                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo();
                    process.StartInfo.FileName = Path.Combine(idealFinishInstallation.InstalledPath, programName);
                    if (process.Start())
                    {
                        while (process.MainWindowHandle == IntPtr.Zero && !process.HasExited)
                        {
                            Thread.Sleep(100);
                        }
                        if (process.MainWindowHandle != IntPtr.Zero)
                        {
                            Thread.Sleep(1000);
                            ShowWindow(process.MainWindowHandle, SW_SHOWMINIMIZED);
                        }
                    }
                    
                    break;
                }
            }
        }
    }
}
