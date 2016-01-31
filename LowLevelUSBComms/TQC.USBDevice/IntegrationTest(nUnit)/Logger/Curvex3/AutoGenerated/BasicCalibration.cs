﻿
using System;
using NUnit.Framework;
using System.Collections.Generic;
using TQC.USBDevice;
namespace TQC.USBDevice.TQCCurveX3Basic.BasicCalibration
{
    [TestFixture]
    public class ReadDeviceInformation 
    {
        private USBLogger.USBProductId ProductId;
        public ReadDeviceInformation()
        {
            ProductId = USBLogger.USBProductId.USB_CURVEX_3a;
            return;
        }

        [Test]
        public void DeviceName()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x3, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Device Name");
                    }

                    Console.WriteLine("Device Name = '{0}'", TQCUsbLogger.DecodeString(result));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void DeviceType()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x66, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("DeviceType");
                    }

                    if (result.Length < 2)
                    {
                        throw new TooLittleDataReceivedException("DeviceType", result.Length, 2);
                    }
                    UInt16 status = BitConverter.ToUInt16(result, 0);
                    Console.WriteLine("DeviceType={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void DeviceSerialNumber()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x0, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Device Serial Number");
                    }

                    if (result.Length < 4)
                    {
                        throw new TooLittleDataReceivedException("Device Serial Number", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine("Device Serial Number={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void FirmwareFeatures()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x65, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Firmware features");
                    }

                    if (result.Length < 4)
                    {
                        throw new TooLittleDataReceivedException("Firmware features", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine("Firmware features={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void SWVersion()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x1, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("SW Version");
                    }

                    if (result.Length < 4)
                    {
                        throw new TooLittleDataReceivedException("SW Version", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine("SW Version={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void HWVersion()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x2, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("HW Version");
                    }

                    if (result.Length < 4)
                    {
                        throw new TooLittleDataReceivedException("HW Version", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine("HW Version={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void NumberOfChannels()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0xB, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Number of channels");
                    }

                    if (result.Length < 1)
                    {
                        throw new TooLittleDataReceivedException("Number of channels", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Number of channels={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test, ExpectedException(typeof(TQC.USBDevice.EnumerationNotSuportedException))]
        public void LengthOfBatchNames()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x13, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Length of Batch Names");
                    }

                    if (result.Length < 1)
                    {
                        throw new TooLittleDataReceivedException("Length of Batch Names", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Length of Batch Names={0}", status);
                    //Got back 7 as exception
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void MaxNumberOfPaintTypes()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0xF, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Max number of Paint Types");
                    }

                    if (result.Length < 1)
                    {
                        throw new TooLittleDataReceivedException("Max number of Paint Types", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Max number of Paint Types={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void MaxNumberOfBatches()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x7, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Max number of batches");
                    }

                    if (result.Length < 1)
                    {
                        throw new TooLittleDataReceivedException("Max number of batches", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Max number of batches={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void NumberOfBatchesUsed()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x8, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Number of batches used");
                    }

                    if (result.Length < 1)
                    {
                        throw new TooLittleDataReceivedException("Number of batches used", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Number of batches used={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void PhysicalMemory()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x6, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Physical Memory");
                    }

                    if (result.Length < 4)
                    {
                        throw new TooLittleDataReceivedException("Physical Memory", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine("Physical Memory={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void GetCommunicationsProtocol()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x64, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Get Communications Protocol");
                    }

                    if (result.Length < 2)
                    {
                        throw new TooLittleDataReceivedException("Get Communications Protocol", result.Length, 2);
                    }
                    UInt16 status = BitConverter.ToUInt16(result, 0);
                    Console.WriteLine("Get Communications Protocol={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void ManufactureName()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x4, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Manufacture Name");
                    }

                    Console.WriteLine("Manufacture Name = '{0}'", TQCUsbLogger.DecodeString(result));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void DeviceManufacturedDate()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x1, 0x5, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Device Manufactured Date");
                    }

                    if (result.Length < 4)
                    {
                        throw new TooLittleDataReceivedException("Device Manufactured Date", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    DateTime start = new DateTime(1970, 1, 1);
                    DateTime actualDate = start.AddSeconds(status);
                    
                    Console.WriteLine("Device Manufactured Date={0}", actualDate);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }
    }
    [TestFixture]
    public class ReadCalibrationDetails 
    {
        private USBLogger.USBProductId ProductId;
        public ReadCalibrationDetails()
        {
            ProductId = USBLogger.USBProductId.USB_CURVEX_3a;
            return;
        }

        [Test]
        public void TypeOfChannel1()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0x64, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Type of channel 1");
                    }

                    if (result.Length < 1)
                    {
                        throw new TooLittleDataReceivedException("Type of channel 1", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Type of channel 1={0}", status);
                    Assert.That(result[0], Is.EqualTo(1));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void NameOfChannel1()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0xC8, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Name of channel 1");
                    }

                    Console.WriteLine("Name of channel 1 = '{0}'", TQCUsbLogger.DecodeString(result));
                    Assert.That(TQCUsbLogger.DecodeString(result), Is.EqualTo("Probe 1"));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void TypeOfChannel2()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0x65, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Type of channel 2");
                    }

                    if (result.Length < 1)
                    {
                        throw new TooLittleDataReceivedException("Type of channel 2", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Type of channel 2={0}", status);
                    Assert.That(result[0], Is.EqualTo(1));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void NameOfChannel2()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0xC9, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Name of channel 2");
                    }

                    Console.WriteLine("Name of channel 2 = '{0}'", TQCUsbLogger.DecodeString(result));
                    Assert.That(TQCUsbLogger.DecodeString(result), Is.EqualTo("Probe 2"));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void TypeOfChannel3()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0x66, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Type of channel 3");
                    }

                    if (result.Length < 1)
                    {
                        throw new TooLittleDataReceivedException("Type of channel 3", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Type of channel 3={0}", status);
                    Assert.That(result[0], Is.EqualTo(1));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void NameOfChannel3()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0xCA, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Name of channel 3");
                    }

                    Console.WriteLine("Name of channel 3 = '{0}'", TQCUsbLogger.DecodeString(result));
                    Assert.That(TQCUsbLogger.DecodeString(result), Is.EqualTo("Probe 3"));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void TypeOfChannel4()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0x67, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Type of channel 4");
                    }

                    if (result.Length < 1)
                    {
                        throw new TooLittleDataReceivedException("Type of channel 4", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Type of channel 4={0}", status);
                    Assert.That(result[0], Is.EqualTo(1));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void NameOfChannel4()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0xCB, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Name of channel 4");
                    }

                    Console.WriteLine("Name of channel 4 = '{0}'", TQCUsbLogger.DecodeString(result));
                    Assert.That(TQCUsbLogger.DecodeString(result), Is.EqualTo("Probe 4"));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void CalibrationCompany()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0x1, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Calibration company");
                    }

                    Console.WriteLine("Calibration company = '{0}'", TQCUsbLogger.DecodeString(result));
                    Assert.That(TQCUsbLogger.DecodeString(result), Is.EqualTo("Windows7-PC"));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void CalibrationUserName()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0x2, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Calibration user name");
                    }

                    Console.WriteLine("Calibration user name = '{0}'", TQCUsbLogger.DecodeString(result));
                    Assert.That(TQCUsbLogger.DecodeString(result), Is.EqualTo("Windows7"));
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void DateOfLoggerCalibration()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0x0, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Date of logger Calibration");
                    }

                    if (result.Length < 4)
                    {
                        throw new TooLittleDataReceivedException("Date of logger Calibration", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    DateTime start = new DateTime(1970, 1, 1);
                    DateTime actualDate = start.AddSeconds(status);
                    
                    Console.WriteLine("Date of logger Calibration={0}", actualDate);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void CalibrationDetails1()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0x14, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Calibration Details 1");
                    }

                    if (result.Length < (sizeof(float) * 2) )
                    {
                        throw new TooLittleDataReceivedException("Calibration Details 1", result.Length, (sizeof(float) * 2) );
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        float status = BitConverter.ToSingle(result, i*sizeof(float));
                        Console.WriteLine("Calibration Details 1 #{0}={1}", i, status);
                    }
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void CalibrationDetails2()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0x15, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Calibration Details 2");
                    }

                    if (result.Length < (sizeof(float) * 2) )
                    {
                        throw new TooLittleDataReceivedException("Calibration Details 2", result.Length, (sizeof(float) * 2) );
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        float status = BitConverter.ToSingle(result, i*sizeof(float));
                        Console.WriteLine("Calibration Details 2 #{0}={1}", i, status);
                    }
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void CalibrationDetails3()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0x16, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Calibration Details 3");
                    }

                    if (result.Length < (sizeof(float) * 2) )
                    {
                        throw new TooLittleDataReceivedException("Calibration Details 3", result.Length, (sizeof(float) * 2) );
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        float status = BitConverter.ToSingle(result, i*sizeof(float));
                        Console.WriteLine("Calibration Details 3 #{0}={1}", i, status);
                    }
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void CalibrationDetails4()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x2, 0x17, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Calibration Details 4");
                    }

                    if (result.Length < (sizeof(float) * 2) )
                    {
                        throw new TooLittleDataReceivedException("Calibration Details 4", result.Length, (sizeof(float) * 2) );
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        float status = BitConverter.ToSingle(result, i*sizeof(float));
                        Console.WriteLine("Calibration Details 4 #{0}={1}", i, status);
                    }
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }
    }
    [TestFixture]
    public class ReadDeviceSetupDetails 
    {
        private USBLogger.USBProductId ProductId;
        public ReadDeviceSetupDetails()
        {
            ProductId = USBLogger.USBProductId.USB_CURVEX_3a;
            return;
        }

        [Test]
        public void SampleRate()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)0x7, 0x1, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Sample Rate");
                    }

                    if (result.Length < 4)
                    {
                        throw new TooLittleDataReceivedException("Sample Rate", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine("Sample Rate={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }
    }
    [TestFixture]
    public class WriteDeviceInformation 
    {
        private USBLogger.USBProductId ProductId;
        public WriteDeviceInformation()
        {
            ProductId = USBLogger.USBProductId.USB_CURVEX_3a;
            return;
        }

        [Test]
        public void DeviceSerialNumber()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    
                    request.Add(0x1);
                    request.Add(0x0);
                    request.Add(0x0);
                    request.Add(0x0);

                    var result = logger.GetResponse(0, (USBLogger.Commands)0x11, 0x0, request);
                    

                    //Got back OK
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }
    }
    [TestFixture]
    public class WriteCalibrationDetails 
    {
        private USBLogger.USBProductId ProductId;
        public WriteCalibrationDetails()
        {
            ProductId = USBLogger.USBProductId.USB_CURVEX_3a;
            return;
        }

        [Test]
        public void CalibrationCompany()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    
                    request.Add(0x57);
                    request.Add(0x69);
                    request.Add(0x6E);
                    request.Add(0x64);
                    request.Add(0x6F);
                    request.Add(0x77);
                    request.Add(0x73);
                    request.Add(0x37);
                    request.Add(0x2D);
                    request.Add(0x50);
                    request.Add(0x43);
                    request.Add(0x0);

                    var result = logger.GetResponse(0, (USBLogger.Commands)0x12, 0x1, request);
                    

                    //Got back OK
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }
    }
}
