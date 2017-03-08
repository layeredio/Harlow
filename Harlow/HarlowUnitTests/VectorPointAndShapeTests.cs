using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using Harlow;

namespace HarlowUnitTests {

    [TestClass]
	public class VectorPointAndShapeTests {
	
		[TestMethod]
		public void VectorPoint_Constructor() {
			var vp = new VectorPoint(3);
			
			Assert.AreEqual(ShapeType.Point.ToString(), vp.Type);

			Assert.IsNotNull(vp.Coordinates);
			Assert.AreEqual(vp.Coordinates.Length, 3);

			Assert.IsInstanceOfType(vp.Coordinates, typeof(double[]));
		}

		[TestMethod]
		public void VectorShape_Constructor() {
			var vp = new VectorShape(3, ShapeType.Polygon);
			
			Assert.IsNotNull(vp.Coordinates);
			Assert.AreEqual(vp.Coordinates.Capacity, 3);

			Assert.IsInstanceOfType(vp.Coordinates, typeof(List<PointA[]>));
		}

	}
}
