using System;
using CommunicationsWithEctron.Engine.GPIB_Device;
using Microsoft.Win32;

namespace CommunicationsWithEctron.Engine
{
    public class CalibEquipmentEctron1140A : ICalibEquipment
    {
        string m_Name;
        static string m_SerialNumber;
        static string m_CalibrationTrace;

        static CalibEquipmentEctron1140A()
        {
            Load();
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public string SerialNumber
        {
            get
            {
                return m_SerialNumber;
            }
            set
            {
                m_SerialNumber = value;
            }
        }

        public string CalibrationTrace
        {
            get
            {
                return m_CalibrationTrace;
            }
            set
            {
                m_CalibrationTrace = value;
            }
        }

        public string CommandString(CALIBRATION_COMMAND command)
        {
            switch (command)
            {
                case CALIBRATION_COMMAND.NAME:
                    return "*IDN?";

                case CALIBRATION_COMMAND.ENABLE_TEMPERATURE:
                    return @":INST:MODE:ENTR TEMP";
//@":INST:MAT ALLOY
//:UNIT:TEMP C
//:INST:TEMP:STAND ITS-90
//";
                case CALIBRATION_COMMAND.TEMPERATURE_GET:
                    return ":SOURCE:TEMP:VAL?";
                case CALIBRATION_COMMAND.TEMPERATURE_SET:
                    return ":SOURCE:TEMP:VAL {0}";



                case CALIBRATION_COMMAND.ENABLE_READING:
                    return @":INST:MODE:ENTR VOLT";
//@":INST:MAT COPPER";

                case CALIBRATION_COMMAND.VOLTAGE_GET:
                    return ":SOURCE:VOLT:VAL?";
                case CALIBRATION_COMMAND.VOLTAGE_SET:
                    return ":SOURCE:VOLT:VAL {0}";


                case CALIBRATION_COMMAND.VOLTAGE_READ:
                    return "SENS:VAL?";
                case CALIBRATION_COMMAND.TEMPERATURE_READ:
                    return "SENS:VAL?";

            }
            throw new NotImplementedException();
        }



        internal static void Save()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TQC\Autocal\Ectron1140A", true);
            if (reg == null)
            {
                reg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TQC\Autocal\Ectron1140A");
            }
            reg.SetValue("SerialNumber", m_SerialNumber);
            reg.SetValue("CalibrationTrace", m_CalibrationTrace);

        }

        internal static void Load()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TQC\Autocal\Ectron1140A", true);
            if (reg == null)
            {
                reg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TQC\Autocal\Ectron1140A");
            }
            m_SerialNumber = reg.GetValue("SerialNumber", "123") as string;
            m_CalibrationTrace = reg.GetValue("CalibrationTrace", "123") as string;
        }

    }

}
