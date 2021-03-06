﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationTestNUnit.Logger.GeneralLogger;
using NUnit.Framework;
using TQC.USBDevice;

namespace IntegrationTestNUnit.Logger.USBProduct2
{
    [TestFixture,Ignore]
    class Basic : BasicCommands
    {
        public Basic()
            : base(USBLogger.USBProductId.USB_PRODUCT2)
        {
        }
    }

    [TestFixture, Ignore]
    class Calibration : CalibrationCommands
    {
        public Calibration()
            : base(USBLogger.USBProductId.USB_PRODUCT2)
        {
        }
    }


    [TestFixture, Ignore]
    class ReadValues : ReadValuesCommands
    {
        public ReadValues()
            : base(USBLogger.USBProductId.USB_PRODUCT2)
        {
        }
    }
    
}
