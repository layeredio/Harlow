using System;

namespace Harlow
{

    public class PointD
    {
        public PointD()
        {

        }

        public PointD(double xVal, double yVal)
        {
            X = xVal;
            Y = yVal;
        }

        public PointD(PointD P)
        {
            X = P.X;
            Y = P.Y;
        }

        public double X { get; set; }
        public double Y { get; set; }

    }
}
