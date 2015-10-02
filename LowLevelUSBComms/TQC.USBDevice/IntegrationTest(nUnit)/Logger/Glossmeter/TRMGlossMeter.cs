using System;
using System.Collections.Generic;
using IntegrationTestNUnit.Logger.GeneralLogger;
using NUnit.Framework;
using TQC.USBDevice;
using TQC.USBDevice.GlossMeter;

namespace IntegrationTestNUnit.Logger.GlossMeter
{
    [TestFixture]
    class TrmGlossMeterBasicCommands : Basic
    {
        public TrmGlossMeterBasicCommands() //: base(USBLogger.USBProductId.Glossmeter)
        {
        }
        protected override TQCUsbLogger OpenLogger(bool miniumum = true)
        {
            return OpenGlossMeterLogger(miniumum);
        }

        override protected GlossMeterLogger OpenGlossMeterLogger(bool miniumum = true)
        {
            var logger = new TrmLogger(null);
            if (logger.Open(miniumum))
            {
                return logger;
            }
            throw new Exception("Failed to connect to logger " + ProductId.ToString());
            
        }

        [Test]
        public void CheckTrm()
        {
            using (var logger = OpenGlossMeterLogger(false))
            {

                Assert.That(logger.IsTmr, Is.EqualTo(true), "TRM is not set!");
                Console.WriteLine("CalibrationCertificate: '{0}'", logger.CalibrationCertificate);
                logger.Close();
            }
        }





    }

    //[TestFixture]
    //class Basic : BasicCommands
    //{
    //    public Basic()
    //        : base(USBLogger.USBProductId.Glossmeter)
    //    {
    //    }
    //}

    //[TestFixture]
    //class Calibration : CalibrationCommands
    //{
    //    public Calibration()
    //        : base(USBLogger.USBProductId.Glossmeter)
    //    {
    //    }
    //}


    //[TestFixture]
    //class ReadValues : ReadValuesCommands
    //{
    //    public ReadValues()
    //        : base(USBLogger.USBProductId.Glossmeter)
    //    {
    //    }
    //}
    
}
