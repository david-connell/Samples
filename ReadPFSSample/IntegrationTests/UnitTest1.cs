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
        String m_fileName2 = @"..\..\..\TestData\51200 @ 20120416 #18.18.10.pfs";

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
                
                Assert.IsTrue(file.TempUnits == TempUnits.Farenheight);
                Assert.IsTrue(file.Notes == "Some notes here");
                Assert.AreEqual(file.SampleRate, 30.0f);
                Assert.AreEqual(file.NumberOfReadings, 6046);
                Assert.AreEqual(file.DownloadDate, new DateTime(2006,08,30,10,59,41));
            }
        }

        
        [TestMethod]
        public void ValidateProbe1()
        {
            using (var file = new PFSFile(m_fileName))
            {

                var probe = file.getProbe(0);

                Assert.IsTrue(probe.Name == "T.air°");
                Assert.IsTrue(probe.AirProbe);
                Assert.IsTrue(probe.Enabled);
                Assert.IsTrue(probe.Color.Name == "0");

                var data = probe.Data;
                Assert.AreEqual<PFSReading>(data[0].ToReading(), 21.7.ToReading());
                Assert.AreEqual<PFSReading>(data[1].ToReading(), 21.4.ToReading());

            }
        }

        [TestMethod]
        public void ValidateProbe4()
        {
            using (var file = new PFSFile(m_fileName))
            {

                var probe = file.getProbe(4);

                var data = probe.Data;
                Assert.AreEqual<PFSReading>(data[0].ToReading(), 4.9.ToReading());
                Assert.AreEqual<PFSReading>(data[1].ToReading(), 3.7.ToReading());

            }
        }

        [TestMethod]
        public void ValidateFile2()
        {
            using (var file = new PFSFile(m_fileName2))
            {

                var probe = file.getProbe(0);

                var data = probe.Data;
                Assert.AreEqual<PFSReading>(data[0].ToReading(), 17.7.ToReading());
                Assert.AreEqual<PFSReading>(data[1].ToReading(), 17.2.ToReading());

                probe = file.getProbe(1);

                data = probe.Data;
                Assert.AreEqual<PFSReading>(new PFSReading (State.OpenCircuit ) , data[0].ToReading());
                

            }
        }

        [TestMethod]
        public void ValidateFile3()
        {
            using (var file = new PFSFile(m_fileName2))
            {                
                var data = file.AsynchnousData;
                Assert.AreEqual(data.Samples[0].Readings[0], 17.7.ToReading());
                Assert.AreEqual(data.Samples[10].Readings[0], 16.8.ToReading());                
            }
        }
    }
}
