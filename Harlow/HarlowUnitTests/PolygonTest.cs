using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Harlow;

namespace HarlowUnitTests
{
    [TestClass]
    public class PolygonTest
    {
        string polyFile = @"..\..\Shapefiles\Polygon\tl_2014_06_place.shp";

        [TestMethod]
        public void Tiger_US_PlaceBoundaries()
        {
            ShapeFileReader reader = new ShapeFileReader(polyFile);
            reader.LoadFile();

            string json = reader.FeaturesAsJson();
            File.WriteAllText("polygon.json", json);

            Assert.AreEqual(json.Length, 25309572);
            Assert.AreEqual(reader.Features.Length, 1516);

            Assert.AreEqual(reader.FeatureAsJson(0).Length, 4785);
            Assert.AreEqual(reader.FeatureAsJson(1515).Length, 13142);

            Assert.AreEqual(reader.Features[0].Coordinates[0][0].Value[0], -118.456008);
            Assert.AreEqual(reader.Features[0].Coordinates[0][0].Value[1], 34.284903);
            Assert.AreEqual(reader.Features[42].Coordinates[0][0].Value[0], -118.30807);
            Assert.AreEqual(reader.Features[42].Coordinates[0][0].Value[1], 34.161224);
            Assert.AreEqual(reader.Features[1515].Coordinates[0][0].Value[0], -122.060783);
            Assert.AreEqual(reader.Features[1515].Coordinates[0][0].Value[1], 37.05574);

            Assert.AreEqual(reader.Features[0].Properties.Count, 16);

            Assert.AreEqual(reader.Features[42].Properties["statefp"], "06");
            Assert.AreEqual(reader.Features[42].Properties["placefp"], "30000");
            Assert.AreEqual(reader.Features[42].Properties["placens"], "02410597");
            Assert.AreEqual(reader.Features[42].Properties["geoid"], "0630000");
            Assert.AreEqual(reader.Features[42].Properties["name"], "glendale");
            Assert.AreEqual(reader.Features[42].Properties["namelsad"], "glendale city");
            Assert.AreEqual(reader.Features[42].Properties["lsad"], "25");
            Assert.AreEqual(reader.Features[42].Properties["classfp"], "c1");
            Assert.AreEqual(reader.Features[42].Properties["pcicbsa"], "y");
            Assert.AreEqual(reader.Features[42].Properties["pcinecta"], "n");
            Assert.AreEqual(reader.Features[42].Properties["mtfcc"], "g4110");
            Assert.AreEqual(reader.Features[42].Properties["funcstat"], "a");
            Assert.AreEqual(reader.Features[42].Properties["aland"], "78848571");
            Assert.AreEqual(reader.Features[42].Properties["awater"], "337673");
            Assert.AreEqual(reader.Features[42].Properties["intptlat"], "+34.1813929");
            Assert.AreEqual(reader.Features[42].Properties["intptlon"], "-118.2458301");
        }
    }
}
