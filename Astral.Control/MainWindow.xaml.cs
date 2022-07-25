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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Astral.Control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPage
    {
        public MainWindow(WindowsAPI.DecorateWindow decorateWindow)
        {
            InitializeComponent();
            
            // Add the sexy Windows 11 corner borders.
            decorateWindow.AddWindows11Borders(this);
        }

        public event EventHandler<IPage>? Replaced;

        private void WindowClicked(object sender, MouseButtonEventArgs e) =>
            this.DragMove();

        private void Minimize(object sender, MouseButtonEventArgs e) =>
            this.WindowState = WindowState.Minimized;

        private void Close(object sender, MouseButtonEventArgs e) =>
            this.Close();

        private void Maximize(object sender, MouseButtonEventArgs e) =>
            this.WindowState = this.WindowState == WindowState.Maximized ?
                WindowState.Normal : WindowState.Maximized;

        private void TopBorderClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                Maximize(sender, e);
        }
    }
}
