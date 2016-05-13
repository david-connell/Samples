using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationTestNUnit.Logger.GeneralLogger;
using NUnit.Framework;
using TQC.USBDevice;

namespace IntegrationTestNUnit.Logger.Curvex3
{
    [TestFixture]
    class Basic : BasicCommands
    {
        public Basic()
            : base(USBLogger.USBProductId.USB_CURVEX_3_STANDARD)
        {
        }
        

    }

    [TestFixture]
    class Calibration : CalibrationCommands
    {
        public Calibration()
            : base(USBLogger.USBProductId.USB_CURVEX_3_STANDARD)
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
                        LinearCalibrationDetails details = new LinearCalibrationDetails(1,0);
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
    }


    [TestFixture]
    class ReadValues : ReadValuesCommands
    {
        public ReadValues()
            : base(USBLogger.USBProductId.USB_CURVEX_3_STANDARD)
        {
        }
       
    }
    
}
