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
    class Triangle : GeneralFractal
    {
        /// <summary>
        /// Название фрактала.
        /// </summary>
        public override string Name { get; }

        /// <summary>
        /// Конструктор для инициализации треугольника.
        /// </summary>
        /// <param name="name"></param>
        public Triangle(string name) : base(name)
        {
            Name = name;
        }

        /// <summary>
        /// Создание нового треугольника.
        /// </summary>
        /// <param name="top">Вершина</param>
        /// <param name="right">Правая точка основания</param>
        /// <param name="left">Левая точка основания</param>
        /// <param name="pol">Полигон</param>
        /// <param name="colors">Градиент</param>
        /// <param name="step">Шаг рекурсии (для градиента) </param>
        /// <param name="gradCheck">Проверка на градиент</param>
        /// <returns></returns>
        private Polygon CreateTriangle(Point top, Point right, Point left, Polygon pol, Color[] colors, int step, bool gradCheck) 
        {
            SolidColorBrush newBr = new SolidColorBrush(colors[step]);
            pol.Points.Add(top);
            pol.Points.Add(right);
            pol.Points.Add(left);

            if (gradCheck == true)
            {
                pol.Stroke = newBr;
            } else
            {
                pol.Stroke = Brushes.Black;
            }
            return pol;
        }

        /// <summary>
        /// Отрисовка труегольника Серпинского.
        /// </summary>
        /// <param name="canvas">Канвас</param>
        /// <param name="depthCnt">Глубина рекурсии</param>
        /// <param name="triangle">Треугольник (полигон)</param>
        /// <param name="iteration">Шаг рекурсии</param>
        /// <param name="gradient">Градиент</param>
        /// <param name="gradCheck">Проверка на градиент</param>
        public void DrawTriangle(Canvas canvas, int depthCnt, Polygon triangle, int iteration, IEnumerable<Color> gradient, bool gradCheck)
        {
            Color[] colors = gradient.ToArray();

            // При нулевой итерации отрисовка начального треугольника
            if (iteration == 0)
            {
                Polygon firstTria = new();
                firstTria.Points.Clear();
                firstTria.Stroke = Brushes.Black;
                Point top = new(canvas.Width / 2d, 0);
                Point right = new(canvas.Width, canvas.Height);
                Point left = new(0, canvas.Height);
                triangle = CreateTriangle(top, right, left, firstTria, colors, iteration, gradCheck);
                canvas.Children.Add(firstTria);

                // Вход в рекурсию.
                DrawTriangle(canvas, depthCnt, triangle, iteration + 1, gradient, gradCheck);
            } else
            {
                // Условия для выхода из рекурсии.
                if (iteration < depthCnt)
                {
                    Point[] arrPoint = new Point[3];
                    int cnt = 0;

                    // Получение массива точек.
                    foreach (var point in triangle.Points)
                    {
                        arrPoint[cnt++] = point;
                    }

                    // Получение центральных точек всех сторон треугольника
                    Point leftMid = new((arrPoint[0].X + arrPoint[2].X) / 2d, (arrPoint[0].Y + arrPoint[2].Y) / 2d);
                    Point rightMid = new((arrPoint[0].X + arrPoint[1].X) / 2d, (arrPoint[0].Y + arrPoint[1].Y) / 2d);
                    Point bottomMid = new((arrPoint[1].X + arrPoint[2].X) / 2d, (arrPoint[1].Y + arrPoint[2].Y) / 2d);

                    // Построение треугольника.
                    Polygon finalTria = CreateTriangle(leftMid, rightMid, bottomMid, new Polygon(), colors, iteration, gradCheck);
                    canvas.Children.Add(finalTria);

                    // Передаём оставшиеся труегольники в этот же метод.
                    DrawTriangle(canvas, depthCnt, CreateTriangle(arrPoint[0], leftMid, rightMid, new Polygon(), colors, iteration, gradCheck), iteration + 1, gradient, gradCheck);
                    DrawTriangle(canvas, depthCnt, CreateTriangle(leftMid, arrPoint[2], bottomMid, new Polygon(), colors, iteration, gradCheck), iteration + 1, gradient, gradCheck);
                    DrawTriangle(canvas, depthCnt, CreateTriangle(rightMid, bottomMid, arrPoint[1], new Polygon(), colors, iteration, gradCheck), iteration + 1, gradient, gradCheck);

                }
                else return;
            }
        }
    }
}
