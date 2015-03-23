using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Harlow;

namespace HarlowUnitTests
{
    [TestClass]
    public class PolygonTest
    {
        string polyFile = String.Format("..{0}..{0}Shapefiles{0}Polygon{0}tl_2014_06_place.shp", Path.DirectorySeparatorChar);

        [TestMethod]
        public void Tiger_US_PlaceBoundaries()
        {
            ShapeFileReader reader = new ShapeFileReader(polyFile);
            reader.LoadFile();

            string json = reader.FeaturesAsJson();
            File.WriteAllText("polygon.json", reader.FeatureAsJson(42));

            Assert.AreEqual(25301992, json.Length);
            Assert.AreEqual(1516, reader.Features.Length);

            Assert.AreEqual(4780, reader.FeatureAsJson(0).Length);
            Assert.AreEqual(13137, reader.FeatureAsJson(1515).Length);

            VectorShape[] features = reader.Features as VectorShape[];

            Assert.AreEqual(-118.456008, features[0].Coordinates[0][0].Value[0]);
            Assert.AreEqual(34.284903, features[0].Coordinates[0][0].Value[1]);
            Assert.AreEqual(-118.30807, features[42].Coordinates[0][0].Value[0]);
            Assert.AreEqual(34.161224, features[42].Coordinates[0][0].Value[1]);
            Assert.AreEqual(-122.060783, features[1515].Coordinates[0][0].Value[0]);
            Assert.AreEqual(37.05574, features[1515].Coordinates[0][0].Value[1]);

            Assert.AreEqual(16, reader.Features[0].Properties.Count);

            Assert.AreEqual("06", reader.Features[42].Properties["statefp"]);
            Assert.AreEqual("30000", reader.Features[42].Properties["placefp"]);
            Assert.AreEqual("02410597", reader.Features[42].Properties["placens"]);
            Assert.AreEqual("0630000", reader.Features[42].Properties["geoid"]);
            Assert.AreEqual("glendale", reader.Features[42].Properties["name"]);
            Assert.AreEqual("glendale city", reader.Features[42].Properties["namelsad"]);
            Assert.AreEqual("25", reader.Features[42].Properties["lsad"]);
            Assert.AreEqual("c1", reader.Features[42].Properties["classfp"]);
            Assert.AreEqual("y", reader.Features[42].Properties["pcicbsa"]);
            Assert.AreEqual("n", reader.Features[42].Properties["pcinecta"]);
            Assert.AreEqual("g4110", reader.Features[42].Properties["mtfcc"]);
            Assert.AreEqual("a", reader.Features[42].Properties["funcstat"]);
            Assert.AreEqual("78848571", reader.Features[42].Properties["aland"]);
            Assert.AreEqual("337673", reader.Features[42].Properties["awater"]);
            Assert.AreEqual("+34.1813929", reader.Features[42].Properties["intptlat"]);
            Assert.AreEqual("-118.2458301", reader.Features[42].Properties["intptlon"]);
        }
    }
}
