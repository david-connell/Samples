using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TQC.USBDevice;

namespace IntegrationTestNUnit.GRO
{
    [TestFixture(USBLogger.USBProductId.GRADIENT_OVEN, 1)]
    class GROThermocoupleBoardsReadingTests
    {

        USBLogger.USBProductId ProductId { get; set; }
        byte ThermocoupleBoard { get; set; }

        public GROThermocoupleBoardsReadingTests(USBLogger.USBProductId product, byte thermocoupleBoard)
        {
            ProductId = product;
            ThermocoupleBoard = thermocoupleBoard;
        }
        [Test]
        public void ReadChannelValues()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var probeValues = thermocoupleBoard.Board.ProbeValues;
                int id = 1;
                foreach (var value in probeValues)
                {
                    Assert.IsTrue(value > 0);
                    Console.WriteLine("Channel {0} = {1}", id++, value);
                }
                Assert.IsTrue(probeValues.Count() == 8);
                Console.WriteLine(probeValues.Count());

            }
        }



        [Test]
        public void ReadColdJunctionValues()
        {
            using (var thermocoupleBoard = new ThermocoupleBoard(ProductId, ThermocoupleBoard))
            {
                var probeValues = thermocoupleBoard.Board.ColdJunctions;
                int id = 1;
                foreach (var value in probeValues)
                {
                    Assert.IsTrue(value > 0);
                    Console.WriteLine("CJ {0} = {1}", id++, value);
                }
                Assert.IsTrue(probeValues.Count() == 8);
                Console.WriteLine(probeValues.Count());
            }
        }

    }
}
