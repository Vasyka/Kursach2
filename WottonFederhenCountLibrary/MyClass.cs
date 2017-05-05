using System;
using System.IO;
using System.Text.RegularExpressions;
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
        //Получает последовательность из файла
        public static char[] GetNuclStr(char[] nucl, ref string path){
            string s0, s = "";
            char[] str = new char[]{'-'};
			string[] FastaExt = { "..fas", ".fasta", ".fna", ".ffn", ".faa", ".frn" };//расширения FASTA файлов
            if (path != null & path != "" & !path.Contains("/")) path = Path.Combine(@"../..", path);//добавляем путь к файлу, если введено только название
			if (Array.IndexOf(FastaExt, Path.GetExtension(path)) == -1) throw new ArgumentOutOfRangeException(null, "Некорректное расширение файла. Файл должен быть в формате FASTA.");//проверяем расширение
			//Открытие файла
			using (StreamReader f = File.OpenText(path))
			{
                try{
					s0 = f.ReadLine();
					Console.WriteLine("\nОписание последовательности: " + s0.TrimStart('>') + "\n");
					s = f.ReadToEnd();//считываем последовательность
                    s = Regex.Replace(s, @"\s", "");//удаляем пробелы и пр
					str = s.ToCharArray();
                    for (int i = 0; i < str.Length; i++)//заменяем неизвестные нуклеотиды случайными
                    {
                        str[i] = (str[i] == 'N') ? nucl[rnd.Next(0, nucl.Length)] : str[i];
                    }
			    }
				catch (Exception e)
			    {
				    throw e;
			    }
			}
            return str;
        }
		//Печатает цепочку ДНК
		public static void PrintStr(char[] str)
		{
			foreach (char letter in str)
			{
				Console.Write(letter + "");
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
			//Console.WriteLine("В первой рамке:");
			//PrintNuclNumb(nucln, nucl);//кол-во нуклеотидов в рамке
			uint nuclnSum = 0; //сумма количеств нуклеотидов в числителе прибавляемой дроби
			for (int i = 0; i < nucln. Length - 1; i++)//считаем сумму произведений в первом окне
			{
				nuclnSum += nucln[i];
				for (int it = 1; it <= nucln[i + 1]; it++)
				{
					sum += Math.Log((nuclnSum + it) / (double)it, 4);// (nucln[0]+it)/it * ... * (nucln[0]+nucln[1])/nucln[1]
					//Console.Write(" {0}/{1} ", (nuclnSum + it), it);
					//Console.WriteLine(" sum = " + sum);
				}
			}
			cwf[0] = sum / k;
			//string s0 = new string(str);
			hash.Add(String.Join(" ",nucln), cwf[0]);
			//Console.WriteLine("For key = {0}, value = {1}.",s0.Substring(0, k), hash[String.Join(" ", nucln)]);
			return sum;
		}
		//Считает сложность по Вудон-Федерхену
		public static void CountWF(char[] str, int k, char[] nucl, out double[] cwf)
		{
			Dictionary<string, double> hash = new Dictionary<string, double>();

			cwf = new double[str.Length - k + 1];
			uint[] nucln = new uint[nucl.Length];//массив с количеством нуклеотидов в окне
			double sum = CountInFirstFrame(str, k, nucl, cwf, nucln, ref hash);//в первом окне
			//Console.WriteLine("Сдвигаем рамку:");

			//string s = new string(str);
			uint[] m = new uint[nucln.Length];
			uint i, j;
			double b;
			for (int l = 1; l <= str.Length - k; l++)//двигаем рамку и добавляем новые члены в сумму и считаем cwf
			{
				//PrintNuclNumb(nucln, nucl);

				i = CountLetter(nucl, str[l - 1]);//удаляемый символ
				j = CountLetter(nucl, str[l - 1 + k]);//добавляемый символ
				nucln[i]--;					
				//Console.Write("-" + nucl[i] + " ");
				nucln[j]++;
				//Console.Write("+" + nucl[j] + "\n");
				//foreach (uint q in nucln) Console.Write(q + " ");
				m = (uint[])nucln.Clone();
				Array.Sort(m);
				//foreach (uint q in nucln) Console.Write(q + " ");
				if (!(hash.TryGetValue(String.Join(" ", m), out cwf[l])))//если последовательности нет в хэш-таблице
				{
					if (i != j)//если разные символы
					{
						if (nucln[i] == 0) b = 1.0 / nucln[j];
						else b = (nucln[i] + 1) * 1.0 / nucln[j];
						//double a = Math.Log(b, 4);
						//Console.Write("{0}/{1} {2} {3} ", (nucln[i] + 1), nucln[j], b, a);
						sum = sum + Math.Log(b, 4);
						if (sum <= 1.0E-20) sum = 0;
					}
					//Console.WriteLine("sum = " + sum);
					cwf[l] = sum / k;
					hash.Add(String.Join(" ", m), cwf[l]);
					//Console.WriteLine("For key = {0}, value = {1}.", s.Substring(l, k), hash[String.Join(" ", m)]);
				}
				else sum = cwf[l] * k;
			}
			Console.WriteLine();
			for (int t = 0; t < cwf.Length; t++) Console.WriteLine("Сложность c {0} по {1} символ = {2}", t + 1, t + k, cwf[t]);//выводим посчитанные сложности
			Console.WriteLine("\nВ хэше:");
			foreach (var w in hash)
			{
				Console.WriteLine(w.Key + " " + w.Value);
			}
		}
	}
}
