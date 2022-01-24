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

namespace FractalGenrator
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AngleWindow : Window
    {
        public AngleWindow()
        {
            InitializeComponent();
            Binding bind = new();
            bind.ElementName = nameof(slider);
            bind.Path = new PropertyPath(nameof(slider.Value));
        }

        private void AcceptAngle_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Default_Click(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = false;
        }

    }
}