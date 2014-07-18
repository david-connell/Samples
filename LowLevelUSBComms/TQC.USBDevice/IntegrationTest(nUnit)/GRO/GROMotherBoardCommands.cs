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


        [Test]
        public void ReadInternalStatus()
        {
            using (var logger = new GROMainBoard())
            {
                if (logger.Open(ProductId))
                {
                    var channels = logger.InternalChannels();
                    int id = 1;
                    foreach (var channel in channels)
                    {
                        Console.WriteLine("Channel {0} = {1}", id++, channel);
                    }                    
                }
                else
                {
                    throw new Exception("Failed to connect to logger " + ProductId.ToString());
                }
            }
        }


    }
}
