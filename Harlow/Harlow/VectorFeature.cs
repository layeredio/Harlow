using System;
using System.Collections.Generic;


namespace Harlow
{
    
    public class VectorFeature
    {
        
        public VectorFeature(int numOfParts)
        {
            Coordinates = new List<PointD[]>(numOfParts);
            Bbox = new double[4];
            Key = Guid.NewGuid();
        }

        public Guid Key { get; set; }
        
        public ShapeType Type { get; set; }

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
