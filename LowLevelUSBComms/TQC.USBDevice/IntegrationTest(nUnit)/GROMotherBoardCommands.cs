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

    }
}
