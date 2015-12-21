using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TQC.USBDevice.BaseLoggerTypes
{
    public class LoggerPercentage : IEqualityComparer<LoggerPercentage>
    {
        public LoggerPercentage(UInt16 loggerData) : this((loggerData / 10.0f))
        {
        }
        public LoggerPercentage(float percentage)
        {
            if (percentage > 100)
            {
                throw new ArgumentOutOfRangeException("percentage", "Max range is 0->100");
            }
            Value = percentage;
        }
        public float Value { get; private set; }

        public bool Equals(LoggerPercentage x, LoggerPercentage y)
        {
            return x.Value.Equals(y.Value);
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            LoggerPercentage other = obj as LoggerPercentage;
            if (other == null)
            {
                return false;
            }
            return Equals(this, other);
        }
        public override string ToString()
        {
            return string.Format("{0}%", Value);
        }

        public int GetHashCode(LoggerPercentage obj)
        {
            return obj.Value.GetHashCode();
        }

        internal UInt16 ToData
        {
            get
            {
                return (UInt16)((Value * 10.0) + 0.5);
            }
        }
    }
}
