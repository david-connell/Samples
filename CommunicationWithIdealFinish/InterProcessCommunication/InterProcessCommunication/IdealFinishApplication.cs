using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            catch (Exception)
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

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        private enum SetWindowPosFlags : uint
        {
            /// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
            /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
            /// blocking its execution while other threads process the request.</summary>
            /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
            AsynchronousWindowPosition = 0x4000,
            /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
            /// <remarks>SWP_DEFERERASE</remarks>
            DeferErase = 0x2000,
            /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
            /// <remarks>SWP_DRAWFRAME</remarks>
            DrawFrame = 0x0020,
            /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
            /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
            /// is sent only when the window's size is being changed.</summary>
            /// <remarks>SWP_FRAMECHANGED</remarks>
            FrameChanged = 0x0020,
            /// <summary>Hides the window.</summary>
            /// <remarks>SWP_HIDEWINDOW</remarks>
            HideWindow = 0x0080,
            /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
            /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
            /// parameter).</summary>
            /// <remarks>SWP_NOACTIVATE</remarks>
            DoNotActivate = 0x0010,
            /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
            /// contents of the client area are saved and copied back into the client area after the window is sized or 
            /// repositioned.</summary>
            /// <remarks>SWP_NOCOPYBITS</remarks>
            DoNotCopyBits = 0x0100,
            /// <summary>Retains the current position (ignores X and Y parameters).</summary>
            /// <remarks>SWP_NOMOVE</remarks>
            IgnoreMove = 0x0002,
            /// <summary>Does not change the owner window's position in the Z order.</summary>
            /// <remarks>SWP_NOOWNERZORDER</remarks>
            DoNotChangeOwnerZOrder = 0x0200,
            /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
            /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
            /// window uncovered as a result of the window being moved. When this flag is set, the application must 
            /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
            /// <remarks>SWP_NOREDRAW</remarks>
            DoNotRedraw = 0x0008,
            /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
            /// <remarks>SWP_NOREPOSITION</remarks>
            DoNotReposition = 0x0200,
            /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
            /// <remarks>SWP_NOSENDCHANGING</remarks>
            DoNotSendChangingEvent = 0x0400,
            /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
            /// <remarks>SWP_NOSIZE</remarks>
            IgnoreResize = 0x0001,
            /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
            /// <remarks>SWP_NOZORDER</remarks>
            IgnoreZOrder = 0x0004,
            /// <summary>Displays the window.</summary>
            /// <remarks>SWP_SHOWWINDOW</remarks>
            ShowWindow = 0x0040,
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);


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
                            EnsureVisible(process.MainWindowHandle);
                            Thread.Sleep(1000);
                            ShowWindow(process.MainWindowHandle, SW_SHOWMINIMIZED);
                        }
                    }

                    break;
                }
            }
        }

        private static void EnsureVisible(IntPtr mainWindowHandle)
        {
            RECT rect;
            if (GetWindowRect(mainWindowHandle, out rect))
            {
                var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Top - rect.Bottom);
                Rectangle wholeScreen = new Rectangle();
                foreach (Screen scrn in Screen.AllScreens)
                {

                    wholeScreen = Rectangle.Union(wholeScreen, scrn.Bounds);
                }
                if (wholeScreen.Contains(bounds))
                {                                        
                    return;
                }

                var primaryScreen = Screen.AllScreens[0];
                var rectangle = primaryScreen.WorkingArea;
                SetWindowPos(mainWindowHandle, IntPtr.Zero, primaryScreen.Bounds.Left, rectangle.Top, rectangle.Width, rectangle.Height, SetWindowPosFlags.IgnoreZOrder | SetWindowPosFlags.ShowWindow);
                
            }
        }


    }

}
