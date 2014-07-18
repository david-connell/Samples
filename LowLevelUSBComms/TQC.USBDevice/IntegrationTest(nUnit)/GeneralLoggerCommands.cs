﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice;

namespace IntegrationTestNUnit
{
        [TestFixture(USBLogger.USBProductId.Glossmeter)]
        [TestFixture(USBLogger.USBProductId.GRADIENT_OVEN)]
        public class GeneralLoggerCommands
        {            
            USBLogger.USBProductId ProductId ;

            public GeneralLoggerCommands(USBLogger.USBProductId product)
            {
                ProductId = product;
            }

            [Test]
            public void TestConnectivity()
            {
                using (var logger = new USBLogger())
                {
                    if (logger.Open(ProductId))
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
                    if (logger.Open(ProductId))
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

            [Test]
            public void ReadSoftwareVersion()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.Open(ProductId))
                    {
                        var value = logger.SoftwareVersion;
                        Assert.That(value.Major, Is.GreaterThanOrEqualTo(0));
                        logger.Close();
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
                    if (logger.Open(ProductId))
                    {
                        var value = logger.HardwareVersion;
                        Assert.That(value.Major, Is.GreaterThanOrEqualTo(0));                        
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
                    if (logger.Open(ProductId))
                    {
                        var value = logger.DeviceName;
                        Assert.That(value, Is.Not.Null);
                        Assert.That(value, Is.Not.EqualTo(""));                                                
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
                    if (logger.Open(ProductId))
                    {
                        var value = logger.ManufactureName;
                        Assert.That(value, Is.Not.Null);
                        Assert.That(value, Is.Not.EqualTo(""));
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
                    if (logger.Open(ProductId))
                    {
                        var value = logger.ManufactureDate;
                        Assert.That(value, Is.GreaterThan(new DateTime(200, 1, 1)));
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

            [Test]
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

            [Test]
            public void GeNumberOfProbes()
            {
                using (var logger = new TQCUsbLogger())
                {
                    if (logger.Open(ProductId))
                    {
                        switch(ProductId)
                        {
                            case USBLogger.USBProductId.Glossmeter:
                                Assert.That(logger.NumberOfProbes, Is.EqualTo(3));
                                break;
                            case USBLogger.USBProductId.GRADIENT_OVEN:
                                Assert.That(logger.NumberOfProbes, Is.EqualTo(0));
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

