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
            bind.ElementName = nameof(slider1);
            bind.Path = new PropertyPath(nameof(slider1.Value));
        }

        private void AcceptAngle_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Default_Click(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = false;
           
        }
        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rightLabel.Text = $"Right: {(int)slider2.Value}";
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            leftLabel.Text = $"Left: {(int)slider1.Value}";
        }
    }
}
