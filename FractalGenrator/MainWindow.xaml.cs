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
using Fractal;

namespace FractalGenrator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private int depth = 10;
        private int cntDepth = 0;
        private int frames = 0;
        BinaryTree tree = new("Binary Tree");


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            tbLabel.Text = "";
            cntDepth = 1;
            frames = 0;
            CompositionTarget.Rendering += StartAnimation;
        }
        private void StartAnimation(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % 10 == 0)
            {
                tree.DrawBinaryTree(canvas1, cntDepth, new Point(canvas1.Width / 2, canvas1.Height * 0.77), 0.2 * canvas1.Width, 3 * Math.PI / 2, 3);
                string str = $"{tree.Name}. Depth = " + cntDepth.ToString();
                tbLabel.Text = str;
                cntDepth += 1;
                if (cntDepth > depth)
                {
                    tbLabel.Text = $"{tree.Name}. Depth = {depth}. Finished";
                    CompositionTarget.Rendering -= StartAnimation;
                }
            }
        }

        public partial class BinaryTree : GeneralFractal
        {
            public override string Name { get; }
            public BinaryTree(string name) : base(name)
            {
                Name = name;
            }

            private double lengthScale = 0.8;
            private double angleFract = Math.PI / 4;
            public void DrawBinaryTree(Canvas canvas, int cntDepth, Point pt, double length, double angl, double thickness = 1.0)
            {
                double x1 = pt.X + length * Math.Cos(angl);
                double y1 = pt.Y + length * Math.Sin(angl);

                Line line = new Line();

                line.Stroke = Brushes.Black;
                line.StrokeThickness = thickness * 0.88;
                thickness *= 0.88;
                line.X1 = pt.X;
                line.Y1 = pt.Y;
                line.X2 = x1;
                line.Y2 = y1;
                canvas.Children.Add(line);

                if (cntDepth > 1)
                {
                    DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl + angleFract, thickness);
                    DrawBinaryTree(canvas, cntDepth - 1, new Point(x1, y1), length * lengthScale, angl - angleFract, thickness);
                }
                else return;
            }

        }

        
        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            DepthWindow depthWindow = new();

            if (depthWindow.ShowDialog() == true)
            {
                if (int.TryParse(depthWindow.DepthText, out int depthFromWindow))
                {
                    depth = depthFromWindow;
                    MessageBox.Show("Depth was succesfully changed");
                }
                
            }
            else
            {
                MessageBox.Show("Depth was not selected!");
            }

        }
    }
}

