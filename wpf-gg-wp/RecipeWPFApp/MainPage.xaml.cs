using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RecipeWPFApp
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var response = await getData();
            var featuredItem = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);
            this.NavigationService.Navigate(new MainRecipePage(featuredItem[0]));
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.RemoveBackEntry();
            }
        }

        private void Category_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var categoryObject = (Image)sender;
            this.NavigationService.Navigate(new CategoryPage(categoryObject.Uid));
        }

        private async Task<String> getData()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/recipe.php?id=10");
            return response;
        }
    }
}
