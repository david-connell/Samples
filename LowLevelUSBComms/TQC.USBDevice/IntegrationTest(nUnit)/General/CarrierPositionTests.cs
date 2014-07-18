using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice.GradientOven;

namespace IntegrationTestNUnit.General
{
    class CarrierPositionTests
    {
        [Test]
        public void Equality()
        {
            CarrierPosition val1 = new CarrierPosition(1);
            CarrierPosition val2 = new CarrierPosition(1);

            Assert.That(val1, Is.EqualTo(val2));
        }
    }
}
