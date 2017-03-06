using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Harlow;

namespace HarlowUnitTests {

	[TestClass()]
	public class PointATests {

		[TestMethod()]
		public void PointA_DefaultConstructorTest() {
			var p = new PointA();

			Assert.IsInstanceOfType(p.Value, typeof(List<double>));
			Assert.AreEqual(2, p.Value.Count);
			Assert.AreEqual(0.0, p.Value[0]);
			Assert.AreEqual(0.0, p.Value[1]);
		}

		[TestMethod()]
		public void PointA_XandYConstructorTest() {
		
			var p = new PointA(1.85, 3.56);

			Assert.IsInstanceOfType(p.Value, typeof(List<double>));
			Assert.AreEqual(2, p.Value.Count);
			Assert.AreEqual(1.85, p.Value[0]);
			Assert.AreEqual(3.56, p.Value[1]);
		}

		[TestMethod()]
		public void PointA_GetEnumeratorTest() {
			var p = new PointA(123.456, 789.012);
			
			Assert.IsInstanceOfType(p.GetEnumerator(), typeof(IEnumerator<double>));
			
			IEnumerator<double> pe = p.GetEnumerator();
			pe.MoveNext();
			Assert.AreEqual(123.456, pe.Current);
			pe.MoveNext();
			Assert.AreEqual(789.012, pe.Current);

		}
	}
}
