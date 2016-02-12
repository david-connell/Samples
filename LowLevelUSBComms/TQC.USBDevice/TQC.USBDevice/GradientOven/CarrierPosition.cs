using System.Collections.Generic;

namespace TQC.USBDevice.GradientOven
{
    public class CarrierPosition : IEqualityComparer<CarrierPosition> 
    {
        public CarrierPosition(byte positionInMilliMeters)
        {
            PositionInMilliMeters = positionInMilliMeters;
        }
        public byte PositionInMilliMeters { get; private set; }

        public bool Equals(CarrierPosition x, CarrierPosition y)
        {
            return x.PositionInMilliMeters.Equals(y.PositionInMilliMeters);
        }

        public int GetHashCode(CarrierPosition obj)
        {
            return obj.PositionInMilliMeters.GetHashCode();
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }
        public override string ToString()
        {
            return string.Format("{0}mm", PositionInMilliMeters);
        }

        public override bool Equals(object obj)
        {
            CarrierPosition value = obj as CarrierPosition;
            if (value == null)
                return false;
            return Equals(this, value);
        }

        public static bool operator ==(CarrierPosition a, CarrierPosition b)
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

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(CarrierPosition a, CarrierPosition b)
        {
            return !(a == b);
        }
    }
}