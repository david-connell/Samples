using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice.GradientOven;

namespace IntegrationTestNUnit.CodeInfrastructure
{
    class CarrierPositionTests
    {
        [Test]
        public void Equality()
        {
            CarrierPosition val1 = new CarrierPosition(120);
            CarrierPosition val2 = new CarrierPosition(120);

            //Assert.That(val1, Is.EqualTo(val2));
            Assert.That(val1 == val2, Is.True);
        }
        [Test]
        public void NotEquality()
        {
            CarrierPosition val1 = new CarrierPosition(120);
            CarrierPosition val2 = new CarrierPosition(121);

            Assert.That(val1, Is.Not.EqualTo(val2));
            Assert.That(val1 != val2, Is.True);
        }
    }
}
