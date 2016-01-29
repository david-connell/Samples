using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TQC.USBDevice.AutoGenerateTestCode
{
    public class UsbTestWriter
    {
        private UsbCommands m_Commands;
        public string LoggerName { get { return m_Commands.Logger; } }
        public string TypeOfRun { get { return m_Commands.TypeOfRun; } }
        

        public UsbTestWriter(UsbCommands commands)
        {
            m_Commands = commands;
        }
        public void WriteOutNameSpaceStart(System.IO.TextWriter textWriter)
        {
            textWriter.WriteLine("using System;");
            textWriter.WriteLine("using NUnit.Framework;");
            textWriter.WriteLine("using System.Collections.Generic;");
            textWriter.WriteLine("using TQC.USBDevice;");
            textWriter.WriteLine("namespace TQC.USBDevice.{0}.{1}", LoggerName, TypeOfRun);
            textWriter.WriteLine("{");
        }

        public void GenerateOutput(System.IO.TextWriter textWriter)
        {
            bool firstTime = true;
            int previousCommand = -1;

            WriteOutNameSpaceStart(textWriter);
            foreach (var command in m_Commands)
            {
                if (command.CommandId != previousCommand)
                {
                    if (!firstTime)
                    {
                        WriteOutClassEnd(textWriter);
                    }
                    WriteOutClassStart(textWriter, command);

                    WriteOutConstructor(textWriter, command);

                }
                string testAttributes = "";
                if (command.ResponseStatus.HasValue && command.ResponseStatus.Value != 0)
                {
                    switch (command.ResponseStatus.Value)
                    {
                        case 7:
                            testAttributes = ", ExpectedException(typeof(TQC.USBDevice.EnumerationNotSuportedException))"; break;
                        default:
                            testAttributes = ", ExpectedException(typeof(Exception))"; break;
                    }
                }

                textWriter.WriteLine("");
                textWriter.WriteLine("        [Test{0}]", testAttributes);
                textWriter.WriteLine("        public void {0}()", command.TestName);
                textWriter.WriteLine(
@"        {{
            using (var logger = new TQCUsbLogger(null))
            {{
                if (logger.OpenWithMinumumRequests(ProductId))
                {{
{0}
                }}
                else
                {{
                    throw new Exception(""Failed to open logger"");
                }}
            }}
            return;
        }}", command.UnitTestCode);

                previousCommand = command.CommandId;
                firstTime = false;
            }
            WriteOutClassEnd(textWriter);

            textWriter.WriteLine("}");
            
        }

        private static void WriteOutClassEnd(System.IO.TextWriter textWriter)
        {
            textWriter.WriteLine("    }");
        }

        private static void WriteOutClassStart(System.IO.TextWriter textWriter, UsbCommandRequest command)
        {
            textWriter.WriteLine("    [TestFixture]");
            textWriter.WriteLine("    public class {0} ", command.ClassName);
            textWriter.WriteLine("    {");
        }

        private static void WriteOutConstructor(System.IO.TextWriter textWriter, UsbCommandRequest command)
        {
            textWriter.WriteLine("        private USBLogger.USBProductId ProductId;");
            textWriter.WriteLine("        public {0}()", command.ClassName);
            textWriter.WriteLine("        {");
            textWriter.WriteLine("            ProductId = USBLogger.USBProductId.{0};", USBLogger.USBProductId.USB_CURVEX_3a);
            textWriter.WriteLine("            return;");
            textWriter.WriteLine("        }");
        }
    }
}
