using Xamarin.Forms;
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RecipeApp
{
    public partial class MainRecipePage : ContentPage
    {
        public MainRecipePage(Recipe recipe)
        {
            InitializeRecipes(recipe);
            InitializeComponent();

            // Fill in all fields with recipe data on initialization from parameter
            recipe_name.Text = recipe.Title;
            recipe_image.Source = recipe.Imagelink;
            recipe_beschrijving.Text = recipe.Beschrijving.ToString();
            recipe_instructions.Text = recipe.Bereidingswijze;

            var IngredientList = recipe.Ingredienten.Split(',').ToList();  //split string between to get ingredients seperate
            foreach (var ingredient in IngredientList)
            {
                recipe_ingredients.Children.Add(new Label { Text = ingredient, TextColor = Color.Black });
            }
        }

        public static float Chance(Recipe recipe, List<Recipe> recipes)
        {
            float chance = 0.0f;

            foreach (Recipe x in recipes)
            {
                if (x.ID == recipe.ID)
                {
                    continue;
                }

                if (x.Categorie == recipe.Categorie)
                {
                    chance = chance + 10;
                }

                foreach (char ingredient in recipe.Ingredienten)
                {
                    foreach (char y in x.Ingredienten)
                    {
                        if (y == ingredient)
                        {
                            chance = chance + 2;
                        }
                    }

                }
            }
            return chance;
        }

        public List<Recipe> RandomRecipe(List<Recipe> recipes, int count = 3)
        {
            List<int> recipeChance = new List<int>();
            int totalChance = 0;

            for (int x = 0; x < recipes.Count; x++)
            {
                totalChance = (int)(totalChance + Chance(recipes[x], recipes));
            }

            for (int x = 0; x < recipes.Count; x++)
            {
                float chance = Chance(recipes[x], recipes);
                float currentChance = chance / totalChance * 100.0f;
                for (int y = 0; y < currentChance; y++)
                {
                    recipeChance.Add(x);
                }
            }

            Random random = new Random();
            List<Recipe> randomRecipe = new List<Recipe>();
            for (int x = 0; x < count; x++)
            {
                int randomInt = recipeChance[random.Next(0, recipeChance.Count)];
                if (randomRecipe.Contains(recipes[randomInt]))
                {
                    x = x - 1;
                    continue;
                }
                randomRecipe.Add(recipes[randomInt]);
            }

            return randomRecipe;
        }

        public async void InitializeRecipes(Recipe rec)
        {
            var response = await getData();
            //get the data from JSON link in record list
            var records = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);

            List<Recipe> recipes = new List<Recipe>();

            foreach (var recipe in records)
            {
                recipes.Add(recipe);
            }

            List<Recipe> randomRecipe = RandomRecipe(recipes, 3);

            foreach (var recipe in randomRecipe)
            {
                var Clicklabel = new Label { Text = recipe.Title, TextColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Bold };
                Clicklabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => recipe_clicked(recipe)) });
                Recommended.Children.Add(Clicklabel);
                Recommended.Children.Add(new BoxView { HeightRequest = 1, BackgroundColor = Color.LightGray });
                Recommended.Children.Add(new Label { Text = recipe.Beschrijving.ToString(), TextColor = Color.Black });
                var ClickImage = new Image { Source = recipe.Imagelink };
                ClickImage.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => recipe_clicked(recipe)) });
                Recommended.Children.Add(ClickImage);
                //results.Children.Add(new Button { Text = "See recipe", Command = new Command(() => button_clicked((RootObject)repice)) });
            }
        }

        public async Task<String> getData()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://infpr04.esy.es/search.php?input=%category=all");

            return response;
        }

        public void recipe_clicked(Recipe recipe)
        {
            Navigation.PushAsync(new MainRecipePage(recipe));
        }
    }
}
