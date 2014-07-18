using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice;

namespace IntegrationTestNUnit
{
    [TestFixture(USBLogger.USBProductId.GRADIENT_OVEN, 1)]
    class GROThermocoupleBoardTests
    {
        USBLogger.USBProductId ProductId { get; set; }
        byte ThermocoupleBoard { get; set; }

        public GROThermocoupleBoardTests(USBLogger.USBProductId product, byte thermocoupleBoard)
        {
            ProductId = product;
            ThermocoupleBoard = thermocoupleBoard;
        }

        [Test]
        public void ReadSerialNumber()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.SerialNumber;
                Assert.That(value, Is.GreaterThan(0));
            }
        }

        [Test]
        public void ReadSoftwareVersion()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.SoftwareVersion;
                Assert.That(value.Major, Is.EqualTo(0));
            }
        }

        [Test]
        public void ReadHardwareVersion()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.HardwareVersion;
                Assert.That(value.Major, Is.EqualTo(0));
            }
        }

        [Test]
        public void ReadDeviceName()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.DeviceName;
                Assert.That(value, Is.Not.Null);
                Assert.That(value, Is.Not.EqualTo(""));
            }
        }

        [Test]
        public void ReadManufactureName()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.ManufactureName;
                Assert.That(value, Is.Not.Null);
                Assert.That(value, Is.Not.EqualTo(""));
            }
        }


        [Test]
        public void ReadManufactureDate()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.ManufactureDate;
                Assert.That(value, Is.GreaterThan(new DateTime(200, 1, 1)));
            }
        }

        [Test]
        public void ReadProbes()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var probeValues = thermocoupleBoard.Board.ProbeValues;
                Assert.IsTrue(probeValues.Count() == 8);
                foreach (var value in probeValues)
                {
                    Assert.IsTrue(value > 0);
                }
            }
        }

        [Test]
        public void ReadNumberOfProbes()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var numberOfProbes = thermocoupleBoard.Board.NumberOfProbes;
                Assert.That(numberOfProbes, Is.EqualTo(8));
            }
        }


        [TestCase(0, USBLogger.ProbeType.Temperature)]
        [TestCase(1, USBLogger.ProbeType.Temperature)]
        [TestCase(2, USBLogger.ProbeType.Temperature)]
        [TestCase(3, USBLogger.ProbeType.Temperature)]
        [TestCase(4, USBLogger.ProbeType.Temperature)]
        [TestCase(5, USBLogger.ProbeType.Temperature)]
        [TestCase(6, USBLogger.ProbeType.Temperature)]
        [TestCase(7, USBLogger.ProbeType.Temperature)]
        public void ReadProbeType(byte probeId, USBLogger.ProbeType probeType)
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var type = thermocoupleBoard.Board.ProbeType(probeId);
                Assert.That(type, Is.EqualTo(probeType));
            }
        }

    }
}
