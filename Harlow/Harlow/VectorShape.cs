using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harlow
{
    public class VectorShape : VectorFeature
    { 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="numOfParts"></param>
		/// <param name="shapeType"></param>
        public VectorShape(int numOfParts, ShapeType shapeType) : base (shapeType)
        {
            this.Coordinates = new List<PointA[]>(numOfParts);
        }

        /// <summary>
        /// All of the points that make up the vector feature.
        /// First dimention is segment
        /// Second dimention are the points within that segment
        /// </summary>
        new public List<PointA[]> Coordinates { get; set; }
    }
}
