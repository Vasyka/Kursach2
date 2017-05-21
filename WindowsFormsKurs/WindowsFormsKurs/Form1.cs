﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using DrawingLibrary;

namespace WindowsFormsKurs
{
    public partial class Form1 : Form
    {
        DrawingClass Draw = new DrawingClass();

        public Form1()
        {
            try {
                InitializeComponent();
                Draw.CreateGraph(zedGraphControl1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName + ": " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void addButton_Click(object sender, EventArgs e)//Добавляем график
        {
            try {
                //Если число кривых не слишком велико
                if (zedGraphControl1.GraphPane.CurveList.Count < DrawingClass.colors.Length) {

                    //Вызываем окно диалога с польователем и открываем файл
                    string[] input = OpenNewFile();

                    //Рисуем график
                    Draw.AddGraph(zedGraphControl1, input);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(null, "Достигнуто максимальное количество кривых на графике. Удалите хотя бы одну из них.");
                }
            }
            catch(ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName + ": " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void rmvButton_Click(object sender, EventArgs e)//Удаляем график
        {
            try {
                // Если есть что удалять
                if (zedGraphControl1.GraphPane.CurveList.Count > 0)
                {
                    Draw.DelGraph(zedGraphControl1);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(null,"Нет кривых для удаления.");
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().FullName + ": " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public string[] OpenNewFile()//Открытие окна диалога с пользователем и чтение файла
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
                            if (s[1].Length < 25) throw new ArgumentOutOfRangeException(null, "Длина последовательности меньше длины окна. Пожалуйста выберите файл с более длинной последовательностью.");
                            //Создаем файл с логами и записываем в него обновленную последовательность
                            /*string path = @".\logs.txt";
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine("\nОписание последовательности: " + s[0] + "\n");
                                sw.WriteLine("Длина последовательности: " + s[1].Length + " пар нуклеотидов");
                                sw.WriteLine(s[1]);
                            }*/
                        }
                        catch (ArgumentOutOfRangeException e)
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
        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
