using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CountingLibrary
{
    public class CountingClass
    {
        //Массив нуклеотидов
        static char[] nucl;

        ///Массив с количеством нуклеотидов в окне
        static uint[] nucln;

        //Хэш: строка с количеством нуклеотидов разных типов в окне и сложность
        static Dictionary<string, double> hash;

        //Конструкторы
        public CountingClass() { }
        public CountingClass(char[] nucl)
        {
            nucln = new uint[nucl.Length];
            CountingClass.nucl = nucl;
            hash = new Dictionary<string, double>();
        }

        public static double CountInFirstFrame(string str, int k)//Расчитывает сложность по Вудону-Федерхену в первом окне
        {
            double sum = 0;

            //Считаем кол-во нуклеотидов в окне по типам
            for (int i = 0; i < k; i++)
            {
                int index = Array.IndexOf(nucl, str[i]);
                nucln[index]++;
            }
            Array.Sort(nucln, nucl);

            //Считаем сумму произведений в первом окне
            uint nuclnSum = 0; //сумма количеств нуклеотидов в числителе прибавляемой дроби
            for (int i = 0; i < nucln.Length - 1; i++)
            {
                nuclnSum += nucln[i];
                for (int it = 1; it <= nucln[i + 1]; it++)
                {
                    sum += Math.Log((nuclnSum + it) / (double)it, 4);// (nucln[0]+it)/it * ... * (nucln[0]+nucln[1])/nucln[1]
                }
            }

            //Console.WriteLine("For key = {0}, value = {1}.",s0.Substring(0, k), hash[String.Join(" ", nucln)]);

            
            //Сложность в первом окне
            double wf0 = sum / k;
            hash.Add(String.Join(" ", nucln), wf0);

            return wf0;
        }

        public double CountWF(string str, int k)//Считает сложность по Вудон-Федерхену на всей последовательности
        {
            uint[] mass = new uint[nucln.Length];
            int i, j;
            double b;
            
            //Массив сложностей на всех промежутках
            double[] cwf = new double[str.Length - k + 1];

            //Суммарная сложность на всей последовательности
            double sumWF = 0;

            //Сложность в первом окне
            sumWF += CountInFirstFrame(str, k);
            double sum = sumWF * k;

            //Сдвигаем рамку
            for (int l = 1; l <= str.Length - k; l++)
            {
                //При сдвиге рамки одну букву добавляем, одну удаляем
                i = Array.IndexOf(nucl, str[l - 1]);//удаляемый символ
                //добавляемый символ
                if ((j = Array.IndexOf(nucl, str[l - 1 + k])) == -1) throw new ArgumentOutOfRangeException(null, "В последовательности найдены символы отличающихся от заданных нуклеотидов. Возможно это была РНК или последовательность аминокислот.");
                nucln[i]--;
                nucln[j]++;

                //Создаем упорядоченный массив количеств нуклеотидов в окне
                mass = (uint[])nucln.Clone();
                Array.Sort(mass);

                //Если последовательности нет в хэш-таблице
                if (!(hash.TryGetValue(String.Join(" ", mass), out cwf[l])))
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
                    hash.Add(String.Join(" ", mass), cwf[l]);
                    //Console.WriteLine("For key = {0}, value = {1}.", str.Substring(l, k), hash[String.Join(" ", m)]);
                }
                else sum = cwf[l] * k;
                sumWF += cwf[l];
            }
            /*Console.WriteLine();
            for (int t = 0; t < cwf.Length; t++) Console.WriteLine("Сложность c {0} по {1} символ = {2}", t + 1, t + k, cwf[t]);//выводим посчитанные сложности
            Console.WriteLine("\nВ хэше:");
            foreach (var w in hash)
            {
                Console.WriteLine(w.Key + " " + w.Value);
            }*/
            return sumWF / str.Length;
        }

        public string[] OpenNewFile()//Открытие и чтение файла
        {
            string[] s = new string[2] { "", "" };
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                //Фильтр FASTA файлов
                openFileDialog.Filter = "fasta files (*.fasta; *..fas; *.fna; *.ffn; *.faa; *.frn; *.fa; *.seq)|*.fasta; *..fas; *.fna; *.ffn; *.faa; *.frn; *.fa; *.seq";

                //Открываем окно диалога с пользователем
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Открываем файл
                    using (StreamReader f = File.OpenText(openFileDialog.FileName))
                    {
                        try
                        {
                            //Cчитываем информацию о последовательности
                            s[0] = f.ReadLine().TrimStart('>');

                            //Считываем саму последовательность
                            s[1] = f.ReadToEnd();
                            s[1] = Regex.Replace(s[1], @"[\sN]", "");//удаляем пробелы и пр
                            if (s[1].Length > 1100000) throw new ArgumentOutOfRangeException(null, "Длина последовательности значительно превышает 1 млн нуклеотидов. Пожалуйста выберите файл с последовательностью меньших размеров.");

                            //Создаем файл и записываем в него обновленную последовательность
                            string path = @".\logs.txt";
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine("\nОписание последовательности: " + s[0] + "\n");
                                sw.WriteLine("Длина последовательности: " + s[1].Length + " пар нуклеотидов");
                                sw.WriteLine(s[1]);
                            }
                        }
                        catch(ArgumentOutOfRangeException e)
                        {
                            MessageBox.Show(e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Вы не выбрали файл.", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //?Возможно не открывается в старых ОС
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно открыть и прочитать файл.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return s;
        }
    }
}
