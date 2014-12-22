using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TQC.USBDevice.GradientOven
{
    [Serializable]
    public class GocChildException : Exception
    {
        public GocChildException() { }
        public GocChildException(string message) : base(message) { }
        public GocChildException(string message, Exception inner) : base(message, inner) { }
        protected GocChildException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
