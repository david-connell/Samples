﻿using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using TQC.USBDevice.BaseLoggerTypes;


namespace TQC.USBDevice.GradientOven
{
    public class GROMainBoard : TQCUsbLogger , IGROMainBoard
    {
        readonly Dictionary<byte, IGROThermoCoupleBoard> m_ChildDevices = new Dictionary<byte, IGROThermoCoupleBoard>();
        const int NumberOfFansPerBoard = 8;
        const int NumberOfHeatersPerBoard = 8;
        const int NumberOfProbesPerBoard = 8;
        private static ILog m_Log = LogManager.GetLogger("TQC.USBDevice.GradientOven");

        public GROMainBoard(IUsbInterfaceForm mainWinForm)
            : base(mainWinForm, null)
        {
        }

        public bool Open()
        {
            return Open(TQC.USBDevice.USBLogger.USBProductId.GRADIENT_OVEN);
        }

        public IGROThermoCoupleBoard GetChildDevice(byte id)
        {            
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


        byte AbsoluteProbeIdToThermcoupleBoardID(short probeId)
        {
            return (byte)((probeId / NumberOfProbesPerBoard) + 1);
        }

        byte AbsoluteProbeIdToLocalProbeId(short probeId)
        {
            return (byte)(probeId % NumberOfFansPerBoard);
        }

        byte AbsoluteFanIdToThermcoupleBoardID(short fanId)
        {
            return (byte)((fanId / NumberOfFansPerBoard) + 1);
        }

        byte AbsoluteFanIdToLocalFanId(short fanId)
        {
            return (byte)(fanId % NumberOfFansPerBoard);
        }

        public override string ProbeName(int probeId)
        {
            string probeName = string.Empty;
            var thermocoupleBoard = GetChildDevice(AbsoluteProbeIdToThermcoupleBoardID((short)probeId));
            try
            {
                probeName = thermocoupleBoard.ProbeName(AbsoluteProbeIdToLocalProbeId((short)probeId));
            }
            catch (Exception )
            {
            }
            if (string.IsNullOrEmpty(probeName))
            {
                probeName = string.Format("Probe {0}", probeId+1);
            }
            return probeName;
        }

        public override LinearCalibrationDetails CalibrationDetails(int probeId)
        {
            var thermocoupleBoard = GetChildDevice(AbsoluteProbeIdToThermcoupleBoardID((short)probeId));
            return thermocoupleBoard.CalibrationDetails(AbsoluteProbeIdToLocalProbeId((short)probeId));
        }

        public override void SetCalibrationDetails(int probeId, LinearCalibrationDetails newDetails)
        {
            var thermocoupleBoard = GetChildDevice(AbsoluteProbeIdToThermcoupleBoardID((short)probeId));
            thermocoupleBoard.SetCalibrationDetails(AbsoluteProbeIdToLocalProbeId((short)probeId), newDetails);

        }

        public override ProbeType ProbeType(int probeId)
        {
            var thermocoupleBoard = GetChildDevice(AbsoluteProbeIdToThermcoupleBoardID((short)probeId));
            return thermocoupleBoard.ProbeType(AbsoluteProbeIdToLocalProbeId((short)probeId));
        }

        public IEnumerable<byte> ThermocoupleBoardIDs
        {
            get
            {
                //The logger does not support finding out this information! 
                //var response = Request(Commands.ReadDeviceInfo, BitConverter.GetBytes((short)16));
                //List<byte> data = new List<byte>();
                //if (response != null)
                //{
                //    const int startOffset = 6;
                //    for (int i = 0; i < (response.Length - startOffset); i++)
                //    {
                //        data.Add(response[startOffset + i]);
                //    }
                //}
                //return data;
                return new byte [] { 1, 2, 3, 4 };
            }
        }

        public int TotalNumberOfHeaters
        {
            get
            {
                return ThermocoupleBoardIDs.Count() * NumberOfHeatersPerBoard;
            }
        }

        public int TotalNumberOfFans
        {
            get
            {
                return ThermocoupleBoardIDs.Count() * NumberOfFansPerBoard;
            }
        }


        public Percentage ExternalFanSpeed
        {
            get
            {
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)0 ))  ;
                if (response == null)
                {
                    throw new NoDataReceivedException("getExternalFanSpeed");
                }
                if (response.Length < sizeof(byte))
                {
                    throw new TooLittleDataReceivedException("getExternalFanSpeed", response.Length, sizeof(byte));
                }
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

        public MainBoardStatus Status
        {
            get
            {
                return (MainBoardStatus)_GetStatus(0).Status;
            }

        }

       

        public ButtonStatus Button
        {
            get
            {
                ButtonStatus button = ButtonStatus.NothingPressed;
                //m_Log.InfoFormat("Get Button Status");
                var response = Request(Commands.ReadCurrentProbeVals, BitConverter.GetBytes((short)0x30));

                if (response == null)
                {
                    throw new NoDataReceivedException("getButton");
                }
                if (response.Length < sizeof(UInt16) + sizeof(byte))
                {
                    throw new TooLittleDataReceivedException("getButton", response.Length, sizeof(UInt16)+sizeof(byte));
                }
                byte buttonStatus = (byte)response[2];

                //if (buttonStatus != 0)
                {
                    if ((buttonStatus & (0x08)) == 0x08)
                    {
                        button |= ButtonStatus.YellowPressed;
                    }
                    if ((buttonStatus & (0x10)) == 0x10)
                    {
                        button |= ButtonStatus.GreenPressed;
                    }
                    if ((buttonStatus & (0x20)) == 0x20)
                    {
                        button |= ButtonStatus.RedPressed;
                    }
                }
                m_Log.InfoFormat("Read Button Status {0}", button);
                return button;


            }
        }

        public Percentage InternalFanSpeed
        {
            get
            {
                
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)8));
                if (response == null)
                {
                    throw new NoDataReceivedException("getInternalFanSpeed");
                }
                if (response.Length < sizeof(byte))
                {
                    throw new TooLittleDataReceivedException("getInternalFanSpeed", response.Length, sizeof(byte));
                }
                var result =  new Percentage(response[0]);
                m_Log.InfoFormat("Get Internal Fan Speed {0}", result);
                return result;
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)8));
                request.Add(value.Value);
                m_Log.InfoFormat("Set Internal Fan Speed {0}", value);
                Request(Commands.GROSetCommand, request.ToArray());
            }
        }

        public double MaximumHeaterTemperature
        {
            get
            {
                Byte[] response = null;
                try
                {
                    response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)10));
                }
                catch (CommandNotSuportedException )
                {

                }
                catch (EnumerationNotSuportedException)
                {

                }
                if (response == null)
                {
                    m_Log.InfoFormat("Get MaximumHeaterTemperature {0}", 250.0);
                    return 250.0;
                }
                if (response.Length < sizeof(UInt16))
                {
                    throw new TooLittleDataReceivedException("MaximumHeaterTemperature", response.Length, sizeof(UInt16));
                }
                var result =  BitConverter.ToUInt16(response, 0);
                m_Log.InfoFormat("Get MaximumHeaterTemperature {0}", result);
                return result;
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)10));
                request.AddRange(BitConverter.GetBytes((UInt16)value));
                m_Log.InfoFormat("Set MaximumHeaterTemperature {0}", value);
                Request(Commands.GROSetCommand, request.ToArray());
            }
        }
        public Percentage Cooling
        {
            get
            {
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)1));
                if (response == null)
                {
                    throw new NoDataReceivedException("getCooling");
                }
                if (response.Length < sizeof(byte))
                {
                    throw new TooLittleDataReceivedException("getCooling", response.Length, sizeof(byte));
                }
                var result = new Percentage(response[0]);
                m_Log.InfoFormat("Get Cooling {0}", result);
                return result;
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)1));
                request.Add(value.Value);
                m_Log.InfoFormat("Set Cooling {0}", value);
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
                if (response == null)
                {
                    throw new NoDataReceivedException("getPower");
                }
                if (response.Length < sizeof(byte))
                {
                    throw new TooLittleDataReceivedException("getPower", response.Length, sizeof(byte));
                }
                var result = (PowerState)response[0];
                m_Log.InfoFormat("Get Power {0}", result);
                return result;
            }
            set
            {
                List<byte> request = new List<byte>();
                m_Log.InfoFormat("Set Power {0}", value);
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
                if (response == null)
                {
                    throw new NoDataReceivedException("getLift");
                }
                if (response.Length < sizeof(byte))
                {
                    throw new TooLittleDataReceivedException("getLift", response.Length, sizeof(byte));
                }
                var result = (LiftState)response[0];
                m_Log.InfoFormat("Get Lift {0}", result);
                return result;
            }
            set
            {
                List<byte> request = new List<byte>();
                m_Log.InfoFormat("Set Lift {0}", value);
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
                if (response == null)
                {
                    throw new NoDataReceivedException("getClamp");
                }
                if (response.Length < sizeof(byte))
                {
                    throw new TooLittleDataReceivedException("getClamp", response.Length, sizeof(byte));
                }
                var result= (ClampState) response[0];
                m_Log.InfoFormat("Get Clamp {0}", result);
                return result;
            }
            set
            {
                List<byte> request = new List<byte>();
                m_Log.InfoFormat("Set Clamp {0}", value);
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
                if (response == null)
                {
                    throw new NoDataReceivedException("getCarrierPosition");
                }
                if (response.Length < sizeof(byte))
                {
                    throw new TooLittleDataReceivedException("getCarrierPosition", response.Length, sizeof(byte));
                }

                var result = new CarrierPosition(response[0]);
                m_Log.InfoFormat("Get Carrier Position {0}", result);
                return result;
            }
            set
            {
                List<byte> request = new List<byte>();
                m_Log.InfoFormat("Set Carrier Position {0}", value);
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
                if (response == null)
                {
                    throw new NoDataReceivedException("getCarrierSpeed");
                }
                if (response.Length < sizeof(byte))
                {
                    throw new TooLittleDataReceivedException("getCarrierSpeed", response.Length, sizeof(byte));
                }
                var result= new Speed(response[0]);
                m_Log.InfoFormat("Get Carrier Speed {0}", result);
                return result;
            }
            set
            {
                List<byte> request = new List<byte>();
                m_Log.InfoFormat("Set Carrier Speed {0}", value);
                request.AddRange(BitConverter.GetBytes((short)6));
                request.Add(value.SpeedMillimetersPerSecond);
                IssueGROSetCommand(request);
            }
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
            if (response == null)
            {
                throw new NoDataReceivedException(GetFanSettingsMethodDescription(fanId));
            }
            if (response.Length < sizeof(byte))
            {
                throw new TooLittleDataReceivedException(GetFanSettingsMethodDescription(fanId), response.Length, sizeof(byte));
            }
            var result = new Percentage(response[0]);
            m_Log.InfoFormat("Get Fan Setting {0} {1}", fanId, result);
            return result;
        }

        private string GetFanSettingsMethodDescription(short fanId)
        {
            return string.Format("Get Fan Setting {0}", fanId);
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
            m_Log.InfoFormat("Set Fan Setting {0} {1}", fanId, fanSetting);
            request.AddRange(BitConverter.GetBytes((short)(100 + fanId) ) );
            request.Add(fanSetting.Value);

            Request(Commands.GROSetCommand, request.ToArray());
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
                    var value = BitConverter.ToUInt16(response, minBufLen + index * 2);
                    values.Add(value);
                }
            }
            else
            {
                throw new TooLittleDataReceivedException("Read Internal Channels", response.Length, minBufLen);
            }
            return values;
        }

        public override IEnumerable<double> ProbeValues
        {
            get
            {
                List<double> probeValues = new List<double>();
                foreach (var item in ThermocoupleBoardIDs)
                {
                    probeValues.AddRange(_ProbeValues(item));
                }
                return probeValues;
            }
        }

        public float GetTempSetting(short slotId)
        {
            if (slotId < 0 || slotId > 32)
            {
                throw new ArgumentOutOfRangeException("slotId", "Valid slots 0->32");
            }
            
            byte[] response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)(200 + AbsoluteFanIdToLocalFanId(slotId))), AbsoluteFanIdToThermcoupleBoardID(slotId));

            if (response == null)
            {
                throw new NoDataReceivedException(GetTempSettingsMethodDescription(slotId));
            }
            if (response.Length < sizeof(UInt16))
            {
                throw new TooLittleDataReceivedException(GetTempSettingsMethodDescription(slotId), response.Length, sizeof(UInt16));
            }
            return BitConverter.ToUInt16(response, 0)/ 10.0f ;
        }

        private string GetTempSettingsMethodDescription(short slotId)
        {
            return string.Format("Get Temp Setting {0}. #{1}.{2}", slotId, AbsoluteFanIdToLocalFanId(slotId), AbsoluteFanIdToThermcoupleBoardID(slotId));            
        }

        public void SetTempSetting(short channelId, float  temperatureSettingInDegreesC)
        {
            if (channelId < 0 || channelId > 32)
            {
                throw new ArgumentOutOfRangeException("slotId", "Valid slots 0->32");
            }
            if ((temperatureSettingInDegreesC < 0.0f) || (temperatureSettingInDegreesC > 500.0f) )
            {
                throw new ArgumentOutOfRangeException("temperatureSettingInDegreesC", "Valid Temeprature 0->500C");
            }

            List<byte> request = new List<byte>();

            request.AddRange(BitConverter.GetBytes((short)(200 + AbsoluteFanIdToLocalFanId(channelId))));
            request.AddRange(BitConverter.GetBytes(((UInt16)(temperatureSettingInDegreesC* 10.0f+0.5)) ));
            Request(Commands.GROSetCommand, request.ToArray(), AbsoluteFanIdToThermcoupleBoardID(channelId));
        }

        public void SetTempSettingByPercentage(short channelId, float percentage)
        {
            if (channelId < 0 || channelId > 32)
            {
                throw new ArgumentOutOfRangeException("slotId", "Valid slots 0->32");
            }
            List<byte> request = new List<byte>();

            request.AddRange(BitConverter.GetBytes((short)(300 + AbsoluteFanIdToLocalFanId(channelId))));
            request.AddRange(BitConverter.GetBytes((byte) percentage));
            Request(Commands.GROSetCommand, request.ToArray(), AbsoluteFanIdToThermcoupleBoardID(channelId));
        }

        public float GetTempSettingByPercentage(short channelId)
        {
            if (channelId < 0 || channelId > 32)
            {
                throw new ArgumentOutOfRangeException("slotId", "Valid slots 0->32");
            }
            List<byte> request = new List<byte>();

            request.AddRange(BitConverter.GetBytes((short)(300 + AbsoluteFanIdToLocalFanId(channelId))));
            
            var response = Request(Commands.GROReadCommand, request.ToArray(), AbsoluteFanIdToThermcoupleBoardID(channelId));

            if (response == null)
            {
                throw new NoDataReceivedException(GetTempSettingsMethodDescription(channelId));
            }
            if (response.Length < sizeof(byte))
            {
                throw new TooLittleDataReceivedException(GetTempSettingsMethodDescription(channelId), response.Length, sizeof(UInt16));
            }
            return response[0];
        }


        //Combines the boards
        public override Int32 NumberOfProbes
        {
            get
            {
                Int32 total = 0;
                foreach (var item in ThermocoupleBoardIDs)
                {
                    total += _NumberOfProbes(item);
                }
                return total;
            }
        }

    }
}
