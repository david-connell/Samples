using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationTestNUnit.Logger.GeneralLogger;
using NUnit.Framework;
using TQC.USBDevice;
using TQC.USBDevice.GlossMeter;

namespace IntegrationTestNUnit.Logger.GlossMeter
{
    [TestFixture]
    class Basic : BasicCommands
    {
        public Basic()
            : base(USBLogger.USBProductId.Glossmeter)
        {
        }

        protected virtual GlossMeterLogger OpenGlossMeterLogger(bool miniumum = true)
        {
            var logger = new GlossMeterLogger(null);
            if (logger.Open(USBLogger.USBProductId.Glossmeter, miniumum))
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
        public void CheckBuzz()
        {
            using (var logger = OpenGlossMeterLogger(false))
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
            using (var logger = OpenGlossMeterLogger(false))
            {

                logger.WriteTextString(lineNo, text);
                logger.Close();
            }
        }

        [Test]
        public void CheckResetScreen()
        {
            using (var logger = OpenGlossMeterLogger(false))
            {

                logger.ResetScreen();
                logger.Close();
            }
        }


        [TestCase(0)]
        public void CheckCoefficients(int channelId)
        {
            using (var logger = OpenGlossMeterLogger(false))
            {

                var coeffs = logger.get_CalibrationCoeffients(channelId);
                //Assert.That(coeffs.Count, Is.EqualTo(5));
                foreach (var coeff in coeffs)
                {
                    Console.WriteLine(coeff);
                }
                logger.Close();
            }
        }

        [TestCase(0)]
        public void CheckSettingCoefficients(int channelId)
        {
            using (var logger = OpenGlossMeterLogger(false))
            {

                var coeffs = logger.get_CalibrationCoeffients(channelId);
                foreach (var coeff in coeffs)
                {
                    Console.WriteLine("Read {0}", coeff);
                }

                //Assert.That(coeffs.Count, Is.EqualTo(5));
                coeffs[1] += 0.1;
                foreach (var coeff in coeffs)
                {
                    Console.WriteLine("Write {0}", coeff);
                }

                logger.set_CalibrationCoeffients(channelId, coeffs);
                coeffs = logger.get_CalibrationCoeffients(channelId);
                foreach (var coeff in coeffs)
                {
                    Console.WriteLine("Read {0}", coeff);
                }
                logger.Close();
            }
        }

        [Test]
        public void CheckInternalGloss()
        {
            using (var logger = OpenGlossMeterLogger(false))
            {

                var result = logger.InternalGloss;

                Assert.That(result.Count, Is.EqualTo(3));
                foreach (var value in result)
                {
                    Console.WriteLine(value);
                    Assert.That(value, Is.GreaterThan(50.0).And.LessThan(150));
                }


                logger.Close();
            }
        }

        [Test]
        public void CheckSettingOfInternalGloss()
        {
            using (var logger = OpenGlossMeterLogger(false))
            {

                var result = logger.InternalGloss;


                var vals = new List<double>();
                vals.Add(100);
                vals.Add(200);
                vals.Add(300);
                logger.InternalGloss = vals;

                var temp = logger.InternalGloss;
                for (int id = 0; id < 3; id++)
                {
                    Assert.That(temp[id], Is.EqualTo((id + 1) * 100));
                }
                logger.InternalGloss = result;


                logger.Close();
            }
        }

        [Test]
        public void TestScan()
        {
            TestButton(x => x.IsScanButtonPressed, "SCAN");
        }

        [Test]
        public void TestOk()
        {
            TestButton(x => x.IsOKPressed, "OK");
        }


        [Test]
        public void TestUp()
        {
            TestButton(x => x.IsUpPressed, "Up");
        }

        [Test]
        public void TestDown()
        {
            TestButton(x => x.IsDownPressed, "Down");
        }

        [Test]
        public void TestInCraddle()
        {
            TestButton(x => x.IsInCraddle, "Craddle");
        }

        private void TestButton(Func<GlossMeterLogger, bool> buttonToPress, string nameOfButton)
        {
            using (var logger = OpenGlossMeterLogger(false))
            {
                DateTime until = DateTime.Now.AddSeconds(10);
                while (DateTime.Now < until)
                {
                    Assert.That(logger.ReadButtonStatus(), Is.EqualTo(true));
                    Console.WriteLine("Looking for {0}", nameOfButton);
                    if (buttonToPress(logger))
                    {
                        break;
                    }
                }
                Assert.That(buttonToPress(logger), Is.EqualTo(true));

                logger.Close();
            }
        }


    }

    [TestFixture]
    class Calibration : CalibrationCommands
    {
        public Calibration()
            : base(USBLogger.USBProductId.Glossmeter)
        {
        }
    }


    [TestFixture]
    class ReadValues : ReadValuesCommands
    {
        public ReadValues()
            : base(USBLogger.USBProductId.Glossmeter)
        {
        }
    }
    


}
