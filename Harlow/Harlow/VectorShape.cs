using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harlow
{
    public class VectorShape : VectorFeature
    {
        public VectorShape(int numOfParts, ShapeType shapeType) :
            base (numOfParts, shapeType)
        {
            if (shapeType != ShapeType.Point)
            {
                Bbox = new double[4];
            }

            this.Coordinates = new List<PointA[]>(numOfParts);
            this.Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// All of the points that make up the vector feature.
        /// First dimention is segment
        /// Second dimention are the points within that segment
        /// </summary>
        new public List<PointA[]> Coordinates { get; set; }
    }
}
