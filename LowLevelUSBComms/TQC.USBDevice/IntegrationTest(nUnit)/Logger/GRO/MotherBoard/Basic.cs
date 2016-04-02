using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IntegrationTestNUnit.Logger.GeneralLogger;
using NUnit.Framework;
using TQC.USBDevice;
using TQC.USBDevice.GradientOven;

namespace IntegrationTestNUnit.Logger.GRO.MotherBoard
{
    [TestFixture]
    class Basic : BasicCommands
    {
        public Basic()
            : base(USBLogger.USBProductId.GRADIENT_OVEN)
        {
        }

        [TestCase(5, SpeedTestType.DeviceType)]
        [TestCase(10, SpeedTestType.DeviceType)]
        [TestCase(100, SpeedTestType.DeviceType)]
        [TestCase(100, SpeedTestType.Status)]
        [TestCase(100, SpeedTestType.Name)]
        public void Speed(int numberOfSeconds, SpeedTestType typeOfTest)
        {
            using (var logger = new GROMainBoard(null))
            {
                if (logger.OpenWithMinumumRequests(USBLogger.USBProductId.GRADIENT_OVEN))
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
                                        var type = logger.Status;
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

    }

    [TestFixture]
    class Calibration : CalibrationCommands
    {
        public Calibration()
            : base(USBLogger.USBProductId.GRADIENT_OVEN)
        {
        }
    }


    [TestFixture]
    class ReadValues : ReadValuesCommands
    {
        public ReadValues()
            : base(USBLogger.USBProductId.GRADIENT_OVEN)
        {
        }
    }
    
}
