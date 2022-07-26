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
    public partial class MainWindow : Window
    {
        public MainWindow(
            WindowsAPI.DecorateWindow decorateWindow,
            WindowsAPI.OSCheck osCheck,
            IEnumerable<IPage> pages)
        {
            InitializeComponent();

            // Add the sexy Windows 11 corner borders.
            decorateWindow.AddWindows11Borders(this);

            if (osCheck.IsNewWindows)
            {
                ParentBorder.BorderBrush = null;
                ParentBorder.BorderThickness = new Thickness(0);
            }

            SetPage(pages.FirstOrDefault(x => x is Pages.Welcome));
        }

        private void SetPage(IPage e)
        {
            while (MainContainer.Children.Count > 0)
                MainContainer.Children.RemoveAt(0);

            if (e is UserControl page)
                MainContainer.Children.Add(page);
        }

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
