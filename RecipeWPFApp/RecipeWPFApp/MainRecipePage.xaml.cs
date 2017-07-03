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
    /// Interaction logic for MainRecipePage.xaml
    /// </summary>
    public partial class MainRecipePage : Page
    {
        public MainRecipePage(Recipe recipe)
        {
            InitializeRecipes(recipe);
            InitializeComponent();
            InitializeText(recipe);
        }

        public static float Chance(Recipe current, Recipe recipe)
        {
            float chance = 0.0f;

            if (recipe.ID != current.ID)
            {
                if (recipe.Categorie == current.Categorie)
                {
                    chance = chance + 2;
                }

                var currentIngredientList = current.Ingredienten.Split(',').ToList();
                for (int x = 0; x < currentIngredientList.Count; x++)
                {
                    if (currentIngredientList[x][0] == ' ')
                    {
                        currentIngredientList[x] = currentIngredientList[x].Substring(1);
                    }
                }

                var IngredientList = recipe.Ingredienten.Split(',').ToList();
                for (int x = 0; x < IngredientList.Count; x++)
                {
                    if (IngredientList[x][0] == ' ')
                    {
                        IngredientList[x] = IngredientList[x].Substring(1);
                    }
                }

                foreach (string ingredient in IngredientList)
                {
                    foreach (string y in currentIngredientList)
                    {
                        if (y == ingredient)
                        {
                            chance = chance + 1;
                        }
                    }
                }
            }
            return chance;
        }

        public List<Recipe> RandomRecipes(Recipe current, List<Recipe> recipes, int count = 3)
        {
            int totalChance = 0;
            for (int x = 0; x < recipes.Count; x++)
            {
                totalChance = (int)(totalChance + Chance(current, recipes[x]));
            }

            List<int> recipeChance = new List<int>();
            for (int x = 0; x < recipes.Count; x++)
            {
                float chance = Chance(current, recipes[x]);
                float currentChance = chance / totalChance * 100.0f;
                for (int y = 0; y < currentChance; y++)
                {
                    recipeChance.Add(x);
                }
            }

            Random random = new Random();
            List<Recipe> randomRecipe = new List<Recipe>();

            int tries = 0;
            for (int x = 0; x < count; x++)
            {
                if (tries > 3)
                {
                    break;
                }

                int randomInt = recipeChance[random.Next(0, recipeChance.Count)];
                if (randomRecipe.Contains(recipes[randomInt]))
                {
                    tries = tries + 1;
                    x = x - 1;
                    continue;
                }
                randomRecipe.Add(recipes[randomInt]);
            }

            return randomRecipe;
        }

        private async void InitializeRecipes(Recipe currentRecipe)
        {
            var response = await getData();
            var records = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);

            List<Recipe> recipes = new List<Recipe>();
            foreach(var recipe in records)
            {
                recipes.Add(recipe);
            }
            List<Recipe> randomRecipes = RandomRecipes(currentRecipe, recipes, 5);

            var counter = 0;
            foreach (var recipe in randomRecipes)
            {
                var image = new Image
                {
                    Uid = recipe.ID,
                    Height = 100,
                    Source = ToBitmapImage(recipe.Imagelink),
                    Stretch = Stretch.UniformToFill
                };

                TextBlock title = new TextBlock { Text = recipe.Title, FontSize = 15, FontWeight = FontWeights.Bold, Foreground = Brushes.Red, Background = Brushes.WhiteSmoke };

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

        private void InitializeText(Recipe recipe)
        {
            recipeName.Text = recipe.Title;
            recipeImage.Source = ToBitmapImage(recipe.Imagelink);
            recipeInstructions.Text = recipe.Bereidingswijze;

            var ingredientList = recipe.Ingredienten.Split(',').ToList();
            for (int x = 0; x < ingredientList.Count; x++)
            {
                if (ingredientList[x][0] == ' ')
                {
                    ingredientList[x] = ingredientList[x].Substring(1);
                }
                recipeIngredients.Children.Add(new TextBlock { Text = ingredientList[x], FontSize = 15 });
            }
        }

        private async void recipe_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var recipeObject = (Image)sender;
            var response = await getData(recipeObject.Uid);
            var records = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);
            this.NavigationService.Navigate(new MainRecipePage(records[0]));
        }

        private async Task<String> getData()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://infpr04.heliohost.org/search.php?input=%category=all");
            return response;
        }

        private async Task<String> getData(string id)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://infpr04.heliohost.org/recipe.php?id=" + id);
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
