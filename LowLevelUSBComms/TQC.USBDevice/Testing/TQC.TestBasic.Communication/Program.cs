using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TQC.TestBasic.Communication
{
    public static class Program
    {
        public static Form1 s_MainFrame;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            s_MainFrame = new Form1();
            Application.Run(s_MainFrame);
        }
    }
}
