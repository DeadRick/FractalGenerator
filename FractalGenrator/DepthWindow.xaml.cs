using System;
using System.Windows;
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
