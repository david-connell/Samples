using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunicationsWithEctron.Engine.GPIB_Device;
using NationalInstruments.NI4882;

namespace CommunicationsWithEctron.Engine.GPIB_NI.USB
{
    public class GPIB : IGPIB
    {
        private bool m_IsOpen;
        private Device m_device;
        ICalibEquipment m_ICalibEquipment;


        public IList<IGPIBDevice> GetAvailableGPIBDevices()
        {
            IList<IGPIBDevice> devices = new List<IGPIBDevice>();

            devices.Add(new GPIBDevice(0, 1, 0));
            return devices;
        }

        public void Open(IGPIBDevice device)
        {
            GPIBDevice myDevice = device as GPIBDevice;
            if (myDevice != null)
            {
                Close();
                m_device = new Device(myDevice.BoardId, myDevice.PAD, myDevice.SAD);
                m_device.SetEndOnWrite = true;
                try
                {
                    m_device.GoToLocal();
                    m_IsOpen = true;
                    Write(CalibEquipment.CommandString(CALIBRATION_COMMAND.NAME));
                    CalibEquipment.Name = Read();

                }
                catch (InvalidOperationException ex)
                {
                    throw new GPIBException(string.Format("Failed to open NI USB Device (Board {0} GPIB Address {1})", myDevice.BoardId, myDevice.PAD), ex);
                }
                catch (GpibException gex)
                {
                    throw new GPIBException(string.Format("Failed to open NI USB Device (Board {0} GPIB Address {1}). Reason {2}", myDevice.BoardId, myDevice.PAD, gex.Message), gex);
                }
            }
        }

        public void Close()
        {
            if (m_device != null)
            {
                m_device.Dispose();
                m_device = null;
                m_IsOpen = false;
            }
        }

        public bool IsOpen
        {
            get { return m_IsOpen; }
        }


        public void Write(String write)
        {
            if (IsOpen)
            {
                m_device.Write(ReplaceCommonEscapeSequences(write));
            }
        }

        public String Read()
        {
            String result = string.Empty;
            if (IsOpen)
            {
                result = InsertCommonEscapeSequences(m_device.ReadString());
            }
            return result;
        }

        public string ReadDebugCommand(string command)
        {
            Write(command);
            return Read();
        }

        public string EnableTemperature()
        {
            return ReadDebugCommand(CalibEquipment.CommandString(GPIB_Device.CALIBRATION_COMMAND.ENABLE_TEMPERATURE));
        }

        public string EnableVoltage()
        {
            return ReadDebugCommand(CalibEquipment.CommandString(GPIB_Device.CALIBRATION_COMMAND.ENABLE_READING));
        }

        //public double ReadCurrentTemp()
        //{
        //    return ReadValueBackFromDevice(GPIB_Device.CALIBRATION_COMMAND.TEMPERATURE_READ, 'C');
        //}

        //public double ReadCurrentVoltage()
        //{
        //    return ReadValueBackFromDevice(GPIB_Device.CALIBRATION_COMMAND.VOLTAGE_READ, 'V');
        //}
        public double Voltage
        {
            get
            {
                return ReadValueBackFromDevice(GPIB_Device.CALIBRATION_COMMAND.VOLTAGE_GET, 'V');

            }
            set
            {
                Write(string.Format(CalibEquipment.CommandString(CALIBRATION_COMMAND.VOLTAGE_SET), value));
            }
        }

        public double Temperature
        {
            get
            {
                return ReadValueBackFromDevice(GPIB_Device.CALIBRATION_COMMAND.TEMPERATURE_GET, 'C');

            }
            set
            {
                Write(string.Format(CalibEquipment.CommandString(CALIBRATION_COMMAND.TEMPERATURE_SET), value));
            }
        }

        private double ReadValueBackFromDevice(GPIB_Device.CALIBRATION_COMMAND command, char unit)
        {
            double result = 0;
            string val;
            Write(CalibEquipment.CommandString(command));
            val = Read();
            int offset = val.IndexOf(unit);
            val = val.Substring(0, offset);
            double.TryParse(val, out result);
            return result;
        }
        public void Dispose()
        {
            Close();
        }
        private string ReplaceCommonEscapeSequences(string s)
        {
            return s; // s.Replace("\\n", "\n").Replace("\\r", "\r");
        }

        private string InsertCommonEscapeSequences(string s)
        {
            return s;// s.Replace("\n", "\\n").Replace("\r", "\\r");
        }

        public ICalibEquipment CalibEquipment
        {
            get
            {
                return m_ICalibEquipment;
            }
            set
            {
                m_ICalibEquipment = value;
            }
        }


    }

}
