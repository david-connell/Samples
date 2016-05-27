using System;
using IntegrationTestNUnit.Logger.GeneralLogger;
using NUnit.Framework;
using TQC.USBDevice;
using TQC.USBDevice.Curvex3;

namespace IntegrationTestNUnit.Logger.Curvex3.Standard
{
    [TestFixture]
    class Standard : BasicCommands
    {
        public const USBLogger.USBProductId USBCurvex3 = USBLogger.USBProductId.USB_CURVEX_3_STANDARD;

        public Standard()
            : base(USBCurvex3)
        {
        }
        

    }

    [TestFixture]
    class Calibration : CalibrationCommands
    {
        public Calibration()
            : base(Standard.USBCurvex3)
        {
        }

        [Test]
        public void SetCalibrationConstant()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    int maxProbes = logger.NumberOfProbes;
                    if (maxProbes > 0)
                    {
                        LinearCalibrationDetails details = new LinearCalibrationDetails(1, 0);
                        logger.SetCalibrationDetails(0, details);
                        Assert.That(logger.CalibrationDetails(0), Is.EqualTo(details));
                    }
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void SetLoggerType()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.DeviceType;
                    logger.DeviceType = DeviceType.CurveX3_High;
                    Assert.That(logger.DeviceType, Is.EqualTo(DeviceType.CurveX3_High));
                    logger.DeviceType = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void SetSerialNumber()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.SerialNumber;
                    logger.SerialNumber = 1000;
                    Assert.That(logger.SerialNumber, Is.EqualTo(1000));
                    logger.SerialNumber = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        [Test]
        public void SetDeviceName()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.DeviceName;
                    logger.DeviceName = "Hello world";
                    Assert.That(logger.DeviceName, Is.EqualTo("Hello world"));
                    logger.DeviceName = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void SetManufactureName()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.ManufactureName;
                    logger.ManufactureName = "Hello world";
                    Assert.That(logger.ManufactureName, Is.EqualTo("Hello world"));
                    logger.ManufactureName = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        [Test]
        public void SetManufactureDate()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.ManufactureDate;
                    logger.ManufactureDate = DateTime.Today;
                    Assert.That(logger.ManufactureDate, Is.EqualTo(DateTime.Today));
                    logger.ManufactureDate = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void SetCalibrationCertificate()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.CalibrationCertificate;
                    logger.CalibrationCertificate = "Test";
                    Assert.That(logger.CalibrationCertificate, Is.EqualTo("Test"));
                    logger.CalibrationCertificate = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }
        [Test]
        public void SetCalibrationCompany()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.CalibrationCompany;
                    logger.CalibrationCompany = "Test1";
                    Assert.That(logger.CalibrationCompany, Is.EqualTo("Test1"));
                    logger.CalibrationCompany = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void SetCalibrationUsername()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.CalibrationUserName;
                    logger.CalibrationUserName = "Test1";
                    Assert.That(logger.CalibrationUserName, Is.EqualTo("Test1"));
                    logger.CalibrationUserName = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void SetCalibrationDate()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.Calibration;
                    var result = DateTime.Now;
                    logger.Calibration = result;
                    Assert.That(Math.Abs((logger.Calibration - result).TotalSeconds), Is.LessThan(1));
                    logger.Calibration = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [TestCase(0)]
        public void SetCalibrationDetails(int probeId)
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.CalibrationDetails(probeId);
                    LinearCalibrationDetails result = new LinearCalibrationDetails(1, 0.02);
                    logger.SetCalibrationDetails(probeId, result);
                    var resultFromLogger = logger.CalibrationDetails(probeId);
                    Assert.That(Math.Abs(resultFromLogger.M - result.M), Is.LessThan(0.1));
                    Assert.That(Math.Abs(resultFromLogger.C - result.C), Is.LessThan(0.005));
                    logger.SetCalibrationDetails(probeId, original);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void SetCalibrationEquipmentName()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.CalibrationEquipmentName;
                    var result = "Fred FlintStone";
                    logger.CalibrationEquipmentName = result;
                    Assert.That(logger.CalibrationEquipmentName, Is.EqualTo(result));
                    logger.CalibrationEquipmentName = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }
        [Test]
        public void SetCalibrationEquipmentSerialNumber()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.CalibrationEquipmentSerialNumber;
                    var result = "Fred FlintStone";
                    logger.CalibrationEquipmentSerialNumber = result;
                    Assert.That(logger.CalibrationEquipmentSerialNumber, Is.EqualTo(result));
                    logger.CalibrationEquipmentSerialNumber = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void SetCalibrationEquipmentTracability()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.CalibrationEquipmentTracability;
                    var result = "Fred FlintStone";
                    logger.CalibrationEquipmentTracability = result;
                    Assert.That(logger.CalibrationEquipmentTracability, Is.EqualTo(result));
                    logger.CalibrationEquipmentTracability = original;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }
        [TestCase(0)]
        public void SetCalibrationDateForProbe(int probeId)
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var original = logger.CalibrationReport(probeId);
                    var result = new CalibrationReport(DateTime.Now);
                    result.Points.Add(new CalibrationReport.CalibrationReportPoint(1000, 1001));
                    result.Points.Add(new CalibrationReport.CalibrationReportPoint(2000, 2001));
                    result.Points.Add(new CalibrationReport.CalibrationReportPoint(3000, 3001));
                    result.Points.Add(new CalibrationReport.CalibrationReportPoint(4000, 4001));
                    logger.SetCalibrationReport(probeId, result);
                    var resultFromLogger = logger.CalibrationReport(probeId);
                    Assert.That(resultFromLogger.DateTime, Is.EqualTo(result.DateTime));
                    for (int index = 0; index < result.Points.Count; index++)
                    {
                        Assert.That(resultFromLogger.Points[index].Actual, Is.EqualTo(result.Points[index].Actual));
                        Assert.That(resultFromLogger.Points[index].Nominal, Is.EqualTo(result.Points[index].Nominal));
                    }
                    logger.SetCalibrationReport(probeId, original);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void ResetRechargeableBatteryCapacity()
        {
            using (var logger = new Curvex3Standard(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    logger.ResetRechargeableBatteryCapacity();
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }
    }



    [TestFixture]
    class ReadValues : ReadValuesCommands
    {
        public ReadValues()
            : base(Standard.USBCurvex3)
        {
        }

    }
}