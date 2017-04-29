using System;
using System.Collections.Generic;
namespace testhash
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Dictionary<string, double> hash = new Dictionary<string, double>();
			char[] str = { 'a', 'g', 't', 'c', 'a','a','a'};
			double[] cwf = new double[5] { 0, 0, 0, 0, 0 };
			string s = new string(str);
			cwf[1] = 1;
			hash.Add(s.Substring(1, 3), cwf[1]);
			Console.WriteLine("For key = {0}, value = {1}.", s.Substring(1, 3), hash[s.Substring(1, 3)]);
			Console.WriteLine(s);
		}
	}
}
