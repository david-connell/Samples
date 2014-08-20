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
            return PositionInMilliMeters.GetHashCode();
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
    }
}