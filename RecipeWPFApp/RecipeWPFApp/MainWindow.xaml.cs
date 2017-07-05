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

            Main.Navigate(CreateWindow.Create(1));
        }

        private void Main_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (Main.CanGoBack && e.Content.ToString() != "RecipeWPFApp.MainPage" && e.Content.ToString() != "RecipeWPFApp.BrowsePage" && e.Content.ToString() != "RecipeWPFApp.AccountPage" && e.Content.ToString() != "RecipeWPFApp.RegisterPage")
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
            Main.Navigate(CreateWindow.Create(1));
            while (Main.CanGoBack)
            {
                Main.RemoveBackEntry();
            }
        }

        private void Browse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.Navigate(CreateWindow.Create(2));
            while (Main.CanGoBack)
            {
                Main.RemoveBackEntry();
            }
        }

        private void Account_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.Navigate(CreateWindow.Create(3));
            while (Main.CanGoBack)
            {
                Main.RemoveBackEntry();
            }
        }

        private void Register_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main.Navigate(CreateWindow.Create(4));
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
