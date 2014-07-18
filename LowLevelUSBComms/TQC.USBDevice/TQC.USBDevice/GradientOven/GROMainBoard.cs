using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.USBDevice.GradientOven
{    
    public enum PowerState : byte
    {
        ON = 1,
        OFF = 0,
    }

    public enum ClampState : byte
    {
        Open = 1,
        Closed = 0,
    }

    public enum LiftState : byte
    {
        Up = 1,
        Down = 0,
    }

    public class CarrierPosition
    {
        public CarrierPosition(byte positionInMilliMeters)
        {
            PositionInMilliMeters = positionInMilliMeters;
        }
        public byte PositionInMilliMeters { get; private set; }
    }


    public class Percentage
    {
        public Percentage(byte percentage)
        {
            if (percentage > 100)
            {
                throw new ArgumentOutOfRangeException("percentage", "Max range is 0->100");
            }
            Value = percentage;
        }
        public byte Value { get; private set; }
    }

    public class Speed
    {
        public Speed(byte speed)
        {
            SpeedMillimetersPerSecond = speed;
        }
        public byte SpeedMillimetersPerSecond { get; private set; }
    }


    public class GROMainBoard : TQCUsbLogger , IGROMainBoard
    {
        Dictionary<byte, IGROThermoCoupleBoard> m_ChildDevices = new Dictionary<byte, IGROThermoCoupleBoard>();

        public IGROThermoCoupleBoard GetChildDevice(byte id)
        {
            IGROThermoCoupleBoard result = null;
            if (ThermocoupleBoardIDs.Contains(id))
            {
                if (!m_ChildDevices.ContainsKey(id))
                {
                    m_ChildDevices[id] = new GROThermocoupleBoard(this, id);
                }
                return m_ChildDevices[id];
            }
            else
            {
                throw new GocChildException(string.Format("Child device {0} does not exist", id));   
            }            
        }

        public IEnumerable<byte> ThermocoupleBoardIDs
        {
            get
            {
                //ToDo request this information 
                return new byte [] { 1, 2, 3, 4 };
            }
        }

        public Percentage FanSpeed
        {
            get
            {
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)0 ))  ;
                return new Percentage(response[0]);
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)0));
                request.Add(value.Value) ;
                Request(Commands.GROSetCommand, request.ToArray());
            }
        }

        public Percentage Cooling
        {
            get
            {
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)1));
                return new Percentage(response[0]);
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)1));
                request.Add(value.Value);
                Request(Commands.GROSetCommand, request.ToArray());
            }
        }
        /// <summary>
        /// Turns off or on the power supply
        /// </summary>
        public PowerState Power
        {
            get
            {
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)2));
                return (PowerState)response[0];
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)2));
                request.Add((byte)value );
                Request(Commands.GROSetCommand, request.ToArray());
            }
        }

        /// <summary>
        /// Sets/Gets the position of the lift
        /// </summary>
        public LiftState Lift
        {
            get
            {
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)7));
                return (LiftState)response[0];
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)7));
                request.Add((byte)value);
                IssueGROSetCommand(request);                
            }
        }

        /// <summary>
        /// The clamp
        /// </summary>
        public ClampState Clamp
        {
            get
            {
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)4));
                return (ClampState) response[0];
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)4));
                request.Add((byte)value );
                IssueGROSetCommand(request);
            }
        }

        private void IssueGROSetCommand(List<byte> request)
        {
            Request(Commands.GROSetCommand, request.ToArray());            
        }

        /// <summary>
        /// The position of the carrier in millimeters
        /// </summary>
        public CarrierPosition CarrierPosition
        {
            get
            {
                List<byte> request = new List<byte>();
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)5));
                return new CarrierPosition(response[0]);
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)5));
                request.Add(value.PositionInMilliMeters);
                IssueGROSetCommand(request);                
            }
        }


        /// <summary>
        /// The speed of the carrier in millimeters/sec
        /// </summary>
        public Speed CarrierSpeed
        {
            get
            {
                List<byte> request = new List<byte>();
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)6));
                return new Speed(response[0]);
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)6));
                request.Add(value.SpeedMillimetersPerSecond);
                IssueGROSetCommand(request);
            }
        }

        byte AbsoluteFanIdToThermcoupleBoardID(short fanId)
        {
            return (byte)((fanId / 8) + 1);
        }

        byte AbsoluteFanIdToLocalFanId(short fanId)
        {
            return (byte)(fanId % 8) ;
        }
        /// <summary>
        /// Gets the Fan setting
        /// </summary>
        /// <param name="fanId">The Fan That's specified, (0->32)</param>
        /// <returns>% setting of the fan</returns>
        public Percentage GetFanSetting(short fanId)
        {
            if (fanId < 0 || fanId > 32)
            {
                throw new ArgumentOutOfRangeException("fanId", "Valid fans 0->32");
            }
            byte[] response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)(100 + fanId)));
            return new Percentage(response[0]);
        }

        /// <summary>
        /// Sets the Fan setting
        /// </summary>
        /// <param name="fanId">The Fan That's specified, (0->32)</param>
        /// <returns>% setting of the fan</returns>
        public void SetFanSetting(short fanId, Percentage fanSetting)
        {
            if (fanId < 0 || fanId > 32)
            {
                throw new ArgumentOutOfRangeException("fanId", "Valid fans 0->32");
            }           
            List<byte> request = new List<byte>();

            request.AddRange(BitConverter.GetBytes((short)(100 + fanId) ) );
            request.Add(fanSetting.Value);

            Request(Commands.GROSetCommand, request.ToArray());
            return ;

        }


        public void UserInterfaceStatus(out byte buttonStatus, out Int32 status)
        {
            buttonStatus = 0;
            status = 0;

            var response = GetProbeValues(0, 0x30, 0);
            
            if (response.Length == 7)
            {
                buttonStatus = response[2];
                status = BitConverter.ToInt32(response, 3);
            }
            return;
        }

        public IList<UInt16> InternalChannels()
        {
            List<UInt16> values = new List<ushort>();
            var response = GetProbeValues(0, 0x11, 0);
            const int minBufLen = 6;
            if (response.Length > minBufLen)
            {
                for (int index = 0; index < (response.Length - minBufLen) / 2; index++)
                {
                    var value = BitConverter.ToUInt16(response, minBufLen + index *2 );
                    values.Add(value);
                }
                
                
            }
            return values;
        }

        public float GetTempSetting(short slotId)
        {
            if (slotId < 0 || slotId > 32)
            {
                throw new ArgumentOutOfRangeException("slotId", "Valid slots 0->32");
            }
            byte[] response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)(200 + AbsoluteFanIdToLocalFanId(slotId))), AbsoluteFanIdToThermcoupleBoardID(slotId));
            return BitConverter.ToUInt16(response, 0)/ 10.0f ;
        }

        public void SetTempSetting(short fanId, float  temperatureSettingInDegreesC)
        {
            if (fanId < 0 || fanId > 32)
            {
                throw new ArgumentOutOfRangeException("slotId", "Valid slots 0->32");
            }
            if ((temperatureSettingInDegreesC < 0.0f) || (temperatureSettingInDegreesC > 500.0f) )
            {
                throw new ArgumentOutOfRangeException("temperatureSettingInDegreesC", "Valid Temeprature 0->500C");
            }

            List<byte> request = new List<byte>();

            request.AddRange(BitConverter.GetBytes((short)(200 + AbsoluteFanIdToLocalFanId(fanId))));
            request.AddRange(BitConverter.GetBytes(((UInt16)(temperatureSettingInDegreesC* 10.0f+0.5)) ));

            Request(Commands.GROSetCommand, request.ToArray(), AbsoluteFanIdToThermcoupleBoardID(fanId));
            return;

        }
    }
}
