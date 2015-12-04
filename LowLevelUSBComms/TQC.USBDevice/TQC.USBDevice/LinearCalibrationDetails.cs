using System.Collections.Generic;
namespace TQC.USBDevice
{
    public class LinearCalibrationDetails : IEqualityComparer<LinearCalibrationDetails>
    {
        public double M { get; private set; }
        public double C { get; private set; }
        public LinearCalibrationDetails(double m, double c)
        {
            M = m;
            C = c;
        }
        public override string ToString()
        {
            return string.Format("Y={0}X + {1}", M, C);
        }

        public bool Equals(LinearCalibrationDetails x, LinearCalibrationDetails y)
        {
            return x.M.Equals(y.M) && x.C.Equals(y.C);
        }

        public override bool Equals(object obj)
        {
            var other = obj as LinearCalibrationDetails;
            if (other == null)
            {
                return false;
            }
            return Equals(this, other);
        }
        public int GetHashCode(LinearCalibrationDetails obj)
        {
            return obj.M.GetHashCode() + obj.C.GetHashCode();
        }
        public override int GetHashCode()
        {
            return GetHashCode(this);
        }
    }
}