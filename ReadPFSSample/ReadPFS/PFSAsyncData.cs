using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.IdealFinish.PFSWrapper
{
    public class AsynchSamples
    {
        public DateTime TimeOfSample {get; set;}
        public IList<PFSReading> Readings { get; internal set; }

        public AsynchSamples()
        {
            Readings = new List<PFSReading>();
        }
    }
    public class PFSAsyncData
    {
        public IList<string> ProbeNames { get; internal set; }
        public IList<AsynchSamples> Samples { get; internal set; }

        public PFSAsyncData()
        {
            ProbeNames = new List<string>();
            Samples = new List<AsynchSamples>();
        }
    }
}
