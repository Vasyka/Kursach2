﻿using System;
using System.IO;
using System.Diagnostics;
using WottonCountLibrary2;
namespace A2
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            char[] nucl = { 'A', 'T', 'G', 'C' };
			string path = "", str = "";
            int k;
			do
            {
                try{
                    Console.Clear();
                    BinomCount b = new BinomCount(nucl);
					//Console.Write("Введите путь к файлу в виде: /Dir1/Dir2/File.fasta или если он находится в той же директории, что и проект, то введите имя файла: File.fasta: ");
                    str = b.GetNuclStr(ref path);
					Console.WriteLine("Последовательность:\n" + str);
					Console.WriteLine("Длина последовательности: " + str.Length + " пар нуклеотидов");
					Console.Write("Введите длину окна: ");
					if (!int.TryParse(Console.ReadLine(), out k) || k <= 0 || k > str.Length)
						throw new ArgumentOutOfRangeException(null, "Длина окна должна быть положительным целым числом, не большим длины самой последовательности.");
                    var SW = Stopwatch.StartNew();
                    BinomCount.BinomCountWF(str,k);
					SW.Stop();
					Console.WriteLine("\nВремя выполнения в миллисекундах: " + Convert.ToString(SW.ElapsedMilliseconds));
					Console.WriteLine("Время в секундах: " + Convert.ToString(SW.Elapsed.Seconds));
					Console.WriteLine("Время в тиках: " + Convert.ToString(SW.ElapsedTicks));
                    SW.Reset();
                }
				catch (ArgumentOutOfRangeException ex)
				{
					Console.WriteLine("Ошибка ввода. " + ex.Message);
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
