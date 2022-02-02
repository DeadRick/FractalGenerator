using System;
using System.Windows;

namespace FractalGenrator
{
    /// <summary>
    /// Interaction logic for LengthWindow.xaml
    /// </summary>
    public partial class LengthWindow : Window
    {
        /// <summary>
        /// Инициализация.
        /// </summary>
        public LengthWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ok.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptLength_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        
        /// <summary>
        /// Свойство линии.
        /// </summary>
        public string LengthText
        {
            get { return lengthFractalBox.Text; }
        }
    }
}
