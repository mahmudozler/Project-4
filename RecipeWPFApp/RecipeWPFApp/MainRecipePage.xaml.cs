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
        public Recipe recipe;

        public MainRecipePage(Recipe recipe)
        {
            this.recipe = recipe;
            InitializeRecipes(recipe);
            InitializeComponent();

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

            // Bookmarkbutton if a user is logged in
            bookmark_button.Click += add_bookmark; //set click action

            if (Global.status == "logged_out")
            {
                recipe_page.Children.Remove(bookmark_button);
            }

            // Change button depending on if recipe is bookmarked or not 
            IfBookmarked();

            // set command of rate button 
            init_rate_form();
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
            foreach (var recipe in records)
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

        private async void recipe_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var recipeObject = (Image)sender;
            var response = await getData(recipeObject.Uid);
            var records = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);
            this.NavigationService.Navigate(new MainRecipePage(records[0]));
        }

        private async void add_bookmark(object sender, RoutedEventArgs e)
        {
            //Generate list with ids of all bookmarks
            HttpClient client = new HttpClient();
            var bookmark_id_list = await Bookmark_check();

            if (bookmark_id_list.Contains(recipe.ID) == false)
            {
                // if recipe not bookmarked yet Add to bookmark 
                var response = await client.GetStringAsync("http://145.24.222.221/bookmark.php?user=" + Global.username + "&add=" + recipe.ID);
                bookmark_button.Content = "Remove Bookmark";
                bookmark_button.Background = (Brush)new BrushConverter().ConvertFrom("#9E3636");
            }
            else
            {
                // Remove bookmark and add bookmark button
                var response = await client.GetStringAsync("http://145.24.222.221/bookmark.php?user=" + Global.username + "&remove=" + recipe.ID);
                bookmark_button.Content = "Bookmark this Recipe";
                bookmark_button.Background = (Brush)new BrushConverter().ConvertFrom("#e04021");
            }
        }

        private async Task<System.Collections.Generic.List<string>> Bookmark_check()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/bookmark.php?user=" + Global.username);
            var bookmark_list = JsonConvert.DeserializeObject<System.Collections.Generic.List<BookmarkItem>>(response);
            var bookmark_id_list = new System.Collections.Generic.List<string>();
            foreach (var bookmark in bookmark_list) { bookmark_id_list.Add(bookmark.recept); }
            return bookmark_id_list;
        }

        private async void IfBookmarked()
        {
            var bookmark_id_list = await Bookmark_check();
            if (bookmark_id_list.Contains(recipe.ID))
            {
                //if recipe laready bookmarked
                bookmark_button.Content = "Remove Bookmark";
                bookmark_button.Background = (Brush)new BrushConverter().ConvertFrom("#9E3636");
            }
        }

        private async void insert_rating(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt16(rate_form_rating.Text) <= 10 && Convert.ToInt16(rate_form_rating.Text) > 0)
                {
                    var client = new HttpClient();
                    var response = await client.GetStringAsync("http://145.24.222.221/rate.php?user=" + Global.username + "&add=" + recipe.ID + "&val=" + rate_form_rating.Text);
                    this.init_rate_form();
                }
                else
                {
                    rate_form_resultlabel.Text = "Invalid Rating";

                }
            }
            catch { MessageBox.Show("Error", "Wrong input most likely"); }
        }


        private async void remove_rating(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/rate.php?user=" + Global.username + "&remove=" + recipe.ID + "&val=" + rate_form_rating.Text);
            this.rate_form_button.Content = "Rate";
            this.init_rate_form();
        }

        private async void init_rate_form()
        {
            string totalrate;
            var client = new HttpClient();

            var average = await client.GetStringAsync("http://145.24.222.221/rate.php?recipe=" + recipe.ID);
            var averagejson = JsonConvert.DeserializeObject<System.Collections.Generic.List<Average>>(average);


            if (Global.status == "logged_in")
            {
                var userrating = await client.GetStringAsync("http://145.24.222.221/rate.php?user=" + Global.username);
                var userratinglist = JsonConvert.DeserializeObject<System.Collections.Generic.List<UserRating>>(userrating);

                if (userratinglist.Count > 0)
                {
                    bool Isset = false;
                    foreach (UserRating ur in userratinglist)
                    {
                        if (ur.recept == recipe.ID)
                        {
                            Isset = true;
                            rate_form_rating.IsEnabled = false;
                            rate_form_rating.Text = ur.beoordeling;
                            if (averagejson[0].beoordeling != null) { totalrate = averagejson[0].beoordeling.Substring(0, 4); } else { totalrate = "N/R"; };
                            rate_form_resultlabel.Text = "Average Score: " + totalrate;
                            rate_form_button.Content = "Remove Rating";
                            rate_form_button.Click += remove_rating;
                            break;
                        }
                    }
                    if (!Isset)
                    {
                        rate_form_rating.IsEnabled = true;
                        rate_form_rating.Text = "";
                        rate_form_button.Click += insert_rating;
                        if (averagejson[0].beoordeling != null) { totalrate = averagejson[0].beoordeling.Substring(0, 4); } else { totalrate = "N/R"; };
                        rate_form_resultlabel.Text = "Average Score: " + totalrate;
                    }
                }
                else
                {
                    rate_form_rating.IsEnabled = true;
                    rate_form_rating.Text = "";
                    rate_form_button.Click += insert_rating;
                    if (averagejson[0].beoordeling != null) { totalrate = averagejson[0].beoordeling.Substring(0, 4); } else { totalrate = "N/R"; };
                    rate_form_resultlabel.Text = "Average Score: " + totalrate;
                }
            }
            else
            {

                rate_form_rating.IsEnabled = false;
                rate_form_rating.Text = "Log in to rate";
                rate_form_button.Click += errorDisplay;
                if (averagejson[0].beoordeling != null) { totalrate = averagejson[0].beoordeling.Substring(0, 4); } else { totalrate = "N/R"; };
                rate_form_resultlabel.Text = "Average Score: " + totalrate;
            }
        }

        private void keyboard(object sender, TextCompositionEventArgs e)
        {
            int x;

            if (!Int32.TryParse(e.Text, out x))
            {
                if (e.Text != "," || rate_form_rating.Text.Contains(","))
                {
                    e.Handled = true;
                }
            }
        }

        private void errorDisplay(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Error", "You're not logged in yet");
        }

        private async Task<String> getData()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/search.php?input=%category=all");
            return response;
        }

        private async Task<String> getData(string id)
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
