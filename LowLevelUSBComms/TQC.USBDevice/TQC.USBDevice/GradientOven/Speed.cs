using System.Collections.Generic;

namespace TQC.USBDevice.GradientOven
{
    public class Speed : IEqualityComparer<Speed>
    {
        public Speed(byte speed)
        {
            SpeedMillimetersPerSecond = speed;
        }
        public byte SpeedMillimetersPerSecond { get; private set; }

        public bool Equals(Speed x, Speed y)
        {
            return x.SpeedMillimetersPerSecond.Equals(y.SpeedMillimetersPerSecond);
        }
        public override int GetHashCode()
        {
            return SpeedMillimetersPerSecond.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Speed other = obj as Speed;
            if (other == null)
            {
                return false;
            }
            return Equals(this, other);
        }
        public override string ToString()
        {
            return string.Format("{0}mm/sec", SpeedMillimetersPerSecond);
        }

        public int GetHashCode(Speed obj)
        {
            return obj.SpeedMillimetersPerSecond.GetHashCode();
        }
    }
}