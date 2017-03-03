using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Harlow
{
    
    public abstract class VectorFeature
    {
		/// <summary>
		/// Defailt constructor for the VectorFeature abstract class
		/// </summary>
		/// <param name="shapeType"></param>
        public VectorFeature(ShapeType shapeType)
        {
            if (shapeType != ShapeType.Point)
            {
                Bbox = new double[4];
            }

            Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// GeoJson feature type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Bounding box for the feature (X,Y,X,Y)
        /// </summary>
        public double[] Bbox { get; set; }

        /// <summary>
        /// Just a placeholder
        /// </summary>
        public virtual object Coordinates { get; set; }

        /// <summary>
        /// The textual descriptors of this feature from the dbf file.
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }

    }
}
