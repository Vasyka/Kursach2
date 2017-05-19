using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace WindowsFormsKurs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreateGraph(zedGraphControl1);
        }
        // Массив цветов графиков
        static Color[] colors = new Color[] {
            Color.Green,
            Color.Brown,
            Color.Indigo,
            Color.Orange,
            Color.Blue,
            Color.Red,
            Color.YellowGreen};

        static Random rnd = new Random();
        public static double cwf(int l)
        {
            return l + rnd.Next(-3,3);
        } 
        
        private static void CreateGraph(ZedGraphControl a)//Изначальный график
        {
            GraphPane myPane = a.GraphPane;

            //Названия графика и осей
            myPane.Title.Text = "График зависимости сложности ДНК от длины окна";
            myPane.XAxis.Title.Text = "L - длина окна";
            myPane.YAxis.Title.Text = "WF(L) - сложность для данного L";

            //Границы графика по оси X 
            myPane.XAxis.Scale.Min = 14;
            myPane.XAxis.Scale.Max = 26;

            //Очищаем график от старых кривых
            myPane.CurveList.Clear();

            //Обновляем график
            a.AxisChange();
            a.Refresh();
            a.Invalidate();
        }
        private static void DrawGraph(ZedGraphControl zgc)//Рисование нового графика
        {
            GraphPane myPane = zgc.GraphPane;

            string dnaname = "Felis Catus(Кошка)";

            //Создаем и заполняем список точек
            PointPairList list = new PointPairList();
            for (int L = 15; L <= 25; L++)
            {
                list.Add(L, cwf(L));
            }
            
            //Добавляем сетку и легенду
            GraphProperties(myPane);

            //Выбираем цвет графика
            Color CurveColor = colors[myPane.CurveList.Count];
            /*Color CurveColor = colors[colors.Length - 1];
            IEnumerable<Color> col = myPane.CurveList.Select(p => p.Color);
            Color[] curves = colors.Expect(colorCurves);*/

            //Рисуем график
            LineItem myCurve = myPane.AddCurve(dnaname,
               list, CurveColor, SymbolType.Circle);
            

            //Стиль линии
            myCurve.Line.Width = 3;
            
            //Сглаживание графика
            myCurve.Line.IsSmooth = true;
            //myCurve.Line.IsAntiAlias = true;

            //Обновляем график
            zgc.AxisChange();
            zgc.Refresh();
            zgc.Invalidate();
        }
        private static void GraphProperties(GraphPane myPane)//Добавляем свойства графика
        {
            //Положение легенды
            myPane.Legend.Position = LegendPos.Float;//задание положения координатами
            myPane.Legend.Location.CoordinateFrame = CoordType.PaneFraction;//координаты панели
            myPane.Legend.Location.AlignH = AlignH.Right;
            myPane.Legend.Location.AlignV = AlignV.Bottom;
            myPane.Legend.Location.TopLeft = new PointF(0.98f, 0.98f);

            //Добавляем крупную пунктирную сетку по оси X
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.DashOn = 5;
            myPane.XAxis.MajorGrid.DashOff = 3;

            //Добавляем крупную пунктирную сетку по оси Y
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.DashOn = 5;
            myPane.YAxis.MajorGrid.DashOff = 2;

            //Добавляем мелкую пунктирную сетку по оси X
            myPane.XAxis.MinorGrid.IsVisible = true;
            myPane.XAxis.MinorGrid.DashOn = 1;
            myPane.XAxis.MinorGrid.DashOff = 2;
            //Добавляем мелкую пунктирную сетку по оси Y
            myPane.YAxis.MinorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.DashOn = 1;
            myPane.YAxis.MinorGrid.DashOff = 2;
        }

        private void addButton_Click(object sender, EventArgs e)//Добавляем график
        {
            //Если число кривых не слишком велико
            if(zedGraphControl1.GraphPane.CurveList.Count < 7) {
                DrawGraph(zedGraphControl1);
            }
            else
            {
                MessageBox.Show("Достигнуто максимальное количество кривых на графике. Удалите хотя бы одну из них.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void rmvButton_Click(object sender, EventArgs e)//Удаляем график
        {
            GraphPane Pane = zedGraphControl1.GraphPane;
            
            // Если есть что удалять
            if (Pane.CurveList.Count > 0)
            {
                // Номер графика для удаления
                int index = 1;

                // Удалим кривую по индексу
                Pane.CurveList.RemoveAt(index);

                // Обновим график
                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
            }
            else
            {
                MessageBox.Show("Нет кривых для удаления.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
