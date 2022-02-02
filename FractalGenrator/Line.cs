using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Fractal;

namespace FractalGenrator
{
    public partial class Line : GeneralFractal
    {
        public override string Name { get; }
        
        public Line(string name) : base(name)
        {
            Name = name;
        }

        /// <summary>
        /// Отрисовка линии.
        /// </summary>
        /// <param name="canvas">Канвас</param>
        /// <param name="cntDepth">Глубина рекурсии</param>
        /// <param name="pt">Начальная точка</param>
        /// <param name="lineLength">Длина линии</param>
        /// <param name="lengthTo">Расстояние между последующими итерациями</param>
        /// <param name="colors">Градиент</param>
        /// <param name="gradCheck">Проверка на градиент</param>
        public void DrawLine(Canvas canvas, int cntDepth, Point pt, double lineLength, double lengthTo, IEnumerable<Color> colors, bool gradCheck)
        {
            
            // Получение координат.
            double x1 = pt.X;
            double y1 = pt.Y + lengthTo;
            double x2 = pt.X + lineLength;
            double y2 = y1;

            System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();

            // Раскраска в градиент.
            if (gradCheck)
            {
                Color[] clrs = colors.ToArray();
                SolidColorBrush newBr = new SolidColorBrush(clrs[cntDepth - 1]);
                line.Stroke = newBr;
            }
            else { line.Stroke = Brushes.Black; }

            // Создание новой линии и добавление ее на канвас.
            line.StrokeThickness = 10;
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            canvas.Children.Add(line);

            // Врата в рекурсию. Если линия слишком маленькая, не отрисовывать!
            if (cntDepth > 1 && lineLength > 0.001)
            {
                DrawLine(canvas, cntDepth - 1, new Point(x1, y1), lineLength / 3, lengthTo, colors, gradCheck);
                DrawLine(canvas, cntDepth - 1, new Point(x1 + (lineLength / 3) * 2 , y1), lineLength / 3, lengthTo, colors, gradCheck);
            }
            else return;
        }

    }
}
