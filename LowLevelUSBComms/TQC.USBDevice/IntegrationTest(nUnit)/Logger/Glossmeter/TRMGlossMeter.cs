using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationTestNUnit.Logger.GeneralLogger;
using NUnit.Framework;
using TQC.USBDevice;
using TQC.USBDevice.GlossMeter;

namespace IntegrationTestNUnit.Logger.Glossmeter
{
    [TestFixture]
    class TRMGlossMeterBasicCommands : BasicCommands
    {
        public TRMGlossMeterBasicCommands() : base(USBLogger.USBProductId.Glossmeter)           
        {
        }
        protected override TQCUsbLogger OpenLogger(bool miniumum = true)
        {
            return OpenTmrLogger(miniumum);
        }

        TmrLogger OpenTmrLogger(bool miniumum = true)
        {
            var logger = new TmrLogger();
            if (logger.Open(miniumum))
            {
                return logger;
            }
            throw new Exception("Failed to connect to logger " + ProductId.ToString());
            
        }

        [Test]
        public void CheckSerialNumber()
        {
            using (var logger = OpenLogger(false))
            {
                Console.WriteLine("Logger serial Number is: '{0}'", logger.LoggerSerialNumber);
                Console.WriteLine("Version: '{0}'", logger.Version);
                logger.Close();
            }
        }

        [Test]
        public void CheckTRM()
        {
            using (var logger = OpenTmrLogger(false))
            {

                Assert.That(logger.IsTmr, Is.EqualTo(true), "TMR is not set!");
                logger.Close();
            }
        }


        [Test]
        public void CheckBuzz()
        {
            using (var logger = OpenTmrLogger(false))
            {

                logger.Buzzer(1, 10, 2, 10);
                logger.Close();
            }
        }
        [TestCase(0, "Line 0")]
        [TestCase(1, "Line 1")]
        [TestCase(2, "Line 2")]
        [TestCase(3, "Line 3")]
        [TestCase(4, "Line 4")]
        [TestCase(5, "Line 5")]
        public void CheckText(int lineNo, string text)
        {
            using (var logger = OpenTmrLogger(false))
            {

                logger.WriteTextString(lineNo, text);
                logger.Close();
            }
        }

        [Test]
        public void CheckResetScreen()
        {
            using (var logger = OpenTmrLogger(false))
            {

                logger.ResetScreen();
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
