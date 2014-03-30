using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace TQC.GOC.InterProcessCommunication.Model
{

    public enum TempUnits
    {
        DegreesC = 0,
        DegreesF = 1,
    }

    public enum ThicknessUnits
    {
        THOUSAND_OF_INCH = 0,
        MICROMETERS,
        NANOMETERS,
        MILIMETERS,
        METERS,
        INCHES,
        AUTO_THICKNESS,

    }  

    public class DataRunDetail
    {
        public DateTime StartOfRun { get; private set; }
        public string OperatorName { get; private set; }
        public double SampleRate { get; private set; }
        public IList<Channel> Channels { get; private set; }
        public TempUnits DefaultTemperatureUnits { get; private set; }
        public ThicknessUnits DefaultThicknessUnits { get; private set; }
        public string SerialNumber { get; private set; }
        private List<SamplePoint> m_Samples = new List<SamplePoint>();
        private int m_BatchId;

        public DataRunDetail(string serialNumber, IList<Channel> channels, DateTime start, double sample, string operatorName, TempUnits tempUnits = TempUnits.DegreesC, ThicknessUnits thickness = ThicknessUnits.AUTO_THICKNESS)
        {
            if (String.IsNullOrEmpty(serialNumber))
                throw new ArgumentNullException("serialNumber");

            StartOfRun = new DateTime(start.Year, start.Month, start.Day, start.Hour, start.Minute, 0);
            SampleRate = sample;
            Channels = channels;
            OperatorName = operatorName;
            DefaultTemperatureUnits = tempUnits;
            DefaultThicknessUnits = thickness;
            SerialNumber = serialNumber;
            m_BatchId = GetNextBatchID();

        }

        private int GetNextBatchID()
        {
            int val = (int)Registry.CurrentUser.GetValue(@"SOFTWARE\TQC\GOC\BatchId", 1);
            Registry.CurrentUser.SetValue(@"SOFTWARE\TQC\GOC\BatchId", val + 1);
            return val;
        }

        public int NumberOfChannels
        {
            get
            {
                return Channels.Count;
            }
        }
        public int BatchId
        {
            get
            {                
                return m_BatchId;
            }
        }

        internal void AddSample(SamplePoint point)
        {
            if (point.Samples.Length != NumberOfChannels)
            {
                throw new ArgumentException("Incorrect number of channel passed for given data run", "point");
            }
            m_Samples.Add(point);
        }
        internal IList<SamplePoint> Samples
        {
            get
            {
                return m_Samples;
            }
        }

    }
}
