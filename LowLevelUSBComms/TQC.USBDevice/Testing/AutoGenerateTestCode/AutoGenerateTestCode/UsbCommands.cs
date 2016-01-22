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

        internal string UnitTestCode(int commandId)
        {
            return UsbEnumeration.UnitTestCode(commandId);
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
        protected string ReadDate()
        {
            return ReadUint32();
        }
        protected string ReadFloat(int numberOfFloats, bool isDepricated = false)
        {
            string code = String.Format(@"
                    if (result.Length < (sizeof(float) * {1}) )
                    {{
                            throw new TooLittleDataReceivedException(""{0}"", result.Length, (sizeof(float) * {1}) );
                    }}
                    for (int i = 0; i < {1}; i++)
                    {
                        float status = BitConverter.ToSingle(result, i*sizeof(float));
                        Console.WriteLine(""{0}{{0}}={{1}}"", i, status);
                    }", ToString(), numberOfFloats);
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
        protected string ReadUint8(bool isDepricated = false)
        {
            string code = String.Format(@"
                    if (result.Length < 1)
                    {{
                            throw new TooLittleDataReceivedException(""{0}"", result.Length, 1);
                    }}
                    byte status = result[0];
                    Console.WriteLine(""{0}={{0}}"", status);", ToString());
            return code;
        }

        protected string ReadString()
        {
            string code = String.Format(@"
                    Console.WriteLine(""{2} = '{{0}}'"", TQCUsbLogger.DecodeString(result));", CommandId, Enumeration, ToString());
            return code;
        }


        internal virtual string UnitTestCode(int commandId)
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
        internal override string UnitTestCode(int commandId)
        {
            if (commandId == 0x01)
            {
                return UsbReadDeviceInfoTextCode(commandId);
            }
            return base.UnitTestCode(commandId);
        }

        private string UsbReadDeviceInfoTextCode(int commandId)
        {
            CommandId = commandId;
            string textStart = string.Format(
@"                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands){0}, {1}, request);
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
                case 13: t = ReadUint8(true); break;
                case 14: t = ReadUint32(true); break;
                case 15: t = ReadUint8(); break;
                case 16: t = "Available Components"; break;
                case 17: t = ReadUint8(true); break;
                case 18: t = ReadUint8(true); break;
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
                case 5: t = "Device Manufactured date"; break;
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

                case 100: t = "GetCommas Protocol"; break;
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
        internal override string UnitTestCode(int commandId)
        {
            if (commandId == 0x02)
            {
                return UsbReadDeviceInfoTextCode(commandId);
            }
            return base.UnitTestCode(commandId);
        }

        private string UsbReadDeviceInfoTextCode(int commandId)
        {
            CommandId = commandId;
            string textStart = string.Format(
@"                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands){0}, {1}, request);
                    if (result == null)
                    {{
                        throw new NoDataReceivedException(""{2}"");
                    }}
", commandId, Enumeration, ToString());
            string t = "?";
            switch (Enumeration)
            {
                case 0: t = ReadDate(); break;
                case 1: t = ReadString(); break;
                case 2: t = ReadString(); break;
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
                case 100: t = ReadUint8(); break;
                case 101: t = ReadUint8(); break;
                case 102: t = ReadUint8(); break;
                case 103: t = ReadUint8(); break;
                case 104: t = ReadUint8(); break;
                case 105: t = ReadUint8(); break;
                case 106: t = ReadUint8(); break;
                case 107: t = ReadUint8(); break;
                case 108: t = ReadUint8(); break;

                case 200: t =ReadString(); break;
                case 201: t =ReadString(); break;
                case 202: t =ReadString(); break;
                case 203: t =ReadString(); break;
                case 204: t =ReadString(); break;
                case 205: t =ReadString(); break;
                case 206: t =ReadString(); break;
                case 207: t =ReadString(); break;
                case 208: t =ReadString(); break;

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
                case 20: t = _T("Calibration Details1"); break;
                case 21: t = _T("Calibration Details2"); break;
                case 22: t = _T("Calibration Details3"); break;
                case 23: t = _T("Calibration Details4"); break;
                case 24: t = _T("Calibration Details5"); break;
                case 25: t = _T("Calibration Details6"); break;
                case 26: t = _T("Calibration Details7"); break;
                case 27: t = _T("Calibration Details8"); break;
                case 28: t = _T("Calibration Details9"); break;
                case 29: t = _T("Calibration Details10"); break;
                case 30: t = _T("Calibration Details11"); break;
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
    public class UsbWriteCalibrationDetails : UsbReadDeviceInfo
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

        internal override string UnitTestCode(int commandId)
        {
            if (commandId == 0x07)
            {
                return UsbReadDeviceInfoTextCode(commandId);
            }
            else if (commandId == 0x17)
            {
                return UsbWriteDeviceInfoTextCode(commandId);
            }
            return base.UnitTestCode(commandId);
        }
        private string ReadPaintType()
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
        private string ReadLimit()
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

        private string UsbWriteDeviceInfoTextCode(int commandId)
        {
            CommandId = commandId;
            string textStart = string.Format(
@"                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands){0}, {1}, request);
                    if (result == null)
                    {{
                        throw new NoDataReceivedException(""{2}"");
                    }}
", commandId, Enumeration, ToString());
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
                    var result = logger.GetResponse(0, (USBLogger.Commands){0}, {1}, request);
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



    public class UsbCommandRequest : IEqualityComparer<UsbCommandRequest>
    {
        public int CommandId{ get;set;}
        public int EnumerationID { get;set;}
        public string CommandIdToString{ get;set;}
        public string EnumerationToString{ get;set;}

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
                    case 0x1: return new UsbReadDeviceInfo(EnumerationID);
                    case 0x11: return new UsbWriteDeviceInfo(EnumerationID);
                    case 0x2: return new UsbReadCalibrationDetails(EnumerationID);
                    case 0x21: return new UsbWriteCalibrationDetails(EnumerationID);
                    case 0x07: return new UsbReadDeviceSetDetails(EnumerationID);
                    case 0x17: return new UsbWriteDeviceSetDetails(EnumerationID);

                }
                return null;
            }
        }
        public UsbCommandRequest(string text)
        {
            var items = text.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length == 3)
            {
                int commandId;
                if (int.TryParse(items[0].Substring(2), System.Globalization.NumberStyles.HexNumber, null, out commandId))
                    CommandId = commandId;
                else
                {
                    throw new Exception(string.Format("Hex is not the write format '{0}' <{1}>", items[0].Substring(2), text));
                }
                EnumerationID = int.Parse(items[1].Substring(2), System.Globalization.NumberStyles.HexNumber);
                var texts = items[2].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (texts.Length == 2)
                {
                    CommandIdToString = texts[0];
                    EnumerationToString = texts[1];
                }
            }
            else
            {
                throw new Exception(string.Format("Unknown data format '{0}'", text));
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
 	        return String.Format("{0}/{1}", CommandIdToString, EnumerationToString);
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
                return UsbCommand.UnitTestCode(CommandId);
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
            foreach (var line in m_TextLines)
            {
                var cmd = new UsbCommandRequest(line.Replace("\n", ""));
                if (!m_Commands.Contains(cmd))
                {
                    m_Commands.Add(cmd);
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
