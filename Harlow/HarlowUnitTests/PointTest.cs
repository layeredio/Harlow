using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Harlow;

namespace HarlowUnitTests
{
    [TestClass]
    public class PointTest
    {
        string pointFile = String.Format("..{0}..{0}Shapefiles{0}Point{0}builtupp_usa.shp", Path.DirectorySeparatorChar);
        
        [TestMethod]
        public void USGS_CitiesAndTowns()
        {
            ShapeFileReader reader = new ShapeFileReader(pointFile);
            reader.LoadFile();

            string json = reader.FeaturesAsJson();
            File.WriteAllText("point.json", reader.FeatureAsJson(42));

            Assert.AreEqual(6062745, json.Length);
            Assert.AreEqual(38187, reader.Features.Length);

            Assert.AreEqual(157, reader.FeatureAsJson(0).Length);
            Assert.AreEqual(159, reader.FeatureAsJson(38186).Length);

            VectorPoint[] features = reader.Features as VectorPoint[];

            Assert.AreEqual(-100.06096779999996, features[0].Coordinates[0]);
            Assert.AreEqual(48.813056899065479, features[0].Coordinates[1]);
            Assert.AreEqual(-101.22071379999994, features[42].Coordinates[0]);
            Assert.AreEqual(48.513074399064209, features[42].Coordinates[1]);
            Assert.AreEqual(-149.22967529296875, features[38186].Coordinates[0]);
            Assert.AreEqual(61.541870116397909, features[38186].Coordinates[1]);

            Assert.AreEqual(5, reader.Features[0].Properties.Count);
            
            Assert.AreEqual("dunseith", reader.Features[0].Properties["nam"]);
            Assert.AreEqual("al020", reader.Features[0].Properties["f_code"]);
            Assert.AreEqual("773", reader.Features[0].Properties["pop"]);
            Assert.AreEqual("2010", reader.Features[0].Properties["ypc"]);
            Assert.AreEqual("usa", reader.Features[0].Properties["soc"]);

            Assert.AreEqual(reader.Features[42].Properties.Count, 5);
            Assert.AreEqual("glenburn", reader.Features[42].Properties["nam"]);
            Assert.AreEqual("al020", reader.Features[42].Properties["f_code"]);
            Assert.AreEqual("380", reader.Features[42].Properties["pop"]);
            Assert.AreEqual("2010", reader.Features[42].Properties["ypc"]);
            Assert.AreEqual("usa", reader.Features[42].Properties["soc"]);

            Assert.AreEqual(reader.Features[38186].Properties.Count, 5);
            Assert.AreEqual("matanuska", reader.Features[38186].Properties["nam"]);
            Assert.AreEqual("al020", reader.Features[38186].Properties["f_code"]);
            Assert.AreEqual("-999", reader.Features[38186].Properties["pop"]);
            Assert.AreEqual("2010", reader.Features[38186].Properties["ypc"]);
            Assert.AreEqual("usa", reader.Features[38186].Properties["soc"]);
        }
    }
}
