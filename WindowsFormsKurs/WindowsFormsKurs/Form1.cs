using Microsoft.Win32;
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
                    Draw.AddGraph(zedGraphControl1);
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
        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
