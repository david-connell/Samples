using System;
using System.IO;
namespace TQC.GOC.InterProcessCommunication
{
    interface IGOCInterProcessServer : IDisposable
    {
        /// <summary>
        /// Event called on Connecting to Ideal Finish
        /// </summary>
        event ConnectHandler Connect;
        /// <summary>
        /// Event called on disconnecting from Ideal Finish
        /// </summary>
        event ConnectHandler Disconnect;

        event ExceptionHandler ExceptionThrown;

        /// <summary>
        /// The server is created at this point. It's Thread safe and is non-blocking
        /// </summary>
        /// <param name="writer"></param>
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
