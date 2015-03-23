using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Harlow;

namespace HarlowUnitTests
{
    [TestClass]
    public class LineTest
    {
        string lineFile = String.Format("..{0}..{0}Shapefiles{0}Line{0}tl_2014_06075_roads.shp", Path.DirectorySeparatorChar);

        [TestMethod]
        public void Tiger_CA_SanFran_Roads()
        {
            ShapeFileReader reader = new ShapeFileReader(lineFile);
            reader.LoadFile();

            string json = reader.FeaturesAsJson();
            File.WriteAllText("line.json", reader.FeatureAsJson(42));

            Assert.AreEqual(2166881, json.Length);
            Assert.AreEqual(4589, reader.Features.Length);

            Assert.AreEqual(283, reader.FeatureAsJson(0).Length);
            Assert.AreEqual(649, reader.FeatureAsJson(4588).Length);

            Assert.AreEqual(-122.480706, reader.Features[0].Coordinates[0][0].Value[0]);
            Assert.AreEqual(37.792316, reader.Features[0].Coordinates[0][0].Value[1]);
            Assert.AreEqual(-122.469023, reader.Features[42].Coordinates[0][0].Value[0]);
            Assert.AreEqual(37.737949, reader.Features[42].Coordinates[0][0].Value[1]);
            Assert.AreEqual(-122.364304, reader.Features[4588].Coordinates[0][0].Value[0]);
            Assert.AreEqual(37.819468, reader.Features[4588].Coordinates[0][0].Value[1]);

            Assert.AreEqual(4, reader.Features[0].Properties.Count);

            Assert.AreEqual("110498938555", reader.Features[0].Properties["linearid"]);
            Assert.AreEqual("n van horn ln", reader.Features[0].Properties["fullname"]);
            Assert.AreEqual("m", reader.Features[0].Properties["rttyp"]);
            Assert.AreEqual("s1400", reader.Features[0].Properties["mtfcc"]);

            Assert.AreEqual("110498935016", reader.Features[42].Properties["linearid"]);
            Assert.AreEqual("w portal ave", reader.Features[42].Properties["fullname"]);
            Assert.AreEqual("m", reader.Features[42].Properties["rttyp"]);
            Assert.AreEqual("s1400", reader.Features[42].Properties["mtfcc"]);

            Assert.AreEqual("110498933806", reader.Features[4588].Properties["linearid"]);
            Assert.AreEqual("avenue n", reader.Features[4588].Properties["fullname"]);
            Assert.AreEqual("m", reader.Features[4588].Properties["rttyp"]);
            Assert.AreEqual("s1400", reader.Features[4588].Properties["mtfcc"]);

        }
    }
}
