using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice;

namespace IntegrationTestNUnit
{
    [TestFixture(USBLogger.USBProductId.GRADIENT_OVEN, 1)]
    class GROThermocoupleBoardTests
    {
        USBLogger.USBProductId ProductId { get; set; }
        byte ThermocoupleBoard { get; set; }

        public GROThermocoupleBoardTests(USBLogger.USBProductId product, byte thermocoupleBoard)
        {
            ProductId = product;
            ThermocoupleBoard = thermocoupleBoard;
        }

        
        public void ReadProbes()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var probeValues = thermocoupleBoard.Board.ProbeValues;
                Assert.IsTrue(probeValues.Count() == 8);
                foreach (var value in probeValues)
                {
                    Assert.IsTrue(value > 0);
                }
            }
        }

        
        public void ReadNumberOfProbes()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var numberOfProbes = thermocoupleBoard.Board.NumberOfProbes;
                Assert.That(numberOfProbes, Is.EqualTo(8));
            }
        }


        [TestCase(0, USBLogger.ProbeType.Temperature)]
        [TestCase(1, USBLogger.ProbeType.Temperature)]
        [TestCase(2, USBLogger.ProbeType.Temperature)]
        [TestCase(3, USBLogger.ProbeType.Temperature)]
        [TestCase(4, USBLogger.ProbeType.Temperature)]
        [TestCase(5, USBLogger.ProbeType.Temperature)]
        [TestCase(6, USBLogger.ProbeType.Temperature)]
        [TestCase(7, USBLogger.ProbeType.Temperature)]
        public void ReadProbeType(byte probeId, USBLogger.ProbeType probeType)
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var type = thermocoupleBoard.Board.ProbeType(probeId);
                Assert.That(type, Is.EqualTo(probeType));
            }
        }

    }
}
