using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TQC.USBDevice.AutoGenerateTestCode
{

    public class UsbOffloadReadLoggedInformation : UsbCommand
    {

        public override IEnumerable<string> TestCaseData
        {
            get
            {
                yield return "[TestCase((UInt16)0x01, TestName=\"Batch with ID = 01\")]";
                yield return "[TestCase((UInt16)0xFF, TestName=\"Batch with ID = FF\")]";
            }
        }
        public override string MethodSignature
        {
            get
            {
                return "UInt16 batchID";
            }
        }
        public class USBReadCurrentValuesDetail : UsbEnumeration
        {
            
            public USBReadCurrentValuesDetail(int id)
            {
                Enumeration = id;
            }
            internal override string UnitTestCode(int commandId, UsbCommandRequest usbCommandRequest)
            {
                if (commandId == 0x04)
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
                    case 0: break;
                    case 1: textStart =
@"
                    {
                        List<byte> request = new List<byte>();
                        request.Add(0x0);
                        request.Add(0x0);
                         try
                        {
                            var result = logger.GetResponse(0, (USBLogger.Commands)0x4, batchID, request);
                            if (result != null)
                            {
                                throw new TooLittleDataReceivedException(""Offload"", result.Length, 0);
                            }
                        }
                        catch (BatchNotAvailableException ex)
                        {
                            Console.WriteLine(""OK, No Batch found"");
                            return;
                        }
                    }
                    for (UInt16 counter = 1; counter < 1025; counter++)
                    {
                        List<byte> request = new List<byte>();
                        request.AddRange(BitConverter.GetBytes(counter));
                        try
                        {
                            var result = logger.GetResponse(0, (USBLogger.Commands)0x4, batchID, request);
                            Console.WriteLine(""Block {0}"", counter);
                            if (result == null)
                            {
                                Console.WriteLine(""Finished"");
                                break;
                            }
                        }
                        catch (NoDataReceivedException)
                        {
                            Console.WriteLine(""Finished by exception"");
                            break;
                        }
                    }";
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
                    case 0: t = "Start"; break;
                    case 1: t = "Offload"; break;


                    default:
                        throw new Exception(string.Format("Unsupported Enum 0x{0:X}", Enumeration));

                }
                return t;
            }
        }

        public UsbOffloadReadLoggedInformation(int command, int enumeration)
        {
            CommandId = command;
            UsbEnumeration = new USBReadCurrentValuesDetail(enumeration);

        }
        public override string ToString()
        {
            return "Offload Logged Information";
        }
    }

}

