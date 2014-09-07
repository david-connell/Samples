using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TQC.GOC.InterProcessCommunication;
using TQC.GOC.InterProcessCommunication.Model;

namespace TestClassToCommunicate
{
    public interface IOvenModel
    {
        List<double> TemperatureValues { get; }
        int GetProbeCount();
        string GetProbeName(int probeIndex);
        ChannelType GetProbeType(int probeIndex);
        string SerialNumber{ get; }
    }

    public interface IDateTimeProvider
    {
        DateTime Now {get;}
    }
    public class LinkToIdealFinish : IDisposable
    {
        ConsoleTextWriter m_TextWriter = new ConsoleTextWriter();
        GOCServerImplementation m_Server = new GOCServerImplementation();
        IOvenModel m_Model;
        IDateTimeProvider m_DateTimeProvider;
        bool m_IsRunning;
        private Thread m_ProcessingLoop;
        private DataRunDetail m_DataRunDetails;

        private EventWaitHandle m_TerminateHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        public LinkToIdealFinish(System.ComponentModel.IContainer container, System.Drawing.Icon icon)
        {
            m_Server.CreateServer(m_TextWriter, container, icon);
        }

        private static List<Channel> CreateListOfIdealFinishChannels(IOvenModel model)
        {
            List<Channel> channels = new List<Channel>();
            int probeCount = model.GetProbeCount();
            for (int probeIndex = 0; probeIndex < probeCount; probeIndex++)
            {
                var probeName = model.GetProbeName(probeIndex);
                var probeType = model.GetProbeType(probeIndex);
                channels.Add(new Channel(probeName, probeType));
            }
            return channels;
        }
        private string BatchId
        {
            get { return "0001"; }
        }

        public void Terminate()
        {
            m_TerminateHandle.Set();
        }

        public void TerminateWithWait()
        {
            Terminate();
            while (m_IsRunning)
            {
                Thread.Sleep(100);
            }
        }
        public bool StartDataRun(IOvenModel model, double sampleRate, IDateTimeProvider dateTimeProvider)
        {
            StopData();

            m_TerminateHandle.Reset();
            m_Model = model;
            m_DateTimeProvider = dateTimeProvider;
            m_DataRunDetails =
                new DataRunDetail(
                    string.Format("{0}-{1}", model.SerialNumber, BatchId),
                    CreateListOfIdealFinishChannels(model),
                    dateTimeProvider.Now,
                    sampleRate,
                    "Name" + Environment.UserName);

            bool status = m_Server.DataRunStart(m_DataRunDetails);

            if (status)
            {
                m_IsRunning = true;
                m_ProcessingLoop = new Thread(ProcessingLoop);
                m_ProcessingLoop.Start();

            }
            return status;
        }

        private void ProcessingLoop()
        {
            DateTime startOfRun;
            bool hasRunStarted = false; ;
            try
            {
                int millisecondsToWait = 0;
                while (true)
                {
                    if (m_TerminateHandle.WaitOne(millisecondsToWait))
                    {
                        break;
                    }
                    else
                    {
                        var dateTime = m_DateTimeProvider.Now;
                        var vals = m_Model.TemperatureValues;
                        SendDataToIdealFinish(dateTime, vals);
                        if (!hasRunStarted)
                        {
                            startOfRun = dateTime;
                            hasRunStarted = true;
                        }
                        do
                        {
                            dateTime = dateTime.AddSeconds(m_DataRunDetails.SampleRate);
                            millisecondsToWait = (int)(dateTime - m_DateTimeProvider.Now).TotalMilliseconds;
                        } while (millisecondsToWait < 0);
                        
                        Console.WriteLine("Wait for {0}", millisecondsToWait);
                        
                    }
                }
            }
            finally
            {
                StopData();
            }
        }

        private bool StopData()
        {
            bool result = false;
            if (m_IsRunning)
            {
                
                m_Model = null;
                m_DataRunDetails = null;
                result =  m_Server.DataRunStop();
                m_IsRunning = false;
                
            }            
            return result;
        }



        private void SendDataToIdealFinish(DateTime sampleTime, List<double> vals)
        {
            m_Server.Data(new SamplePoint(sampleTime, vals.ToArray()));
        }


        public void Dispose()
        {
            if (m_IsRunning)
            {
                TerminateWithWait();                
            }
            m_TerminateHandle.Dispose();
            m_Server.Dispose();
        }
    }


    public class ConsoleTextWriter : TextWriter
    {
        StringBuilder m_output = new StringBuilder();
        public override void Write(char value)
        {
            lock (m_output)
            {
                m_output.Append(value);
            }

        }
        public override string ToString()
        {
            lock (m_output)
            {
                return m_output.ToString();
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}
