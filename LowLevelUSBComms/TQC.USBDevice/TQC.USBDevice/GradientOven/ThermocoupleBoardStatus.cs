﻿using System;


namespace TQC.USBDevice.GradientOven
{
    [FlagsAttribute]
    public enum ThermcoupleBoardStatusValue : uint
    {
        OK = 0x00,
        ThermocoupleSensorOverTemperature = 0x00010000,
        ThermocoupleSensorDisconnected = 0x00020000,
    }

    public class ThermcoupleBoardStatus
    {
        private byte m_OverTempBySensor;
        private byte m_DisconnectedBySensor;

        public ThermcoupleBoardStatusValue ThermcoupleBoardStatusValue { get; internal set; }

        public ThermcoupleBoardStatus(TQC.USBDevice.TQCUsbLogger.BoardStatus status)
        {
            ThermcoupleBoardStatusValue = (ThermcoupleBoardStatusValue)status.Status;
            m_OverTempBySensor = status.AdditionalVal;
            m_DisconnectedBySensor = status.AdditionalValOne;
        }

        public override string ToString()
        {
            return string.Format("{0} Over temp {1}  Disconected {2}", ThermcoupleBoardStatusValue, SensorOverTempBitfield.ToString("X2"), SensorDisconnectedBitfield.ToString("X2"));
        }
        public byte SensorOverTempBitfield
        {
            get
            {
                return m_OverTempBySensor;
            }
        }

        public byte SensorDisconnectedBitfield
        {
            get
            {
                return m_DisconnectedBySensor;
            }
        }

        public bool IsSensorOverTemperature(int sensorId)
        {            
            if (sensorId < 0 || sensorId >= 8)
            {
                throw new DataOutOfRangeException();
            }
            byte bitToCheck = (byte)(1 << sensorId);
            var overTemp = (bitToCheck & SensorOverTempBitfield) == bitToCheck;
            return overTemp;
        }

        public bool IsSensorDisconnected(int sensorId)
        {            
            if (sensorId < 0 || sensorId >= 8)
            {
                throw new DataOutOfRangeException();
            }
            byte bitToCheck = (byte)(1 << sensorId);
            var value = (bitToCheck & SensorDisconnectedBitfield) == bitToCheck;
            return value;
        }
    }
}
