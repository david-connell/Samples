using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IntegrationTestNUnit.Logger.GeneralLogger;
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

        [TestCase(5, SpeedTestType.DeviceType, false, 5, 5)]
        [TestCase(5, SpeedTestType.DeviceType, false, 0, 0)]
        //[TestCase(10, SpeedTestType.DeviceType)]
        [TestCase(200, SpeedTestType.Status, false, 0, 0)]
        [TestCase(200, SpeedTestType.Status, true, 0, 0)]
        [TestCase(200, SpeedTestType.Status, false, 10, 0)]
        [TestCase(200, SpeedTestType.Status, true, 10, 0)]
        [TestCase(200, SpeedTestType.Status, false, 5, 5)]
        [TestCase(200, SpeedTestType.DeviceType, false, 5, 5)]
        [TestCase(200, SpeedTestType.Status, true, 5, 5)]
        [TestCase(200, SpeedTestType.Status, false, 8, 8)]
        [TestCase(200, SpeedTestType.Status, false, 10, 10)]
        [TestCase(200, SpeedTestType.Status, false, 3, 3)]
        [TestCase(200, SpeedTestType.Status, false, 2, 2)]
        [TestCase(200, SpeedTestType.Status, false, 1, 1)]
        [TestCase(200, SpeedTestType.Status, false, 4, 4)]
        [TestCase(200, SpeedTestType.Status, false, 20, 20)]
        //[TestCase(100, SpeedTestType.Status)]
        //[TestCase(100, SpeedTestType.Name)]
        //[TestCase(200, SpeedTestType.Name)]

        //[TestCase(5, SpeedTestType.DeviceType)]
        //[TestCase(10, SpeedTestType.DeviceType)]
        //[TestCase(100, SpeedTestType.DeviceType)]
        //[TestCase(100, SpeedTestType.Status)]
        //[TestCase(100, SpeedTestType.Name)]
        public void Speed(int numberOfSeconds, SpeedTestType typeOfTest,bool useNative, int pre, int post)
        {
            Configuration config = new Configuration();
            config.UseNativeCommunication = useNative;
            config.PreSleepMilliseconds = pre;
            config.PostSleepMilliseconds = post;
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var stopWatch = new Stopwatch();
                int continiousExceptions = 0;
                int count = 0;
                int exceptionCount = 0;
                int totalNumberOfMilliseconds = numberOfSeconds * 1000;
                stopWatch.Start();
                while (stopWatch.ElapsedMilliseconds < totalNumberOfMilliseconds)
                {
                    try
                    {
                        switch (typeOfTest)
                        {
                            case SpeedTestType.DeviceType:
                                {
                                    var type = thermocoupleBoard.Board.DeviceType;
                                }
                                break;
                            case SpeedTestType.Name:
                                {
                                    var type = thermocoupleBoard.Board.DeviceName;
                                }
                                break;
                            case SpeedTestType.SerialNumber:
                                {
                                    var type = thermocoupleBoard.Board.SerialNumber;
                                }
                                break;
                            case SpeedTestType.Status:
                                {
                                    var type = thermocoupleBoard.Board.Status;
                                }
                                break;
                        }
                        count++;
                        continiousExceptions = 0;
                    }
                    catch (Exception ex)
                    {
                        continiousExceptions++;
                        exceptionCount++;
                        Console.WriteLine("Exception {0}", ex.GetType().ToString());
                    }
                    Assert.That(continiousExceptions, Is.LessThan(5), "Too many continious exceptions to carry on");
                }
                stopWatch.Stop();

                Console.WriteLine("Calls Rate = {0} / sec", count / stopWatch.Elapsed.TotalSeconds);
                Console.WriteLine("Exceptions = {0}", exceptionCount);
                
            }
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
            using (var logger = new GROMainBoard(null))
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
            Assert.That(Math.Abs(value.C), Is.LessThan(20), "Constant it too great");
            Assert.That(value.M, Is.Not.EqualTo(0), "M should not be equal to 0");
            Assert.That(Math.Abs(value.M), Is.GreaterThan(0.1), "M should greater than 0.1");
            Assert.That(Math.Abs(value.M), Is.LessThan(10.0), "M should less than 10.0");
            Console.WriteLine("Probe {0} is {1}", probeId + 1, value);
        }


        [Test]
        public void ReadProbeNames()
        {
            using (var logger = new GROMainBoard(null))
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
