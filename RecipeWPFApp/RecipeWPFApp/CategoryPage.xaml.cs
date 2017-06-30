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
    /// Interaction logic for CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : Page
    {
        public CategoryPage(string category)
        {
            InitializeRecipes(category);
            InitializeComponent();
        }

        private async void InitializeRecipes(string category)
        {
            var response = await getData(category);
            var records = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);

            foreach (var recipe in records)
            {
                var image = new Image
                {
                    Uid = recipe.ID,
                    Height = 100,
                    Source = ToBitmapImage(recipe.Imagelink),
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Stretch = Stretch.Fill
                };

                string description;
                if (recipe.Beschrijving.ToString().Length > 120)
                {
                    description = recipe.Beschrijving.ToString().Substring(0, 120) + "...";
                }
                else
                {
                    description = recipe.Beschrijving.ToString();
                }
            }
        }

        private async void recipe_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var recipeObject = (Image)sender;
            var response = await getData(recipeObject.Uid);
            var records = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);
            this.NavigationService.Navigate(new MainRecipePage(records[0]));
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.RemoveBackEntry();
            }
        }

        private async Task<String> getData(string category)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://infpr04.esy.es/search.php?input=&category=" + category);
            return response;
        }

        private async Task<String> getData(int id)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://infpr04.esy.es/recipe.php?id=" + id);
            return response;
        }

        private BitmapImage ToBitmapImage(string link)
        {
            BitmapImage source = new BitmapImage();

            source.BeginInit();
            source.UriSource = new Uri(link);
            source.EndInit();

            return source;
        }
    }
}
