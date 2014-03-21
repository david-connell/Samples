using System;
using System.IO;
namespace TQC.GOC.InterProcessCommunication
{
    interface IGOCInterProcessServer : IDisposable
    {
        event ConnectHandler Connect;
        event ConnectHandler Disconnect;
        event ExceptionHandler ExceptionThrown;
        void CreateServer(TextWriter writer);
    }

    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; internal set; }
        public ExceptionEventArgs(Exception ex)
        {
            Exception = ex;
        }
    }
    public delegate void ConnectHandler(object sender, EventArgs e);
    public delegate void ExceptionHandler(object sender, ExceptionEventArgs e);
}
