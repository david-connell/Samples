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
    class ReadValues
    {
        USBLogger.USBProductId ProductId;

        public ReadValues(USBLogger.USBProductId product)
        {
            ProductId = product;
        }

        [Test]
        public void ReadChannelValues()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {
                    var probeValues = logger.ProbeValues;
                    
                    int id = 1;
                    foreach (var value in probeValues)
                    {
                        Assert.That(value, Is.GreaterThanOrEqualTo( 0) );
                        Console.WriteLine("Channel {0} value = {1}", id++, value);
                    }
                    Assert.That(probeValues.Count(), Is.EqualTo(logger.NumberOfProbes));
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void ReadColdJunctionValues()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {
                    var probeValues = logger.ColdJunctions;

                    int id = 1;
                    foreach (var value in probeValues)
                    {
                        Assert.That(value, Is.GreaterThanOrEqualTo(0));
                        Console.WriteLine("CJ {0} value = {1}", id++, value);
                    }
                    Assert.That(probeValues.Count(), Is.GreaterThanOrEqualTo(1));
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        [Test]
        public void ReadUserInterfaceStatus()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {
                    byte buttonStatus;
                    int status;
                    logger.UserInterfaceStatus(out buttonStatus, out status);

                    Console.WriteLine("Status = {0}", status);
                    Console.WriteLine("Button Status = {0}", buttonStatus);                    
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

    }
}
