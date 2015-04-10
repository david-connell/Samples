using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice;

namespace IntegrationTestNUnit.Logger.GeneralLogger
{
        public abstract class BasicCommands
        {            
            protected USBLogger.USBProductId ProductId ;

            public BasicCommands(USBLogger.USBProductId product)
            {
                ProductId = product;
            }

            [Test]
            public void TestVersioning()
            {
                using (var logger = new USBLogger())
                {
                    var version = logger.COMObjectVersion;
                    Assert.That(version, Is.GreaterThan(new Version(1, 0)));
                    Console.WriteLine("Version = {0}", version);
                }
            }

            [Test]
            public void TestConnectivity()
            {
                using (var logger = new USBLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        Console.WriteLine("Logger serial Number is: '{0}'", logger.LoggerSerialNumber);
                        Console.WriteLine("Version: '{0}'", logger.Version);                        
                        logger.Close();
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }

            [Test]
            public void InvalidCommand()
            {
                using (var logger = new USBLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        Console.WriteLine("Logger serial Number is: '{0}'", logger.LoggerSerialNumber);
                        Console.WriteLine("Version: '{0}'", logger.Version);
                        //logger.Request(TQC.USBDevice.USBLogger.Commands.NotValidCommand, BitConverter.GetBytes((short)1));
                        Assert.Throws(typeof(CommandNotSuportedException), 
                            ()=>
                                logger.Request(TQC.USBDevice.USBLogger.Commands.NotValidCommand, BitConverter.GetBytes((short)1))
                                );
	  

                        
                        logger.Close();
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }

            

            [Test]
            public void GetSerialNummberRawAndNormal()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.Open(ProductId)) //Need more that basic
                    {
                        string serialNumber = logger.LoggerSerialNumber;
                        int serialNumberViaCommand = logger.SerialNumber;


                        Console.WriteLine("Serial Number: '{0}' {1}", serialNumber, serialNumberViaCommand);
                        int serialNumberAsInt = int.Parse(serialNumber);
                        
                        Assert.AreEqual(serialNumberAsInt, serialNumberViaCommand);

                        logger.Close();
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }


            [Test]
            public void EnableDebuging()
            {
                using (var logger = new TQCUsbLogger())
                {
                    logger.DebugOpen();
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {                        
                        Console.WriteLine(logger.DebugOutputFromPreviousCommand);
                        logger.DebugClose();
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }

            [TestCase(0)]
            [TestCase(1)]
            [TestCase(2)]
            public void DebugingPurging(int numberOfDays)
            {
                using (var logger = new TQCUsbLogger())
                {
                    Console.WriteLine(logger.DebugPurgePolicy) ;
                    logger.DebugPurgePolicy = numberOfDays;
                    Assert.That(logger.DebugPurgePolicy, Is.EqualTo(numberOfDays));
                }
            }

            [Test]
            public void DebugingFile()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        Console.WriteLine(logger.DebugFileNameBase);
                    }
                }
            }


            [TestCase(@"C:\Debug\myfile.txt")]
            public void DebugingFileBase(string fileName)
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        logger.DebugClose();
                        logger.DebugFileNameBase = fileName;
                        Console.WriteLine(logger.DebugFileNameBase);
                        Assert.That(logger.DebugFileNameBase, Is.EqualTo(fileName));
                        logger.DebugOpen();
                    }
                }
            }

            [Test]
            public void DebugingFileNameFollowsTheBase()
            {
                using (var logger = new TQCUsbLogger())
                {
                    string fileName  = string.Format(@"C:\Debug\{0}", Guid.NewGuid().ToString("D"));
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        logger.DebugOpen(fileName);
                        
                        Console.WriteLine(logger.DebugFileNameBase);
                        Assert.That(logger.DebugFileNameBase, Is.EqualTo(fileName));
                        Assert.That(logger.DebugFileName.Substring(0, fileName.Length), Is.EqualTo(fileName));
                        logger.DebugClose();
                        var contents = File.ReadAllLines(logger.DebugFileName);
                        Assert.That(contents.Any(), Is.EqualTo(true));
                        Console.WriteLine("File {0}", logger.DebugFileName);
                        Console.WriteLine(String.Join("\r\n", contents));
                    }
                }
            }


            [Test]
            public void ReadSoftwareVersion()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        logger.DebugOpen();
                        var value = logger.SoftwareVersion;
                        Console.WriteLine("Debug = {0}", logger.IsDebugOutputOpen);
                        Console.WriteLine(logger.DebugOutputFromPreviousCommand);
                        logger.DebugClose() ;
                        Assert.That(value.Major, Is.GreaterThanOrEqualTo(0));
                        Console.WriteLine(value);
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }

            [Test]
            public void ReadManufactureName()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        var value = logger.ManufactureName;                        
                        Console.WriteLine("Manufacture Name= '{0}'", value);
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }

            [Test]
            public void ReadHardwareVersion()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        var value = logger.HardwareVersion;
                        Assert.That(value.Major, Is.GreaterThanOrEqualTo(0));
                        Console.WriteLine(value);
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }

            [Test]
            public void ReadDeviceName()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        var value = logger.DeviceName;
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
            public void ReadManufactureDate()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        var value = logger.ManufactureDate;
                        Assert.That(value, Is.GreaterThan(new DateTime(200, 1, 1)));
                        Console.WriteLine(value);
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }

            [Test]
            public void ReadProtocolVersion()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        var value = logger.ProtocolVersion;
                        Assert.That(value.Major, Is.GreaterThanOrEqualTo(0));
                        Console.WriteLine(value);
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }

            [Test]
            public void ReadDeviceType()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        var value = logger.DeviceType;
                        switch (ProductId)
                        {
                            case USBLogger.USBProductId.Glossmeter:
                                Assert.That(value, Is.EqualTo(DeviceType.PolyGlossmeter));
                                break;
                            case USBLogger.USBProductId.GRADIENT_OVEN:
                                Assert.That(value, Is.EqualTo(DeviceType.GRO));
                                break;
                            case USBLogger.USBProductId.USB_CURVEX_3:
                            case USBLogger.USBProductId.USB_CURVEX_3a:
                                Assert.That(value, Is.EqualTo(DeviceType.CurveX3_Basic));
                                break;
                            case USBLogger.USBProductId.USB_THERMOCOUPLE_SIMULATOR:
                                Assert.That(value, Is.EqualTo(DeviceType.ThermocoupleSimulator));
                                break;
                            default:
                                throw new Exception(string.Format("Unknown logger type {0} value {1}", ProductId, value));
                        }
                        
                        Console.WriteLine(value);
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }


            [Test]
            public void GetCalibrationRawAndNormal()
            {
                using (var logger = new USBLogger())
                {
                    if (logger.Open(ProductId))
                    {
                        string calibrationCompany = logger.CalibrationCompany;


                        var response = logger.Request(TQC.USBDevice.USBLogger.Commands.ReadCalibrationDetails, BitConverter.GetBytes((short)1));
                        string calibrationCompanyRaw = System.Text.Encoding.Default.GetString(response).Split('\0')[0];
                        


                        Assert.AreEqual(calibrationCompany, calibrationCompanyRaw);

                        logger.Close();
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }

            [Test]
            public void ReadNumberOfProbes()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        switch(ProductId)
                        {
                            case USBLogger.USBProductId.Glossmeter:
                                Assert.That(logger.NumberOfProbes, Is.EqualTo(3));
                                break;
                            case USBLogger.USBProductId.GRADIENT_OVEN:
                                Assert.That(logger.NumberOfProbes, Is.EqualTo(0));
                                break;
                            case USBLogger.USBProductId.USB_CURVEX_3a:
                                Assert.That(logger.NumberOfProbes, Is.EqualTo(4));
                                break;
                            default:
                                throw new Exception(string.Format("Logger {0} is currently not supported", ProductId));
                        }
                        logger.Close();
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }


            [Test]
            public void ReadNumberOfBatches()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        switch (ProductId)
                        {
                            case USBLogger.USBProductId.Glossmeter:
                                Assert.That(logger.NumberOfBatches, Is.EqualTo(8));
                                break;
                            case USBLogger.USBProductId.GRADIENT_OVEN:
                                Assert.That(logger.NumberOfBatches, Is.EqualTo(0));
                                break;
                            case USBLogger.USBProductId.USB_CURVEX_3a:
                                Assert.That(logger.NumberOfBatches, Is.EqualTo(10));
                                break;
                            default:
                                throw new Exception(string.Format("Logger {0} is currently not supported", ProductId));
                        }
                        logger.Close();
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }

            [Test]
            public void CheckOffloafingAndSendingSetup()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
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


            [Test]
            public void StateOfLogger()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        Console.WriteLine("Logger = {0}", logger.StateOfLogger);
                        logger.Close();
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }

        

        [Test]
        public void EnterBootloaderDirect()
        {
            //if (MessageBox.Show("Are you sure you want to enter bootloader?", "Bootloader", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {

                        logger.EnterBootloaderMode();
                        Console.WriteLine("***BOOTLOADER Mode****");
                    }
                    else
                    {
                        throw new Exception("Failed to connect to logger " + ProductId.ToString());
                    }
                }
            }
        }
    }
}


