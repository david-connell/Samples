using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice;


namespace IntegrationTestNUnit.Logger.GeneralLogger
{
    [TestFixture(USBLogger.USBProductId.Glossmeter)]
    [TestFixture(USBLogger.USBProductId.GRADIENT_OVEN)]
    [TestFixture(USBLogger.USBProductId.USB_CURVEX_3a)]
    class CalibrationCommands
    {
        USBLogger.USBProductId ProductId;
        public CalibrationCommands(USBLogger.USBProductId product)
        {
            ProductId = product;
        }

        [Test]
        public void ReadDateOfCalibration()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {
                    var value = logger.CalibrationDate;
                    Assert.That(value, Is.GreaterThan(new DateTime(2000, 1, 1)));
                    Console.WriteLine(value);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void ReadCalibrationCompany()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {
                    var value = logger.CalibrationCompany;
                    Assert.That(value, Is.Not.Null);
                    Assert.That(value, Is.Not.EqualTo(""));
                    Console.WriteLine(value);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        [Test]
        public void ReadCalibrationUserName()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {
                    var value = logger.CalibrationUserName;
                    Assert.That(value, Is.Not.Null);
                    Assert.That(value, Is.Not.EqualTo(""));
                    Console.WriteLine(value);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }
        [Test]
        public void ReadCalibrationDetails()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {
                    int maxProbes = logger.NumberOfProbes;
                    for (int probeId = 0 ; probeId < maxProbes; probeId++)
                        ReadCalibrationDetails(logger, probeId);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        private void ReadCalibrationDetails(TQCUsbLogger logger, int probeId)
        {
            var value = logger.CalibrationDetails(probeId);
            Assert.That(value.C, Is.Not.EqualTo(0));
            Assert.That(value.M, Is.Not.EqualTo(0));
            Console.WriteLine("Probe {0} is {1}", probeId+1, value);
        }

        [Test]
        public void ReadTypesOfProbe()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {
                    int maxProbes = logger.NumberOfProbes;
                    for (int probeId = 0; probeId < maxProbes; probeId++)
                        ReadTypeOfProbe(logger, probeId);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }
        private void ReadTypeOfProbe(TQCUsbLogger logger, int probeId)
        {
            USBLogger.ProbeType type = USBLogger.ProbeType.Temperature;
            if (ProductId == USBLogger.USBProductId.Glossmeter)
            {
                type = USBLogger.ProbeType.Gloss;
            }
            var value = logger.ProbeType(probeId);
            Assert.That(value, Is.EqualTo(type));
            Console.WriteLine("Probe {0} is {1}", probeId + 1, value);
        }

        [Test]
        public void ReadProbeNames()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
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
