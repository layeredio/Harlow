using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using Harlow;

namespace HarlowUnitTests {

    [TestClass]
	public class VectorFeatureTest {
	
		[TestMethod]
		public void VectorFeature_ConstructAPoint() {
			object[] parm = new object[1];
			parm[0] = ShapeType.Point;

			var  vfMock = new Mock<VectorFeature>(parm);

			Assert.AreEqual(ShapeType.Point.ToString(), vfMock.Object.Type);
			Assert.IsNull(vfMock.Object.Coordinates);
			Assert.IsNull(vfMock.Object.Bbox);
			Assert.IsNotNull(vfMock.Object.Properties);
			
		}

		[TestMethod]
		public void VectorFeature_ConstructANonPoint() {
			object[] parm = new object[1];
			parm[0] = ShapeType.Polygon;

			var  vfMock = new Mock<VectorFeature>(parm);
			
			Assert.AreEqual(ShapeType.Polygon.ToString(), vfMock.Object.Type);
			Assert.IsNull(vfMock.Object.Coordinates);
			Assert.IsNotNull(vfMock.Object.Bbox);
			Assert.AreEqual(4, vfMock.Object.Bbox.Length);
			Assert.IsNotNull(vfMock.Object.Properties);
			
		}

		[TestMethod]
		public void VectorFeature_TypePropertyIsWritable()
		{
			object[] parm = new object[1];
			parm[0] = ShapeType.Polygon;

			var vfMock = new Mock<VectorFeature>(parm);

			vfMock.Object.Type = "Line";
			Assert.AreEqual("Line", vfMock.Object.Type);
		}

		[TestMethod]
		public void VectorFeature_BboxPropertyIsWritable()
		{
			object[] parm = new object[1];
			parm[0] = ShapeType.Polygon;

			var vfMock = new Mock<VectorFeature>(parm);

			vfMock.Object.Bbox = new double[3] { 1.0, 2.0, 3.0};
			Assert.AreEqual(1.0, vfMock.Object.Bbox[0]);
		}

		[TestMethod]
		public void VectorFeature_DictionaryPropertyIsWritable()
		{
			object[] parm = new object[1];
			parm[0] = ShapeType.Polygon;

			var vfMock = new Mock<VectorFeature>(parm);

			vfMock.Object.Properties = new Dictionary<string, string> { {"test","add"} };
			Assert.AreEqual("add", vfMock.Object.Properties["test"]);
		}
	}
}
