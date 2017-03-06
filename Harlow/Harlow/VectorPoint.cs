using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harlow
{
    public class VectorPoint : VectorFeature
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="numOfPoints"></param>
        public VectorPoint(int numOfPoints) : base (ShapeType.Point)
        {
            this.Coordinates = new double[numOfPoints];
        }

        /// <summary>
        /// All of the points that make up the vector feature.
        /// Points don't have segments
        /// </summary>
        new public double[] Coordinates { get; set; }
    }
}
