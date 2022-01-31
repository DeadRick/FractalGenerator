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
        BinaryTree tree = new("Binary Tree");
        Carpet carpet = new("Carpet", depth);
        Triangle triangle = new("Triangle");

        ScaleTransform transformScale = new();

        LinkedList<int> lastFractal = new();

        //private Delegate LastMethod;

        double widthWindow = System.Windows.SystemParameters.PrimaryScreenWidth;
        double heightWindow = System.Windows.SystemParameters.PrimaryScreenHeight;

        static SolidColorBrush startGrad;
        static SolidColorBrush endGrad;

        int SelectedZoom;
        IEnumerable<Color> gradient;
        static Polyline pl = new();
        static Polygon pol = new();
        bool flagTree, flagFlake, flagLine, flagCarpet, flagTriangle, saveCheck, gradientCheck = false;
        public bool angleCheck = false;
        private static double angleL, angleR;
        private double SnowflakeSize;
        private static int depth = 10;
        private int cntDepth = 0;
        private int frames = 0;
        private int fps = 30;
        private int lengthBetween = 20;
        double[] dTheta = new double[4] { 0, Math.PI / 3, -2 * Math.PI / 3, Math.PI / 3 };

        public MainWindow()
        {
            InitializeComponent();
            Width = System.Windows.SystemParameters.PrimaryScreenWidth / 2;
            Height = (System.Windows.SystemParameters.PrimaryScreenHeight / 2) + 10;
            MinWidth = widthWindow / 2;
            MinHeight = heightWindow / 2;
            MaxWidth = Width;
            MaxHeight = Height;
            Title = "Fractals";

            startColor.SelectedBrush = Brushes.Brown;
            endColor.SelectedBrush = Brushes.Green;

            startGrad = startColor.SelectedBrush;
            endGrad = endColor.SelectedBrush;

            gradient = GetGradients(startGrad.Color, endGrad.Color, depth);

            canvas1.LayoutTransform = transformScale;
            canvas1.Width = canvas1.Height;

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


        private void ZoomSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SelectedZoom = (int)sliderZoom.Value;
            transformScale.ScaleX = SelectedZoom;
            transformScale.ScaleY = SelectedZoom;
            zoomLabel.Text = $"Zoom: {(int)sliderZoom.Value}";
        }
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

        private void ClickSettings(bool flagCheck, int fractal)
        {
            UnfollowAll();
            saveCheck = true;
            lastFractal.AddLast(fractal);
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
            }
            else
            {
                gradientCheck = false;
            }
        }
        private void btnTriangle_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(flagTriangle, 5);
            if (depth > 10) { depth = 10; }
            flagTriangle = false;
            Title = triangle.Name;
            CompositionTarget.Rendering += StartAnimationTriangle;
        }
        private void btnCarpet_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(flagCarpet, 4);
            if (depth > 8) { depth = 7; }
            flagCarpet = true;
            Title = carpet.Name;
            CompositionTarget.Rendering += StartAnimationCarpet;
        }
        private void btnLine_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(flagLine, 3);
            if (depth > 10) { depth = 10; }
            flagLine = true;
            Title = line.Name;
            CompositionTarget.Rendering += StartAnimationLine;
        }
        private void btnFlake_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(flagFlake, 2);
            if (depth > 10) { depth = 10; }
            flagFlake = true;
            Title = flake.Name;
            canvas1.Children.Add(pl);
            CompositionTarget.Rendering += StartAnimationFlake;
        }

        private void btnTree_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(flagTree, 1);
            if (depth > 15) { depth = 15; }
            gradient = GetGradients(endColor.SelectedBrush.Color, startColor.SelectedBrush.Color, depth);
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
                        SaveCanvasToFile(canvas1, 500, path);
                    }
                }
            }
            else MessageBox.Show("There is nothing to save!");
        }

        private void Length_Click(object sender, RoutedEventArgs e)
        {
            LengthWindow lenWindow = new();
            if (lenWindow.ShowDialog() == true)
            {
                if (int.TryParse(lenWindow.LengthText, out int lengthBet) || (lengthBetween > 0) || (lengthBetween < 51))
                {
                    lengthBetween = lengthBet;
                } else
                {
                    MessageBox.Show("Incorrect input for length! (From 1 to 50)");
                    lenWindow.Close();
                }
            } else
            {
                lengthBetween = 20;
            }
        }

        private void Angle_Click(object sender, RoutedEventArgs e)
        {
            if (flagTree == false)
            {
                AngleWindow angleWindow = new();

                if (angleWindow.ShowDialog() == true)
                {
                    angleL = angleWindow.slider1.Value;
                    angleR = angleWindow.slider2.Value;
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
                    if (int.TryParse(depthWindow.DepthText, out int depthFromWindow) && (depthFromWindow >= 1) && (depthFromWindow <= 16))
                    {
                        depth = depthFromWindow;
                        cntDepth = 1;


                        MessageBox.Show("Depth was succesfully changed");
                        if (lastFractal.Count != 0)
                        {
                            canvas1.Children.Clear();
                            if (lastFractal.Last.Value == 1)
                            {
                                gradient = GetGradients(endColor.SelectedBrush.Color, startColor.SelectedBrush.Color, depthFromWindow);
                            }
                            else
                            {
                                gradient = GetGradients(startColor.SelectedBrush.Color, endColor.SelectedBrush.Color, depthFromWindow);
                            }
                            switch (lastFractal.Last.Value)
                            {
                                case 1:
                                    CompositionTarget.Rendering += StartAnimationTree;
                                    break;
                                case 2:
                                    CompositionTarget.Rendering += StartAnimationFlake;
                                    break;
                                case 3:
                                    CompositionTarget.Rendering += StartAnimationLine;
                                    break;
                                case 4:
                                    CompositionTarget.Rendering += StartAnimationCarpet;
                                    break;
                                case 5:
                                    CompositionTarget.Rendering += StartAnimationTriangle;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect input for depth! (From 1 to 16)");
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
                    Permission();
                    CompositionTarget.Rendering -= StartAnimationTriangle;
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
                    Permission();
                    CompositionTarget.Rendering -= StartAnimationCarpet;
                }
            }
        }
        private void StartAnimationLine(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % 30 == 0)
            {
                line.DrawLine(canvas1, cntDepth, new Point(0, 0), canvas1.Width, lengthBetween, gradient, gradientCheck);

                string str = $"{line.Name}. Depth = {cntDepth}";
                tbLabel.Text = str;
                cntDepth += 1;
                if (cntDepth > depth)
                {
                    tbLabel.Text = $"{line.Name}. Depth = {depth}. Finished";
                    Permission();
                    CompositionTarget.Rendering -= StartAnimationLine;
                }
            }
        }
        private void StartAnimationTree(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % 20 == 0)
            {
                tree.DrawBinaryTree(canvas1, cntDepth, new Point(canvas1.Width / 2, canvas1.Height * 0.77), 0.2 * canvas1.Width, 3 * Math.PI / 2, angleCheck, gradient, gradientCheck, 2, angleL, angleR);
                string str = $"{tree.Name}. Depth = {cntDepth}";
                tbLabel.Text = str;
                cntDepth += 1;
                if (cntDepth > depth)
                {
                    tbLabel.Text = $"{tree.Name}. Depth = {depth}. Finished";
                    Permission();
                    CompositionTarget.Rendering -= StartAnimationTree;
                }
            }
        }

        private void StartAnimationFlake(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % 20 == 0)
            {
                pl.Points.Clear();
                flake.DrawSnowFlake(canvas1, SnowflakeSize, cntDepth, gradient, gradientCheck);
                string str = "Snow Flake - Depth = " + cntDepth.ToString();
                tbLabel.Text = str;
                cntDepth += 1;

                if (cntDepth > depth)
                {
                    tbLabel.Text = $"{flake.Name} - Depth = {depth}. Finished";
                    Permission();
                    CompositionTarget.Rendering -= StartAnimationFlake;
                }
            }
        }

        public static void SaveCanvasToFile(Canvas canvas, int dpi, string filename)
        {
            var renderTarget = new RenderTargetBitmap(5000, 5000, dpi, dpi, PixelFormats.Pbgra32);
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

        private void Permission()
        {
            flagTree = false;
            flagFlake = false;
            flagLine = false;
            flagTriangle = false;
            flagCarpet = false;
        }
        private void UnfollowAll()
        {
            CompositionTarget.Rendering -= StartAnimationTriangle;
            CompositionTarget.Rendering -= StartAnimationCarpet;
            CompositionTarget.Rendering -= StartAnimationLine;
            CompositionTarget.Rendering -= StartAnimationTree;
            CompositionTarget.Rendering -= StartAnimationFlake;
        }
        private void DrawTheLastFractal()
        {
            if (lastFractal.Count != 0)
            {
                canvas1.Children.Clear();
                switch (lastFractal.Last.Value)
                {
                    case 1:
                        tree.DrawBinaryTree(canvas1, depth, new Point(canvas1.Width / 2, canvas1.Height * 0.77), 0.2 * canvas1.Width, 3 * Math.PI / 2, angleCheck, gradient, gradientCheck, 2, angleL, angleR);
                        break;
                    case 2:
                        flake.DrawSnowFlake(canvas1, SnowflakeSize, depth, gradient, gradientCheck);
                        break;
                    case 3:
                        line.DrawLine(canvas1, depth, new Point(0, 0), canvas1.Width, lengthBetween, gradient, gradientCheck);
                        break;
                    case 4:
                        carpet.DrawCarpet(canvas1, depth, pol, 0, gradient, gradientCheck);
                        break;
                    case 5:
                        triangle.DrawTriangle(canvas1, depth, pol, 0, gradient, gradientCheck);
                        break;
                }
            }
        }
        private void WindowChanging()
        {
            Width = MinWidth;
            Height = MinHeight;
            MaxWidth = Width;
            MaxHeight = Height;
            canvas1.Height = canvas1.Width;
            if (!(flagFlake || flagTree || flagLine || flagCarpet || flagTriangle))
            {
                DrawTheLastFractal();
            }
        }
        private void Maximum_Click(object sender, RoutedEventArgs e)
        {
            MinWidth = widthWindow;
            MinHeight = heightWindow - 20;
            canvas1.Width = heightWindow;
            WindowChanging();
        }

        private void Medium_Click(object sender, RoutedEventArgs e)
        {
            MinWidth = widthWindow / 2 + heightWindow / 4;
            MinHeight = heightWindow / 2 + heightWindow / 4;
            canvas1.Width = heightWindow / 2 + heightWindow / 4;
            WindowChanging();
        }

        private void Minimum_Click(object sender, RoutedEventArgs e)
        {
            MinWidth = widthWindow / 2;
            MinHeight = heightWindow / 2;
            canvas1.Width = heightWindow / 2;
            WindowChanging();
        }
    }
}

