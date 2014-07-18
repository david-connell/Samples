using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice;
using TQC.USBDevice.GradientOven;

namespace IntegrationTestNUnit
{
    
    [TestFixture(USBLogger.USBProductId.GRADIENT_OVEN)]
    class GROMotherBoardCommands
    {
        USBLogger.USBProductId ProductId;

        public GROMotherBoardCommands(USBLogger.USBProductId product)
        {
            ProductId = product;
        }


        [Test]
        public void ReadInternalStatus()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.Open(ProductId))
                {
                    var channels = logger.InternalChannels();
                    int id = 1;
                    foreach (var channel in channels)
                    {
                        Console.WriteLine("Channel {0} = {1}", id++, channel);
                    }                    
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        [Test]
        public void ReadFanSpeed()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.Open(ProductId))
                {
                    var val = logger.FanSpeed;
                    Console.WriteLine("FanSpeed = {0}", val);                    
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        [Test]
        public void SetFanSpeed()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.Open(ProductId))
                {
                    var val = logger.FanSpeed;
                    Console.WriteLine("FanSpeed = {0}", val);
                    if (val.Value < 10)
                        val = new Percentage((byte) (val.Value+1));
                    else
                        val = new Percentage((byte)(val.Value - 1));
                    Console.WriteLine("Set FanSpeed = {0}", val);
                    
                    logger.FanSpeed = val;
                    Thread.Sleep(100);
                    Assert.That(logger.FanSpeed, Is.EqualTo(val));
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void ReadCooling()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.Open(ProductId))
                {
                    var val = logger.Cooling;
                    Console.WriteLine("Cooling = {0}", val);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void ReadPowerSupply()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.Open(ProductId))
                {
                    var val = logger.Power;
                    Console.WriteLine("Power = {0}", val);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        [Test]
        public void ReadClamp()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.Open(ProductId))
                {
                    var val = logger.Clamp;
                    Console.WriteLine("Clamp = {0}", val);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void ReadCarrierPosition()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.Open(ProductId))
                {
                    var val = logger.CarrierPosition;
                    Console.WriteLine("CarrierPosition = {0}", val);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        [Test]
        public void ReadCarrierSpeed()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.Open(ProductId))
                {
                    var val = logger.CarrierSpeed;
                    Console.WriteLine("CarrierSpeed = {0}", val);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void ReadLift()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.Open(ProductId))
                {
                    var val = logger.Lift;
                    Console.WriteLine("Lift = {0}", val);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [TestCase(1)]
        public void ReadTemperatureHeater(short id)
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.Open(ProductId))
                {
                    var val = logger.GetTempSetting(id);
                    Console.WriteLine("GetTempSetting {0} = {1}", id, val);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

    }
}
