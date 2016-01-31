using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TQC.USBDevice.AutoGenerateTestCode
{
    public class UsbCommand
    {
        public int CommandId { get; protected set; }
        public UsbEnumeration UsbEnumeration { get; protected set; }


        internal string UnitTestCode(int commandId, UsbCommandRequest usbCommandRequest)
        {
            return UsbEnumeration.UnitTestCode(commandId, usbCommandRequest);
        }
    }
    public class UsbEnumeration
    {
        public int Enumeration { get; set; }
        protected int CommandId { get; set; }
        protected  string _T(string value)
        {
            return value;
        }

        protected string GeneralRequest(UsbCommandRequest usbCommandRequest)
        {
            StringBuilder extraBytesInRequest = new StringBuilder();

            extraBytesInRequest.AppendLine();
            foreach (var extraByte in usbCommandRequest.ExtraRequestInfo)
            {
                extraBytesInRequest.AppendLine(string.Format("                    request.Add(0x{0:X});", extraByte));
            }
            
            string textStart = string.Format(
@"                    List<byte> request = new List<byte>();
                    {2}
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x{0:X}, 0x{1:X}, request);
                    
", CommandId, Enumeration, extraBytesInRequest);
            return textStart;
        }


        protected string ReadPaintType()
        {
            string code = String.Format(@"
                    if (result.Length < 4)
                    {{
                        throw new TooLittleDataReceivedException(""{0}"", result.Length, 4);
                    }}
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine(""{0}={{0}}"", status);", ToString());
            return code;
        }
        protected string ReadLimit()
        {
            string code = String.Format(@"
                    if (result.Length < 4)
                    {{
                         throw new TooLittleDataReceivedException(""{0}"", result.Length, 4);
                    }}
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine(""{0}={{0}}"", status);", ToString());
            return code;
        }
        protected string ReadDate()
        {

            string code = String.Format(@"
                    if (result.Length < 4)
                    {{
                        throw new TooLittleDataReceivedException(""{0}"", result.Length, 4);
                    }}
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    DateTime start = new DateTime(1970, 1, 1);
                    DateTime actualDate = start.AddSeconds(status);
                    
                    Console.WriteLine(""{0}={{0}}"", actualDate);", ToString());
            return code;
        }
        protected string ReadFloat(int numberOfFloats, bool isDepricated = false)
        {
            string code = String.Format(@"
                    if (result.Length < (sizeof(float) * {1}) )
                    {{
                        throw new TooLittleDataReceivedException(""{0}"", result.Length, (sizeof(float) * {1}) );
                    }}
                    for (int i = 0; i < {1}; i++)
                    {{
                        float status = BitConverter.ToSingle(result, i*sizeof(float));
                        Console.WriteLine(""{0} #{{0}}={{1}}"", i, status);
                    }}", ToString(), numberOfFloats);
            return code;

        }

        protected string ReadUint32(bool isDepricated = false)
        {
            string code = String.Format(@"
                    if (result.Length < 4)
                    {{
                        throw new TooLittleDataReceivedException(""{0}"", result.Length, 4);
                    }}
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine(""{0}={{0}}"", status);", ToString());
            return code;
        }
        protected string ReadUint16(bool isDepricated = false)
        {
            string code = String.Format(@"
                    if (result.Length < 2)
                    {{
                        throw new TooLittleDataReceivedException(""{0}"", result.Length, 2);
                    }}
                    UInt16 status = BitConverter.ToUInt16(result, 0);
                    Console.WriteLine(""{0}={{0}}"", status);", ToString());
            return code;
        }
        protected string ReadUint8(byte ? expectedValue = null, bool isDepricated = false)
        {
            string code = String.Format(@"
                    if (result.Length < 1)
                    {{
                        throw new TooLittleDataReceivedException(""{0}"", result.Length, 1);
                    }}
                    byte status = result[0];
                    Console.WriteLine(""{0}={{0}}"", status);", ToString());
            if (expectedValue.HasValue)
            {
                code += String.Format(@"
                    Assert.That(result[0], Is.EqualTo({0}));", expectedValue.Value);
            }
            return code;
        }

        protected string ReadString(string expectedValue = null)
        {
            string code = String.Format(@"
                    Console.WriteLine(""{2} = '{{0}}'"", TQCUsbLogger.DecodeString(result));", CommandId, Enumeration, ToString());
            if (!String.IsNullOrWhiteSpace(expectedValue))
            {
                code += String.Format(@"
                    Assert.That(TQCUsbLogger.DecodeString(result), Is.EqualTo(""{0}""));", expectedValue);
            }
            return code;
        }


        internal virtual string UnitTestCode(int commandId, UsbCommandRequest usbCommandRequest)
        {
            return "NO CODE WRITTEN";
        }
    }

    public class ReadWriteDeviceInformation : UsbEnumeration
    {
        public ReadWriteDeviceInformation(int id)
        {
            Enumeration = id;
        }
        internal override string UnitTestCode(int commandId, UsbCommandRequest usbCommandRequest)
        {
            CommandId = commandId;
            if (commandId == 0x01)
            {
                return UsbReadDeviceInfoTextCode(commandId);
            }
            else if (commandId == 0x11)
            {
                return GeneralRequest(usbCommandRequest);
            }
            return base.UnitTestCode(commandId, usbCommandRequest);
        }

        private string UsbReadDeviceInfoTextCode(int commandId)
        {
            CommandId = commandId;
            string textStart = string.Format(
@"                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x{0:X}, 0x{1:X}, request);
                    if (result == null)
                    {{
                        throw new NoDataReceivedException(""{2}"");
                    }}
", commandId, Enumeration, ToString());
            string t = "?";
            switch (Enumeration)
            {
                case 0: t = ReadUint32(); break;
                case 1: t = ReadUint32(); break;
                case 2: t = ReadUint32(); break;
                case 3: t = ReadString(); break;
                case 4: t = ReadString(); break;
                case 5: t = ReadDate(); break;
                case 6: t = ReadUint32(); break;
                case 7: t = ReadUint8(); break;
                case 8: t = ReadUint8(); break;
                case 9: t = ReadUint16(); break;
                case 10: t = ReadUint16(); break;
                case 11: t = ReadUint8(); break;
                case 12: t = ReadUint8(); break;
                case 13: t = ReadUint8(null, true); break;
                case 14: t = ReadUint32(true); break;
                case 15: t = ReadUint8(); break;
                case 16: t = "Available Components"; break;
                case 17: t = ReadUint8(null, true); break;
                case 18: t = ReadUint8(null, true); break;
                case 19: t = ReadUint8(); break;

                case 100: t = ReadUint16(); break;
                case 101: t = ReadUint32(); break;
                case 102: t = ReadUint16(); break;
                case 199: t = ReadString(); break;
                case 200: t = ReadString(); break;
            }
            return textStart + t;

        }

        public override string ToString()
        {
            string t = "?";
            switch (Enumeration)
            {
                case 0: t = "Device Serial Number"; break;
                case 1: t = "SW Version"; break;
                case 2: t = "HW Version"; break;
                case 3: t = "Device Name"; break;
                case 4: t = "Manufacture Name"; break;
                case 5: t = "Device Manufactured Date"; break;
                case 6: t = "Physical Memory"; break;
                case 7: t = "Max number of batches"; break;
                case 8: t = "Number of batches used"; break;
                case 9: t = "Max number of logs in one batch"; break;
                case 10: t ="Number of logs used"; break;
                case 11: t ="Number of channels"; break;
                case 12: t ="Current state of logger"; break;
                case 13: t ="Offload time stamp size"; break;
                case 14: t ="Offload timestamp multiplier"; break;
                case 15: t ="Max number of Paint Types"; break;
                case 16: t ="Available Components"; break;
                case 17: t ="Offload Available"; break;
                case 18: t ="Sending setup"; break;
                case 19: t ="Length of Batch Names"; break;

                case 100: t = "Get Communications Protocol"; break;
                case 101: t = "Firmware features"; break;
                case 102: t = "DeviceType"; break;
                case 199: t = "Calib Cert for Glossmeter"; break;
                case 200: t = "Calib Cert"; break;
                case 201: t = "Glossmeter Text line 1"; break;
                case 202: t = "Glossmeter Text line 2"; break;
                case 203: t = "Glossmeter Text line 3"; break;
                case 204: t = "Glossmeter Text line 4"; break;
                case 205: t = "Glossmeter Text line 5"; break;
            }
            return t;
        }
    }

    public class UsbReadDeviceInfo : UsbCommand
    {
        protected UsbReadDeviceInfo(int command, int enumeration)
        {
            CommandId = command;
            UsbEnumeration = new ReadWriteDeviceInformation(enumeration);

        }
        public UsbReadDeviceInfo(int enumeration) : this(0x1, enumeration)
        {
        }
        public override string ToString()
        {
            return "Read Device Information";
        }
    }
    public class UsbWriteDeviceInfo : UsbReadDeviceInfo
    {
        public UsbWriteDeviceInfo(int enumeration) : base(0x11, enumeration)
        {
        }
        public override string ToString()
        {
            return "Write Device Information";
        }
    }


    public class USBReadWriteCalibrationDetails: UsbEnumeration
    {
        public USBReadWriteCalibrationDetails(int id)
        {
            Enumeration = id;
        }
        internal override string UnitTestCode(int commandId, UsbCommandRequest usbCommandRequest)
        {
            CommandId = commandId;
            if (commandId == 0x02)
            {
                return UsbReadDeviceInfoTextCode(commandId, usbCommandRequest);
            }
            else if (commandId == 0x12)
            {
                return GeneralRequest(usbCommandRequest);
            }
            return base.UnitTestCode(commandId, usbCommandRequest);
        }

        private string UsbReadDeviceInfoTextCode(int commandId, UsbCommandRequest usbCommandRequest)
        {
            CommandId = commandId;
            string textStart = string.Format(
@"                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x{0:X}, 0x{1:X}, request);
                    if (result == null)
                    {{
                        throw new NoDataReceivedException(""{2}"");
                    }}
", commandId, Enumeration, ToString());
            string t = "?";
            switch (Enumeration)
            {
                case 0: t = ReadDate(); break;
                case 1: t = ReadString(usbCommandRequest.ResponseAsString); break;
                case 2: t = ReadString(usbCommandRequest.ResponseAsString); break;
                case 11: t = ReadFloat(3); break;
                case 20: t = ReadFloat(2); break;
                case 21: t = ReadFloat(2); break;
                case 22: t = ReadFloat(2); break;
                case 23: t = ReadFloat(2); break;
                case 24: t = ReadFloat(2); break;
                case 25: t = ReadFloat(2); break;
                case 26: t = ReadFloat(2); break;
                case 27: t = ReadFloat(2); break;
                case 28: t = ReadFloat(2); break;
                case 29: t = ReadFloat(2); break;
                case 30: t = ReadFloat(2); break;
                case 100: t = ReadUint8(usbCommandRequest.ResponseAsByte); break;
                case 101: t = ReadUint8(usbCommandRequest.ResponseAsByte); break;
                case 102: t = ReadUint8(usbCommandRequest.ResponseAsByte); break;
                case 103: t = ReadUint8(usbCommandRequest.ResponseAsByte); break;
                case 104: t = ReadUint8(usbCommandRequest.ResponseAsByte); break;
                case 105: t = ReadUint8(usbCommandRequest.ResponseAsByte); break;
                case 106: t = ReadUint8(usbCommandRequest.ResponseAsByte); break;
                case 107: t = ReadUint8(usbCommandRequest.ResponseAsByte); break;
                case 108: t = ReadUint8(usbCommandRequest.ResponseAsByte); break;

                case 200: t = ReadString(usbCommandRequest.ResponseAsString); break;
                case 201: t = ReadString(usbCommandRequest.ResponseAsString); break;
                case 202: t = ReadString(usbCommandRequest.ResponseAsString); break;
                case 203: t = ReadString(usbCommandRequest.ResponseAsString); break;
                case 204: t = ReadString(usbCommandRequest.ResponseAsString); break;
                case 205: t = ReadString(usbCommandRequest.ResponseAsString); break;
                case 206: t = ReadString(usbCommandRequest.ResponseAsString); break;
                case 207: t = ReadString(usbCommandRequest.ResponseAsString); break;
                case 208: t = ReadString(usbCommandRequest.ResponseAsString); break;

                case 300: t = _T("Calibration UINT 1"); break;
                case 301: t = _T("Calibration UINT 2"); break;
                case 302: t = _T("Calibration UINT 3"); break;
                case 303: t = _T("Calibration UINT 4"); break;
                case 304: t = _T("Calibration UINT 5"); break;
                case 305: t = _T("Calibration UINT 6"); break;
                case 306: t = _T("Calibration UINT 7"); break;
                case 307: t = _T("Calibration UINT 8"); break;
                case 308: t = _T("Calibration UINT 9"); break;
            }
            return textStart + t;
        }
        public override string ToString()
        {
            string t = "?";
            switch (Enumeration)
            {
                case 0: t = _T("Date of logger Calibration"); break;
                case 1: t = _T("Calibration company"); break;
                case 2: t = _T("Calibration user name"); break;
                case 11: t = _T("Holder tile values"); break;
                case 20: t = _T("Calibration Details 1"); break;
                case 21: t = _T("Calibration Details 2"); break;
                case 22: t = _T("Calibration Details 3"); break;
                case 23: t = _T("Calibration Details 4"); break;
                case 24: t = _T("Calibration Details 5"); break;
                case 25: t = _T("Calibration Details 6"); break;
                case 26: t = _T("Calibration Details 7"); break;
                case 27: t = _T("Calibration Details 8"); break;
                case 28: t = _T("Calibration Details 9"); break;
                case 29: t = _T("Calibration Details 10"); break;
                case 30: t = _T("Calibration Details 11"); break;
                case 100: t = _T("Type of channel 1"); break;
                case 101: t = _T("Type of channel 2"); break;
                case 102: t = _T("Type of channel 3"); break;
                case 103: t = _T("Type of channel 4"); break;
                case 104: t = _T("Type of channel 5"); break;
                case 105: t = _T("Type of channel 6"); break;
                case 106: t = _T("Type of channel 7"); break;
                case 107: t = _T("Type of channel 8"); break;
                case 108: t = _T("Type of channel 9"); break;

                case 200: t = _T("Name of channel 1"); break;
                case 201: t = _T("Name of channel 2"); break;
                case 202: t = _T("Name of channel 3"); break;
                case 203: t = _T("Name of channel 4"); break;
                case 204: t = _T("Name of channel 5"); break;
                case 205: t = _T("Name of channel 6"); break;
                case 206: t = _T("Name of channel 7"); break;
                case 207: t = _T("Name of channel 8"); break;
                case 208: t = _T("Name of channel 9"); break;

                case 300: t = _T("Calibration UINT 1"); break;
                case 301: t = _T("Calibration UINT 2"); break;
                case 302: t = _T("Calibration UINT 3"); break;
                case 303: t = _T("Calibration UINT 4"); break;
                case 304: t = _T("Calibration UINT 5"); break;
                case 305: t = _T("Calibration UINT 6"); break;
                case 306: t = _T("Calibration UINT 7"); break;
                case 307: t = _T("Calibration UINT 8"); break;
                case 308: t = _T("Calibration UINT 9"); break;
            }
            return t;
        }
    }

    public class UsbReadCalibrationDetails : UsbCommand
    {
        protected UsbReadCalibrationDetails(int command, int enumeration)
        {
            CommandId = command;
            UsbEnumeration = new USBReadWriteCalibrationDetails(enumeration);

        }
        public UsbReadCalibrationDetails(int enumeration)
            : this(0x2, enumeration)
        {
        }
        public override string ToString()
        {
            return "Read Calibration Details";
        }
    }
    public class UsbWriteCalibrationDetails : UsbReadCalibrationDetails
    {
        public UsbWriteCalibrationDetails(int enumeration)
            : base(0x12, enumeration)
        {
        }
        public override string ToString()
        {
            return "Write Calibration Details";
        }
    }


    public class USBReadWriteDeviceSetupDetail : UsbEnumeration
    {
        public USBReadWriteDeviceSetupDetail(int id)
        {
            Enumeration = id;
        }

        internal override string UnitTestCode(int commandId, UsbCommandRequest usbCommandRequest)
        {
            if (commandId == 0x07)
            {
                return UsbReadDeviceInfoTextCode(commandId);
            }
            else if (commandId == 0x17)
            {
                return UsbWriteDeviceInfoTextCode(commandId, usbCommandRequest);
            }
            return base.UnitTestCode(commandId, usbCommandRequest);
        }
        

        private string UsbWriteDeviceInfoTextCode(int commandId, UsbCommandRequest usbCommandRequest)
        {

            CommandId = commandId;
            string textStart = GeneralRequest(usbCommandRequest)+String.Format(@"
                    if (result != null)
                    {{
                        throw new TooLittleDataReceivedException(""{0}"", result.Length, 0);
                    }}
", ToString());
            return textStart;
        }

        private string UsbReadDeviceInfoTextCode(int commandId)
        {
            CommandId = commandId;
            string dataToInsert = "";
            if (Enumeration == 8)
            {
                dataToInsert = @"
                    request.Add(1);";
            }
            string textStart = string.Format(
@"                    List<byte> request = new List<byte>();{3}
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x{0:X}, 0x{1:X}, request);
                    if (result == null)
                    {{
                        throw new NoDataReceivedException(""{2}"");
                    }}
", commandId, Enumeration, ToString(), dataToInsert);
            string t = "?";
            switch (Enumeration)
            {
                case 0: t = ReadDate(); break;
                case 1: t = ReadUint32(); break;
                case 2: t = ReadUint16(); break;
                case 3: t = ReadDate(); break;
                case 4: t = ReadUint8(); break;
                case 5: t = ReadUint8(); break;
                case 6: t = ReadUint8(); break;
                case 7: t = ReadUint8(); break;
                case 8: t = ReadString(); break;

                case 9: t = ReadUint8(); break;
                case 10: t = ReadUint8(); break;
                case 11: t = _T("Logging Sample"); break;

                case 1000: t = ReadPaintType(); break;
                case 1001: t = ReadPaintType(); break;
                case 1002: t = ReadPaintType(); break;
                case 1003: t = ReadPaintType(); break;
                case 1004: t = ReadPaintType(); break;
                case 1005: t = ReadPaintType(); break;
                case 1006: t = ReadPaintType(); break;
                case 1007: t = ReadPaintType(); break;
                case 1008: t = ReadPaintType(); break;

                case 2000: t =ReadLimit(); break;
                case 2001: t =ReadLimit(); break;
                case 2002: t =ReadLimit(); break;
                case 2003: t =ReadLimit(); break;
                case 2004: t =ReadLimit(); break;
                case 2005: t =ReadLimit(); break;
                case 2006: t =ReadLimit(); break;
                case 2007: t =ReadLimit(); break;
                case 2008: t =ReadLimit(); break;

            }
            return textStart + t;
        }
        public override string ToString()
        {
            string t = "?";
            switch (Enumeration)
            {
                case 0: t = _T("RealTimeClock"); break;
                case 1: t = _T("Sample Rate"); break;
                case 2: t = _T("Temperature Trigger"); break;
                case 3: t = _T("Time Trigger"); break;
                case 4: t = _T("Log Start"); break;
                case 5: t = _T("Temperature Units"); break;
                case 6: t = _T("Current Paint type"); break;
                case 7: t = _T("Display date format"); break;
                case 8: t = _T("Batch Name"); break;

                case 9: t = _T("Number of Paint types"); break;
                case 10: t = _T("Number of Limits"); break;
                case 11: t = _T("Logging Sample"); break;

                case 1000: t = _T("Paint type 1"); break;
                case 1001: t = _T("Paint type 2"); break;
                case 1002: t = _T("Paint type 3"); break;
                case 1003: t = _T("Paint type 4"); break;
                case 1004: t = _T("Paint type 5"); break;
                case 1005: t = _T("Paint type 6"); break;
                case 1006: t = _T("Paint type 7"); break;
                case 1007: t = _T("Paint type 8"); break;
                case 1008: t = _T("Paint type 9"); break;

                case 2000: t = _T("Limit 1"); break;
                case 2001: t = _T("Limit 2"); break;
                case 2002: t = _T("Limit 3"); break;
                case 2003: t = _T("Limit 4"); break;
                case 2004: t = _T("Limit 5"); break;
                case 2005: t = _T("Limit 6"); break;
                case 2006: t = _T("Limit 7"); break;
                case 2007: t = _T("Limit 8"); break;
                case 2008: t = _T("Limit 9"); break;

            }
            return t;
        }
    }

    public class UsbReadDeviceSetDetails : UsbCommand
    {
        protected UsbReadDeviceSetDetails(int command, int enumeration)
        {
            CommandId = command;
            UsbEnumeration = new USBReadWriteDeviceSetupDetail(enumeration);

        }
        public UsbReadDeviceSetDetails(int enumeration)
            : this(0x07, enumeration)
        {
        }
        public override string ToString()
        {
            return "Read Device Setup Details";
        }
    }
    public class UsbWriteDeviceSetDetails: UsbReadDeviceSetDetails 
    {
        public UsbWriteDeviceSetDetails(int enumeration)
            : base(0x17, enumeration)
        {
        }
        public override string ToString()
        {
            return "Write Device Setup Details";
        }
    }

    public class UsbReadCurrentValues : UsbCommand
    {
        public class USBReadCurrentValuesDetail : UsbEnumeration
        {
            public USBReadCurrentValuesDetail(int id)
            {
                Enumeration = id;
            }
            internal override string UnitTestCode(int commandId, UsbCommandRequest usbCommandRequest)
            {
                if (commandId == 0x05)
                {
                    return UsbReadDeviceInfoTextCode(commandId, usbCommandRequest);
                }
                return base.UnitTestCode(commandId, usbCommandRequest);
            }

            private string UsbReadDeviceInfoTextCode(int commandId, UsbCommandRequest usbCommandRequest)
            {
                CommandId = commandId;
                string textStart = string.Format(
@"                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x{0:X}, 0x{1:X}, request);
                    if (result == null)
                    {{
                        throw new NoDataReceivedException(""{2}"");
                    }}
", commandId, Enumeration, ToString());
                
                switch (Enumeration)
                {
                    case 0x0101:
                    case 0x0105:
                        textStart += String.Format(@"
                    if (result.Length < 6)
                    {{
                        throw new NoDataReceivedException(""{2} need to have mode step up & time stamp"");
                    }}
                    Assert.That(result[0], Is.EqualTo({0}), ""Mode not correct"");
                    Assert.That(result[1], Is.EqualTo({1}), ""SetID not correct"");
", Enumeration & 0xFF, (Enumeration >> 8) & 0xFF, ToString()) ;
                        int numberOfReadings = (usbCommandRequest.ResponseBuffer.Length - 6) / 2;
                        for (int channelId = 0; channelId < numberOfReadings; channelId++)
                        {
                            textStart +=
                            String.Format(
@"                    Console.WriteLine(""Value {0} = {{0}}"",  BitConverter.ToInt16(result, {1}) /10.0);
", channelId+1, channelId*2+6);
                        }
                    break;
                    default:
                        throw new Exception(string.Format("Unsupported Enum 0x{0:X}", Enumeration));
                }
                return textStart ;
            }
            public override string ToString()
            {
                String t = "??";
                switch (Enumeration)
                {
                    case 0x0101: t = "Payload Mode Gloss"; break;
                    case 0x0105: t = "Payload Mode Temperature"; break;
                    case 0x0106: t = "Payload Mode Temperature And CJ"; break;
                    case 0x0107: t = "Payload Mode Temperature GRO"; break;
                    case 0x0110: t = "Payload Mode Internal"; break;
                    case 0x0130: t = "Payload Mode UI"; break;
                    case 0x0111: t = "Payload Mode UI"; break;

                    default:
                        throw new Exception(string.Format("Unsupported Enum 0x{0:X}", Enumeration));

                }
                return t;
            }
        }

        public UsbReadCurrentValues(int command, int enumeration)
        {
            CommandId = command;
            UsbEnumeration = new USBReadCurrentValuesDetail(enumeration);

        }
        public override string ToString()
        {
            return "Read Current Values";
        }
    }

    public class UsbReadCurrentRawValues : UsbCommand
    {
        private int EnumerationID;

        public class USBReadCurrentRawValuesDetail : UsbEnumeration
        {
            public USBReadCurrentRawValuesDetail(int id)
            {
                Enumeration = id;
            }
            internal override string UnitTestCode(int commandId, UsbCommandRequest usbCommandRequest)
            {
                if (commandId == 0x06)
                {
                    return UsbReadDeviceInfoTextCode(commandId, usbCommandRequest);
                }
                return base.UnitTestCode(commandId, usbCommandRequest);
            }

            private string UsbReadDeviceInfoTextCode(int commandId, UsbCommandRequest usbCommandRequest)
            {
                CommandId = commandId;
                string textStart = string.Format(
@"                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x{0:X}, 0x{1:X}, request);
                    if (result == null)
                    {{
                        throw new NoDataReceivedException(""{2}"");
                    }}
", commandId, Enumeration, ToString());

                switch (Enumeration)
                {
                    case 0x0101: textStart = Int16RawData(usbCommandRequest, textStart, "ADC"); break;
                    case 0x0102: textStart = Int16RawData(usbCommandRequest, textStart, "Voltage"); break;
                    default:
                        throw new Exception(string.Format("Unsupported Enum 0x{0:X}", Enumeration));
                }
                return textStart;
            }

            private string Int16RawData(UsbCommandRequest usbCommandRequest, string textStart, string text)
            {
                textStart += String.Format(@"
                    if (result.Length < 6)
                    {{
                        throw new NoDataReceivedException(""{2} need to have mode step up & time stamp"");
                    }}
                    Assert.That(result[0], Is.EqualTo({0}), ""Mode not correct"");
                    Assert.That(result[1], Is.EqualTo({1}), ""SetID not correct"");
", Enumeration & 0xFF, (Enumeration >> 8) & 0xFF, ToString());
                int numberOfReadings = (usbCommandRequest.ResponseBuffer.Length - 6) / 2;
                for (int channelId = 0; channelId < numberOfReadings; channelId++)
                {
                    textStart +=
                    String.Format(
@"                    Console.WriteLine(""{0} {1} = {{0}}"",  BitConverter.ToInt16(result, {2}));
", text, channelId + 1, channelId * 2 + 2);
                }
                return textStart;
            }
            public override string ToString()
            {
                String t = "??";
                switch (Enumeration)
                {
                    case 0x0101: t = "Payload ADC"; break;
                    case 0x0102: t = "Payload Microvolts"; break;
                    case 0x0103: t = "Payload Temperature"; break;
                    default:
                        throw new Exception(string.Format("Unsupported Enum 0x{0:X}", Enumeration));

                }
                return t;
            }
        }
        public UsbReadCurrentRawValues(int command, int enumeration)
        {
            CommandId = command;
            UsbEnumeration = new USBReadCurrentRawValuesDetail(enumeration);

        }

        public override string ToString()
        {
            return "Read Current Raw Values";
        }
    }

    public class UsbCommandRequest : IEqualityComparer<UsbCommandRequest>
    {
        public int CommandId{ get;set;}
        public int EnumerationID { get;set;}

        public byte[] ExtraRequestInfo { get; private set; }

        public UInt16? ResponseStatus { get; private set; }
        public byte[] ResponseBuffer { get; private set; }

        internal static string DecodeString(byte[] result)
        {
            var textResult = Encoding.UTF8.GetString(result, 0, result.Length);
            int location = textResult.IndexOf('\0');
            if (location >= 0)
            {
                textResult = textResult.Substring(0, location);
            }
            return textResult.Replace('�', ' ').Trim();
        }

        public byte? ResponseAsByte
        {
            get
            {
                if (ResponseBuffer != null && ResponseBuffer.Length > 0)
                {
                    return ResponseBuffer[0];
                }
                return null;
            }
        }
        public string ResponseAsString
        {
            get
            {
                if (ResponseBuffer != null && ResponseBuffer.Length > 0)
                {
                    return DecodeString(ResponseBuffer);
                }
                return null;
            }
        }

        public string TestName
        {
            get
            {
                var item = UsbCommand;
                return UsbCommandRequest.ConvertTextToDotNetName(item.UsbEnumeration.ToString());
            }
        }
        public string ClassName
        {
            get
            {
                var item = UsbCommand;
                return UsbCommandRequest.ConvertTextToDotNetName(item.ToString());
            }
        }
        
        public UsbCommand UsbCommand
        {
            get
            {
                switch (CommandId)
                {
                    case 0x01: return new UsbReadDeviceInfo(EnumerationID);
                    case 0x11: return new UsbWriteDeviceInfo(EnumerationID);
                    case 0x02: return new UsbReadCalibrationDetails(EnumerationID);
                    case 0x12: return new UsbWriteCalibrationDetails(EnumerationID);

                    case 0x03: return new UsbReadLoggedInformation(CommandId, EnumerationID);
                    case 0x04: return new UsbOffloadReadLoggedInformation(CommandId, EnumerationID);
                    case 0x07: return new UsbReadDeviceSetDetails(EnumerationID);
                    case 0x17: return new UsbWriteDeviceSetDetails(EnumerationID);
                    case 0x05: return new UsbReadCurrentValues(CommandId, EnumerationID);
                    case 0x06: return new UsbReadCurrentRawValues(CommandId, EnumerationID);

                    
                }
                throw new Exception(string.Format("Failed to implement Protocol Command 0x{0:X}", CommandId));
            }
        }
        public UsbCommandRequest(string text)
        {
            //Console.WriteLine("buffer: '{0}'", text);
            var items = text.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length >= 1)
            {
                ParseRequestBuffer(items[0]);
                if (items.Length >= 2)
                {
                    ParseResponseBuffer(items[1]);
                }
            }
            
        }

        private void ParseResponseBuffer(string responseBuffer)
        {
            if (responseBuffer.Length > 0)
            {
                if (responseBuffer[0] == 'W')
                {
                    UInt16 commandId;
                    if (UInt16.TryParse(responseBuffer.Substring(3), System.Globalization.NumberStyles.HexNumber, null, out commandId))
                    {
                        ResponseStatus = commandId;
                    }
                }
                else
                {
                    var requestItems = responseBuffer.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                    var responseBytes = new List<byte>();
                    for (int id = 0; id < requestItems.Length; id++)
                    {
                        responseBytes.Add((byte)int.Parse(requestItems[id].Substring(2), System.Globalization.NumberStyles.HexNumber));
                    }
                    ResponseBuffer = responseBytes.ToArray();
                }
            }
        }

        private void ParseRequestBuffer(string requestBuffer)
        {
            //Console.WriteLine("Request buffer: '{0}'", requestBuffer);
            var requestItems = requestBuffer.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            if (requestItems.Length >=  2)
            {
                int commandId;
                if (int.TryParse(requestItems[0].Substring(2), System.Globalization.NumberStyles.HexNumber, null, out commandId))
                    CommandId = commandId;
                else
                {
                    throw new Exception(string.Format("Hex is not the write format '{0}' <{1}>", requestItems[0].Substring(2), requestBuffer));
                }
                EnumerationID = int.Parse(requestItems[1].Substring(2), System.Globalization.NumberStyles.HexNumber);
                var requestBytes = new List<byte>();
                for (int id = 2; id < requestItems.Length; id++)
                {
                    requestBytes.Add((byte)int.Parse(requestItems[id].Substring(2), System.Globalization.NumberStyles.HexNumber));
                }
                ExtraRequestInfo = requestBytes.ToArray();
            }
            else
            {
                throw new Exception(string.Format("Unknown request data format '{0}'", requestBuffer));
            }
        }


        public override int GetHashCode()
        {
 	        return CommandId.GetHashCode() ^ EnumerationID.GetHashCode();
        }

        public int GetHashCode(UsbCommandRequest obj)
        {
            return obj.GetHashCode();
        }
        public bool Equals(UsbCommandRequest x, UsbCommandRequest y)
        {
            return x.CommandId.Equals(y.CommandId) && x.EnumerationID.Equals(y.EnumerationID);
        }
        public override string ToString()
        {
 	        return String.Format("{0}.{1}", ClassName, TestName);
        }

        public override bool Equals(object obj)
        {
            var value = obj as UsbCommandRequest;
            if (value == null)
                return false;
            return Equals(this, value);
        }

        public static string ConvertTextToDotNetName(string text)
        {
            var items = new List<string>();
            if (!String.IsNullOrEmpty(text))
            {
                var parts = text.Split(new char[] { ' ' });

                foreach (var part in parts)
                {
                    var section = part;
                    section = (part[0].ToString()).ToUpper()[0].ToString() + section.Substring(1);
                    items.Add(section);
                }
            }
            if (items.Count > 0)
            {
                return string.Join("", items.ToArray());
            }
            return "";

        }

        public string UnitTestCode
        {
            get
            {
                string buffer = UsbCommand.UnitTestCode(CommandId, this);
                if (ResponseStatus.HasValue )
                {
                    if (ResponseStatus.Value == 0)
                    {
                        buffer += @"
                    //Got back OK";
                    }
                    else
                    {
                        buffer += string.Format(@"
                    //Got back {0} as exception", ResponseStatus.Value);
                    }
                }
                return buffer;
            }
        }

        
    }

    public class UsbCommands :IEnumerable<UsbCommandRequest>
    {
        public string Logger { get; private set; }
        public string TypeOfRun { get; private set; }
        
        private string[] m_TextLines;
        List<UsbCommandRequest> m_Commands = new List<UsbCommandRequest>(); 
        internal static UsbCommands ParseFile(string logger, string typeOfRun, string fileName)
        {
            return new UsbCommands(logger, typeOfRun, File.ReadAllLines(fileName));
        }
        public static UsbCommands ParseString(string logger, string typeOfRun, string[] toParse)
        {
            return new UsbCommands(logger, typeOfRun, toParse);
        }

        private UsbCommands(string logger, string typeOfRun, string[] lines)
        {
            Logger = UsbCommandRequest.ConvertTextToDotNetName(logger);
            TypeOfRun = UsbCommandRequest.ConvertTextToDotNetName(typeOfRun);
            m_TextLines = lines ;
            ParseData();
        }

        private void ParseData()
        {
            foreach (var textLine in m_TextLines)
            {
                string line = textLine.Replace("\n", "");
                if (line.Length > 0)
                {
                    if (line[0] == '#')
                    {
                        Logger = UsbCommandRequest.ConvertTextToDotNetName(line.Substring(1));
                    }
                    else
                    {
                        var cmd = new UsbCommandRequest(line);
                        if (!m_Commands.Contains(cmd))
                        {
                            m_Commands.Add(cmd);
                        }
                    }
                }
            }
            m_Commands = m_Commands.OrderBy(x => x.CommandId).ToList();
        }

        public IEnumerator<UsbCommandRequest> GetEnumerator()
        {
            return m_Commands.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
