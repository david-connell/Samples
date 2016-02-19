using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TQC.USBDevice
{
    [Serializable]
    public class DeviceResponseException : Exception
    {
        public string RequestDescription { get; set; }
        public DeviceResponseException()
            : base(string.Format("Logger Response not valid"))
        {
            
        }
        public DeviceResponseException(string message) : base(message) { }
        public DeviceResponseException(string requestDescription, string message) : base(string.Format("{0}.{1}", requestDescription, message) )
        {
            RequestDescription = requestDescription;
        }
        public DeviceResponseException(string message, Exception inner) : base(message, inner) { }
        protected DeviceResponseException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    [Serializable]
    public class NoDataReceivedException : DeviceResponseException
    {
        public NoDataReceivedException(string requestDescription)
            : base(requestDescription, string.Format("Reponse from logger was empty."))
        {
            
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    [Serializable]
    public class TooLittleDataReceivedException : DeviceResponseException
    {
        public int Length { get; set; }
        public int Expected {get; set;}
        public TooLittleDataReceivedException(string requestDescription, int length, int expected)
            : base(requestDescription, string.Format("Buffer recieved was too short. Expected {0} bytes but recieved {1}.", expected, length))
        {
            Length = length;
            Expected = expected;
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
    

    [Serializable]
    public class DeviceResponseUnexpectedException : DeviceResponseException
    {
        public USBLogger.USBCommandResponseCode USBCommandResponseCode { get; private set; }
        public DeviceResponseUnexpectedException(USBLogger.USBCommandResponseCode code) : base(string.Format("Logger unexpected returned back {0}", code.ToString()) )
        { 
            USBCommandResponseCode = code;            
        }
        public DeviceResponseUnexpectedException(string message) : base(message) { }
        public DeviceResponseUnexpectedException(string message, Exception inner) : base(message, inner) { }
        protected DeviceResponseUnexpectedException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
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
    

    

    [Serializable]
    public class ResponsePacketErrorCRCException : DeviceResponseUnexpectedException
    {
        public ResponsePacketErrorCRCException()
            : base(USBLogger.USBCommandResponseCode.ResponsePacketErrorCRC)
        {
        }
    }

    

    [Serializable]
    public class ResponsePacketErrorBadLengthException : DeviceResponseUnexpectedException
    {
        public ResponsePacketErrorBadLengthException()
            : base(USBLogger.USBCommandResponseCode.ResponsePacketErrorBadLength)
        {
        }
    }

    
    [Serializable]
    public class ResponsePacketErrorBadCommandException : DeviceResponseUnexpectedException
    {
        public ResponsePacketErrorBadCommandException()
            : base(USBLogger.USBCommandResponseCode.ResponsePacketErrorBadCommand)
        {
        }
    }

    
    [Serializable]
    public class ResponsePacketErrorTimeoutException : DeviceResponseUnexpectedException
    {
        public ResponsePacketErrorTimeoutException ()
            : base(USBLogger.USBCommandResponseCode.ResponsePacketErrorTimeout)
        {
        }
        protected ResponsePacketErrorTimeoutException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class UsbDisconnectedException : DeviceResponseUnexpectedException
    {
        public UsbDisconnectedException() : base(USBLogger.USBCommandResponseCode.ResponseUsbDeviceRemoved) { }
        protected UsbDisconnectedException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
    
    

    

}
