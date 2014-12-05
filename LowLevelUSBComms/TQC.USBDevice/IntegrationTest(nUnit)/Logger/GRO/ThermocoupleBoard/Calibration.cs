using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice;
using TQC.USBDevice.GradientOven;

namespace IntegrationTestNUnit.Logger.GRO.ThermocoupleBoard
{
    [TestFixture("Theromcouple board 1", (byte)1)]
    [TestFixture("Theromcouple board 2", (byte)2)]
    [TestFixture("Theromcouple board 3", (byte)3)]
    [TestFixture("Theromcouple board 4", (byte)4)]
    class Calibration
    {
        USBLogger.USBProductId ProductId { get; set; }
        byte ThermocoupleBoard { get; set; }

        public Calibration(string nameOfBoard, byte thermocoupleBoard)
        {
            ProductId = USBLogger.USBProductId.GRADIENT_OVEN;
            ThermocoupleBoard = thermocoupleBoard;
        }

        [Test]
        public void ReadDateOfCalibration()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.Calibration;
                Assert.That(value, Is.GreaterThan(new DateTime(2000, 1, 1)));
                Console.WriteLine(value);
            }
        }

        [Test]
        public void ReadCalibrationCompany()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var value = thermocoupleBoard.Board.CalibrationCompany;
                Assert.That(value, Is.Not.Null);
                Assert.That(value, Is.Not.EqualTo(""));
                Console.WriteLine(value);
            }
        }


        [Test]
        public void ReadCalibrationUserName()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {                               
                var value = thermocoupleBoard.Board.CalibrationUserName;
                Assert.That(value, Is.Not.Null);
                Assert.That(value, Is.Not.EqualTo(""));
                Console.WriteLine(value);
            }
        }

        [TestCase(0, ProbeType.Temperature)]
        [TestCase(1, ProbeType.Temperature)]
        [TestCase(2, ProbeType.Temperature)]
        [TestCase(3, ProbeType.Temperature)]
        [TestCase(4, ProbeType.Temperature)]
        [TestCase(5, ProbeType.Temperature)]
        [TestCase(6, ProbeType.Temperature)]
        [TestCase(7, ProbeType.Temperature)]
        public void ReadTypeOfProbe(byte probeId, ProbeType probeType)
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var type = thermocoupleBoard.Board.ProbeType(probeId);
                Assert.That(type, Is.EqualTo(probeType));
                Console.WriteLine(type);
            }
        }


        [Test]
        public void ReadCalibrationDetails()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    int maxProbes = logger.NumberOfProbes;
                    for (int probeId = 0; probeId < maxProbes; probeId++)
                        ReadCalibrationDetails(logger, probeId);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        private void ReadCalibrationDetails(GROMainBoard logger, int probeId)
        {
            var value = logger.GetChildDevice(ThermocoupleBoard).CalibrationDetails(probeId);
            Assert.That(value.C, Is.Not.EqualTo(0));
            Assert.That(value.M, Is.Not.EqualTo(0));
            Console.WriteLine("Probe {0} is {1}", probeId + 1, value);
        }


        [Test]
        public void ReadProbeNames()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    int maxProbes = logger.NumberOfProbes;
                    for (int probeId = 0; probeId < maxProbes; probeId++)
                        ReadProbeName(logger, probeId);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        private void ReadProbeName(TQCUsbLogger logger, int probeId)
        {

            var value = logger.ProbeName(probeId);
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.Not.EqualTo(""));
            Console.WriteLine("Probe {0} is{1}'", probeId + 1, value);
        }

    }
}
