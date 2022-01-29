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
        public override string Name { get; }

        public Triangle(string name) : base(name)
        {
            Name = name;
        }

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
        public void DrawTriangle(Canvas canvas, int depthCnt, Polygon triangle, int iteration, IEnumerable<Color> gradient, bool gradCheck)
        {
            Color[] colors = gradient.ToArray();
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

                DrawTriangle(canvas, depthCnt, triangle, iteration + 1, gradient, gradCheck);
            } else
            {
                if (iteration < depthCnt)
                {
                    Point[] tria = new Point[3];
                    int cnt = 0;

                    foreach (var point in triangle.Points)
                    {
                        tria[cnt++] = point;
                    }


                    Point leftMid = new((tria[0].X + tria[2].X) / 2d, (tria[0].Y + tria[2].Y) / 2d);
                    Point rightMid = new((tria[0].X + tria[1].X) / 2d, (tria[0].Y + tria[1].Y) / 2d);
                    Point bottomMid = new((tria[1].X + tria[2].X) / 2d, (tria[1].Y + tria[2].Y) / 2d);

                    Polygon finalTria = CreateTriangle(leftMid, rightMid, bottomMid, new Polygon(), colors, iteration, gradCheck);

                    canvas.Children.Add(finalTria);

                    DrawTriangle(canvas, depthCnt, CreateTriangle(tria[0], leftMid, rightMid, new Polygon(), colors, iteration, gradCheck), iteration + 1, gradient, gradCheck);
                    DrawTriangle(canvas, depthCnt, CreateTriangle(leftMid, tria[2], bottomMid, new Polygon(), colors, iteration, gradCheck), iteration + 1, gradient, gradCheck);
                    DrawTriangle(canvas, depthCnt, CreateTriangle(rightMid, bottomMid, tria[1], new Polygon(), colors, iteration, gradCheck), iteration + 1, gradient, gradCheck);

                }
                else return;
            }
        }
    }
}
