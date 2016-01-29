using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TQC.USBDevice.AutoGenerateTestCode
{

    public class UsbReadLoggedInformation : UsbCommand
    {
        public class USBReadCurrentValuesDetail : UsbEnumeration
        {
            public USBReadCurrentValuesDetail(int id)
            {
                Enumeration = id;
            }
            internal override string UnitTestCode(int commandId, UsbCommandRequest usbCommandRequest)
            {
                if (commandId == 0x03)
                {
                    return UsbReadDeviceInfoTextCode(commandId, usbCommandRequest);
                }
                return base.UnitTestCode(commandId, usbCommandRequest);
            }

            private string UsbReadDeviceInfoTextCode(int commandId, UsbCommandRequest usbCommandRequest)
            {
                CommandId = commandId;

                string textStart = GeneralRequest(usbCommandRequest) + String.Format(@"
                    if (result == null)
                    {{
                        throw new NoDataReceivedException(""{0}"");
                    }}
", ToString());


                switch (Enumeration)
                {
                    case 0: textStart += ReadUint8(); break;
                    case 1: textStart += ReadUint32(); break;
                    case 2: textStart += ReadDate(); break;
                    case 3: textStart += ReadUint32(); break;
                    case 4: textStart += ReadString(); break;
                    case 5: textStart += ReadUint8(); break;
                    case 6: textStart += ReadUint32(); break;
                    case 7: textStart += ReadUint32(); break;
                    case 8:
                        textStart = GeneralRequest(usbCommandRequest) + String.Format(@"
                    if (result != null)
                    {{
                        throw new Exception(""{0} returned back some data!"");
                    }}
", ToString());
                        
                        break;
                    default:
                        throw new Exception(string.Format("Unsupported Enum 0x{0:X}", Enumeration));
                }
                return textStart;
            }
            public override string ToString()
            {
                String t = "??";
                switch (Enumeration)
                {
                    case 0: t = "GetLoggedBatchIds"; break;
                    case 1: t = "Get Sample Rate"; break;
                    case 2: t = "Get Start Time of batch"; break;
                    case 3: t = "Number of Samples in batch"; break;
                    case 4: t = "Batch Name"; break;
                    case 5: t = "Number of channels in batch"; break;
                    case 6: t = "Get Enabled Channels"; break;
                    case 7: t = "Get Number of Events"; break;
                    case 8: t = "Paint type"; break;

                    case 100: t = "Get Limit Channel00"; break;
                    case 101: t = "Get Limit Channel01"; break;
                    case 102: t = "Get Limit Channel02"; break;
                    case 103: t = "Get Limit Channel03"; break;
                    case 104: t = "Get Limit Channel04"; break;
                    case 105: t = "Get Limit Channel05"; break;
                    case 106: t = "Get Limit Channel06"; break;
                    case 107: t = "Get Limit Channel07"; break;
                    case 108: t = "Get Limit Channel08"; break;
                    case 109: t = "Get Limit Channel09"; break;
                    case 110: t = "Get Limit Channel10"; break;
                    case 111: t = "Get Limit Channel11"; break;
                    case 112: t = "Get Limit Channel12"; break;
                    case 113: t = "Get Limit Channel13"; break;
                    case 114: t = "Get Limit Channel14"; break;
                    case 115: t = "Get Limit Channel15"; break;
                    case 116: t = "Get Limit Channel16"; break;
                    case 117: t = "Get Limit Channel17"; break;
                    case 118: t = "Get Limit Channel18"; break;
                    case 119: t = "Get Limit Channel19"; break;
                    case 120: t = "Get Limit Channel20"; break;
                    case 121: t = "Get Limit Channel21"; break;
                    case 122: t = "Get Limit Channel22"; break;
                    case 123: t = "Get Limit Channel23"; break;
                    case 124: t = "Get Limit Channel24"; break;
                    case 125: t = "Get Limit Channel25"; break;
                    case 126: t = "Get Limit Channel26"; break;
                    case 127: t = "Get Limit Channel27"; break;
                    case 128: t = "Get Limit Channel28"; break;
                    case 129: t = "Get Limit Channel29"; break;
                    
                    case 200: t = "GetCure Channel00"; break;
                    case 201: t = "GetCure Channel01"; break;
                    case 202: t = "GetCure Channel02"; break;
                    case 203: t = "GetCure Channel03"; break;
                    case 204: t = "GetCure Channel04"; break;
                    case 205: t = "GetCure Channel05"; break;
                    case 206: t = "GetCure Channel06"; break;
                    case 207: t = "GetCure Channel07"; break;
                    case 208: t = "GetCure Channel08"; break;
                    case 209: t = "GetCure Channel09"; break;
                    case 210: t = "GetCure Channel10"; break;
                    case 211: t = "GetCure Channel11"; break;
                    case 212: t = "GetCure Channel12"; break;
                    case 213: t = "GetCure Channel13"; break;
                    case 214: t = "GetCure Channel14"; break;
                    case 215: t = "GetCure Channel15"; break;
                    case 216: t = "GetCure Channel16"; break;
                    case 217: t = "GetCure Channel17"; break;
                    case 218: t = "GetCure Channel18"; break;
                    case 219: t = "GetCure Channel19"; break;
                    case 220: t = "GetCure Channel20"; break;
                    case 221: t = "GetCure Channel21"; break;
                    case 222: t = "GetCure Channel22"; break;
                    case 223: t = "GetCure Channel23"; break;
                    case 224: t = "GetCure Channel24"; break;
                    case 225: t = "GetCure Channel25"; break;
                    case 226: t = "GetCure Channel26"; break;
                    case 227: t = "GetCure Channel27"; break;
                    case 228: t = "GetCure Channel28"; break;
                    case 229: t = "GetCure Channel29"; break;
                    


                    default:
                        throw new Exception(string.Format("Unsupported Enum 0x{0:X}", Enumeration));

                }
                return t;
            }
        }

        public UsbReadLoggedInformation(int command, int enumeration)
        {
            CommandId = command;
            UsbEnumeration = new USBReadCurrentValuesDetail(enumeration);

        }
        public override string ToString()
        {
            return "Read Logged Information";
        }
    }

}
