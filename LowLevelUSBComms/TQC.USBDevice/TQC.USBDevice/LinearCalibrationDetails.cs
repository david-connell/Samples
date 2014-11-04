namespace TQC.USBDevice
{
    public class LinearCalibrationDetails
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
    }
}