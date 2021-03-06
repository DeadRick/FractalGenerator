using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Fractal;

namespace FractalGenrator
{
    public partial class Carpet : GeneralFractal
    {
        public override string Name { get; }

        public Carpet(string name) : base(name)
        {
            Name = name;
        }

        /// <summary>
        /// Создание нового квадрата.
        /// </summary>
        /// <param name="x0">Координаты по оси Х</param>
        /// <param name="width">Длина квадрата</param>
        /// <param name="y0">Координаты по оси Y</param>
        /// <param name="height">Высота квадрата</param>
        /// <param name="colors">Список цветов (градиент)</param>
        /// <param name="step"> Шаг рекурсии (для градиента)</param>
        /// <param name="gradCheck">Проверка на градиент</param>
        /// <returns></returns>
        private Polygon FillPoly(double x0, double width, double y0, double height, Color[] colors, int step, bool gradCheck)
        {
            Polygon pol = new();
            // Смена цвета (для градиента), если он нужен.
            if (gradCheck)
            {
                SolidColorBrush newBr = new SolidColorBrush(colors[step - 1]);
                pol.Fill = newBr;
            } else
            {
                pol.Fill = Brushes.AntiqueWhite;
            }
            
            // Создание квадрата по координатам с последующим добавлением в полигон.
            Point point00 = new Point(x0, y0);
            Point point01 = new Point(x0 + width, y0);
            Point point11 = new Point(x0 + width, y0 + height);
            Point point10 = new Point(x0, y0 + height);
            pol.Points.Add(point00);
            pol.Points.Add(point01);
            pol.Points.Add(point11);
            pol.Points.Add(point10);

            return pol;
        }

        /// <summary>
        /// Отрисовка ковра Серпинского.
        /// </summary>
        /// <param name="canvas">Канвас</param>
        /// <param name="depth">Глубина рекурсии</param>
        /// <param name="rect">Квадрат (полигон)</param>
        /// <param name="iteration">Шаг рекурсии</param>
        /// <param name="colors">Градиент</param>
        /// <param name="gradCheck">Проверка на градиент</param>
        public void DrawCarpet(Canvas canvas, int depth, Polygon rect, int iteration, IEnumerable<Color> colors, bool gradCheck)
        {
            // При первой итерации (нулевой), отрисовка одного квадарата на весь фон канваса.
            if (iteration == 0)
            {
                Polygon firstRect = new Polygon();
                
                // Фон ковра Серпинского - синий.
                firstRect.Fill = Brushes.Blue;
                firstRect.Points.Add(new(0, 0));
                firstRect.Points.Add(new(canvas.Width, 0));
                firstRect.Points.Add(new(canvas.Width, canvas.Height));
                firstRect.Points.Add(new(0, canvas.Height));

                // Добавления квадрата на канвас.
                canvas.Children.Add(firstRect);

                // Вход в рекурсию с прибавлением итерации для входа в другое условие.
                DrawCarpet(canvas, depth, firstRect, iteration + 1, colors, gradCheck);
            }
            else
            {
                if (iteration < depth)
                {
                    Color[] colorsArray = colors.ToArray();
                    Point[] pointArr = new Point[4];

                    // Заполнение массива точек квадрата.
                    int cnt = 0;
                    foreach (var point in rect.Points)
                    {
                        pointArr[cnt++] = point;
                    }

                    // Получение ширины и высоты квадрата (на всякий случай, хотя высота не так и нужна).
                    double width = pointArr[1].X - pointArr[0].X;
                    double height = pointArr[3].Y - pointArr[0].Y;

                    // Деление высоты и ширины на 3.        XY  .   X . .   X . .
                    width /= 3d;                //          . . .   . . .   . . .
                    height /= 3d;               //          . . .   . . .   . . .
                                                //          
                    // Получение разметки во всех секторах: Y . .   X . .   X . .
                    double x0 = pointArr[0].X;  //          . . .   . . .   . . .
                    double x1 = x0 + width;     //          . . .   . . .   . . .
                    double x2 = x0 + width * 2d;//              
                                                //          Y . .   X . .   X . .
                    double y0 = pointArr[0].Y;  //          . . .   . . .   . . .
                    double y1 = y0 + height;    //          . . .   . . .   . . .
                    double y2 = y0 + height * 2d;//

                    // Полигоны разбил на координаты.
                    Polygon pol0_0 = FillPoly(x0, width, y0, height, colorsArray, iteration, gradCheck);
                    Polygon pol1_0 = FillPoly(x1, width, y0, height, colorsArray, iteration, gradCheck);
                    Polygon pol2_0 = FillPoly(x2, width, y0, height, colorsArray, iteration, gradCheck);
                    Polygon pol0_1 = FillPoly(x0, width, y1, height, colorsArray, iteration, gradCheck);
                    // Белый Квадрат.
                    Polygon pol1_1 = FillPoly(x1, width, y1, height, colorsArray, iteration, gradCheck);

                    Polygon pol2_1 = FillPoly(x2, width, y1, height, colorsArray, iteration, gradCheck);
                    Polygon pol0_2 = FillPoly(x0, width, y2, height, colorsArray, iteration, gradCheck);
                    Polygon pol1_2 = FillPoly(x1, width, y2, height, colorsArray, iteration, gradCheck);
                    Polygon pol2_2 = FillPoly(x2, width, y2, height, colorsArray, iteration, gradCheck);

                    // Добавление белого квадрата в центр.
                    canvas.Children.Add(pol1_1);
 
                    // Отрисовка остальных квадртов, то есть переход в другие сектора для отрисовки белого квадрата в центре.
                    DrawCarpet(canvas, depth, pol0_0, iteration + 1, colors, gradCheck);
                    DrawCarpet(canvas, depth, pol1_0, iteration + 1, colors, gradCheck);
                    DrawCarpet(canvas, depth, pol2_0, iteration + 1, colors, gradCheck);
                    DrawCarpet(canvas, depth, pol0_1, iteration + 1, colors, gradCheck);
                    DrawCarpet(canvas, depth, pol2_1, iteration + 1, colors, gradCheck);
                    DrawCarpet(canvas, depth, pol0_2, iteration + 1, colors, gradCheck);
                    DrawCarpet(canvas, depth, pol1_2, iteration + 1, colors, gradCheck);
                    DrawCarpet(canvas, depth, pol2_2, iteration + 1, colors, gradCheck);
                }
                else
                {
                    return;
                }
            }
        }
    }
}


