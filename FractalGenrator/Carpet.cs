using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class Carpet : GeneralFractal
    {
        public override string Name { get; }

        Polyline pl = new Polyline();
        public int startDepth { get; }

        public Carpet(string name, int depth) : base(name)
        {
            Name = name;
            startDepth = depth;
        }

        private Polygon FillPoly(double x0, double width, double y0, double height)
        {
            Polygon pol = new();
            pol.Fill = Brushes.White;
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

        public void DrawCarpet(Canvas canvas, int depth, Polygon rect, int iteration)
        {

            if (iteration == 0)
            {
                Polygon firstRect = new Polygon();
                
                firstRect.Fill = Brushes.Blue;
                firstRect.Points.Add(new(0, 0));
                firstRect.Points.Add(new(canvas.Width, 0));
                firstRect.Points.Add(new(canvas.Width, canvas.Height));
                firstRect.Points.Add(new(0, canvas.Height));
                canvas.Children.Add(firstRect);
                DrawCarpet(canvas, depth, firstRect, iteration + 1);
            }
            else
            {
                if (iteration < depth)
                { 
                    int cnt = 0;
                    Point[] ponpon = new Point[4];

                    foreach (var point in rect.Points)
                    {
                        ponpon[cnt++] = point;
                    }
                    double width = ponpon[1].X - ponpon[0].X;
                    double height = ponpon[3].Y - ponpon[0].Y;
                    
                    if (width < 1.0)
                    {
                        return;
                    }

                    width /= 3d;
                    height /= 3d;

                    double x0 = ponpon[0].X;
                    double x1 = x0 + width;
                    double x2 = x0 + width * 2d;

                    double y0 = ponpon[0].Y;
                    double y1 = y0 + height;
                    double y2 = y0 + height * 2d;


                    Polygon pol0_0 = FillPoly(x0, width, y0, height);
                    Polygon pol1_0 = FillPoly(x1, width, y0, height);
                    Polygon pol2_0 = FillPoly(x2, width, y0, height);
                    Polygon pol0_1 = FillPoly(x0, width, y1, height);
                    Polygon pol1_1 = FillPoly(x1, width, y1, height); // white
                    Polygon pol2_1 = FillPoly(x2, width, y1, height);
                    Polygon pol0_2 = FillPoly(x0, width, y2, height);
                    Polygon pol1_2 = FillPoly(x1, width, y2, height);
                    Polygon pol2_2 = FillPoly(x2, width, y2, height);

                    canvas.Children.Add(pol1_1);
 
                    DrawCarpet(canvas, depth, pol0_0, iteration + 1);
                    DrawCarpet(canvas, depth, pol1_0, iteration + 1);
                    DrawCarpet(canvas, depth, pol2_0, iteration + 1);
                    DrawCarpet(canvas, depth, pol0_1, iteration + 1);
                    DrawCarpet(canvas, depth, pol2_1, iteration + 1);
                    DrawCarpet(canvas, depth, pol0_2, iteration + 1);
                    DrawCarpet(canvas, depth, pol1_2, iteration + 1);
                    DrawCarpet(canvas, depth, pol2_2, iteration + 1);


                }
                else
                {
                    return;
                }
            }
        }
    }
}


