using System;
using System.IO;
namespace liveDNA
{
	class MainClass
	{
        public static Random rnd = new Random();
		public static void Main(string[] args)
		{
			string s0, s = "", path = "";
            char[] nucl = { 'A', 'T', 'G', 'C' };
            string[] FastaExt = { "..fas", ".fasta", ".fna", ".ffn", ".faa", ".frn" };//расширения FASTA файлов
			try
            {
                //Ввод пути к файлу
                Console.Write("Введите путь к файлу в виде: /Dir1/Dir2/File.fasta или если он находится в той же директории, что и проект, то введите имя файла: File.fasta: ");
                path = Console.ReadLine();
                if (Array.IndexOf(FastaExt, Path.GetExtension(path)) == -1) throw new ArgumentOutOfRangeException(null, "Ошибка. Некорректное расширение файла. Файл должен быть в формате FASTA.");//проверяем расширение
                if (path != null & path != "" & !path.Contains("/")) path = Path.Combine("../../..", path);//добавляем путь к файлу, если введено только название
				using (StreamReader f = File.OpenText(path))//открытие файла
                {
                    try
                    {
                        s0 = f.ReadLine();
                        Console.WriteLine("\nОписание последовательности: " + s0.TrimStart('>') + "\n");
                        s = f.ReadToEnd();//считываем последовательность
                        char[] str = s.ToCharArray();
                        for (int i = 0; i < s.Length; i++)//заменяем неизвестные нуклеотиды случайными
                        {
                            str[i] = (str[i] == 'N') ? nucl[rnd.Next(0, nucl.Length)] : str[i];
                            Console.Write(str[i]);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
			catch (ArgumentOutOfRangeException e)
			{
				Console.WriteLine(e.Message);
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
            catch(Exception ex){
                Console.WriteLine(ex.GetType().FullName + ": " + ex.Message);
            }
		}
	}
}
