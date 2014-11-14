using System;
using System.Collections.Generic;
using System.Text;


namespace CommunicationsWithEctron.Engine.GPIB_Device
{
    public enum CALIBRATION_COMMAND
    {
        NAME,
        ENABLE_TEMPERATURE,
        TEMPERATURE_GET,
        TEMPERATURE_SET,

        ENABLE_READING,
        VOLTAGE_GET,
        VOLTAGE_SET,
        TEMPERATURE_READ,
        VOLTAGE_READ,

    } ;
    public interface ICalibEquipment
    {
        string Name { get; set; }
        string SerialNumber { get; set; }
        string CalibrationTrace { get; set; }
        string CommandString(CALIBRATION_COMMAND command);
    }

}
