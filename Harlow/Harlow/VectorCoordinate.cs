using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harlow
{
    public class VectorCoordinate
    {
        public VectorCoordinate(int numOfParts)
        {
            Coordinates = new List<PointD[]>(numOfParts);
        }

        public string Type { get; set; } 
        public List<PointD[]> Coordinates { get; set; }
    }
}
