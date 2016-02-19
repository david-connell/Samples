using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace TQC.USBDevice.Exceptions
{
    [Serializable]
    public class LoggerCannotBeConfiguredException : Exception
    {
        public LoggerCannotBeConfiguredException() { }
        public LoggerCannotBeConfiguredException(string message) : base(message) { }
        public LoggerCannotBeConfiguredException(string message, Exception inner) : base(message, inner) { }
        protected LoggerCannotBeConfiguredException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
