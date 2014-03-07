using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TQC.USBDevice;

namespace IntegrationTestProject1
{
    [TestClass]
    public class GlossMeterTests
    {
        USBLogger.USBProductId ProductId = USBLogger.USBProductId.Glossmeter;
        [TestMethod]
        public void TestConnectivity()
        {
            using (var logger = new USBLogger())
            {
                if (logger.Open(ProductId))
                {
                    Console.WriteLine("Logger serial Number is: '{0}'", logger.LoggerSerialNumber);
                    Console.WriteLine("Version: '{0}'", logger.Version);
                    Assert.AreEqual(logger.LoggerType, USBLogger.DeviceType.PolyGlossmeter);
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
            using (var logger = new USBLogger())
            {
                if (logger.Open(ProductId))
                {
                    string serialNumber = logger.LoggerSerialNumber;


                    List<byte> request = new List<byte>();
                    request.Add(0);
                    request.Add(0);

                    var response = logger.Request(TQC.USBDevice.USBLogger.Commands.ReadDeviceInfo, request.ToArray());
                    var number = BitConverter.ToInt32(response, 0);

                    int serialNumberAsInt = int.Parse(serialNumber);
                    Assert.AreEqual(serialNumberAsInt, number);

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

    }
}
