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
    /// Interaction logic for FrameWindow.xaml
    /// </summary>
    public partial class FrameWindow : Window
    {
        /// <summary>
        /// Инициализация окна для выбора количества кадров.
        /// </summary>
        public FrameWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptFrame_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string frameText
        {
            get { return frameCount.Text; }
        }
    }
}
