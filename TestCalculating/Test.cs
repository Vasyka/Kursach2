using NUnit.Framework;
using System;
using System.Collections.Generic;
namespace TestCalculating
{
	using WottonFederhenCountLibrary;
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestCase()
		{
			char[] nucl = { 'A', 'T', 'G', 'C' };
			Dictionary<string, double> hash = new Dictionary<string, double>();
			Assert.AreEqual(WottonFederhenCountLibrary.WottonCount.CountInFirstFrame("AGTCAC", 3),Math.Log(6,4));//example test for CountInFirstFrame
		}
	}
}
