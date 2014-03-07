using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TQC.IdealFinish.PFS;


namespace TQC.IdealFinish.PFSWrapper
{
    public enum TempUnits
    {
        Centigrade=0,
        Farenheight=1

    }
    public class PFSFile   : IDisposable
    {
        internal TQC_DataFile m_pfsWrapper;
        private string FileName {get; set;}
        public int ReadFileStatus { get; set; }
        Guid FileGuid { get; set; }



        
        public PFSFile() : this(null)
        {
            
        }

        public PFSFile(string fileName) 
        {
            m_pfsWrapper = new TQC_DataFile();
            ReadFile(fileName);
        }
        ~PFSFile()
        {
            Dispose(false);
        }

        bool ReadFile(String fileName)
        {
            ReadFileStatus = -1;            
            if (!string.IsNullOrEmpty(fileName))
            {
                ReadFileStatus = m_pfsWrapper.intFileRead(fileName) ;
                if (HasReadOK)
                {
                    FileGuid = Guid.NewGuid();
                    FileName = fileName;                    
                }
                else
                {
                    
                }
            }
            
            return HasReadOK;
        }

        public bool HasReadOK
        {
            get
            {
                return ReadFileStatus == 0;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool inDisposing)
        {
            if (inDisposing)
            {
                m_pfsWrapper = null;                
                GC.SuppressFinalize(this);
            }
        }
        public class Probe
        {
            PFSFile m_File;
            int m_ProbeId;
            Guid FileGuid { get; set; }
            internal Probe (PFSFile file, int probeId)
            {
                m_File = file;
                m_ProbeId = probeId;
                FileGuid = m_File.FileGuid;
            }

            public string Name
            {
                get
                {
                    ValidateFileNotChanged();
                    return m_File.m_pfsWrapper.get_strProbeName(m_ProbeId);
                }
            }

            public bool AirProbe
            {
                get
                {
                    ValidateFileNotChanged();
                    return m_File.m_pfsWrapper.get_bIsAirProbe(m_ProbeId);
                }
            }

            public bool Enabled
            {
                get
                {
                    ValidateFileNotChanged();
                    return m_File.m_pfsWrapper.get_bProbeEnabled(m_ProbeId);
                }
            }

            public Color Color
            {
                get
                {
                    ValidateFileNotChanged();
                    return Color.FromArgb(m_File.m_pfsWrapper.get_lProbeColor(m_ProbeId));
                }
            }

            /// <summary>
            /// Get all Synchronous Data
            /// </summary>
            public double[] Data
            {
                get
                {
                    ValidateFileNotChanged();
                    return m_File.m_pfsWrapper.ProbeData(m_ProbeId);
                }
            }


            private void ValidateFileNotChanged()
            {
                if (m_File.HasFileChanged(FileGuid))
                {
                    throw new Exception("File has changed!");
                }
            }
        }
        protected bool HasFileChanged(Guid guid)
        {
            return guid != FileGuid;
        }

        public Probe getProbe(int probeId)
        {
            Probe probe = new Probe(this, probeId);
            
            return probe;
        }

        public uint Probes
        {
            get
            {
                return m_pfsWrapper.get_varProfileAttribute(TQC_ProfileAttributeType.TQC_NUMBER_OF_PROBES);
            }
        }
        public int Readings
        {
            get
            {
                return m_pfsWrapper.get_varProfileAttribute(TQC_ProfileAttributeType.TQC_NUMBER_OF_READINGS);
            }
        }
        public string OperatorName
        {
            get
            {
                return m_pfsWrapper.get_varProfileAttribute(TQC_ProfileAttributeType.TQC_OPERATOR_NAME);
            }
        }
        public DateTime StartDate
        {
            get
            {
                return DateTime.FromOADate(m_pfsWrapper.get_varProfileAttribute(TQC_ProfileAttributeType.TQC_START_RUN_DATE_TIME));
            }
        }
        public String Notes
        {
            get
            {
                return m_pfsWrapper.get_varProfileAttribute(TQC_ProfileAttributeType.TQC_USER_NOTES);
            }
        }
        public DateTime DownloadDate
        {
            get
            {
                return DateTime.FromOADate(m_pfsWrapper.get_varProfileAttribute(TQC_ProfileAttributeType.TQC_DOWNLOAD_DATE_TIME));
            }
        }

        public float SampleRate
        {
            get
            {
                return m_pfsWrapper.get_varProfileAttribute(TQC_ProfileAttributeType.TQC_SAMPLE_RATE);
            }
        }

        public float NumberOfReadings
        {
            get
            {
                return m_pfsWrapper.get_varProfileAttribute(TQC_ProfileAttributeType.TQC_NUMBER_OF_READINGS);
            }
        }

        public TempUnits TempUnits
        {
            get
            {
                switch ((int)m_pfsWrapper.get_varProfileAttribute(TQC_ProfileAttributeType.TQC_DEFAULT_TEMP_UNITS))
                {
                    case 0:
                        return PFSWrapper.TempUnits.Centigrade;
                    case 1:
                        return PFSWrapper.TempUnits.Farenheight;
                }
                return PFSWrapper.TempUnits.Centigrade;
            }
        }
        public PFSAsyncData AsynchnousData
        {
            get
            {
                var data = m_pfsWrapper.AllProbeDataEx(0);

                PFSAsyncData result = new PFSAsyncData();

                object[,] dataRead = data as object[,];
                AsynchSamples sample = null ;
                for (int rowId = 1; rowId <=(Readings + 1); ++rowId)
                {
                    for (int channelId = 1; channelId <=(Probes + 1); ++channelId)
                    {
                        if (rowId == 1)
                        {
                            if (channelId > 1)
                            {
                                result.ProbeNames.Add(dataRead[rowId, channelId] as string);
                            }
                        }
                        else
                        {
                            double value = (double) dataRead[rowId, channelId];
                            if (channelId == 1)
                            {
                                sample = new AsynchSamples();
                                result.Samples.Add(sample);
                                sample.TimeOfSample = DateTime.FromOADate(value);
                            }
                            else
                            {
                                sample.Readings.Add(value.ToReading());
                            }
                        }
                    }
                }
                return result ;
            }
        }
        
    }
}
