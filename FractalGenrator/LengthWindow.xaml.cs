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
