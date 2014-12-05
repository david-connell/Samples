using System;
using System.Collections.Generic;

namespace TQC.USBDevice.GradientOven
{
    
    public class Percentage : IEqualityComparer<Percentage>
    {
        public Percentage(byte percentage)
        {
            if (percentage > 100)
            {
                throw new ArgumentOutOfRangeException("percentage", "Max range is 0->100");
            }
            Value = percentage;
        }
        public byte Value { get; private set; }

        public bool Equals(Percentage x, Percentage y)
        {
            return x.Value.Equals(y.Value);
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Percentage other = obj as Percentage;
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

        public int GetHashCode(Percentage obj)
        {
            return obj.Value.GetHashCode();
        }
    }
}