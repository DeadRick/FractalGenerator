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
        static Polyline pl = new();
        bool flagTree = false;
        bool flagFlake = false;

        public MainWindow()
        {
            InitializeComponent();
            double ysize = 0.8 * canvas1.Height /
           (Math.Sqrt(3) * 4 / 3);
            double xsize = 0.8 * canvas1.Width / 2;
            double size = 0;
            if (ysize < xsize)
                size = ysize;
            else
                size = xsize;
            SnowflakeSize = 2 * size;
            pl.Stroke = Brushes.Black;
        }

        
        private double SnowflakeSize;
        private int depth = 10;
        private int cntDepth = 0;
        private int frames = 0;
        double[] dTheta = new double[4] { 0, Math.PI / 3, -2 * Math.PI / 3, Math.PI / 3 };

        
        
        Flake flake = new("Flake", ref pl);
        BinaryTree tree = new("Binary Tree");
        


        private void btnTree_Click(object sender, RoutedEventArgs e)
        {
            flagTree = true;
            canvas1.Children.Clear();
            tbLabel.Text = "";
            cntDepth = 1;
            frames = 0;
            CompositionTarget.Rendering += StartAnimationTree;
        }
        private void StartAnimationTree(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % 10 == 0)
            {
                tree.DrawBinaryTree(canvas1, cntDepth, new Point(canvas1.Width / 2, canvas1.Height * 0.77), 0.2 * canvas1.Width, 3 * Math.PI / 2, 1.5);
                string str = $"{tree.Name}. Depth = " + cntDepth.ToString();
                tbLabel.Text = str;
                cntDepth += 1;
                if (cntDepth > depth)
                {
                    tbLabel.Text = $"{tree.Name}. Depth = {depth}. Finished";
                    CompositionTarget.Rendering -= StartAnimationTree;
                    flagTree = false;
                }
            }
        }

        
        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            DepthWindow depthWindow = new(depth);

            if (flagFlake || flagTree)
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
        private void btnFlake_Click(object sender, RoutedEventArgs e)
        {
            flagFlake = true;
            canvas1.Children.Clear();
            tbLabel.Text = "";
            frames = 0;
            cntDepth = 0;
            canvas1.Children.Add(pl);
            CompositionTarget.Rendering += StartAnimationFlake;
        }
        private void StartAnimationFlake(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % 20 == 0)
            {
                pl.Points.Clear();
                flake.DrawSnowFlake(canvas1, SnowflakeSize, cntDepth);
                string str = "Snow Flake - Depth = " +
                cntDepth.ToString() +  " " + depth.ToString();
                tbLabel.Text = str;
                cntDepth += 1;

                if (cntDepth > depth)
                {
                    tbLabel.Text = $"{flake.Name} - Depth = {depth}. Finished";
                    CompositionTarget.Rendering -= StartAnimationFlake;
                    flagFlake = false;
                }
            }
        }
    }
}

