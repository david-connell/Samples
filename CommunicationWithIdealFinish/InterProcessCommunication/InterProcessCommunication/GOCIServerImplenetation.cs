using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TQC.GOC.InterProcessCommunication.Model;

namespace TQC.GOC.InterProcessCommunication
{
    internal class NamedPipeReader
    {
        public NamedPipeServerStream PipeServer { get; set; }
        public byte[] Buffer { get; set; }
        public bool CanDoNextCommand { get; set; }
        public bool PipeBroken { get; set; }

        public NamedPipeReader()
        {
            Buffer = new byte[255];
        }

        public int Length
        {
            get
            {
                return Buffer.Length;
            }
        }

    }
    public class GOCServerImplementation : IIdealFinishAnalysis, IGOCInterProcessServer
    {        
        private Thread m_Server;
        private TextWriter m_Writer;
        private bool m_IsRunning;
        private EventWaitHandle m_TerminateHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        bool m_HasConnected;

        public event ConnectHandler Connect;
        public event ConnectHandler Disconnect;
        public event ExceptionHandler ExceptionThrown;

        public void CreateServer(TextWriter writer)
        {
            m_IsRunning = true;
            m_Writer = writer;
            m_Server = new Thread(ServerLoop);
            m_Server.Start();
        }

        protected virtual void OnConnect(EventArgs e)
        {
            if (Connect != null)
                Connect(this, e);
        }

        protected virtual void OnDisconnect(EventArgs e)
        {
            if (Disconnect != null)
                Disconnect(this, e);
        }

        protected virtual void OnException(Exception e)
        {
            if (ExceptionThrown != null)
                ExceptionThrown(this, new ExceptionEventArgs(e));
        }

        private void ServerLoop()
        {
            while (m_IsRunning)
            {
                ProcessNextClient();
            }

            m_TerminateHandle.Set();
        }
        
        

        private void ProcessClientThread(object o)
        {
            NamedPipeServerStream pipeStream = (NamedPipeServerStream)o;
            try
            {
                while (m_IsRunning)
                {
                    NamedPipeReader pipeReader = new NamedPipeReader { PipeServer = pipeStream };

                    pipeStream.BeginRead(pipeReader.Buffer, 0, pipeReader.Length, ClientMessage, pipeReader);
                    while (m_IsRunning && !pipeReader.CanDoNextCommand)
                    {
                        if (m_TerminateHandle.WaitOne(10))
                        {
                            break;
                        }
                        if (pipeReader.PipeBroken)
                        {
                            break;
                        }
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                OnException(ex);
            }            
            finally
            {
                try
                {
                    pipeStream.Close();
                    pipeStream.Dispose();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }

            }
            OnDisconnect(EventArgs.Empty);            
        }

        private void ClientMessage(IAsyncResult ar)
        {
            if (ar != null)
            {
                NamedPipeReader reader = ar.AsyncState as NamedPipeReader;
                if (reader != null)
                {
                    try
                    {
                        reader.PipeServer.EndRead(ar);
                        string message = System.Text.ASCIIEncoding.ASCII.GetString(reader.Buffer).Trim(new char[] { '\0', ' ' });
                        message = message.Replace(Environment.NewLine, " ").TrimEnd() + "BOB";
                        var buf = Encoding.ASCII.GetBytes(message);

                        reader.PipeServer.Write(buf, 0, buf.Length);
                        reader.CanDoNextCommand = true;

                    }
                    catch (IOException ex)
                    {
                        reader.PipeBroken = true;
                        //Pipe broken somehow!
                    }
                    catch (Exception ex)
                    {
                        OnException(ex);
                    }
                }
            }            
        }
        protected void ProcessNextClient()
        {
            m_HasConnected = false;
            using (NamedPipeServerStream pipeServer = 
                new NamedPipeServerStream("TQC.GradientOven", PipeDirection.InOut,
                    1, PipeTransmissionMode.Message, PipeOptions.Asynchronous))
            {
                m_Writer.WriteLine("[Server] Pipe created {0}", pipeServer.GetHashCode());

                try
                {
                    pipeServer.BeginWaitForConnection(new AsyncCallback(WaitForConnectionCallBack), pipeServer);
                }
                catch (IOException ex)
                {
                    pipeServer.Disconnect();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
                while (true)
                {
                    m_TerminateHandle.Reset();
                    if (m_TerminateHandle.WaitOne(1000))
                    {
                        return;
                    }
                    if (m_HasConnected)
                    {
                        return;
                    }
                    if (!m_IsRunning)
                    {
                        return;
                    }
                }

            }            
        }

        private void WaitForConnectionCallBack(IAsyncResult iar)
        {
            try
            {                
                NamedPipeServerStream pipeServer = (NamedPipeServerStream)iar.AsyncState;
             
                
                pipeServer.EndWaitForConnection(iar);
                                
                OnConnect(EventArgs.Empty);
                m_HasConnected = true;  //Make sure that we can wait for a new connection...

                Thread t = new Thread(ProcessClientThread);
                t.Start(pipeServer);

            }
            catch (Exception ex)
            {
                OnException(ex);
                return;
            }
        }


        public void Dispose()
        {
            m_IsRunning = false;
            m_TerminateHandle.WaitOne();
        }

        private string IdealAnalysisFolder { get; set; }
        private DataRunDetail DataRunDetails { get; set; }
        bool CollectingData { get; set; }
        public string Folder
        {
            get 
            {
                return IdealAnalysisFolder;
            }
        }

        public bool DataRunStart(DataRunDetail details)
        {
            DataRunDetails = details;
            CollectingData = true;
            return CollectingData;
        }

        public bool DataRunStop()
        {
            CollectingData = false;
            return CollectingData;
        }

        public bool Data(SamplePoint sample)
        {
            if (CollectingData)
            {
                DataRunDetails.AddSample(sample);
            }
            return CollectingData;
        }
    }

}
