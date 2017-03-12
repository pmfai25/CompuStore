
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;

namespace CompuStore.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Brush defaultBackground = new SolidColorBrush(Color.FromArgb(80,17,158,218));
        Brush defaultForeground = new SolidColorBrush(Colors.White);
        Brush activeBackground = new SolidColorBrush(Colors.White);
        Brush actveForeground = new SolidColorBrush(Colors.OrangeRed);
        Style defaultStyle, activeStyle;
        List<Button> btns;
        public MainWindow()
        {
            InitializeComponent();
            defaultStyle = (Style)Resources["defaultStyle"];
            activeStyle = (Style)Resources["activeStyle"];
            btns = new List<Button>();
            btns.AddRange(new[] { btnClients,btnItems,   btnSales,  btnSuppliers });
            btnSuppliers_Click(btnSuppliers, null);
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            SetActive(sender as Button);            
        }

        private void btnItems_Click(object sender, RoutedEventArgs e)
        {
            SetActive(sender as Button);
        }

        private void btnSuppliers_Click(object sender, RoutedEventArgs e)
        {
            SetActive(sender as Button);
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            SetActive(sender as Button);
        }

        private void btnSales_Click(object sender, RoutedEventArgs e)
        {
            SetActive(sender as Button);
        }
        private void SetActive(Button button)
        {
            foreach (var b in btns)
                b.Style = defaultStyle;
            button.Style = activeStyle;
        }
    }
}
