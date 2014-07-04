using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TQC.USBDevice;

namespace IntegrationTestProject1
{
    [TestClass]
    public class GradientOvenTests
    {
        USBLogger.USBProductId ProductId = USBLogger.USBProductId.GRADIENT_OVEN;
        //USBLogger.USBProductId ProductId = USBLogger.USBProductId.Glossmeter;
        [TestMethod]
        public void TestConnectivity()
        {
            using (var logger = new USBLogger())
            {
                if (logger.Open(ProductId))
                {
                    Console.WriteLine("Logger serial Number is: '{0}'", logger.LoggerSerialNumber);
                    Console.WriteLine("Version: '{0}'", logger.Version);
                    //Assert.AreEqual(logger.LoggerType, USBLogger.DeviceType.PolyGlossmeter);
                    logger.Close();
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        [TestMethod]
        public void GetSerialNummberRawAndNormal()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {
                    string serialNumber = logger.LoggerSerialNumber;
                    int serialNumberAsInt = int.Parse(serialNumber);
                    int serialNumberViaCommand = logger.SerialNumber;
                    Assert.AreEqual(serialNumberAsInt, serialNumberViaCommand);

                    logger.Close();
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [TestMethod]
        public void GetCalibrationRawAndNormal()
        {
            using (var logger = new USBLogger())
            {
                if (logger.Open(ProductId))
                {
                    string calibrationCompany = logger.CalibrationCompany;


                    var response = logger.Request(TQC.USBDevice.USBLogger.Commands.ReadCalibrationDetails, BitConverter.GetBytes((short)1));
                    string calibrationCompanyRaw = System.Text.Encoding.Default.GetString(response).TrimEnd('\0');


                    Assert.AreEqual(calibrationCompany, calibrationCompanyRaw);

                    logger.Close();
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }
        [TestMethod]
        public void GetDeviceName()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {
                    var deviceName = logger.DeviceName;                    
                    Assert.IsFalse(String.IsNullOrEmpty(deviceName));
                    Console.WriteLine("Device {0}", deviceName);
                    logger.Close();
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [TestMethod]
        public void GeNumberOfProbes()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {                    
                    Assert.IsTrue(logger.NumberOfProbes == 3);                    
                    logger.Close();
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [TestMethod]
        public void CheckOffloafingAndSendingSetup()
        {
            using (var logger = new TQCUsbLogger())
            {
                if (logger.Open(ProductId))
                {
                    Assert.IsTrue(logger.CanOffload);
                    Assert.IsTrue(logger.CanSendSetup);
                    logger.Close();
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }
    }
}
