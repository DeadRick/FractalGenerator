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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FractalGenrator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int depth = 0;
        private int frames = 0;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            tbLabel.Text = "";
            frames = 0;
            depth = 1;
            CompositionTarget.Rendering += StartAnimation;
        }
        private void StartAnimation(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % 60 == 0)
            {
                DrawBinaryTree(canvas1, depth, new Point(canvas1.Width / 2, canvas1.Height * 0.95), 0.2 * canvas1.Width, 3 * Math.PI / 2);
                string str = "Binary Tree. Depth = " + depth.ToString();
                tbLabel.Text = str;
                depth += 1;
                if (depth > 15)
                {
                    tbLabel.Text = "Binary Tree. Depth = 15. Finished";
                    CompositionTarget.Rendering -= StartAnimation;
                }
            }
        }
        private double lengthScale = 0.75;
        private double radiusFract = Math.PI / 3;
        private void DrawBinaryTree(Canvas canvas, int depth, Point pt, double length, double rad)
        {
            double x1 = pt.X + length * Math.Cos(rad);
            double y1 = pt.Y + length * Math.Sin(rad);

            Line line = new Line();

            line.Stroke = Brushes.Black;
            line.StrokeThickness = 0.2;
            line.X1 = pt.X;
            line.Y1 = pt.Y;
            line.X2 = x1;
            line.Y2 = y1;
            canvas.Children.Add(line);

            if (depth > 1)
            {
                DrawBinaryTree(canvas, depth - 1, new Point(x1, y1), length * lengthScale, rad + radiusFract);
                DrawBinaryTree(canvas, depth - 1, new Point(x1, y1), length * lengthScale, rad - radiusFract * 0.5);
            }
            else return;
        }
    }
}

