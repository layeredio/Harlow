using System;
using System.Collections.Generic;


namespace Harlow
{
    
    public class VectorFeature
    {
        
        public VectorFeature(int numOfParts)
        {
            Geometry = new VectorCoordinate(numOfParts);
            Properties = new Dictionary<string, string>(numOfParts);
            Bbox = new double[2, 2];
        }


        public string Type { get { return "feature"; } }

        /// <summary>
        /// Bounding box for the feature (X,Y,X,Y)
        /// </summary>
        public double[,] Bbox { get; set; }


        /// <summary>
        /// All of the points that make up the vector feature.
        /// First dimention is segment
        /// Second dimention are the points within that segment
        /// </summary>
        public VectorCoordinate Geometry { get; set; }


        /// <summary>
        /// The textual descriptors of this feature from the dbf file.
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }

    }
}
