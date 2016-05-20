using System;
using System.Collections.Generic;

namespace TQC.USBDevice
{
    public class CalibrationReport
    {
        public CalibrationReport(DateTime dateTime)
        {
            DateTime = dateTime;
            Points = new List<CalibrationReportPoint>();
        }
        public class CalibrationReportPoint
        {
            public CalibrationReportPoint(Int32 nominal, Int32 actual)
            {
                Nominal = nominal;
                Actual = actual;
            }
            public Int32 Nominal { get; private set; }
            public Int32 Actual { get; private set; }
        }
        public DateTime DateTime { get; private set; }
        public List<CalibrationReportPoint> Points { get; private set; }

        public List<byte> NormalRequest 
        {
            get
            {
                List<byte> data = new List<byte> {(byte) Points.Count};
                foreach (var point in Points)
                {
                    data.AddRange(BitConverter.GetBytes(point.Nominal));
                }
                return data;
            }
        }

        public List<byte> ActualRequest
        {
            get
            {
                List<byte> data = new List<byte> {(byte) Points.Count};
                foreach (var point in Points)
                {
                    data.AddRange(BitConverter.GetBytes(point.Actual));
                }
                return data;
            }
        }
    }

    public interface ISimpleTQCDevice : ICoreCommands
    {
        int NumberOfProbes { get; }
        string ProbeName(int probeId);
        ProbeType ProbeType(int probeId);
        LinearCalibrationDetails CalibrationDetails(int probeId);
        void SetCalibrationDetails(int probeId, LinearCalibrationDetails details);

        CalibrationReport CalibrationReport(int probeId);
        void SetCalibrationReport(int probeId, CalibrationReport details);

        DateTime Calibration { get; }
        string CalibrationCompany { get; }
        string CalibrationUserName { get; }

        IEnumerable<double> ColdJunctions { get; }
        IEnumerable<double> ProbeValues { get; }
        bool Initialize();
        bool IsInitializing(out int percentage);

    }

   
}
