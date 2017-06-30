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

        private void Home_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.Navigate(new MainPage());
            if (Main.CanGoBack)
            {
                Main.RemoveBackEntry();
            }
        }
    }
}
