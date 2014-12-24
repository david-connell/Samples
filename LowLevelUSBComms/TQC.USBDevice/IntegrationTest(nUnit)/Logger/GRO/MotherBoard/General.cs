using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice;
using TQC.USBDevice.GradientOven;

namespace IntegrationTestNUnit.Logger.GRO
{
        
    class General
    {
        USBLogger.USBProductId ProductId;

        public General()
        {
            ProductId = USBLogger.USBProductId.GRADIENT_OVEN;
        }

        [TestCase(1000)]
        public void EricTest1(int numerOfAttempts)
        {
            float temperatureValue = 0.0f;
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    for (int loopCounter = 0; loopCounter < numerOfAttempts; loopCounter++)
                    {
                        short slotId = 0;
                        
                        var button = logger.Button;
                        logger.SetTempSetting(slotId, temperatureValue);
                        logger.GetTempSetting(slotId);
                    }
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }

        }
        [TestCase(ButtonStatus.OKPressed, "Press the OK Button")]
        [TestCase(ButtonStatus.CancelPressed, "Press the Cancel Button")]
        public void CheckButton(ButtonStatus buttonToCheck, string outputMessage)
        {
            using (var logger = new GROMainBoard())
            {
                Console.WriteLine("Test '{0}' ", outputMessage);
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    int state = 0;
                    int attempt = 0;
                    while (state != -1)
                    {
                        var val = logger.Button;
                        Console.WriteLine("Button = '{0}' ", val);

                        if (state == 0) //Look for No Button Pressed...
                        {
                            Console.WriteLine("Make sure that no button is pressed!");
                            Assert.That(val, Is.EqualTo(ButtonStatus.NothingPressed));
                            state = 1;
                            Console.WriteLine(outputMessage);                            
                        }
                        else if (state == 1) //Now see if the button is pressed!
                        {
                            if ((val & buttonToCheck) == buttonToCheck)
                            {
                                state = -1;
                            }
                            else
                            {
                                attempt++;
                                if (attempt > 50)   //50 attempts at 100ms = 5secs long test :(
                                {
                                    throw new Exception(string.Format("Failed to read the {0}", buttonToCheck));
                                }
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }

        }

        [TestCase(1)]
        public void ReadProbeName(short id)
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.ProbeName(id);
                    Console.WriteLine("ProbeName {0} = {1}", id, val);
                    var newVal = val + 1;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void ReadThermocoupleBoardIDs()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var boards = logger.ThermocoupleBoardIDs;
                    foreach (var board in boards)
                    {
                        Console.WriteLine("ThermocoupleBoardID {0}", board);
                    }                    
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [TestCase(1)]
        public void ReadProbeType(short id)
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.ProbeType(id);
                    Console.WriteLine("ProbeType {0} = {1}", id, val);
                    var newVal = val + 1;
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [Test]
        public void ReadInternalStatus()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
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
        public void ReadExternalFanSpeed()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.ExternalFanSpeed;
                    Console.WriteLine("ExternalFanSpeed = {0}", val);                    
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        [Test]
        public void SetExternalFanSpeed()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.ExternalFanSpeed;
                    Console.WriteLine("FanSpeed = {0}", val);
                    if (val.Value < 10)
                        val = new Percentage((byte) (val.Value+1));
                    else
                        val = new Percentage((byte)(val.Value - 1));
                    Console.WriteLine("Set External FanSpeed = {0}", val);
                    
                    logger.ExternalFanSpeed = val;
                    Thread.Sleep(100);
                    Assert.That(logger.ExternalFanSpeed, Is.EqualTo(val));
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }



        [Test]
        public void ReadInternalFanSpeed()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.InternalFanSpeed;
                    Console.WriteLine("InternalFanSpeed = {0}", val);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


        [Test]
        public void SetInternalFanSpeed()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.InternalFanSpeed;
                    Console.WriteLine("InternalFanSpeed = {0}", val);
                    if (val.Value < 10)
                        val = new Percentage((byte)(val.Value + 1));
                    else
                        val = new Percentage((byte)(val.Value - 1));
                    Console.WriteLine("Set Internal FanSpeed = {0}", val);

                    logger.InternalFanSpeed = val;
                    Thread.Sleep(100);
                    Assert.That(logger.ExternalFanSpeed, Is.EqualTo(val));
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
                if (logger.OpenWithMinumumRequests(ProductId))
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
        public void SetCooling()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.Cooling;
                    Console.WriteLine("Cooling = {0}", val);

                    if (val.Value < 10)
                        val = new Percentage((byte)(val.Value + 1));
                    else
                        val = new Percentage((byte)(val.Value - 1));
                    Console.WriteLine("Set Cooling = {0}", val);

                    logger.Cooling = val;
                    Thread.Sleep(100);
                    Assert.That(logger.Cooling, Is.EqualTo(val));
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
                if (logger.OpenWithMinumumRequests(ProductId))
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
        public void SetPowerSupply()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.Power;
                    Console.WriteLine("Power = {0}", val);
                    logger.Power = PowerState.OFF;
                    Assert.That(logger.Power, Is.EqualTo(val));
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
                if (logger.OpenWithMinumumRequests(ProductId))
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
        public void SetClamp()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.Clamp;
                    Console.WriteLine("Clamp = {0}", val);
                    val = val == ClampState.Open ? ClampState.Closed : ClampState.Open;
                    Console.WriteLine("Setting Clamp = {0}", val);
                    logger.Clamp = val;
                    Thread.Sleep(5000);
                    Assert.That(logger.Clamp, Is.EqualTo(val));
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
                if (logger.OpenWithMinumumRequests(ProductId))
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
        public void SetCarrierPosition()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.CarrierPosition;
                    Console.WriteLine("CarrierPosition = {0}", val);
                    if (val.PositionInMilliMeters < 10)
                        val = new CarrierPosition((byte)(val.PositionInMilliMeters + 1));
                    else
                        val = new CarrierPosition((byte)(val.PositionInMilliMeters - 1));
                    Console.WriteLine("Set CarrierPosition = {0}", val);
                    logger.CarrierPosition = val;
                    Thread.Sleep(1000);
                    Assert.That(logger.CarrierPosition, Is.EqualTo(val));
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
                if (logger.OpenWithMinumumRequests(ProductId))
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
        public void SetCarrierSpeed()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.CarrierSpeed;
                    Console.WriteLine("CarrierSpeed = {0}", val);
                    if (val.SpeedMillimetersPerSecond < 10)
                        val = new Speed((byte)(val.SpeedMillimetersPerSecond + 1));
                    else
                        val = new Speed((byte)(val.SpeedMillimetersPerSecond - 1));
                    Console.WriteLine("Set CarrierSpeed = {0}", val);
                    logger.CarrierSpeed = val;
                    Assert.That(logger.CarrierSpeed, Is.EqualTo(val));

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
                if (logger.OpenWithMinumumRequests(ProductId))
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


        [Test]
        public void SetLift()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.Lift;
                    Console.WriteLine("Lift = {0}", val);
                    val = val == LiftState.Down ? LiftState.Up : LiftState.Down;
                    Console.WriteLine("Set to Lift = {0}", val);
                    logger.Lift = val;
                    Thread.Sleep(5000);
                    Assert.That(logger.Lift, Is.EqualTo(val));
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

        [TestCase(1)]
        public void SetTemperatureHeater(short id)
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.OpenWithMinumumRequests(ProductId))
                {
                    var val = logger.GetTempSetting(id);
                    Console.WriteLine("GetTempSetting {0} = {1}", id, val);
                    var newVal = val+1;
                    Console.WriteLine("SetTempSetting {0} = {1}", id, newVal);
                    logger.SetTempSetting(id, newVal);
                    Thread.Sleep(1000);
                    Assert.That(Math.Abs(logger.GetTempSetting(id) - newVal), Is.LessThan(0.5));
                    logger.SetTempSetting(id, val);
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }

    }
}
