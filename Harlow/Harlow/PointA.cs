using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;

namespace Harlow
{
    [JsonArray]
    public class PointA: IEnumerable<double>
    {
        public PointA()
        {
            Value = new List<double> { 0.0, 0.0 };
        }

        public PointA(double xVal, double yVal)
        {
            Value = new List<double>() {xVal, yVal};
        }

        public List<double> Value;

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
