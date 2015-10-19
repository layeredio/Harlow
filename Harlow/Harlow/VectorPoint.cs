using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harlow
{
    public class VectorPoint : VectorFeature
    {
        public VectorPoint(int numOfPoints, ShapeType shapeType) :
            base (numOfPoints, shapeType)
        {
            this.Coordinates = new double[numOfPoints];
            this.Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// All of the points that make up the vector feature.
        /// Points don't have segments
        /// </summary>
        new public double[] Coordinates { get; set; }
    }
}
