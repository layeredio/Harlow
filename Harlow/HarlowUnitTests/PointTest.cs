using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Harlow;

namespace HarlowUnitTests
{
    [TestClass]
    public class PointTest
    {
        string pointFile = @"..\..\Shapefiles\Point\builtupp_usa.shp";
        
        [TestMethod]
        public void USGS_CitiesAndTowns()
        {
            ShapeFileReader reader = new ShapeFileReader(pointFile);
            reader.LoadFile();

            string json = reader.FeaturesAsJson();
            File.WriteAllText("point.json", json);

            Assert.AreEqual(json.Length, 8028178);
            Assert.AreEqual(reader.Features.Length, 38187);

            Assert.AreEqual(reader.Features[0].Coordinates[0][0].Value[0], -100.06096779999996);
            Assert.AreEqual(reader.Features[0].Coordinates[0][0].Value[1], 48.813056899065479);
            Assert.AreEqual(reader.Features[42].Coordinates[0][0].Value[0], -101.22071379999994);
            Assert.AreEqual(reader.Features[42].Coordinates[0][0].Value[1], 48.513074399064209);
            Assert.AreEqual(reader.Features[38186].Coordinates[0][0].Value[0], -149.22967529296875);
            Assert.AreEqual(reader.Features[38186].Coordinates[0][0].Value[1], 61.541870116397909);

            Assert.AreEqual(reader.Features[0].Properties.Count, 5);
            
            Assert.AreEqual(reader.Features[0].Properties["nam"], "dunseith");
            Assert.AreEqual(reader.Features[0].Properties["f_code"], "al020");
            Assert.AreEqual(reader.Features[0].Properties["pop"], "773");
            Assert.AreEqual(reader.Features[0].Properties["ypc"], "2010");
            Assert.AreEqual(reader.Features[0].Properties["soc"], "usa");

            Assert.AreEqual(reader.Features[42].Properties.Count, 5);
            Assert.AreEqual(reader.Features[42].Properties["nam"], "glenburn");
            Assert.AreEqual(reader.Features[42].Properties["f_code"], "al020");
            Assert.AreEqual(reader.Features[42].Properties["pop"], "380");
            Assert.AreEqual(reader.Features[42].Properties["ypc"], "2010");
            Assert.AreEqual(reader.Features[42].Properties["soc"], "usa");

            Assert.AreEqual(reader.Features[38186].Properties.Count, 5);
            Assert.AreEqual(reader.Features[38186].Properties["nam"], "matanuska");
            Assert.AreEqual(reader.Features[38186].Properties["f_code"], "al020");
            Assert.AreEqual(reader.Features[38186].Properties["pop"], "-999");
            Assert.AreEqual(reader.Features[38186].Properties["ypc"], "2010");
            Assert.AreEqual(reader.Features[38186].Properties["soc"], "usa");
        }
    }
}
