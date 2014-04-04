using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TQC.GOC.InterProcessCommunication.DataToBeSent;
using TQC.GOC.InterProcessCommunication.Model;

namespace TQC.GOC.InterProcessCommunication
{    
    public class GOCServerImplementation : IIdealFinishAnalysis, IGOCInterProcessServer
    {        
        private Thread m_Server;
        private TextWriter m_Writer;
        private bool m_IsRunning;
        private DateTime m_PingLastSend;
        private EventWaitHandle m_TerminateHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        private bool m_IsTerminating;

        public event ConnectHandler Connect;
        public event ConnectHandler Disconnect;
        public event ExceptionHandler ExceptionThrown;
        public event GOCServerStatusHandler GOCServerStatus;
        Queue<IDataToBeSent> m_QueueOfData = new Queue<IDataToBeSent>();

        public GOCServerImplementation()
        {
            
        }

        public void CreateServer(TextWriter writer)
        {
            if (m_IsRunning)
            {
                Exception ex = new Exception("Cannot create more than one InterProcessServer");
                OnException(ex);
                throw ex;
            }
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

        protected virtual void OnGOCServerStatus(GOCServerStatus status)
        {
            if (GOCServerStatus != null)
                GOCServerStatus(this, new GOCServerStatusEventArgs(status));
        }

        public DateTime LastPing
        {
            get
            {
                lock (m_QueueOfData)
                    return m_PingLastSend;
            }
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
            lock (m_QueueOfData)
            {
                Clear();
                if (CollectingData)
                {
                    Push(new StartOfRunData(DataRunDetails));
                    foreach (var sample in DataRunDetails.Samples)
                    {
                        Push(new SampleData(sample));
                    }
                }
            }
            try
            {
                SendCommandToGetFolder(namedPipeServerData);
                while (m_IsRunning && !namedPipeServerData.PipeBroken && !m_IsTerminating)
                {
                    while (m_IsRunning)
                    {
                        var dataToSend = Pop();
                        if (dataToSend != null)
                        {
                            m_Writer.WriteLine("Send {0}", dataToSend);
                            dataToSend.Send(namedPipeServerData);
                            OnGOCServerStatus(InterProcessCommunication.GOCServerStatus.SendingDataSamples);
                        }
                        else
                        {
                            SendPing(namedPipeServerData);
                            OnGOCServerStatus(InterProcessCommunication.GOCServerStatus.PingIng);
                            lock (m_QueueOfData)
                            {
                                m_PingLastSend = DateTime.Now;
                            }
                        }
                        if (m_TerminateHandle.WaitOne(100))
                        {
                            m_IsTerminating = true;
                            break;
                        }
                        if (namedPipeServerData.PipeBroken)
                        {
                            break;
                        }

                    }
                }
            }
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
            m_Writer.WriteLine("SendCommandToGetFolder");
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
            if (m_IsRunning)
            {
                OnGOCServerStatus(InterProcessCommunication.GOCServerStatus.DataFolderRecieved);
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
                        string message = System.Text.UnicodeEncoding.Unicode.GetString(reader.Buffer).Trim(new char[] { '\0', ' ' });
                        lock (m_Server)
                        {
                            var results = message.Split('*');
                            IdealAnalysisFolder = results[1];
                            m_IdealAnalysisVersion = new Version(results[0]);
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

        

        protected void ProcessNextClient()
        {
            
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

        private Version m_IdealAnalysisVersion;
        private string IdealAnalysisFolder { get; set; }
        private DataRunDetail DataRunDetails { get; set; }
        private bool CollectingData { get; set; }

        public Version IdealFinishAnalysisVersion
        {
            get
            {
                return m_IdealAnalysisVersion;
            }
        }
        public string DataFolder
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
            if (CollectingData == false)
            {
                Clear();
                Push(new StartOfRunData(details));
                DataRunDetails = details;
                CollectingData = true;
            }
            return CollectingData;
        }

        public bool DataRunStop()
        {
            Push(new StopOfRunData());
            CollectingData = false;
            return CollectingData;
        }

        public bool Data(SamplePoint sample)
        {
            if (CollectingData)
            {
                Push(new SampleData(sample));
                if (sample.Samples.Length != DataRunDetails.NumberOfChannels)
                {
                    throw new ArgumentException("sample", "sample data mismatch on Channel definition");
                }
                DataRunDetails.AddSample(sample);
            }
            return CollectingData;
        }

        void Push(IDataToBeSent data)
        {
            lock (m_QueueOfData)
            {
                m_QueueOfData.Enqueue(data);
            }
        }
        IDataToBeSent Pop()
        {
            lock (m_QueueOfData)
            {
                if (m_QueueOfData.Count == 0)
                {
                    return null;
                }
                return m_QueueOfData.Dequeue();
            }            
        }

        void Clear()
        {
            lock (m_QueueOfData)
            {
                m_QueueOfData.Clear();
            }            
        }        
    }

}
