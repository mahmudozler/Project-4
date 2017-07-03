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
            InitializeText(category);
        }

        private async void InitializeRecipes(string category)
        {
            var response = await getData(category);
            var records = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);
            var counter = 0;
            foreach (var recipe in records)
            {
                var image = new Image
                {
                    Uid = recipe.ID,
                    Height = 100,
                    Source = ToBitmapImage(recipe.Imagelink),
                    Stretch = Stretch.UniformToFill
                };

                TextBlock title = new TextBlock { Text = recipe.Title, FontSize = 15, FontWeight = FontWeights.Bold, Foreground = Brushes.Red, Background = Brushes.WhiteSmoke};

                TextBlock description;
                if (recipe.Beschrijving.ToString().Length > 500)
                {
                    description = new TextBlock { Text = recipe.Beschrijving.ToString().Substring(0, 500) + "...", TextWrapping = TextWrapping.Wrap, Background = Brushes.WhiteSmoke };
                }
                else
                {
                    description = new TextBlock { Text = recipe.Beschrijving.ToString(), TextWrapping = TextWrapping.Wrap, Background = Brushes.WhiteSmoke };
                }

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100.0) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10.0) });

                Grid innerGrid = new Grid();
                innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20.0) });
                innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80.0) });

                image.MouseLeftButtonDown += new MouseButtonEventHandler(recipe_MouseLeftButtonDown);
                Grid.SetRow(image, counter);
                grid.Children.Add(image);

                innerGrid.Children.Add(title);
                Grid.SetRow(description, 1);
                innerGrid.Children.Add(description);

                Grid.SetRow(innerGrid, counter);
                Grid.SetColumn(innerGrid, 2);
                grid.Children.Add(innerGrid);

                counter = counter + 2;
            }
        }

        private void InitializeText(string category)
        {
            categoryName.Text = category;
        }

        private async void recipe_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var recipeObject = (Image)sender;
            var response = await getData(recipeObject.Uid, true);
            var records = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);
            this.NavigationService.Navigate(new MainRecipePage(records[0]));
        }

        private async Task<String> getData(string category)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/search.php?input=&category=" + category);
            return response;
        }

        private async Task<String> getData(string id, bool isID)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/recipe.php?id=" + id);
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
