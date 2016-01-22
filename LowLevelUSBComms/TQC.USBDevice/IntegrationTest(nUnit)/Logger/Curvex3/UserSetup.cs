using System;
using NUnit.Framework;
using System.Collections.Generic;
using TQC.USBDevice;
namespace TQC.USBDevice.CurveX3.UserSetup
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
        public void DeviceType()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 102, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 0, request);
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
        public void DeviceName()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 3, request);
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
        public void FirmwareFeatures()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 101, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 1, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 2, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 11, request);
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

        [Test]
        public void LengthOfBatchNames()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 19, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 15, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 7, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 8, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 6, request);
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
        public void GetCommasProtocol()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 100, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("GetCommas Protocol");
                    }

                    if (result.Length < 2)
                    {
                            throw new TooLittleDataReceivedException("GetCommas Protocol", result.Length, 2);
                    }
                    UInt16 status = BitConverter.ToUInt16(result, 0);
                    Console.WriteLine("GetCommas Protocol={0}", status);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 4, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 5, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Device Manufactured date");
                    }

                    if (result.Length < 4)
                    {
                            throw new TooLittleDataReceivedException("Device Manufactured date", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine("Device Manufactured date={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void CurrentStateOfLogger()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 12, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Current state of logger");
                    }

                    if (result.Length < 1)
                    {
                            throw new TooLittleDataReceivedException("Current state of logger", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Current state of logger={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void SendingSetup()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)1, 18, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Sending setup");
                    }

                    if (result.Length < 1)
                    {
                            throw new TooLittleDataReceivedException("Sending setup", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Sending setup={0}", status);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)2, 100, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)2, 200, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Name of channel 1");
                    }

                    Console.WriteLine("Name of channel 1 = '{0}'", TQCUsbLogger.DecodeString(result));
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)2, 101, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)2, 201, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Name of channel 2");
                    }

                    Console.WriteLine("Name of channel 2 = '{0}'", TQCUsbLogger.DecodeString(result));
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)2, 102, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)2, 202, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Name of channel 3");
                    }

                    Console.WriteLine("Name of channel 3 = '{0}'", TQCUsbLogger.DecodeString(result));
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)2, 103, request);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)2, 203, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Name of channel 4");
                    }

                    Console.WriteLine("Name of channel 4 = '{0}'", TQCUsbLogger.DecodeString(result));
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)2, 1, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Calibration company");
                    }

                    Console.WriteLine("Calibration company = '{0}'", TQCUsbLogger.DecodeString(result));
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)2, 2, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Calibration user name");
                    }

                    Console.WriteLine("Calibration user name = '{0}'", TQCUsbLogger.DecodeString(result));
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)2, 0, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Date of logger Calibration");
                    }

                    if (result.Length < 4)
                    {
                            throw new TooLittleDataReceivedException("Date of logger Calibration", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine("Date of logger Calibration={0}", status);
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
                    var result = logger.GetResponse(0, (USBLogger.Commands)7, 1, request);
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

        [Test]
        public void RealTimeClock()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)7, 0, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("RealTimeClock");
                    }

                    if (result.Length < 4)
                    {
                            throw new TooLittleDataReceivedException("RealTimeClock", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine("RealTimeClock={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void TemperatureUnits()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)7, 5, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Temperature Units");
                    }

                    if (result.Length < 1)
                    {
                            throw new TooLittleDataReceivedException("Temperature Units", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Temperature Units={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void NumberOfPaintTypes()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)7, 9, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Number of Paint types");
                    }

                    if (result.Length < 1)
                    {
                            throw new TooLittleDataReceivedException("Number of Paint types", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Number of Paint types={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void CurrentPaintType()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)7, 6, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Current Paint type");
                    }

                    if (result.Length < 1)
                    {
                            throw new TooLittleDataReceivedException("Current Paint type", result.Length, 1);
                    }
                    byte status = result[0];
                    Console.WriteLine("Current Paint type={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void PaintType1()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)7, 1000, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Paint type 1");
                    }

                    if (result.Length < 4)
                    {
                            throw new TooLittleDataReceivedException("Paint type 1", result.Length, 4);
                    }
                    UInt32 status = BitConverter.ToUInt32(result, 0);
                    Console.WriteLine("Paint type 1={0}", status);
                }
                else
                {
                    throw new Exception("Failed to open logger");
                }
            }
            return;
        }

        [Test]
        public void BatchName()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    request.Add(1);
                    var result = logger.GetResponse(0, (USBLogger.Commands)7, 8, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Batch Name");
                    }

                    Console.WriteLine("Batch Name = '{0}'", TQCUsbLogger.DecodeString(result));
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
    public class WriteDeviceSetupDetails 
    {
        private USBLogger.USBProductId ProductId;
        public WriteDeviceSetupDetails()
        {
            ProductId = USBLogger.USBProductId.USB_CURVEX_3a;
            return;
        }

        [Test]
        public void TemperatureUnits()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)23, 5, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Temperature Units");
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
        public void RealTimeClock()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)23, 0, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("RealTimeClock");
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
        public void SampleRate()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)23, 1, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Sample Rate");
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
        public void BatchName()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)23, 8, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Batch Name");
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
        public void NumberOfPaintTypes()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)23, 9, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Number of Paint types");
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
        public void PaintType1()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)23, 1000, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Paint type 1");
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
        public void CurrentPaintType()
        {
            using (var logger = new TQCUsbLogger(null))
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    List<byte> request = new List<byte>();
                    var result = logger.GetResponse(0, (USBLogger.Commands)23, 6, request);
                    if (result == null)
                    {
                        throw new NoDataReceivedException("Current Paint type");
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
}
