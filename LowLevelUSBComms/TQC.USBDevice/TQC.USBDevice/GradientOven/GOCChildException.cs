using System;
using System.Runtime.Serialization;

namespace TQC.USBDevice.GradientOven
{
    [Serializable]
    public class GocChildException : Exception
    {
        public GocChildException() { }
        public GocChildException(string message) : base(message) { }
        public GocChildException(string message, Exception inner) : base(message, inner) { }
        protected GocChildException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}
