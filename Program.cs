using System;


namespace Kursach
{
	class Program
	{
		static Random rnd = new Random();
		//Создает цепочку ДНК из случайных нуклеотидов
		static void GenerateRndStr(out char[] str, uint n, char[] nucl)
		{
			str = new char[n];
			for (int i = 0; i < n; i++)
			{
				str[i] = nucl[rnd.Next(nucl.Length)];
			}
		}
		//Печатает цепочку ДНК
		static void PrintStr(char[] str)
		{
			foreach (char letter in str)
			{
				Console.Write(letter + " ");
			}
			Console.WriteLine();
		}
		//Выводит количество найденных в окне нуклеотидов
		static void PrintNuclNumb(uint[] nucln, char[] nucl)
		{
			for (uint i = 0; i < nucln.Length; i++)
			{
				Console.Write("{0}: {1} ", nucl[i], nucln[i]);
			}
			Console.WriteLine();
		}
		//Находит номер нуклеотида в массиве
		static uint CountLetter(char[] nucl, char letter)
		{
			uint i = 0;
			while (letter != nucl[i])
			{
				i++;
			}
			return i;
		}
		//CWF в первом окне
		static double CountInFirstFrame(char[] str, uint k, char[] nucl, double[] cwf, uint[] nucln)
		{
			double sum = 0;
			for (uint i = 0; i < k; i++)//считаем кол-во нуклеотидов в окне по типам
			{
				nucln[CountLetter(nucl, str[i])]++;
			}
			Array.Sort(nucln, nucl);
			Console.WriteLine("В первой рамке:");
			PrintNuclNumb(nucln, nucl);//кол-во нуклеотидов в рамке
			for (double i = 1; i <= k; i++)//считаем сумму произведений в первом окне
			{
				double j = i, k1 = k - nucln[nucln.Length - 1];
				int t = nucln.Length - 2;
				while (t >= 0)
				{
					if (i > k1) j -= nucln[t];
					k1 -= nucln[t];
					t--;
				}
				Console.Write(" {0}/{1} ", i, j);
				sum += Math.Log(i / j, 4);
				Console.WriteLine(" sum = " + sum);
			}
			cwf[0] = sum / k;
			return sum;
		}
		//Считает сложность по Вудон-Федерхену
		static void CountWF(char[] str, uint k, char[] nucl, out double[] cwf)
		{
			cwf = new double[str.Length - k + 1];
			uint[] nucln = new uint[nucl.Length];//массив с количеством нуклеотидов в окне
			double sum = CountInFirstFrame(str, k, nucl, cwf, nucln);//в первом окне
			Console.WriteLine("Сдвигаем рамку:");
			for (uint l = 1; l <= str.Length - k; l++)//двигаем рамку и добавляем новые члены в сумму и считаем cwf
			{
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
			}
			Console.WriteLine();
			for (int i = 0; i < cwf.Length; i++) Console.WriteLine("Сложность c {0} по {1} символ = {2}", i + 1, i + k, cwf[i]);//выводим посчитанные сложности
		}
		static void Main(string[] args)
		{
			char[] str;
			char[] nucl = { 'A', 'T', 'G', 'C' };
			double[] cwf;
			uint n, k;
			do
			{
				Console.WriteLine("Введите длину последовательности:");
				try
				{
					if (!uint.TryParse(Console.ReadLine(), out n) || n == 0)
						throw new ArgumentOutOfRangeException(null, "Длина последовательности должна быть положительным целым числом.");
					GenerateRndStr(out str, n, nucl);
					PrintStr(str);
					Console.WriteLine("Введите длину окна:");
					if (!uint.TryParse(Console.ReadLine(), out k) || k == 0 || k > n)
						throw new ArgumentOutOfRangeException(null, "Длина окна должна быть положительным целым числом, не большим длины самой последовательности.");
					CountWF(str, k, nucl, out cwf);
				}
				catch (ArgumentOutOfRangeException e)
				{
					Console.WriteLine("Ошибка ввода. " + e.Message);
				}
				catch (Exception e)
				{
					Console.WriteLine("Что-то пошло не так. " + e.Message);
				}
				Console.WriteLine("Для выхода из программы нажмите Esc");
			} while (Console.ReadKey(true).Key != ConsoleKey.Escape);
		}
	}
}

