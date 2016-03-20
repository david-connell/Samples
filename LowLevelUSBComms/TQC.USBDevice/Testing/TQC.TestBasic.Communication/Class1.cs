using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using log4net.Core;

namespace TQC.TestBasic.Communication
{
    public class ACMAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            Program.s_MainFrame.AppendText(RenderLoggingEvent(loggingEvent));
        }
    }

}
