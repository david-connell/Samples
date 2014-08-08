using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice.GradientOven;

namespace IntegrationTestNUnit.CodeInfrastructure
{
    class SpeedTests
    {
        [Test]
        public void Equality()
        {
            Speed val1 = new Speed(1);
            Speed val2 = new Speed(1);

            Assert.That(val1, Is.EqualTo(val2));
        }
    }
}
