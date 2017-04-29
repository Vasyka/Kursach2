using System;
using System.Collections.Generic;

namespace CountTheNumberOfHashes
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			int k;
			int[] mass = new int[4];
			SortedSet<string> s;
			do
			{
				try
				{
					Console.WriteLine("Введите длину окна");
					if(!int.TryParse(Console.ReadLine(), out k) | k <= 0) throw new ArgumentOutOfRangeException();
					s = new SortedSet<string>();
					for (int i = k; i >= k/4; i--)
					{
						for (int j = 0; j <= k - i; j++)
						{
							for (int l = 0; l <= k - j - i; l++)
							{
								for (int t = 0; t <= k - l - j - i; t++)
								{
									if (i + j + l + t == k)
									{
										mass[0] = i;
										mass[1] = j;
										mass[2] = l;
										mass[3] = t;
										Array.Sort(mass);
										s.Add(String.Join(" ", mass));
										Console.WriteLine(i + " " + j + " " + l + " " + t);
									}
								}
							}
						}
					}
					Console.WriteLine("Искомое количество вариантов = " + s.Count + ":");
					foreach (string m in s)
					{
						Console.WriteLine(m);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
				Console.WriteLine("Нажмите Esc для выхода");
			} while (Console.ReadKey(true).Key != ConsoleKey.Escape);
		}
	}
}
