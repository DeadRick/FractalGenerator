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
            if (frames % 10 == 0)
            {
                DrawBinaryTree(canvas1, depth, new Point(canvas1.Width * 0.25, canvas1.Height * 0.77), 0.2 * canvas1.Width, 3 * Math.PI / 2, 3);
                string str = "Binary Tree. Depth = " + depth.ToString();
                tbLabel.Text = str;
                depth += 1;
                if (depth > 10)
                {
                    tbLabel.Text = "Binary SOSNA. Depth = 10. Finished";
                    CompositionTarget.Rendering -= StartAnimation;
                }
            }
        }
        private double lengthScale = 0.75;
        private double angleFract = Math.PI / 5;
        private void DrawBinaryTree(Canvas canvas, int depth, Point pt, double length, double angl, double thickness = 1.0)
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

            if (depth > 1)
            {
                DrawBinaryTree(canvas, depth - 1, new Point(x1, y1), length * lengthScale, angl + angleFract + 0.5, thickness);
                DrawBinaryTree(canvas, depth - 1, new Point(x1, y1), length * lengthScale, angl - angleFract + 0.7, thickness);
            }
            else return;
        }

        private void Viewbox_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}

