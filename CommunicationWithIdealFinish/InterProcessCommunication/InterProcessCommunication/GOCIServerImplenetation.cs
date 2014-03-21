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
    internal class NamedPipeServerData
    {
        public byte[] Buffer { get; set; }
        public bool CanDoNextCommand { get; set; }
        public bool PipeBroken { get; set; }
        private NamedPipeServerStream m_PipeServer;

        public NamedPipeServerData(NamedPipeServerStream pipeServer)
        {
            m_PipeServer = pipeServer;
            Buffer = new byte[255];
        }

        public int Length
        {
            get
            {
                return Buffer.Length;
            }
        }

        public NamedPipeServerStream PipeServer
        {
            get
            {
                return m_PipeServer;
            }
        }

    }

    public class GOCServerImplementation : IIdealFinishAnalysis, IGOCInterProcessServer
    {        
        private Thread m_Server;
        private TextWriter m_Writer;
        private bool m_IsRunning;
        private EventWaitHandle m_TerminateHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private bool m_HasConnected;
        private bool m_IsTerminating;

        public event ConnectHandler Connect;
        public event ConnectHandler Disconnect;
        public event ExceptionHandler ExceptionThrown;

        public GOCServerImplementation()
        {
            
        }
        public void CreateServer(TextWriter writer)
        {
            m_IsRunning = true;
            m_Writer = writer;
            m_Server = new Thread(ServerLoop);
            m_Server.Start();
            try
            {
                IdealFinishApplication.StartUp();
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
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
            NamedPipeServerData namedPipeServerData = (NamedPipeServerData)o;
            try
            {


                SendCommandToGetFolder(namedPipeServerData);

                while (m_IsRunning && !namedPipeServerData.PipeBroken && !m_IsTerminating)
                {

                    while (m_IsRunning)
                    {
                        SendPing(namedPipeServerData);
                        if (m_TerminateHandle.WaitOne(10))
                        {
                            m_IsTerminating = true;
                            break;
                        }
                        if (namedPipeServerData.PipeBroken)
                        {
                            break;
                        }

                        ////pipeStream.BeginRead(pipeReader.Buffer, 0, pipeReader.Length, ClientMessage, pipeReader);
                        //while (m_IsRunning && !namedPipeServerData.CanDoNextCommand)
                        //{
                        //    if (m_TerminateHandle.WaitOne(10))
                        //    {
                        //        m_IsTerminating = true;
                        //        break;
                        //    }
                        //    if (namedPipeServerData.PipeBroken)
                        //    {
                        //        break;
                        //    }
                        //}
                    }
                }
            }
            //catch (ObjectDisposedException odex)
            //{
            //    int i;
            //}
            catch (IOException ioex)
            {
                namedPipeServerData.PipeBroken = true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }            
            finally
            {
                try
                {
                    namedPipeServerData.PipeServer.Close();
                    namedPipeServerData.PipeServer.Dispose();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }

            }
            namedPipeServerData.PipeBroken = true;
            OnDisconnect(EventArgs.Empty);            
        }

        
        private void SendCommandToGetFolder(NamedPipeServerData pipeReader)
        {
            string message = "@1";
            byte[] buf = Encoding.ASCII.GetBytes(message);
            pipeReader.PipeServer.Write(buf, 0, buf.Length);
            pipeReader.PipeServer.BeginRead(pipeReader.Buffer, 0, pipeReader.Length, ClientMessageGetFolder, pipeReader);

            while (m_IsRunning && !pipeReader.CanDoNextCommand)
            {
                if (m_TerminateHandle.WaitOne(10))
                {
                    m_IsTerminating = true;
                    break;
                }
                if (pipeReader.PipeBroken)
                {
                    break;
                }
            }                        
        }

        private void SendPing(NamedPipeServerData pipeReader)
        {
            string message = "@2";
            byte[] buf = Encoding.ASCII.GetBytes(message);
            pipeReader.PipeServer.Write(buf, 0, buf.Length);           
        }

        private void ClientMessageGetFolder(IAsyncResult ar)
        {
            if (ar != null)
            {
                NamedPipeServerData reader = ar.AsyncState as NamedPipeServerData;
                if (reader != null)
                {
                    try
                    {
                        reader.PipeServer.EndRead(ar);
                        string message = System.Text.ASCIIEncoding.ASCII.GetString(reader.Buffer).Trim(new char[] { '\0', ' ' });
                        lock (m_Server)
                        {
                            IdealAnalysisFolder = message;
                        }
                        reader.CanDoNextCommand = true;
                    }
                    catch (IOException ex)
                    {
                        reader.PipeBroken = true;                        
                    }
                    catch (Exception ex)
                    {
                        OnException(ex);
                    }
                }
            }
        }


        private void ClientMessage(IAsyncResult ar)
        {
            if (ar != null)
            {
                NamedPipeServerData reader = ar.AsyncState as NamedPipeServerData;
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

                var data = new NamedPipeServerData(pipeServer);
                try
                {
                    pipeServer.BeginWaitForConnection(new AsyncCallback(WaitForConnectionCallBack), data);
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
                    if (data.PipeBroken)
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
                NamedPipeServerData data = (NamedPipeServerData)iar.AsyncState;
             
                
                data.PipeServer.EndWaitForConnection(iar);
                                
                OnConnect(EventArgs.Empty);
                m_HasConnected = true;  //Make sure that we can wait for a new connection...

                Thread t = new Thread(ProcessClientThread);
                t.Start(data);

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
        private bool CollectingData { get; set; }

        public string Folder
        {
            get 
            {
                string result;
                lock (m_Server)
                {
                    result = IdealAnalysisFolder;
                }
                return result;
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
