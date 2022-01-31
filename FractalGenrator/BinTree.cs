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
    public partial class BinaryTree : GeneralFractal
    {
        public override string Name { get; }

        public BinaryTree(string name) : base(name)
        {
            Name = name;   
        }

        // Коэффициент приближение.
        private double lengthScale = 0.7;
        // Угол между двух фракталов.
        private double angleFract = Math.PI / 5;
        // Левый и правый угол.
        private double angleCheckedLeft;
        private double angleCheckedRight;

        /// <summary>
        /// Отрисовка Бинарного Дерева.
        /// </summary>
        /// <param name="canvas">Канвас</param>
        /// <param name="cntDepth">Глубина рекурсии</param>
        /// <param name="pt">Начальная точка дерева</param>
        /// <param name="length">Длина ветвей</param>
        /// <param name="angl">Общий угол</param>
        /// <param name="angleCheck">Флаг, проверяющий нужно ли менять углы</param>
        /// <param name="colors">Массив цветов (градиент)</param>
        /// <param name="gradCheck">Флаг, проверяющий надобность градиента</param>
        /// <param name="thickness">Толщина ветвей</param>
        /// <param name="angleL">Угол левой ветви</param>
        /// <param name="angleR">Угол правой ветви</param>
        public void DrawBinaryTree(Canvas canvas, int cntDepth, Point pt, double length, double angl, bool angleCheck, IEnumerable<Color> colors, bool gradCheck, double thickness = 1.0, double angleL = 30, double angleR = 30)
        { 
            // Расположение ветвей.
            double x1 = pt.X + length * Math.Cos(angl);
            double y1 = pt.Y + length * Math.Sin(angl);

            System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();

            // Градиент.
            if (gradCheck)
            {
                Color[] clrs = colors.ToArray();

                // newBr - newBrush. Цвет, полученный из массива.
                SolidColorBrush newBr = new SolidColorBrush(clrs[cntDepth - 1]);
                line.Stroke = newBr;
            } else
            {
                line.Stroke = Brushes.Black;
            }
            
            // Толщина ветвей.
            line.StrokeThickness = thickness * 0.88;
            thickness *= 0.88;

            // Перевод градусов в радианы.
            angleCheckedLeft = angleL * Math.PI / 180;
            angleCheckedRight = angleR * Math.PI / 180;

            line.X1 = pt.X;
            line.Y1 = pt.Y;
            line.X2 = x1;
            line.Y2 = y1;
            canvas.Children.Add(line);

            // Врата в рекурсию!!!
            // Если ветвь слишком тонкая, перестать отрисовывать.
            if (cntDepth > 1 && thickness > 0.01)
            {

                // Если угол был изменен, то использовать новые углы при отрисовки дерева.
                if (!angleCheck)
                {
                    DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl + angleFract, angleCheck, colors, gradCheck, thickness, angleL, angleR);
                    DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl - angleFract, angleCheck, colors, gradCheck, thickness, angleL, angleR);
                }
                else
                {
                    DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl + angleCheckedRight, true, colors, gradCheck, thickness, angleL, angleR);
                    DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl - angleCheckedLeft, true, colors, gradCheck, thickness, angleL, angleR);
                }
            }
            else
            {
                cntDepth -= 1;
                return;
            }
        }

    }
}
