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

namespace RecipeWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Main.Navigate(new MainPage());
        }

        private void Main_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (Main.CanGoBack && e.Content.ToString() != "RecipeWPFApp.MainPage")
            {
                backButton.Visibility = Visibility.Visible;
            }
            else
            {
                backButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Home_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.Navigate(new MainPage());
            while (Main.CanGoBack)
            {
                Main.RemoveBackEntry();
            }
        }

        private void Browse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.Navigate(new BrowsePage());
            while (Main.CanGoBack)
            {
                Main.RemoveBackEntry();
            }
        }

        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Main.CanGoBack)
            {
                Main.GoBack();
            }
        }
    }
}
