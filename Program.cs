using System;
using System.Diagnostics;
using WottonFederhenCountLibrary;

namespace Kursach
{
	class Program
	{
		static void Main(string[] args)
		{
			char[] str;
			char[] nucl = { 'A', 'T', 'G', 'C' };
			double[] cwf;
			uint n;
			int k;
			do
			{
				Console.WriteLine("Введите длину последовательности:");
				try
				{
					if (!uint.TryParse(Console.ReadLine(), out n) || n == 0)
						throw new ArgumentOutOfRangeException(null, "Длина последовательности должна быть положительным целым числом.");
					WottonCount.GenerateRndStr(out str, n, nucl);
					WottonCount.PrintStr(str);
					Console.WriteLine("Введите длину окна:");
					if (!int.TryParse(Console.ReadLine(), out k) || k <= 0 || k > n)
						throw new ArgumentOutOfRangeException(null, "Длина окна должна быть положительным целым числом, не большим длины самой последовательности.");
					
					Stopwatch SW = new Stopwatch();//счетчик
					SW.Start(); 
					WottonCount.CountWF(str, k, nucl, out cwf);
					SW.Stop();
					Console.WriteLine("\nВремя выполнения в миллисекундах: " + Convert.ToString(SW.ElapsedMilliseconds));
					Console.WriteLine("Время в секундах: " + Convert.ToString(SW.Elapsed.Seconds)); 
					Console.WriteLine("Время в тиках: " + Convert.ToString(SW.ElapsedTicks));
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

