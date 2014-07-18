using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [TestCase(1)]
        public void ReadProbes(byte thermocoupleBoardId)
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, thermocoupleBoardId))
            {                    
                var probeValues = thermocoupleBoard.Board.ProbeValues;
                Assert.IsTrue(probeValues.Count() == 8);
                foreach (var value in probeValues)
                {
                    Assert.IsTrue(value > 0);
                }
            }
        }

        [TestCase(1)]
        public void ReadNumberOfProbes(byte thermocoupleBoardId)
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, thermocoupleBoardId))
            {
                var numberOfProbes = thermocoupleBoard.Board.NumberOfProbes;
                Assert.That(numberOfProbes, Is.EqualTo(8));
            }
        }


        [TestCase(1, 0, USBLogger.ProbeType.Temperature)]
        [TestCase(1, 1, USBLogger.ProbeType.Temperature)]
        [TestCase(1, 2, USBLogger.ProbeType.Temperature)]
        [TestCase(1, 3, USBLogger.ProbeType.Temperature)]
        [TestCase(1, 4, USBLogger.ProbeType.Temperature)]
        [TestCase(1, 5, USBLogger.ProbeType.Temperature)]
        [TestCase(1, 6, USBLogger.ProbeType.Temperature)]
        [TestCase(1, 7, USBLogger.ProbeType.Temperature)]
        public void ReadProbeType(byte thermocoupleBoardId, byte probeId, USBLogger.ProbeType probeType)
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, thermocoupleBoardId))
            {
                var type = thermocoupleBoard.Board.ProbeType(probeId);
                Assert.That(type, Is.EqualTo(probeType));
            }
        }
    }
}
