﻿using System;
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
        //private void DrawRectangle(Canvas gr, int depth, Polygon rect)
        //public void CarpetEdge(Canvas canvas, int cntDepth, double distance)
        //{
        //    Point pt = new Point();
        //    if (cntDepth > 0)
        //    {

        //    }
        //}
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

        public void DrawCarpet(Canvas canvas, int depth, Polygon rect)
        {
            
            if (depth == 1)
            {
                rect.Fill = Brushes.Blue;
                rect.Points.Add(new(0, 0));
                rect.Points.Add(new(500, 0));
                rect.Points.Add(new(500, 500));
                rect.Points.Add(new(0, 500));
                canvas.Children.Add(rect);
                DrawCarpet(canvas, depth + 1, rect);
            }
            else
            {
                if (depth <= startDepth)
                {
                    //rect.Fill = Brushes.Blue;
                    //rect.Points.Add(new(0, 0));
                    //rect.Points.Add(new(500, 0));
                    //rect.Points.Add(new(500, 500));
                    //rect.Points.Add(new(0, 500));
                    //canvas.Children.Add(rect);
                    
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

                    

                    //canvas.Children.Add(pol0_0);
                    canvas.Children.Add(pol1_1);
                    //canvas.Children.Add(pol1_0);
                    //canvas.Children.Add(pol2_0);
                    //canvas.Children.Add(pol0_1);
                    //canvas.Children.Add(pol2_1);
                    //canvas.Children.Add(pol0_2);
                    //canvas.Children.Add(pol1_2);
                    //canvas.Children.Add(pol2_2);
                    DrawCarpet(canvas, depth + 1, pol0_0);
                    DrawCarpet(canvas, depth + 1, pol1_0);
                    DrawCarpet(canvas, depth + 1, pol2_0);
                    DrawCarpet(canvas, depth + 1, pol0_1);
                    DrawCarpet(canvas, depth + 1, pol2_1);
                    DrawCarpet(canvas, depth + 1, pol0_2);
                    DrawCarpet(canvas, depth + 1, pol1_2);
                    DrawCarpet(canvas, depth + 1, pol2_2);


                }
                else
                {
                    return;
                }

                //    double x0 = newPointArr[0].X;
                //    double x1 = x0 + width;
                //    double x2 = x0 + width * 2d;

                //    double y0 = newPointArr[0].Y;
                //    double y1 = y0 + height;
                //    double y2 = y0 + height * 2d;

                //    Polyline c_1_1 = FillPoly(x0, width, y0, height);
                //    Polyline c_2_1 = FillPoly(x1, width, y0, height);
                //    Polyline c_3_1 = FillPoly(x2, width, y0, height);
                //    Polyline c_1_2 = FillPoly(x0, width, y1, height);
                //    Polyline c_3_2 = FillPoly(x1, width, y1, height);
                //    Polyline c_1_3 = FillPoly(x0, width, y2, height);
                //    Polyline c_2_3 = FillPoly(x1, width, y2, height);
                //    Polyline c_3_3 = FillPoly(x2, width, y2, height);

                //    // ■ ■ ■
                //    // □ □ □
                //    // □ □ □

                //    DrawCarpet(canvas, depth - 1, c_1_1);
                //    DrawCarpet(canvas, depth - 1, c_2_1);
                //    DrawCarpet(canvas, depth - 1, c_3_1);

                //    // ■ ■ ■
                //    // ■ □ ■
                //    // □ □ □

                //    DrawCarpet(canvas, depth - 1, c_1_2);
                //    DrawCarpet(canvas, depth - 1, c_3_2);

                //    // ■ ■ ■
                //    // ■ □ ■
                //    // ■ ■ ■

                //    DrawCarpet(canvas, depth - 1, c_1_3);
                //    DrawCarpet(canvas, depth - 1, c_2_3);
                //    DrawCarpet(canvas, depth - 1, c_3_3);

            }
            //    Point[] newPointArr = new Point[4];
            //    Polygon ne = new();

            //    //int cnt;



            //    rect.Points.Add(new(1, 1));
            //    rect.Points.Add(new(10, 13));


            //    if (depth == startDepth)
            //    {
            //        
            //    }
            //    else if (depth > 0) 
            //        {
            //        //newPointArr[cnt++] = point;
            //    }

            //    double width = newPointArr[1].X - newPointArr[0].X;
            //    double height = newPointArr[1].Y - newPointArr[1].Y;

            //    width /= 3d;
            //    height /= 3d;

            //    double x0 = newPointArr[0].X;
            //    double x1 = x0 + width;
            //    double x2 = x0 + width * 2d;

            //    double y0 = newPointArr[0].Y;
            //    double y1 = y0 + height;
            //    double y2 = y0 + height * 2d;

            //    Polyline c_1_1 = FillPoly(x0, width, y0, height);
            //    Polyline c_2_1 = FillPoly(x1, width, y0, height);
            //    Polyline c_3_1 = FillPoly(x2, width, y0, height);
            //    Polyline c_1_2 = FillPoly(x0, width, y1, height);
            //    Polyline c_3_2 = FillPoly(x1, width, y1, height);
            //    Polyline c_1_3 = FillPoly(x0, width, y2, height);
            //    Polyline c_2_3 = FillPoly(x1, width, y2, height);
            //    Polyline c_3_3 = FillPoly(x2, width, y2, height);

            //    // ■ ■ ■
            //    // □ □ □
            //    // □ □ □

            //    DrawCarpet(canvas, depth - 1, c_1_1);
            //    DrawCarpet(canvas, depth - 1, c_2_1);
            //    DrawCarpet(canvas, depth - 1, c_3_1);

            //    // ■ ■ ■
            //    // ■ □ ■
            //    // □ □ □

            //    DrawCarpet(canvas, depth - 1, c_1_2);
            //    DrawCarpet(canvas, depth - 1, c_3_2);

            //    // ■ ■ ■
            //    // ■ □ ■
            //    // ■ ■ ■

            //    DrawCarpet(canvas, depth - 1, c_1_3);
            //    DrawCarpet(canvas, depth - 1, c_2_3);
            //    DrawCarpet(canvas, depth - 1, c_3_3);
            //}



            //Point[] pointArr = new Point[4];
            //pointArr[0] = new Point(pt.X, pt.Y);
            //pointArr[1] = new Point(width, pt.Y);
            //pointArr[2] = new Point(width, width);
            //pointArr[3] = new Point(pt.X, width);

            //if (depth > 1)
            //{
            //    Rectangle recti = new Rectangle();
            //    canvas.Left
            //    double width = rect.Width / 3d;

            //    double x0 = rect.X;
            //    double x1 = x0 + width;
            //    double x2 = x0 + width * 2d;

            //    double height = rect.Height / 3d;

            //    double y0 = rect.Y;
            //    double y1 = y0 + width;
            //    double y2 = y0 + width * 2d;
            //    canvas.Children.Add(rect);
            //    DrawRectangle();
            //}

            //pl.Points.Add(pointArr[0]);
            //for (int j = 1; j < pointArr.Length; j++)
            //{
            //    double x1 = pointArr[j - 1].X;
            //    double y1 = pointArr[j - 1].Y;
            //    double x2 = pointArr[j].X;
            //    double y2 = pointArr[j].Y;
            //    CarpetPoint = new Point(x1, y1);
            //    //CarpetEdge(canvas, depth, length);
            //}

            //// See if we should stop.
            //if (level == 0)
            //{
            //    // Fill the rectangle.
            //    gr.FillRectangle(Brushes.Blue, rect);
            //}
            //else
            //{
            //    // Divide the rectangle into 9 pieces.
            //    float wid = rect.Width / 3f;
            //    float x0 = rect.Left;
            //    float x1 = x0 + wid;
            //    float x2 = x0 + wid * 2f;

            //    float hgt = rect.Height / 3f;
            //    float y0 = rect.Top;
            //    float y1 = y0 + hgt;
            //    float y2 = y0 + hgt * 2f;

            //    // Recursively draw smaller carpets.
            //    DrawRectangle(gr, level - 1, new RectangleF(x0, y0, wid, hgt));
            //    DrawRectangle(gr, level - 1, new RectangleF(x1, y0, wid, hgt));
            //    DrawRectangle(gr, level - 1, new RectangleF(x2, y0, wid, hgt));
            //    DrawRectangle(gr, level - 1, new RectangleF(x0, y1, wid, hgt));
            //    DrawRectangle(gr, level - 1, new RectangleF(x2, y1, wid, hgt));
            //    DrawRectangle(gr, level - 1, new RectangleF(x0, y2, wid, hgt));
            //    DrawRectangle(gr, level - 1, new RectangleF(x1, y2, wid, hgt));
            //    DrawRectangle(gr, level - 1, new RectangleF(x2, y2, wid, hgt));
            //}
        }
    }
}


