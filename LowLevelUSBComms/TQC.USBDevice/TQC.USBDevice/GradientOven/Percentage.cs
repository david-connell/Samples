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

        public static bool operator ==(Percentage a, Percentage b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Percentage a, Percentage b)
        {
            return !(a == b);
        }

    }
}