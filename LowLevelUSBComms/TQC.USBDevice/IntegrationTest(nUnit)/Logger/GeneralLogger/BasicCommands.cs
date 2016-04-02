using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using NUnit.Framework;
using TQC.USBDevice;
using TQC.USBDevice.GlossMeter;
using TQC.USBDevice.GradientOven;

namespace IntegrationTestNUnit.Logger.GeneralLogger
{

    public enum SpeedTestType
    {
        Status,
        DeviceType,
        SerialNumber,
        Name,
    }
        public abstract class BasicCommands
        {            
            protected USBLogger.USBProductId ProductId ;

            public BasicCommands(USBLogger.USBProductId product)
            {
                ProductId = product;
            }
            [TestFixtureSetUp]
            public void Setup()
            {
            }
            [TestFixtureTearDown]
            public void TearDown()
            {
            }


            [Test]
            public void __TestAssemblyVersioning()
            {
                var assembly = Assembly.GetExecutingAssembly();
                var name = assembly.GetName();

                Console.WriteLine("Test Code = {0}", assembly.CodeBase);
                Console.WriteLine("Test Version = {0}", name.Version);
                var loggerAssembly = USBLogger.USBProductId.Glossmeter.GetType().Assembly;
                Console.WriteLine("Logger Code = {0}", loggerAssembly.CodeBase);
                Console.WriteLine("Logger Version = {0}", loggerAssembly.GetName().Version);

                Assert.That(name.Version.Major, Is.EqualTo(loggerAssembly.GetName().Version.Major), "Major Mismatch versions");
                Assert.That(name.Version.Minor, Is.EqualTo(loggerAssembly.GetName().Version.Minor), "Minor Mismatch versions");
                Assert.That(name.Version.Build, Is.EqualTo(loggerAssembly.GetName().Version.Build), "Build Mismatch versions");

                
            }


            [TestCase(5, SpeedTestType.DeviceType, true, 0,0)]
            [TestCase(5, SpeedTestType.DeviceType, false, 0, 0)]
            //[TestCase(10, SpeedTestType.DeviceType)]
            [TestCase(200, SpeedTestType.DeviceType, false, 0, 0)]
            [TestCase(2000, SpeedTestType.DeviceType, false, 0, 0)]
            [TestCase(200, SpeedTestType.DeviceType, false, 10, 0)]
            [TestCase(200, SpeedTestType.DeviceType, false, 5, 5)]
            [TestCase(200, SpeedTestType.DeviceType, false, 2, 2)]
            //[TestCase(100, SpeedTestType.Status)]
            //[TestCase(100, SpeedTestType.Name)]
            //[TestCase(200, SpeedTestType.Name)]
            public void GeneralSpeed(int numberOfSeconds, SpeedTestType typeOfTest, bool useNative, int pre, int post)
            {
                Configuration config = new Configuration();
                config.UseNativeCommunication = useNative;
                config.PreSleepMilliseconds= pre; 
                config.PostSleepMilliseconds = post;
                using (var logger = OpenLogger())
                {
                    
                    {
                        int totalNumberOfMilliseconds = numberOfSeconds * 1000;
                        var stopWatch = new Stopwatch();
                        int count = 0;
                        int exception = 0;
                        stopWatch.Start();
                        while (stopWatch.ElapsedMilliseconds < totalNumberOfMilliseconds)
                        {
                            try
                            {
                                switch (typeOfTest)
                                {
                                    case SpeedTestType.DeviceType:
                                        {
                                            var type = logger.DeviceType;
                                        }
                                        break;
                                    case SpeedTestType.Name:
                                        {
                                            var type = logger.DeviceName;
                                        }
                                        break;
                                    case SpeedTestType.SerialNumber:
                                        {
                                            var type = logger.SerialNumber;
                                        }
                                        break;
                                    case SpeedTestType.Status:
                                        {
                                            byte buttonStatus;
                                            int status;
                                            logger.UserInterfaceStatus(out buttonStatus, out status);
                                        }
                                        break;
                                }
                                count++;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                exception++;
                            }

                        }
                        stopWatch.Stop();

                        Console.WriteLine("Calls Rate = {0} / sec", count / stopWatch.Elapsed.TotalSeconds);
                        Console.WriteLine("Exception count = {0}", exception);
                        logger.Close();
                    }
                }
            }


            [Test]
            public void TestVersioning()
            {
                using (var logger = new USBLogger(null))
                {
                    var version = logger.COMObjectVersion;
                    Assert.That(version, Is.GreaterThan(new Version(1, 0)));
                    Console.WriteLine("Version = {0}", version);
                }
            }

            protected virtual TQCUsbLogger OpenLogger(bool miniumum = true)
            {
                var logger = new TQCUsbLogger(null);
                if (logger.Open(ProductId, miniumum))
                {
                    return logger;
                }
                throw new Exception("Failed to connect to logger " + ProductId.ToString());
            }
            [Test]
            public void TestConnectivity()
            {
                using (var logger = OpenLogger())
                {
                    Console.WriteLine("Logger serial Number is: '{0}'", logger.LoggerSerialNumber);
                    Console.WriteLine("Version: '{0}'", logger.Version);                        
                    logger.Close();
                }
            }

            [Test]
            public void InvalidCommand()
            {
                using (var logger = OpenLogger())
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
            }

            

            [Test]
            public void GetSerialNummberRawAndNormal()
            {
                using (var logger = OpenLogger(false))
                {
                    string serialNumber = logger.LoggerSerialNumber;
                    int serialNumberViaCommand = logger.SerialNumber;


                    Console.WriteLine("Serial Number: '{0}' {1}", serialNumber, serialNumberViaCommand);
                    int serialNumberAsInt = int.Parse(serialNumber);
                        
                    Assert.AreEqual(serialNumberAsInt, serialNumberViaCommand);

                    logger.Close();
                }
            }


            [Test]
            public void EnableDebuging()
            {
                using (var logger = new TQCUsbLogger(null))
                {
                    
                    if (logger.OpenWithMinumumRequests(ProductId))
                    {
                        logger.DebugOpen();
                        Console.WriteLine(logger.DebugOutputFromPreviousCommand);
                        logger.DebugClose();
                    }
                    else
                    {
                        Assert.Fail("Failed to connect to logger " + ProductId.ToString());                        
                    }
                }
            }

            [TestCase(0)]
            [TestCase(1)]
            [TestCase(2)]
            public void DebugingPurging(int numberOfDays)
            {
                using (var logger = OpenLogger(true))
                {                    
                    Console.WriteLine(logger.DebugPurgePolicy) ;
                    logger.DebugPurgePolicy = numberOfDays;
                    Assert.That(logger.DebugPurgePolicy, Is.EqualTo(numberOfDays));
                }
            }

            [Test]
            public void DebugingFile()
            {                
                using (var logger = OpenLogger(false))
                {
                   Console.WriteLine(logger.DebugFileNameBase);                    
                }
            }


            [TestCase(@"C:\Debug\myfile.txt")]
            public void DebugingFileBase(string fileName)
            {
                using (var logger = OpenLogger())
                {
                    logger.DebugClose();
                    logger.DebugFileNameBase = fileName;
                    Console.WriteLine(logger.DebugFileNameBase);
                    Assert.That(logger.DebugFileNameBase, Is.EqualTo(fileName));
                    logger.DebugOpen();
                }
            }

            [Test]
            public void DebugingFileNameFollowsTheBase()
            {
                using (var logger = OpenLogger())
                {
                
                    string fileName  = string.Format(@"C:\Debug\{0}", Guid.NewGuid().ToString("D"));
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


            [Test]
            public void ReadSoftwareVersion()
            {
                using (var logger = OpenLogger())
                {
                        logger.DebugOpen();
                        var value = logger.SoftwareVersion;
                        Console.WriteLine("Debug = {0}", logger.IsDebugOutputOpen);
                        Console.WriteLine(logger.DebugOutputFromPreviousCommand);
                        logger.DebugClose() ;
                        Assert.That(value.Major, Is.GreaterThanOrEqualTo(0));
                        Console.WriteLine(value);                    
                }
            }

            [Test]
            public void ReadManufactureName()
            {
                using (var logger = OpenLogger())
                {
                    var value = logger.ManufactureName;                        
                    Console.WriteLine("Manufacture Name= '{0}'", value);                    
                }
            }

            [Test]
            public void ReadHardwareVersion()
            {
                using (var logger = OpenLogger())
                {
                    var value = logger.HardwareVersion;
                    Assert.That(value.Major, Is.GreaterThanOrEqualTo(0));
                    Console.WriteLine(value);
                }
            }

            [Test]
            public void ReadDeviceName()
            {
                using (var logger = OpenLogger())
                {
                    var value = logger.DeviceName;
                    Assert.That(value, Is.Not.Null);
                    Assert.That(value, Is.Not.EqualTo(""));
                    Console.WriteLine(value);
                }
            }


            [Test]
            public void ReadManufactureDate()
            {
                using (var logger = OpenLogger())
                {
                    var value = logger.ManufactureDate;
                    Assert.That(value, Is.GreaterThan(new DateTime(200, 1, 1)));
                    Console.WriteLine(value);
                }
            }

            [Test]
            public void ReadProtocolVersion()
            {
                using (var logger = OpenLogger())
                {
                    var value = logger.ProtocolVersion;
                    Assert.That(value.Major, Is.GreaterThanOrEqualTo(0));
                    Console.WriteLine(value);
                }
            }

            [Test]
            public void TestDisconnect()
            {                
                using (var logger = GetLoggerByType(ProductId))
                {
                    if (logger.Open(ProductId))
                    {                    
                        DateTime timeToFinish = DateTime.Now.AddSeconds(10);
                        int state = 0;
                        while ((DateTime.Now < timeToFinish) && (state < 2) )
                        {
                            double time2Finish = (timeToFinish-DateTime.Now ).TotalSeconds;
                            bool isConnected = OutputText(logger, string.Format(" Disconnect Test {0} secs", (int)time2Finish));
                            switch (state)
                            {
                                case 0:
                                    if (!isConnected)
                                    {
                                        state++;
                                        timeToFinish = DateTime.Now.AddSeconds(30);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Waiting for Disconnect");
                                        Thread.Sleep(100);
                                    }
                                    break;
                                case 1:
                                    if (isConnected)
                                    {
                                        state++;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Waiting for Connect");
                                        Thread.Sleep(200);
                                    }
                                    break;                                
                            }
                        }
                        OutputText(logger, "Finished");
                        Assert.That(state, Is.Not.EqualTo(1));
                    }
                    
                }
            }

            private bool OutputText(TQCUsbLogger logger, string text)
            {
                bool isConnected = false ;
                try
                {
                    switch (ProductId)
                    {
                        case USBLogger.USBProductId.Glossmeter:
                            {
                                var gml = logger as GlossMeterLogger;
                                if (gml != null)
                                {
                                    gml.WriteTextString(5, text);
                                }
                            }
                            break;
                        case USBLogger.USBProductId.USB_THERMOCOUPLE_SIMULATOR:
                            {
                                var ts = logger as TQC.USBDevice.ThermocoupleSimulator.ThermocoupleSimulator;
                                Console.WriteLine("BoardTemp = {0}", ts.BoardTemperature);
                            }
                            break;
                    }
                    isConnected = true;
                }
                catch (UsbDisconnectedException )
                {
                    ;
                }
                return isConnected;
            }
            

            public TQCUsbLogger GetLoggerByType(USBLogger.USBProductId loggerType)
            {
                switch (loggerType)
                {
                    case USBLogger.USBProductId.Glossmeter:
                        return new GlossMeterLogger(null);
                        

                    case USBLogger.USBProductId.GRADIENT_OVEN:
                        return new GROMainBoard(null);
                        

                    case USBLogger.USBProductId.USB_THERMOCOUPLE_SIMULATOR:
                        return new TQC.USBDevice.ThermocoupleSimulator.ThermocoupleSimulator(null);
                        

                    default:
                    case USBLogger.USBProductId.USB_CURVEX_3:
                    case USBLogger.USBProductId.USB_CURVEX_3a:
                        return new TQCUsbLogger(null);
                        
                }
            }
            static Exception s_ExceptionCaused;
            static ManualResetEvent s_ManualEvent;
            public class TestSpecifiedDevice : UsbLibrary.SpecifiedDevice
            {
                static ILog s_Log = LogManager.GetLogger("UsbLibrary.TestSpecifiedDevice");
                public TestSpecifiedDevice()
                    : base()
                {
                    s_ExceptionCaused = null;
                }

                protected override void WriteCompleted(IAsyncResult ar)
                {
                    WriteCompleteTask task = (WriteCompleteTask)ar.AsyncState;
                    Thread.Sleep(5000);
                    try
                    {
                        if (task.FileStream != null)
                        {
                            task.FileStream.Flush();
                            task.FileStream.EndWrite(ar);
                        }
                    }
                    catch (Exception ex)
                    {
                        s_ExceptionCaused = ex;
                    }
                    task.Event.Set();
                    s_ManualEvent.Set();
                }
            }

            protected virtual TQCUsbLogger OpenTestLogger(Type typeOfDevice)
            {
                var logger = new TQCUsbLogger(null, typeOfDevice);
                if (logger.Open(ProductId, false))
                {
                    return logger;
                }
                throw new Exception("Failed to connect to logger " + ProductId.ToString());
            }


            [Test]
            public void TimeOutOnWriteOutCommand()
            {
                s_ManualEvent = new ManualResetEvent(false);
                using (var logger = OpenTestLogger(typeof(TestSpecifiedDevice)))
                {
                    DeviceType value = DeviceType.Unknown;
                    Assert.Throws<TQC.USBDevice.ResponsePacketErrorTimeoutException>(() => value = logger.DeviceType);
                    logger.Close();
                    if (!s_ManualEvent.WaitOne(6000, false))
                    {
                        Assert.Fail("Failed to wait long enough for exception to be generated");
                    }
                    
                    Assert.That(s_ExceptionCaused, Is.Not.Null, "No exception generated");
                    Assert.That(s_ExceptionCaused.GetType(), Is.EqualTo(typeof(System.ObjectDisposedException)));
                }

            }

            [Test]
            public void ReadDeviceType()
            {
                using (var logger = OpenLogger())
                {
                    DeviceType value = logger.DeviceType;
                    switch (ProductId)
                    {
                        case USBLogger.USBProductId.Glossmeter:
                            Assert.That(value, Is.EqualTo(DeviceType.PolyGlossmeter)
                                .Or.EqualTo(DeviceType.DuoGlossmeter)
                                .Or.EqualTo(DeviceType.SoloGlossmeter));
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
            }


            [Test]
            public void GetCalibrationRawAndNormal()
            {
                using (var logger = OpenLogger(false))
                {
                    string calibrationCompany = logger.CalibrationCompany;


                    var response = logger.Request(TQC.USBDevice.USBLogger.Commands.ReadCalibrationDetails, BitConverter.GetBytes((short)1));
                    string calibrationCompanyRaw = System.Text.Encoding.Default.GetString(response).Split('\0')[0];
                        


                    Assert.AreEqual(calibrationCompany, calibrationCompanyRaw);

                    logger.Close();
                }
            }

            [Test]
            public void ReadNumberOfProbes()
            {
                using (var logger = OpenLogger())
                {

                    switch (ProductId)
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
            }


            [Test]
            public void ReadNumberOfBatches()
            {
                using (var logger = OpenLogger())
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
            }

            [Test]
            public void CheckOffloafingAndSendingSetup()
            {
                using (var logger = OpenLogger())
                {
                    Assert.IsTrue(logger.CanOffload);
                    Assert.IsTrue(logger.CanSendSetup);
                    logger.Close();
                }
            }


            [Test]
            public void StateOfLogger()
            {
                using (var logger = OpenLogger())
                {
                    Console.WriteLine("Logger = {0}", logger.StateOfLogger);
                    logger.Close();
                }
            }

        

        [Test, Ignore]
        public void EnterBootloaderDirect()
        {
            //if (MessageBox.Show("Are you sure you want to enter bootloader?", "Bootloader", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var logger = OpenLogger())
                {

                    logger.EnterBootloaderMode();
                    Console.WriteLine("***BOOTLOADER Mode****");
                }
            }
        }
    }
}


