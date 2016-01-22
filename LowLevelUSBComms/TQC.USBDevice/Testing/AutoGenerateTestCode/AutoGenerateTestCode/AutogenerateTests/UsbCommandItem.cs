using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TQC.USBDevice.AutoGenerateTestCode;

namespace AutogenerateTests
{
    [TestFixture]
    public class UsbCommandItem
    {
        [Test]
        public void ParseValue()
        {
            UsbCommandRequest cmd = new UsbCommandRequest("0x01*0x66*Read Device Information/DeviceType");
            Assert.That(cmd.CommandId, Is.EqualTo(0x01), "Command Id ");
            Assert.That(cmd.EnumerationID, Is.EqualTo(0x66), "Enumeration ");
            Assert.That(cmd.CommandIdToString, Is.EqualTo("Read Device Information"));           
            Assert.That(cmd.UsbCommand.UsbEnumeration.ToString(), Is.EqualTo("DeviceType"));
        }

        [Test]
        public void Equality()
        {
            UsbCommandRequest val1 = new UsbCommandRequest("0x01*0x66*Read Device Information/DeviceType");
            UsbCommandRequest val2 = new UsbCommandRequest("0x01*0x66*Read Device Information/DeviceType");

            Assert.That(val1, Is.EqualTo(val2));
        }
        [Test]
        public void NotEqual()
        {
            UsbCommandRequest val1 = new UsbCommandRequest("0x01*0x66*Read Device Information/DeviceType");
            UsbCommandRequest val2 = new UsbCommandRequest("0x02*0x66*Read Device Information/DeviceType");

            Assert.That(val1, Is.Not.EqualTo(val2));
        }
        [TestCase("", "")]
        [TestCase("A B", "AB")]
        [TestCase("a b", "AB")]
        [TestCase("abc d", "AbcD")]
        public void ConvertTextToDotNetName(string val1, string val2)
        {
            Assert.That(UsbCommandRequest.ConvertTextToDotNetName(val1), Is.EqualTo(val2));
        }


        const string Curvex3Setup =
@"0x01*0x66*Read Device Information/DeviceType
0x01*0x00*Read Device Information/Device Serial Number
0x01*0x03*Read Device Information/Device Name
0x01*0x66*Read Device Information/DeviceType
0x01*0x00*Read Device Information/Device Serial Number
0x01*0x65*Read Device Information/Firmware features
0x01*0x01*Read Device Information/SW Version
0x01*0x02*Read Device Information/HW Version
0x01*0x0b*Read Device Information/Number of channels
0x01*0x13*Read Device Information/Length of Batch Names
0x01*0x0f*Read Device Information/Max number of Paint Types
0x01*0x07*Read Device Information/Max number of batches
0x01*0x08*Read Device Information/Number of batches used
0x01*0x06*Read Device Information/Physical Memory
0x01*0x64*Read Device Information/GetCommas Protocol
0x01*0x04*Read Device Information/Manufacture Name
0x01*0x05*Read Device Information/Device Manufactured date
0x02*0x64*Read Calibration Information/Type of channel 1
0x02*0xc8*Read Calibration Information/Name of channel 1
0x02*0x65*Read Calibration Information/Type of channel 2
0x02*0xc9*Read Calibration Information/Name of channel 2
0x02*0x66*Read Calibration Information/Type of channel 3
0x02*0xca*Read Calibration Information/Name of channel 3
0x02*0x67*Read Calibration Information/Type of channel 4
0x02*0xcb*Read Calibration Information/Name of channel 4
0x07*0x01*Read Device Setup Detail/Sample Rate
0x02*0x01*Read Calibration Information/Calibration company
0x02*0x02*Read Calibration Information/Calibration user name
0x02*0x00*Read Calibration Information/Date of logger Calibration
0x01*0x0c*Read Device Information/Current state of logger
0x01*0x12*Read Device Information/Sending setup
0x07*0x00*Read Device Setup Detail/RealTimeClock
0x07*0x01*Read Device Setup Detail/Sample Rate
0x07*0x05*Read Device Setup Detail/Temperature Units
0x07*0x09*Read Device Setup Detail/Number of Paint types
0x07*0x06*Read Device Setup Detail/Current Paint type
0x07*0x3e8*Read Device Setup Detail/Paint type 1
0x07*0x08*Read Device Setup Detail/Batch Name
0x07*0x08*Read Device Setup Detail/Batch Name
0x07*0x08*Read Device Setup Detail/Batch Name
0x07*0x08*Read Device Setup Detail/Batch Name
0x07*0x08*Read Device Setup Detail/Batch Name
0x07*0x08*Read Device Setup Detail/Batch Name
0x07*0x08*Read Device Setup Detail/Batch Name
0x07*0x08*Read Device Setup Detail/Batch Name
0x07*0x08*Read Device Setup Detail/Batch Name
0x17*0x05*Set Device Setup Detail/Temperature Units
0x17*0x00*Set Device Setup Detail/RealTimeClock
0x17*0x01*Set Device Setup Detail/Sample Rate
0x17*0x08*Set Device Setup Detail/Batch Name
0x17*0x08*Set Device Setup Detail/Batch Name
0x17*0x08*Set Device Setup Detail/Batch Name
0x17*0x08*Set Device Setup Detail/Batch Name
0x17*0x08*Set Device Setup Detail/Batch Name
0x17*0x08*Set Device Setup Detail/Batch Name
0x17*0x08*Set Device Setup Detail/Batch Name
0x17*0x08*Set Device Setup Detail/Batch Name
0x17*0x08*Set Device Setup Detail/Batch Name
0x17*0x08*Set Device Setup Detail/Batch Name
0x17*0x09*Set Device Setup Detail/Number of Paint types
0x17*0x3e8*Set Device Setup Detail/Paint type 1
0x17*0x06*Set Device Setup Detail/Current Paint type";
        [Test]
        public void Test()
        {
            var lines = Curvex3Setup.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var item = UsbCommands.ParseString("CurveX3","UserSetup", lines);
            UsbTestWriter writer = new UsbTestWriter(item);
            writer.GenerateOutput(Console.Out);

        }
    }
}
