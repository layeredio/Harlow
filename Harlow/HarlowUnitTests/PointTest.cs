using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Harlow;

namespace HarlowUnitTests
{
    [TestClass]
    public class PointTest
    {
        string pointFile = @"..\..\Shapefiles\Point\builtupp_usa.shp";
        
        [TestMethod]
        public void CitiesAndTowns()
        {
            ShapeFileReader reader = new ShapeFileReader(pointFile);
            reader.LoadFile();

            Assert.AreEqual(reader.Features.Length, 38187);
            Assert.AreEqual(reader.Features[42].Coordinates[0][0].X, -101.22071379999994);
            Assert.AreEqual(reader.Features[42].Coordinates[0][0].Y, 48.513074399064209);
        }
    }
}
