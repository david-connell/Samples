using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TestClassToCommunicate;
using TQC.GOC.InterProcessCommunication.Model;

namespace NunitTest
{
    public class DateTimeProvider : IDateTimeProvider
    {

        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }

    public class TestOvenModel : IOvenModel
    {
        Random m_Rand = new Random();
        private double val = 10.0;
        public int Called { get; set; }
        public List<double> TemperatureValues
        {
            get 
            {
                List<double> result = new List<double>(GetProbeCount());
                for(int counter = 0 ; counter < GetProbeCount(); counter++)
                {
                    val += 0.1;
                    result.Add(val);
                    //result[counter] = val;
                }
                Called++;
                var dateTime = DateTime.Now;
                Console.WriteLine("Reading {0}.{1} {2}", dateTime.ToLongTimeString(), dateTime.Millisecond, result[0]);
                //Thread.Sleep((int) (m_Rand.NextDouble() * 500));
                return result;
            }
        }

        public int GetProbeCount()
        {
            return 32;
        }

        public string GetProbeName(int probeIndex)
        {
            return string.Format("Probe{0}", probeIndex + 1);
        }

        public ChannelType GetProbeType(int probeIndex)
        {
            return ChannelType.Temperature;
        }

        public string SerialNumber
        {
            get 
            {
                return "111";    
            }
        }
    }
    [TestFixture]
    public class Test1
    {

        System.ComponentModel.IContainer Container
        {
            get
            {
                return null;
            }
        }

        System.Drawing.Icon Icon
        {
            get
            {
                Icon icon = new Icon("tqc.ico");
                return icon;
            }
        }


        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(30)]
        public void TestRunAtSampleRateOf1PerSecond(int numberOfReadings)
        {
            using (var linkToIdealFinish = new LinkToIdealFinish(Container, Icon))
            {
                var model = new TestOvenModel();
                DateTime dateTime = DateTime.Now.AddSeconds(numberOfReadings-0.5);
                linkToIdealFinish.StartDataRun(model, 1.0, new DateTimeProvider());
                while (DateTime.Now < dateTime)
                {
                    Thread.Sleep(100);
                }
                linkToIdealFinish.TerminateWithWait();
                Assert.That(model.Called, Is.EqualTo(numberOfReadings));
            }

        }
        [Test]
        public void TestRunOfSamplesWithTermination()
        {
            using (var linkToIdealFinish = new LinkToIdealFinish(Container, Icon))
            {
                var model = new TestOvenModel();
                DateTime dateTime = DateTime.Now.AddSeconds(10);
                linkToIdealFinish.StartDataRun(model, 1.0, new DateTimeProvider());
                Thread.Sleep(100);
                linkToIdealFinish.TerminateWithWait();
                Assert.That(model.Called, Is.EqualTo(1));
            }

        }

        [Test]
        public void TestRunOf100SamplesAtSampleRateOfPoint1Second()
        {
            using (var linkToIdealFinish = new LinkToIdealFinish(Container, Icon))
            {
                var model = new TestOvenModel();
                DateTime dateTime = DateTime.Now.AddSeconds(10);
                linkToIdealFinish.StartDataRun(model, 0.1, new DateTimeProvider());
                while (DateTime.Now < dateTime)
                {
                    Thread.Sleep(100);
                }
                linkToIdealFinish.TerminateWithWait();
                Assert.That(model.Called, Is.EqualTo(101));
            }

        }

    }
}
