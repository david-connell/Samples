using System;
using NUnit.Framework;
using TQC.USBDevice;
using TQC.USBDevice.GradientOven;

namespace IntegrationTestNUnit.Logger.GRO.ThermocoupleBoard
{
    [TestFixture((byte)1)]
    class General
    {
        USBLogger.USBProductId ProductId { get; set; }
        byte ThermocoupleBoard { get; set; }

        public General(byte thermocoupleBoard)
        {
            ProductId = USBLogger.USBProductId.GRADIENT_OVEN;
            ThermocoupleBoard = thermocoupleBoard;
        }

        [Test]
        public void ReadSerialNumber()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.SerialNumber;
                Assert.That(value, Is.GreaterThan(0));
                Console.WriteLine(value);
            }
        }

        [Test]
        public void ReadSoftwareVersion()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.SoftwareVersion;
                Assert.That(value.Major, Is.EqualTo(0));
                Console.WriteLine(value);
            }
        }

        [Test]
        public void ReadHardwareVersion()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.HardwareVersion;
                Assert.That(value.Major, Is.EqualTo(0));
                Console.WriteLine(value);
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
                Console.WriteLine(value);
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
                Console.WriteLine(value);
            }
        }


        [Test]
        public void ReadManufactureDate()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.ManufactureDate;
                Assert.That(value, Is.GreaterThan(new DateTime(200, 1, 1)));
                Console.WriteLine(value);
            }
        }



        [Test]
        public void ReadNumberOfProbes()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var numberOfProbes = thermocoupleBoard.Board.NumberOfProbes;
                Assert.That(numberOfProbes, Is.EqualTo(8));
                Console.WriteLine(numberOfProbes);
            }
        }

        [Test]
        public void ReadProtocolVersion()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.ProtocolVersion;
                Assert.That(value.Major, Is.GreaterThanOrEqualTo(0));
                Console.WriteLine(value);
            }
        }

        [Test]
        public void ReadDeviceType()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.DeviceType;
                Assert.That(value, Is.GreaterThanOrEqualTo(0));
                Console.WriteLine(value);
            }
        }

        [Test]
        public void Status()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                Console.WriteLine("Status = '{0}'", thermocoupleBoard.Board.Status);
                Assert.That(thermocoupleBoard.Board.Status.ThermcoupleBoardStatusValue, Is.EqualTo(ThermcoupleBoardStatusValue.OK));
                Assert.That(thermocoupleBoard.Board.Status.SensorOverTempBitfield, Is.EqualTo((byte)0));
                Assert.That(thermocoupleBoard.Board.Status.SensorDisconnectedBitfield, Is.EqualTo((byte)0));
            }
        }


        [Test]
        public void Initialize()
        {
            using (ThermocoupleBoard thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                Console.WriteLine("Status = '{0}'", thermocoupleBoard.Board.Initialize());
                int percentage;
                while (thermocoupleBoard.Board.IsInitializing(out percentage))
                {
                    Console.WriteLine("Percentage = {0}", percentage);
                }
            }
        }

    }
}
