using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Fractal;

namespace FractalGenrator
{
    public partial class Flake : GeneralFractal
    {
        public override string Name { get; }

        // Начальная точка для снежинки.
        private Point SnowflakePoint = new Point();
        Polyline pl = new Polyline();

        // Углы для правильной сборки снежинки ( в треугольник ).
        double[] dAngles = new double[4] { 0, Math.PI / 3, -2 * Math.PI / 3, Math.PI / 3 };

        // Коэффициент приближения.
        private double distanceScale = 1.0 / 3;

        /// <summary>
        /// В конструкторе название фрактала и также Polyline.
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="Pline">Polyline</param>
        public Flake(string name, ref Polyline Pline) : base(name)
        {
            Name = name;
            pl = Pline;
        }

        /// <summary>
        /// Основная рекурсия для отрисовки снежинки.
        /// </summary>
        /// <param name="canvas">Канвас</param>
        /// <param name="depth">Глубина рекурсии</param>
        /// <param name="theta">Угл</param>
        /// <param name="distance">Приближение</param>
        private void SnowFlakeEdge(Canvas canvas, int depth, double theta, double distance)
        {
            Point pt = new Point();
           
            // Оптимизация отрисовки, если приближение меньше 0.5, то перестать отрисовывать.
            if ((depth <= 0) || (distance <= 0.5))
            {
                pt.X = SnowflakePoint.X + distance * Math.Cos(theta);
                pt.Y = SnowflakePoint.Y + distance * Math.Sin(theta);
                pl.Points.Add(pt);
                SnowflakePoint = pt;
                return;
            }

            // Перебор для всех сторон.
            distance *= distanceScale;
            for (int j = 0; j < 4; j++)
            {
                theta += dAngles[j];
                SnowFlakeEdge(canvas, depth - 1, theta, distance);
            }
        }
        
        /// <summary>
        /// Начало отрисовки снежинки.
        /// </summary>
        /// <param name="canvas">Канвас</param>
        /// <param name="length">Длина</param>
        /// <param name="depth">Глубина рекурсии</param>
        /// <param name="colors">Цвета градиента</param>
        /// <param name="gradCheck">Проверка на градиент</param>
        public void DrawSnowFlake(Canvas canvas, double length, int depth, IEnumerable<Color> colors, bool gradCheck)
        {
            // Получаем центр канваса. 
            double xmid = canvas.Width / 2;
            double ymid = canvas.Height / 2;

            // Создание новых углов.
            Point[] pta = new Point[4];
            pta[0] = new Point(xmid, ymid + length / 2 * Math.Sqrt(3) * 2 / 3);
            pta[1] = new Point(xmid + length / 2, ymid - length / 2 * Math.Sqrt(3) / 3);
            pta[2] = new Point(xmid - length / 2, ymid - length / 2 * Math.Sqrt(3) / 3);
            pta[3] = pta[0];

            // Перекрашивание в градиент.
            if (gradCheck)
            {
                Color[] clrs = colors.ToArray();
                SolidColorBrush newbr = new SolidColorBrush(clrs[depth - 1]);
                pl.Stroke = newbr;
            }
            else
            {
                pl.Stroke = Brushes.Black;
            }

            // Вход в рекурсии.
            pl.Points.Add(pta[0]);
            for (int j = 1; j < pta.Length; j++)
            {
                double x1 = pta[j - 1].X;
                double y1 = pta[j - 1].Y;
                double x2 = pta[j].X;
                double y2 = pta[j].Y;
                double dx = x2 - x1;
                double dy = y2 - y1;
                double theta = Math.Atan2(dy, dx);
                SnowflakePoint = new Point(x1, y1);
                SnowFlakeEdge(canvas, depth, theta, length);
            }
        }
    }
}
