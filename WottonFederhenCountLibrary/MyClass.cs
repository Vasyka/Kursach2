using System;
using System.Collections.Generic;
namespace WottonFederhenCountLibrary
{
	public static class WottonCount
	{
		static Random rnd = new Random();
		//Создает цепочку ДНК из случайных нуклеотидов
		public static void GenerateRndStr(out char[] str, uint n, char[] nucl)
		{
			str = new char[n];
			for (int i = 0; i < n; i++)
			{
				str[i] = nucl[rnd.Next(nucl.Length)];
			}
		}
		//Печатает цепочку ДНК
		public static void PrintStr(char[] str)
		{
			foreach (char letter in str)
			{
				Console.Write(letter + " ");
			}
			Console.WriteLine();
		}
		//Выводит количество найденных в окне нуклеотидов
		public static void PrintNuclNumb(uint[] nucln, char[] nucl)
		{
			for (uint i = 0; i < nucln.Length; i++)
			{
				Console.Write("{0}: {1} ", nucl[i], nucln[i]);
			}
			Console.WriteLine();
		}
		//Находит номер нуклеотида в массиве
		public static uint CountLetter(char[] nucl, char letter)
		{
			uint i = 0;
			while (letter != nucl[i])
			{
				i++;
			}
			return i;
		}
		//CWF в первом окне
		public static double CountInFirstFrame(char[] str, int k, char[] nucl, double[] cwf, uint[] nucln, ref Dictionary<string, double> hash)
		{
			double sum = 0;
			for (uint i = 0; i < k; i++)//считаем кол-во нуклеотидов в окне по типам
			{
				nucln[CountLetter(nucl, str[i])]++;
			}
			Array.Sort(nucln, nucl);
			Console.WriteLine("В первой рамке:");
			PrintNuclNumb(nucln, nucl);//кол-во нуклеотидов в рамке
			uint nuclnSum = 0; //сумма количеств нуклеотидов в числителе прибавляемой дроби
			for (int i = 0; i < nucln.Length - 1; i++)//считаем сумму произведений в первом окне
			{
				nuclnSum += nucln[i];
				for (int it = 1; it <= nucln[i + 1]; it++)
				{
					sum += Math.Log((nuclnSum + it) / (double)it, 4);// (nucln[0]+it)/it * ... * (nucln[0]+nucln[1])/nucln[1]
					Console.Write(" {0}/{1} ", (nuclnSum + it), it);
					Console.WriteLine(" sum = " + sum);
				}
			}
			cwf[0] = sum / k;
			string s0 = new string(str);
			hash.Add(s0.Substring(0, k), cwf[0]);
			Console.WriteLine("For key = {0}, value = {1}.",s0.Substring(0, k), hash[s0.Substring(0, k)]);
			return sum;
		}
		//Считает сложность по Вудон-Федерхену
		public static void CountWF(char[] str, int k, char[] nucl, out double[] cwf)
		{
			Dictionary<string, double> hash = new Dictionary<string, double>();

			cwf = new double[str.Length - k + 1];
			uint[] nucln = new uint[nucl.Length];//массив с количеством нуклеотидов в окне
			double sum = CountInFirstFrame(str, k, nucl, cwf, nucln, ref hash);//в первом окне
			Console.WriteLine("Сдвигаем рамку:");

			string s = new string(str);

			for (int l = 1; l <= str.Length - k; l++)//двигаем рамку и добавляем новые члены в сумму и считаем cwf
			{
				if(!(hash.TryGetValue(s.Substring(l, k), out cwf[l]))){
					uint i, j;
					double b;
					i = CountLetter(nucl, str[l - 1]);//удаляемый символ
					j = CountLetter(nucl, str[l - 1 + k]);//добавляемый символ
					nucln[i]--;
					Console.Write("-" + nucl[i] + " ");
					nucln[j]++;
					Console.Write("+" + nucl[j] + " ");
					if (i != j)//если разные символы
					{
						if (nucln[i] == 0) b = 1.0 / nucln[j];
						else b = (nucln[i] + 1) * 1.0 / nucln[j];
						double a = Math.Log(b, 4);
						Console.Write("{0}/{1} {2} {3} ", (nucln[i] + 1), nucln[j], b, a);
						sum = sum + Math.Log(b, 4);
					}
					Console.WriteLine("sum = " + sum);
					cwf[l] = sum / k;
					hash.Add(s.Substring(l, k), cwf[l]);
					Console.WriteLine("For key = {0}, value = {1}.", s.Substring(l, k), hash[s.Substring(l, k)]);
				}
			}
			Console.WriteLine();
			for (int i = 0; i < cwf.Length; i++) Console.WriteLine("Сложность c {0} по {1} символ = {2}", i + 1, i + k, cwf[i]);//выводим посчитанные сложности
		}
	}
}
