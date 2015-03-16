using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Harlow
{
    
    public class VectorFeature
    {
        
        public VectorFeature(int numOfParts, ShapeType shapeType)
        {
            if (shapeType == ShapeType.Point)
            {
                Bbox = new double[2];
            }
            else
            {
                Bbox = new double[4];
            }

            Coordinates = new List<PointD[]>(numOfParts);
            Properties = new Dictionary<string, string>(numOfParts);
        }

        public string Type { get; set; }

        /// <summary>
        /// Bounding box for the feature (X,Y,X,Y)
        /// </summary>
        public double[] Bbox { get; set; }


        /// <summary>
        /// All of the points that make up the vector feature.
        /// First dimention is segment
        /// Second dimention are the points within that segment
        /// </summary>
        public List<PointD[]> Coordinates { get; set; }


        /// <summary>
        /// The textual descriptors of this feature from the dbf file.
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }

    }
}
