using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harlow
{
    /// <summary>
    /// The types of Shapefiles we can handle.  Integer
    /// values track with the values in the .shp file.
    /// The constant names track with what the GeoJson
    /// spec expects.
    /// </summary>
    public enum ShapeType
    {
        Null = 0,
        Point = 1,
        MultiLineString = 3,
        Polygon = 5,
        Multipoint = 8
    };
}
