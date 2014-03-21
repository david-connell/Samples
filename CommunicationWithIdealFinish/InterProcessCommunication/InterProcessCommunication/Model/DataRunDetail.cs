using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DataRunDetail(DateTime start, DateTimeOffset sample, IList<Channel> channels, string operatorName, TempUnits tempUnits, ThicknessUnits thickness)
        {
            StartOfRun = start;
            SampleRate = sample;
            Channels = channels;
            OperatorName = operatorName;
            DefaultTemperatureUnits = tempUnits;
            DefaultThicknessUnits = thickness;

        }
        public DateTime StartOfRun { get; private set; }
        public string OperatorName { get; private set; }
        public DateTimeOffset SampleRate { get; private set; }
        public IList<Channel> Channels { get; private set; }
        public TempUnits DefaultTemperatureUnits { get; private set; }
        public ThicknessUnits DefaultThicknessUnits { get; private set; }

        private List<SamplePoint> m_Samples = new List<SamplePoint>();

        public int NumberOfChannels
        {
            get
            {
                return Channels.Count;
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
