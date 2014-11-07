using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationsWithEctron.Engine.GPIB_Device
{
    public class GPIBException : Exception
    {
        public GPIBException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
