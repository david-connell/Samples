using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TQC.USBDevice
{
    [Serializable]
    public class DeviceResponseUnexpectedException : Exception
    {
        public USBLogger.USBCommandResponseCode USBCommandResponseCode { get; private set; }
        public DeviceResponseUnexpectedException(USBLogger.USBCommandResponseCode code) : base(string.Format("Logger unexpected returned back {0}", code.ToString()) )
        { 
            USBCommandResponseCode = code;            
        }
        public DeviceResponseUnexpectedException(string message) : base(message) { }
        public DeviceResponseUnexpectedException(string message, Exception inner) : base(message, inner) { }
        protected DeviceResponseUnexpectedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    public class DeviceBusyException : DeviceResponseUnexpectedException
    {
        public DeviceBusyException()
            : base(USBLogger.USBCommandResponseCode.DeviceBusy)
        {
        }
    }

    [Serializable]
    public class DeviceUnknownErrorException : DeviceResponseUnexpectedException
    {
        public DeviceUnknownErrorException()
            : base(USBLogger.USBCommandResponseCode.UnknownError)
        {
        }
    }

    [Serializable]
    public class CommandCorruptException : DeviceResponseUnexpectedException
    {
        public CommandCorruptException()
            : base(USBLogger.USBCommandResponseCode.CommandCorrupt)
        {
        }
    }


    [Serializable]
    public class CommandOutOfSequenceException : DeviceResponseUnexpectedException
    {
        public CommandOutOfSequenceException()
            : base(USBLogger.USBCommandResponseCode.CommandOutOfSequence)
        {
        }
    }

    [Serializable]
    public class CommandUnexpectedException : DeviceResponseUnexpectedException
    {
        public CommandUnexpectedException()
            : base(USBLogger.USBCommandResponseCode.CommandUnexpected)
        {
        }
    }

    [Serializable]
    public class CommandNotSuportedException : DeviceResponseUnexpectedException
    {
        public CommandNotSuportedException()
            : base(USBLogger.USBCommandResponseCode.CommandNotSuported)
        {
        }
    }

    [Serializable]
    public class EnumerationNotSuportedException : DeviceResponseUnexpectedException
    {
        public EnumerationNotSuportedException()
            : base(USBLogger.USBCommandResponseCode.EnumerationNotSuported)
        {
        }
    }

    [Serializable]
    public class BatchNotAvailableException : DeviceResponseUnexpectedException
    {
        public BatchNotAvailableException()
            : base(USBLogger.USBCommandResponseCode.BatchNotAvailable)
        {
        }
    }
    
    [Serializable]
    public class DataOutOfRangeException : DeviceResponseUnexpectedException
    {
        public DataOutOfRangeException()
            : base(USBLogger.USBCommandResponseCode.DataOutOfRange)
        {
        }
    }
    
    [Serializable]
    public class CommandModeNotSupportedException : DeviceResponseUnexpectedException
    {
        public CommandModeNotSupportedException()
            : base(USBLogger.USBCommandResponseCode.CommandModeNotSupported)
        {
        }
    }
    
    

    

}
