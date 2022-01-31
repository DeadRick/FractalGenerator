using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FractalGenrator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Отловка всех Exception'ов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Error: {e.Exception}");
            e.Handled = true;
        }
    }
}
