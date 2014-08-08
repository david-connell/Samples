using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice.GradientOven;

namespace IntegrationTestNUnit.CodeInfrastructure
{
    class PercentageTests
    {
        [Test]
        public void Equality()
        {
            Percentage val1 = new Percentage(1);
            Percentage val2 = new Percentage(1);

            Assert.That(val1, Is.EqualTo(val2));
        }
    }
}
