using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TQC.IdealFinish.PFSWrapper;

namespace IntegrationTests
{
    [TestClass]
    public class UnitTest1
    {
        String m_fileName = @"..\..\..\TestData\climate2.pfs";

        [TestMethod]
        public void ReadValidFile()
        {
            using (var file = new PFSFile(m_fileName))
            {
                Assert.AreEqual(file.HasReadOK, true);
            }
        }
        [TestMethod]
        public void ReadInvalidValidFile()
        {
            using (var file = new PFSFile(m_fileName + ".pfs"))
            {
                Assert.AreEqual(file.HasReadOK, false);
            }
        }

        [TestMethod]
        public void ValidateFile()
        {
            using (var file = new PFSFile(m_fileName))
            {
                Assert.IsTrue(file.Probes == 5);
                Assert.IsTrue(file.Readings == 6046);
                Assert.IsTrue(file.OperatorName ==@"PCRAOUL\Raoul" );
                Assert.IsTrue(file.StartDate > DateTime.MinValue);
                Assert.IsTrue(file.DownloadDate > DateTime.MinValue);
                Assert.IsTrue(file.TempUnits == TempUnits.Farenheight);
                Assert.IsTrue(file.Notes == "Some notes here");
            }
        }

        public class Reading : IEquatable<Reading>
        {
            public double Value { get; private set; }
            public Reading(double value)
            {
                Value = value;
            }
            public override bool Equals(object obj)
            {
                var other = obj as Reading;
                if (other != null)
                {
                    return Equals(other);
                }
                return false;
            }
            public bool Equals(Reading other)
            {
                return (Value <= (other.Value + 0.1)) && (Value >= (other.Value - 0.1));
            }
        }
        [TestMethod]
        public void ValidateProbe()
        {
            using (var file = new PFSFile(m_fileName))
            {

                var probe = file.getProbe(0);

                Assert.IsTrue(probe.Name == "T.air°");
                Assert.IsTrue(probe.AirProbe);
                Assert.IsTrue(probe.Enabled);
                Assert.IsTrue(probe.Color.Name == "0");

                var data = probe.Data;
                Assert.AreEqual<Reading>(new Reading(data[0]), new Reading(21.7));
                Assert.AreEqual<Reading>(new Reading(data[1]), new Reading(21.4));

            }
        }
    }
}
