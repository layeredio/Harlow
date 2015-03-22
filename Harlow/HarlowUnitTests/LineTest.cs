using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Harlow;

namespace HarlowUnitTests
{
    [TestClass]
    public class LineTest
    {
        string lineFile = @"..\..\Shapefiles\Line\tl_2014_06075_roads.shp";

        [TestMethod]
        public void Tiger_CA_SanFran_Roads()
        {
            ShapeFileReader reader = new ShapeFileReader(lineFile);
            reader.LoadFile();

            string json = reader.FeaturesAsJson();
            File.WriteAllText("line.json", reader.FeatureAsJson(42));

            Assert.AreEqual(json.Length, 2166881);
            Assert.AreEqual(reader.Features.Length, 4589);

            Assert.AreEqual(reader.FeatureAsJson(0).Length, 283);
            Assert.AreEqual(reader.FeatureAsJson(4588).Length, 649);

            Assert.AreEqual(reader.Features[0].Coordinates[0][0].Value[0], -122.480706);
            Assert.AreEqual(reader.Features[0].Coordinates[0][0].Value[1], 37.792316);
            Assert.AreEqual(reader.Features[42].Coordinates[0][0].Value[0], -122.469023);
            Assert.AreEqual(reader.Features[42].Coordinates[0][0].Value[1], 37.737949);
            Assert.AreEqual(reader.Features[4588].Coordinates[0][0].Value[0], -122.364304);
            Assert.AreEqual(reader.Features[4588].Coordinates[0][0].Value[1], 37.819468);

            Assert.AreEqual(reader.Features[0].Properties.Count, 4);

            Assert.AreEqual(reader.Features[0].Properties["linearid"], "110498938555");
            Assert.AreEqual(reader.Features[0].Properties["fullname"], "n van horn ln");
            Assert.AreEqual(reader.Features[0].Properties["rttyp"], "m");
            Assert.AreEqual(reader.Features[0].Properties["mtfcc"], "s1400");

            Assert.AreEqual(reader.Features[42].Properties["linearid"], "110498935016");
            Assert.AreEqual(reader.Features[42].Properties["fullname"], "w portal ave");
            Assert.AreEqual(reader.Features[42].Properties["rttyp"], "m");
            Assert.AreEqual(reader.Features[42].Properties["mtfcc"], "s1400");

            Assert.AreEqual(reader.Features[4588].Properties["linearid"], "110498933806");
            Assert.AreEqual(reader.Features[4588].Properties["fullname"], "avenue n");
            Assert.AreEqual(reader.Features[4588].Properties["rttyp"], "m");
            Assert.AreEqual(reader.Features[4588].Properties["mtfcc"], "s1400");

        }
    }
}
