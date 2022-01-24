﻿using System;
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
    public partial class Line : GeneralFractal
    {
        public override string Name { get; }

        public Line(string name) : base(name)
        {
            Name = name;
        }

        public void DrawLine(Canvas canvas, int cntDepth, Point pt, double pixelBetween, double lineLength, double lengthTo)
        {
            
            double x1 = pt.X;
            double y1 = pt.Y + lengthTo;
            double x2 = pt.X + lineLength;
            double y2 = y1;

            System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();

            line.Stroke = Brushes.Black;

            line.StrokeThickness = 10;
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            canvas.Children.Add(line);

            if (cntDepth > 1)
            {
                DrawLine(canvas, cntDepth - 1, new Point(x1, y1), pixelBetween, lineLength / 2 - (lineLength / 10), lengthTo);
                DrawLine(canvas, cntDepth - 1, new Point(x1 + lineLength / 2 + (lineLength / 10) , y1), pixelBetween, lineLength / 2 - (lineLength / 10), lengthTo);
            }
            else return;
        }

    }
}