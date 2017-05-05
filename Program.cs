using System;
using System.IO;
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
            string path = "";
			double[] cwf;
			int k;
			do
			{
				try
				{
                    Console.Clear();
					/*Console.WriteLine("Введите длину последовательности:");
					if (!uint.TryParse(Console.ReadLine(), out n) || n == 0)
						throw new ArgumentOutOfRangeException(null, "Длина последовательности должна быть положительным целым числом.");
					WottonCount.GenerateRndStr(out str, n, nucl);*/
					//Ввод пути к файлу
					Console.Write("Введите путь к файлу в виде: /Dir1/Dir2/File.fasta или если он находится в той же директории, что и проект, то введите имя файла: File.fasta: ");
					path = Console.ReadLine();
					str = WottonCount.GetNuclStr(nucl, ref path);
                    Console.WriteLine("Последовательность:");
					WottonCount.PrintStr(str);
                    Console.WriteLine("Длина последовательности: " + str.Length + " пар нуклеотидов");
                    Console.Write("Введите длину окна: ");
                    if (!int.TryParse(Console.ReadLine(), out k) || k <= 0 || k > str.Length)
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
				catch (ArgumentException)
				{
					Console.WriteLine("Ошибка. Введена пустая строка или строка с некорректными символами. Введите путь к файлу в виде: \"~/Dir1/Dir2/File.fasta\" или если он находится в той же директории, что и проект, то введите имя файла: \"File.fasta\"");
				}
				catch (IOException)
				{
					Console.WriteLine("Ошибка ввода. Файл \"" + Path.GetFullPath(path) + "\" не найден. Проверьте, что вы правильно написали название файла и ваш файл находится именно там.");
				}
				catch (UnauthorizedAccessException)
				{
					Console.WriteLine("Ошибка. Нельзя получить доступ к файлу \"" + Path.GetFullPath(path) + "\". Проверьте, что ваш файл находится действительно там и вы имеете к нему доступ.");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.GetType().FullName + ": " + ex.Message + ex.TargetSite);
				}
				Console.WriteLine("Для выхода из программы нажмите Esc");
			} while (Console.ReadKey(true).Key != ConsoleKey.Escape);
		}
	}
}

