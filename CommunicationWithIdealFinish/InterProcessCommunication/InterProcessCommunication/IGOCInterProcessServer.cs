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

        event GOCServerStatusHandler GOCServerStatus;

        /// <summary>
        /// The server is created at this point. It's Thread safe and is non-blocking
        /// </summary>
        /// <param name="writer"></param>
        void CreateServer(TextWriter writer);

        /// <summary>
        /// Last time ping was sent
        /// </summary>
        DateTime LastPing { get; }
    }



    public enum GOCServerStatus
    {        
        DataFolderRecieved,
        SendingDataHeader,
        SendingDataSamples,
        SendEndOfData,
        PingIng,

    }

    public class GOCServerStatusEventArgs : EventArgs
    {
        public GOCServerStatus Status { get; private set; }
        public bool ProtocolStatus { get; private set; }
        public GOCServerStatusEventArgs(GOCServerStatus status, bool protocolStatus)
        {
            Status = status;
            ProtocolStatus = protocolStatus;
        }
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
    public delegate void GOCServerStatusHandler(object sender, GOCServerStatusEventArgs e);
}
