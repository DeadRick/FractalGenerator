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
using System.IO;
using Fractal;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;

namespace FractalGenrator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Line line = new("Line");
        Flake flake = new("Flake", ref pl);
        BinaryTree tree = new("Binary Tree", angle);
        Carpet carpet = new("Carpet", depth);
        Triangle triangle = new("Triangle");

        static SolidColorBrush startGrad;
        static SolidColorBrush endGrad;

        IEnumerable<Color> gradient;
        static Polyline pl = new();
        static Polygon pol = new();
        bool flagTree = false;
        bool flagFlake = false;
        bool flagLine = false;
        bool flagCarpet = false;
        bool flagTriangle = false;
        bool saveCheck = false;
        bool gradientCheck = false;
        public bool angleCheck = false;
        private static double angle = Math.PI / 5;
        private double SnowflakeSize;
        private static int depth = 10;
        private int cntDepth = 0;
        private int frames = 0;
        double[] dTheta = new double[4] { 0, Math.PI / 3, -2 * Math.PI / 3, Math.PI / 3 };

        public MainWindow()
        {
            InitializeComponent();
            Width = System.Windows.SystemParameters.PrimaryScreenWidth / 2;
            Height = System.Windows.SystemParameters.PrimaryScreenHeight / 2;
            Title = "Fractals";

            startColor.SelectedBrush = Brushes.Yellow;
            endColor.SelectedBrush = Brushes.Blue;

            startGrad = startColor.SelectedBrush;
            endGrad = endColor.SelectedBrush;

            gradient = GetGradients(startGrad.Color, endGrad.Color, depth);

            double ysize = 0.8 * canvas1.Height / (Math.Sqrt(3) * 4 / 3);
            double xsize = 0.8 * canvas1.Width / 2;
            double size = 0;
            if (ysize < xsize)
                size = ysize;
            else
                size = xsize;
            SnowflakeSize = 2 * size;
            pl.Stroke = Brushes.Black;
        }



        //int cntIter = 10;
        //private void ColorChange()
        //{

        //    IEnumerable<Color> col = GetGradients(Brushes.Black.Color, Brushes.Blue.Color, cntIter);
        //    foreach (var item in col)
        //    {
        //        Thread.Sleep(60);
        //        tbLabel.Text += item.ToString();
        //        SolidColorBrush newBr = new SolidColorBrush(item);
        //        cube.Fill = newBr;
        //    }


        //}

        public static IEnumerable<Color> GetGradients(Color start, Color end, int steps)
        {
            if (steps == 1)
            {
                steps += 2;
            }
            int stepA = ((end.A - start.A) / (steps - 1));
            int stepR = ((end.R - start.R) / (steps - 1));
            int stepG = ((end.G - start.G) / (steps - 1));
            int stepB = ((end.B - start.B) / (steps - 1));

            for (int i = 0; i < steps; i++)
            {
                yield return Color.FromArgb((byte)(start.A + (stepA * i)),
                                            (byte)(start.R + (stepR * i)),
                                            (byte)(start.G + (stepG * i)),
                                            (byte)(start.B + (stepB * i)));
            }
        }


        // Buttons

        private void ClickSettings(bool flagCheck)
        {
            UnfollowAll();
            flagCheck = true;
            saveCheck = true;
            gradient = GetGradients(startColor.SelectedBrush.Color, endColor.SelectedBrush.Color, depth);
            canvas1.Children.Clear();
            tbLabel.Text = "";
            frames = 0;
            cntDepth = 1;
        }

        private void Gradient_Click(object sender, RoutedEventArgs e)
        {
            if (gradientCheck == false)
            {
                gradientCheck = true;
            } else
            {
                gradientCheck = false;
            }
        }
        private void btnTriangle_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(flagTriangle);
            flagTriangle = false;
            Title = triangle.Name;
            CompositionTarget.Rendering += StartAnimationTriangle;
        }
        private void btnCarpet_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(flagCarpet);
            flagCarpet = true;
            Title = carpet.Name;
            CompositionTarget.Rendering += StartAnimationCarpet;
        }
        private void btnLine_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(flagLine);
            flagLine = true;
            Title = line.Name;
            CompositionTarget.Rendering += StartAnimationLine;
        }
        private void btnFlake_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(flagFlake);
            flagFlake = true;
            Title = flake.Name;
            canvas1.Children.Add(pl);
            CompositionTarget.Rendering += StartAnimationFlake;
        }

        private void btnTree_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(flagTree);
            flagTree = true;
            Title = tree.Name;
            CompositionTarget.Rendering += StartAnimationTree;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (saveCheck)
            {
                if (flagTree || flagFlake || flagLine || flagCarpet || flagTriangle)
                {
                    MessageBox.Show("Please, wait the end of rendering.");
                }
                else
                {
                    CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                    string path;
                    dialog.IsFolderPicker = true;
                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        path = dialog.FileName + @"\fractal.png";
                        SaveCanvasToFile(canvas1, 250, path);
                    }
                }
            }
            else MessageBox.Show("There is nothing to save!");
        }

        private void Angle_Click(object sender, RoutedEventArgs e)
        {
            if (flagTree == false)
            {
                AngleWindow angleWindow = new();

                if (angleWindow.ShowDialog() == true)
                {
                    angle = angleWindow.slider.Value;
                    angleCheck = true;
                    MessageBox.Show("Angle of Binary Tree was changed!");
                    angleWindow.Close();
                }
                else
                {
                    angleCheck = false;
                    MessageBox.Show("Angle of Binary tree is default now.");
                    angleWindow.Close();
                }
            }
            else
            {
                MessageBox.Show("Wait the end of rendering.");
            }
        }
        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            DepthWindow depthWindow = new(depth);

            if (flagFlake || flagTree || flagLine || flagCarpet || flagTriangle)
            {
                MessageBox.Show("Wait the end of rendering!");
                depthWindow.Close();
            }
            else
            {
                if (depthWindow.ShowDialog() == true)
                {
                    if (int.TryParse(depthWindow.DepthText, out int depthFromWindow) && (depthFromWindow >= 1) && (depthFromWindow <= 20))
                    {
                        depth = depthFromWindow;
                        gradient = GetGradients(Brushes.Yellow.Color, Brushes.Blue.Color, depthFromWindow);
                        MessageBox.Show("Depth was succesfully changed");
                    }
                    else
                    {
                        MessageBox.Show("Incorrect input for depth! (From 1 to 20)");
                    }
                }
                else
                {
                    MessageBox.Show("Depth was not selected!");

                }
            }
        }


        // Start Animation section

        private void StartAnimationTriangle(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % 30 == 0)
            {
                triangle.DrawTriangle(canvas1, cntDepth, pol, 0, gradient, gradientCheck);
                string str = $"{triangle.Name}. Depth = {cntDepth}.";
                tbLabel.Text = str;
                cntDepth += 1;
                if (cntDepth > depth)
                {
                    tbLabel.Text = $"{triangle.Name}. Depth = {depth}. Finished";
                    CompositionTarget.Rendering -= StartAnimationTriangle;
                    flagTriangle = false;
                    saveCheck = false;
                }
            }
        }

        private void StartAnimationCarpet(object sender, EventArgs e)
        {
            frames += 1;

            if (frames % 30 == 0)
            {
                carpet.DrawCarpet(canvas1, cntDepth, pol, 0, gradient, gradientCheck);
                string str = $"{carpet.Name}. Depth = {cntDepth}";
                tbLabel.Text = str;
                cntDepth += 1;
                if (cntDepth > depth)
                {
                    tbLabel.Text = $"{carpet.Name}. Depth = {depth}. Finished";
                    CompositionTarget.Rendering -= StartAnimationCarpet;
                    flagCarpet = false;
                    saveCheck = false;
                }
            }
        }
        private void StartAnimationLine(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % 30 == 0)
            {
                line.DrawLine(canvas1, cntDepth, new Point(0, 0), 2, canvas1.Width, 20, gradient);

                string str = $"{line.Name}. Depth = {cntDepth}";
                tbLabel.Text = str;
                cntDepth += 1;
                if (cntDepth > depth)
                {
                    tbLabel.Text = $"{line.Name}. Depth = {depth}. Finished";
                    CompositionTarget.Rendering -= StartAnimationLine;
                    flagLine = false;
                    saveCheck = false;
                }
            }
        }
        private void StartAnimationTree(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % 10 == 0)
            {
                tree.DrawBinaryTree(canvas1, cntDepth, new Point(canvas1.Width / 2, canvas1.Height * 0.77), 0.2 * canvas1.Width, 3 * Math.PI / 2, angleCheck, gradient, 2, angle + 25.30);
                string str = $"{tree.Name}. Depth = {cntDepth}";
                tbLabel.Text = str;
                cntDepth += 1;
                if (cntDepth > depth)
                {
                    tbLabel.Text = $"{tree.Name}. Depth = {depth}. Finished";
                    CompositionTarget.Rendering -= StartAnimationTree;
                    flagTree = false;
                    saveCheck = false;
                }
            }
        }

        private void StartAnimationFlake(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % 20 == 0)
            {
                pl.Points.Clear();
                flake.DrawSnowFlake(canvas1, SnowflakeSize, cntDepth, gradient);
                string str = "Snow Flake - Depth = " + cntDepth.ToString();
                tbLabel.Text = str;
                cntDepth += 1;

                if (cntDepth > depth)
                {
                    tbLabel.Text = $"{flake.Name} - Depth = {depth}. Finished";
                    CompositionTarget.Rendering -= StartAnimationFlake;
                    flagFlake = false;
                    saveCheck = false;
                }
            }
        }

        public static void SaveCanvasToFile(Canvas canvas, int dpi, string filename)
        {
            var renderTarget = new RenderTargetBitmap(1080, 1080, dpi, dpi, PixelFormats.Pbgra32);
            renderTarget.Render(canvas);
            SaveRTBAsPNGBMP(renderTarget, filename);
        }
        private static void SaveRTBAsPNGBMP(RenderTargetBitmap bmp, string filename)
        {
            try
            {
                var enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bmp));
                using (var stm = System.IO.File.Create(filename))
                {
                    enc.Save(stm);
                    MessageBox.Show("Picture was saved!");
                }
            }
            catch
            {
                MessageBox.Show("Error creating image.");
            }
        }
        private void UnfollowAll()
        {
            CompositionTarget.Rendering -= StartAnimationTriangle;
            CompositionTarget.Rendering -= StartAnimationCarpet;
            CompositionTarget.Rendering -= StartAnimationLine;
            CompositionTarget.Rendering -= StartAnimationTree;
            CompositionTarget.Rendering -= StartAnimationFlake;
        }
    }
}

