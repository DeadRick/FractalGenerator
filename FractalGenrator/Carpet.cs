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
    public partial class Carpet : GeneralFractal
    {
        public override string Name { get; }

        public Carpet(string name) : base(name)
        {
            Name = name;
        }
        
        public void DrawCarpet(Canvas canvas, int cntDepth, Polygon rect, double scale, double length)
        {
            var Points = rect.Points;
            foreach (Point pt in Points)
            {

            }
        }
    }
}
