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

        //Хэш: строка с количеством нуклеотидов разных типов в окне и сумма(сложность * длину окна)
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

            //Сохраняем сумму в первом окне в хэш
            hash.Add(String.Join(" ", nucln), sum);

            return sum;
        }

        public double CWF(string str, int k)//Считает сложность по Вудон-Федерхену на всей последовательности
        {
            uint[] mass = new uint[nucln.Length];
            int i, j;
            double b, temp;
            double CWF;

            //Сумма на всей последовательности(общая сложность * N * k)
            double sumWF = 0;

            //Создаем файл с логами
            //string path = @".\logs.txt";
            //using (StreamWriter sw = File.CreateText(path))
            //{
               
                //Сумма в первом окне
                double sum = CountInFirstFrame(str, k);
                sumWF = sumWF + sum;
                //sw.WriteLine("sumWF = " + sumWF);

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
                    if (!(hash.TryGetValue(String.Join(" ", mass), out temp)))
                    {
                        if (i != j)//если разные символы
                        {
                            //В сумму добавляем частное количеств нового и старого символа
                            b = (nucln[i] + 1) * 1.0 / nucln[j]; 
                            sum = sum + Math.Log(b, 4);
                            
                            //Если все одинаковые
                            if (sum <= 1.0E-20) sum = 0;
                         }
                        hash.Add(String.Join(" ", mass), sum);
                    }
                    else sum = temp;

                    //Добавляем в общую сумму
                    sumWF += sum;
                    //sw.WriteLine("sumWF = " + sumWF + ", value = " + sum);
                }
                CWF= sumWF / k / str.Length;
                //sw.WriteLine("CWF = " + CWF);
            //}
            
            return CWF;
        }

    }
}
