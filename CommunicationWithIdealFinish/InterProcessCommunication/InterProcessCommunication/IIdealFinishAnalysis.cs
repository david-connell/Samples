using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TQC.GOC.InterProcessCommunication.Model;

namespace TQC.GOC.InterProcessCommunication
{
    public interface IIdealFinishAnalysis
    {
        string Folder { get; }
        bool DataRunStart(DataRunDetail details);
        bool DataRunStop();
        bool Data(SamplePoint sample);
    }
}
