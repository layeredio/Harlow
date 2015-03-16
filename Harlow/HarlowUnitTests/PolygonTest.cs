﻿using System;
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

            Assert.AreEqual(json.Length, 25309572);
            Assert.AreEqual(reader.Features.Length, 1516);

            Assert.AreEqual(reader.Features[0].Coordinates[0][0].Value[0], -118.456008);
            Assert.AreEqual(reader.Features[0].Coordinates[0][0].Value[1], 34.284903);
            Assert.AreEqual(reader.Features[42].Coordinates[0][0].Value[0], -118.30807);
            Assert.AreEqual(reader.Features[42].Coordinates[0][0].Value[1], 34.161224);
            Assert.AreEqual(reader.Features[1515].Coordinates[0][0].Value[0], -122.060783);
            Assert.AreEqual(reader.Features[1515].Coordinates[0][0].Value[1], 37.05574);

            Assert.AreEqual(reader.Features[0].Properties.Count, 16);

            Assert.AreEqual(reader.Features[42].Properties["STATEFP"], "06");
            Assert.AreEqual(reader.Features[42].Properties["PLACEFP"], "30000");
            Assert.AreEqual(reader.Features[42].Properties["PLACENS"], "02410597");
            Assert.AreEqual(reader.Features[42].Properties["GEOID"], "0630000");
            Assert.AreEqual(reader.Features[42].Properties["NAME"], "Glendale");
            Assert.AreEqual(reader.Features[42].Properties["NAMELSAD"], "Glendale city");
            Assert.AreEqual(reader.Features[42].Properties["LSAD"], "25");
            Assert.AreEqual(reader.Features[42].Properties["CLASSFP"], "C1");
            Assert.AreEqual(reader.Features[42].Properties["PCICBSA"], "Y");
            Assert.AreEqual(reader.Features[42].Properties["PCINECTA"], "N");
            Assert.AreEqual(reader.Features[42].Properties["MTFCC"], "G4110");
            Assert.AreEqual(reader.Features[42].Properties["FUNCSTAT"], "A");
            Assert.AreEqual(reader.Features[42].Properties["ALAND"], "78848571");
            Assert.AreEqual(reader.Features[42].Properties["AWATER"], "337673");
            Assert.AreEqual(reader.Features[42].Properties["INTPTLAT"], "+34.1813929");
            Assert.AreEqual(reader.Features[42].Properties["INTPTLON"], "-118.2458301");
        }
    }
}