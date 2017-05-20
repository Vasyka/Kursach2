﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using ZedGraph;
using CountingLibrary;
using System.Windows.Forms;

namespace DrawingLibrary
{
    public class DrawingClass
    {
        static char[] nucl = { 'A', 'T', 'G', 'C' };
        CountingClass c = new CountingClass(nucl);
        // Массив цветов графиков
        public static Color[] colors = new Color[] {
            Color.Green,
            Color.Brown,
            Color.Indigo,
            Color.Orange,
            Color.Blue,
            Color.Red,
            Color.YellowGreen};

        //Список неиспользованных на данный момент цветов кривых
        public static List<Color> ColorList = colors.ToList<Color>();

        public void CreateGraph(ZedGraphControl a)//Изначальный график
        {
            GraphPane myPane = a.GraphPane;

            //Названия графика и осей
            myPane.Title.Text = "График зависимости сложности ДНК от длины окна";
            myPane.XAxis.Title.Text = "L - длина окна";
            myPane.YAxis.Title.Text = "WF(L) - сложность для данного L";

            //Границы графика по оси X 
            myPane.XAxis.Scale.Min = 14;
            myPane.XAxis.Scale.Max = 25;

            //Очищаем график от старых кривых
            myPane.CurveList.Clear();

            //Обновляем график
            a.AxisChange();
            a.Refresh();
            a.Invalidate();
        }

        public void AddGraph(ZedGraphControl zgc)//Рисование нового графика
        {
            //Открываем файл и получаем входную последовательность
            string[] input = c.OpenNewFile();
            if (input == null) throw new NullReferenceException("Получена пустая ссылка на последовательность.");
            string dnaString = input[1];

            if (dnaString != "")//если строка не пуста
            {
                GraphPane myPane = zgc.GraphPane;

                //Получаем идентификатор последовательности
                string dnaName = input[0].Substring(0, input[0].IndexOf(" "));

                //Создаем и заполняем список точек
                PointPairList list = new PointPairList();
                for (int L = 15; L <= 24; L = L + 3)
                {
                    MessageBox.Show("HI", "yea", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    list.Add(L, c.CountWF(dnaString, L));
                }

                //Добавляем сетку и легенду
                GraphProperties(myPane);

                //Выбираем цвет графика
                Color CurveColor = ColorList[0];

                //Удаляем используемый цвет из списка цветов
                ColorList.Remove(CurveColor);

                //Рисуем график
                LineItem myCurve = myPane.AddCurve(dnaName,
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
        }
        public void DelGraph(ZedGraphControl zgc)//Удаление графика
        {
            GraphPane myPane = zgc.GraphPane;

            // Номер графика для удаления
            int index = 1;
            if (myPane.CurveList.Count <= index) throw new IndexOutOfRangeException("Вы пытались удалить кривую, которой уже нет в списке. Попробуйте еще раз.");

            //Вернем цвет кривой в список
            ColorList.Add(myPane.CurveList[index].Color);

            // Удалим кривую по индексу
            myPane.CurveList.RemoveAt(index);

            // Обновим график
            zgc.AxisChange();
            zgc.Invalidate();
        }

        public static void GraphProperties(GraphPane myPane)//Добавляем свойства графика
        {
            //Положение легенды
            myPane.Legend.Position = LegendPos.Float;//задание положения координатами
            myPane.Legend.Location.CoordinateFrame = CoordType.PaneFraction;//координаты панели
            myPane.Legend.Location.AlignH = AlignH.Right;
            myPane.Legend.Location.AlignV = AlignV.Bottom;
            myPane.Legend.Location.TopLeft = new PointF(0.98f, 0.98f);

            //Добавляем крупную пунктирную сетку по оси X
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.DashOn = 8;
            myPane.XAxis.MajorGrid.DashOff = 4;

            //Добавляем крупную пунктирную сетку по оси Y
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.DashOn = 8;
            myPane.YAxis.MajorGrid.DashOff = 4;

            //Добавляем мелкую пунктирную сетку по оси X
            myPane.XAxis.MinorGrid.IsVisible = true;
            myPane.XAxis.MinorGrid.DashOn = 1;
            myPane.XAxis.MinorGrid.DashOff = 2;
            //Добавляем мелкую пунктирную сетку по оси Y
            myPane.YAxis.MinorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.DashOn = 1;
            myPane.YAxis.MinorGrid.DashOff = 2;
        }
    }
}