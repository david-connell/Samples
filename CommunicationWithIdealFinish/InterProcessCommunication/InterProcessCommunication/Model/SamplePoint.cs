using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.GOC.InterProcessCommunication.Model
{
    public class SamplePoint
    {
        public SamplePoint(DateTime sampleTime, double[] samples)
        {
            SampleTime = sampleTime;
            Samples = samples;
        }
        public DateTime SampleTime { get; private set; }
        public double[] Samples { get; private set; }

        internal SamplePoint Clone()
        {
            return new SamplePoint(SampleTime, Samples);
        }
    }
}
