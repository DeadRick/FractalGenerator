using System;
using System.Windows;

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
