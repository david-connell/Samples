﻿using System;
using System.Collections.Generic;

namespace TQC.USBDevice
{
    public interface ISimpleTQCDevice : ICoreCommands
    {
        int NumberOfProbes { get; }
        string ProbeName(int probeId);
        ProbeType ProbeType(int probeId);
        LinearCalibrationDetails CalibrationDetails(int probeId);
        void SetCalibrationDetails(int probeId, LinearCalibrationDetails details);

        DateTime Calibration { get; }
        string CalibrationCompany { get; }
        string CalibrationUserName { get; }

        IEnumerable<double> ColdJunctions { get; }
        IEnumerable<double> ProbeValues { get; }
        bool Initialize();
        bool IsInitializing(out int percentage);

    }

   
}
