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
        public double Angle { get; }

        public BinaryTree(string name, double angle) : base(name)
        {
            Name = name;
            Angle = angle;
        }

        private double lengthScale = 0.7;
        private double angleFract = Math.PI / 5;

        public void DrawBinaryTree(Canvas canvas, int cntDepth, Point pt, double length, double angl, bool angleCheck, IEnumerable<Color> colors, bool gradCheck, double thickness = 1.0, double anglePlus = 1)
        { 
            double x1 = pt.X + length * Math.Cos(angl);
            double y1 = pt.Y + length * Math.Sin(angl);

            System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();

            if (gradCheck)
            {
                Color[] clrs = colors.ToArray();
                SolidColorBrush newBr = new SolidColorBrush(clrs[cntDepth - 1]);
                line.Stroke = newBr;
            } else
            {
                line.Stroke = Brushes.Black;
            }
            line.StrokeThickness = thickness * 0.88;
            thickness *= 0.88;

            line.X1 = pt.X;
            line.Y1 = pt.Y;
            line.X2 = x1;
            line.Y2 = y1;
            canvas.Children.Add(line);

            if (cntDepth > 1 && thickness > 0.05)
            {
                if (!angleCheck)
                {
                    DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl + angleFract, angleCheck, colors, gradCheck, thickness);
                    DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl - angleFract, angleCheck, colors, gradCheck, thickness);
                }
                else
                {
                    if (anglePlus < 0)
                    {
                        DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl + angleFract - anglePlus, angleCheck, colors, gradCheck, thickness);
                        DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl - angleFract - anglePlus, angleCheck, colors, gradCheck, thickness);
                    }
                    else
                    {
                        DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl + angleFract + anglePlus, angleCheck, colors, gradCheck, thickness);
                        DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl - angleFract + anglePlus, angleCheck, colors, gradCheck, thickness);
                    }

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
