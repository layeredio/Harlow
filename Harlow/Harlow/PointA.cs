using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;

namespace Harlow
{
    [JsonArray]
    public class PointA: IEnumerable<double>
    {
        public List<double> Value;

        public PointA(double xVal, double yVal)
        {
            Value = new List<double>() {xVal, yVal};
        }

        public PointA() : this(0.0, 0.0) { }
        
        public IEnumerator<double> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Value.GetEnumerator();
        }
    }
}
