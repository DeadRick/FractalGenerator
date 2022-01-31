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
    /// <summary>
    /// Interaction logic for DepthWindow.xaml
    /// </summary>
    public partial class DepthWindow : Window
    {
        /// <summary>
        /// Инициализация окна для выбора глубины рекурсии.
        /// </summary>
        /// <param name="depth"></param>
        public DepthWindow(int depth)
        {
            InitializeComponent();
            depthFractalBox.Text = depth.ToString();
        }

        /// <summary>
        /// OK кнопка.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        
        /// <summary>
        /// Получение вводимый глубины в переменную.
        /// </summary>
        public string DepthText
        {
            get { return depthFractalBox.Text; }
        }
    }
}
