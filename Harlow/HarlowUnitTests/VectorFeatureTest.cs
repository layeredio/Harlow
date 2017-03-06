using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Harlow;

namespace HarlowUnitTests {

    [TestClass]
	public class VectorFeatureTest {

		[TestMethod]
		public void Test_VectorFeature_Point() {
			object[] parm = new object[1];
			parm[0] = ShapeType.Point;

			var  vfMock = new Mock<VectorFeature>(parm);

			Assert.IsNull(vfMock.Object.Type);
			Assert.IsNull(vfMock.Object.Coordinates);
			Assert.IsNull(vfMock.Object.Bbox);
			Assert.IsNotNull(vfMock.Object.Properties);
			
		}

		[TestMethod]
		public void Test__VectorFeature_NotPoint() {
			object[] parm = new object[1];
			parm[0] = ShapeType.Polygon;

			var  vfMock = new Mock<VectorFeature>(parm);

			Assert.IsNull(vfMock.Object.Type);
			Assert.IsNull(vfMock.Object.Coordinates);
			Assert.IsNotNull(vfMock.Object.Bbox);
			Assert.AreEqual(4, vfMock.Object.Bbox.Length);
			Assert.IsNotNull(vfMock.Object.Properties);
			
		}
	}
}
