using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.USBDevice
{
    public class CarierPosition
    {
        public byte Location { get; set; }
        public float Speed { get; set; }
    }
    public class GROUsbDevice : TQCUsbLogger
    {        
        public byte FanSpeed
        {
            get
            {
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)0 ))  ;
                return response[0];
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)0));
                request.Add(value) ;
                Request(Commands.GROSetCommand, request.ToArray());
            }
        }
        public byte Cooling
        {
            get
            {
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)1));
                return response[0];
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)1));
                request.Add(value);
                Request(Commands.GROSetCommand, request.ToArray());
            }
        }
        public bool Power
        {
            get
            {
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)2));
                return response[0] == 1;
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)2));
                request.Add(value ? (byte)1 : (byte)0);
                Request(Commands.GROSetCommand, request.ToArray());
            }
        }

        public bool Clamp
        {
            get
            {
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)4));
                return response[0] == 1;
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)4));
                request.Add(value ? (byte)1 : (byte)0);
                Request(Commands.GROSetCommand, request.ToArray());
            }
        }

        public CarierPosition CarierPosition
        {
            get
            {
                List<byte> request = new List<byte>();
                var response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)5));
                return new CarierPosition { Location = response[0], Speed = BitConverter.ToSingle(response, 1) };                
            }
            set
            {
                List<byte> request = new List<byte>();

                request.AddRange(BitConverter.GetBytes((short)5));
                request.Add(value.Location);
                request.AddRange(BitConverter.GetBytes(value.Speed));

                Request(Commands.GROSetCommand, request.ToArray());
            }
        }

        public byte GetFanSetting(short fanId)
        {
            if (fanId < 0 || fanId > 100)
            {
                throw new Exception("Invalid fan Id");
            }
            byte[] response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)100 + fanId));            
            return response[0];
        }

        public void SetFanSetting(short fanId, byte fanSetting)
        {
            if (fanId < 0 || fanId > 100)
            {
                throw new Exception("Invalid fan Id");
            }

            List<byte> request = new List<byte>();

            request.AddRange(BitConverter.GetBytes((short)100+fanId));
            request.Add(fanSetting);

            Request(Commands.GROSetCommand, request.ToArray());
            return ;

        }


        public float GetTempSetting(short slotId)
        {
            if (slotId < 0 || slotId > 100)
            {
                throw new Exception("Invalid slot Id");
            }
            byte[] response = Request(Commands.GROReadCommand, BitConverter.GetBytes((short)200 + slotId));
            return BitConverter.ToUInt16(response, 0)/ 10.0f ;
        }

        public void SetTempSetting(short fanId, float  temperatureSettingInDegreesC)
        {
            if (fanId < 0 || fanId > 100)
            {
                throw new Exception("Invalid fan Id");
            }
            if ((temperatureSettingInDegreesC < 0.0f) || (temperatureSettingInDegreesC > 500.0f) )
            {
                throw new Exception("Temeprature our of range");
            }

            List<byte> request = new List<byte>();

            request.AddRange(BitConverter.GetBytes((short)200 + fanId));
            request.AddRange(BitConverter.GetBytes(((UInt16)(temperatureSettingInDegreesC* 10.0f+0.5)) ));

            Request(Commands.GROSetCommand, request.ToArray());
            return;

        }
    }
}
