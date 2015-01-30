using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IntegrationTestNUnit.Logger.GeneralLogger;
using NUnit.Framework;
using TQC.USBDevice;

namespace IntegrationTestNUnit.Logger.ThermocoupleSimulator
{
    [TestFixture]
    class Basic : BasicCommands
    {
        public Basic()
            : base(USBLogger.USBProductId.USB_THERMOCOUPLE_SIMULATOR)
        {
        }

        [Test]
        public void ColdJunctionTemperature()
        {
            using (var logger = new TQC.USBDevice.ThermocoupleSimulator.ThermocoupleSimulator())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var temp = logger.ColdJunctionTemperature;

                    Console.WriteLine("Temp = {0}", temp);
                    Assert.That(temp, Is.GreaterThan(10));
                    Assert.That(temp, Is.LessThan(30));
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }

        }

        [Test]
        public void BoardTemperature()
        {
            using (var logger = new TQC.USBDevice.ThermocoupleSimulator.ThermocoupleSimulator())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var temp = logger.BoardTemperature;

                    Console.WriteLine("Temp = {0}", temp);
                    Assert.That(temp, Is.GreaterThan(10));
                    Assert.That(temp, Is.LessThan(30));
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }

        }


        [TestCase(0)]
        [TestCase(100)]
        [TestCase(200)]
        [TestCase(300)]
        public void SetTemperatureOutput(double temp)
        {
            using (var logger = new TQC.USBDevice.ThermocoupleSimulator.ThermocoupleSimulator())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    Console.WriteLine("Temp = {0}", temp);
                    logger.SetTemperatureOutput(temp);                    
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [TestCase(1933)]
        public void SetSerialNumber(int serialNumber)
        {
            using (var logger = new TQC.USBDevice.ThermocoupleSimulator.ThermocoupleSimulator())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {

                    logger.SerialNumber = serialNumber;
                    Assert.That(logger.SerialNumber, Is.EqualTo(serialNumber));
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }

        }

        [TestCase("This is a certificate")]
        public void SetCalibrationCertificate(string certificate)
        {
            using (var logger = new TQC.USBDevice.ThermocoupleSimulator.ThermocoupleSimulator())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {

                    logger.CalibrationCertificate = certificate;
                    Assert.That(logger.CalibrationCertificate, Is.EqualTo(certificate));
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }

        }

        [Test]
        public void EnterBootloader()
        {
            if (MessageBox.Show("Are you sure you want to enter bootloader?", "Bootloader", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var logger = new TQC.USBDevice.ThermocoupleSimulator.ThermocoupleSimulator())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {

                        logger.EnterBootloaderMode();
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }
        }
    }

    [TestFixture]
    class Calibration : CalibrationCommands
    {
        public Calibration()
            : base(USBLogger.USBProductId.USB_THERMOCOUPLE_SIMULATOR)
        {
        }
    }


    [TestFixture]
    class ReadValues : ReadValuesCommands
    {
        public ReadValues()
            : base(USBLogger.USBProductId.USB_THERMOCOUPLE_SIMULATOR)
        {
        }
    }
    
}
