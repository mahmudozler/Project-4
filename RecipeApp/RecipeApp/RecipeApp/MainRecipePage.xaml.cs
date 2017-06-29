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
            for (int x = 0; x < IngredientList.Count; x++)
            {
                if (IngredientList[x][0] == ' ')
                {
                    IngredientList[x] = IngredientList[x].Substring(1);
                }
                recipe_ingredients.Children.Add(new Label { Text = IngredientList[x], TextColor = Color.Black });
            }
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
                var IngredientList = recipe.Ingredienten.Split(',').ToList();

                for (int x = 0; x < currentIngredientList.Count; x++)
                {
                    if (currentIngredientList[x][0] == ' ')
                    {
                        currentIngredientList[x] = currentIngredientList[x].Substring(1);
                    }
                }

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

        public List<Recipe> RandomRecipe(Recipe current, List<Recipe> recipes, int count = 3)
        {
            List<int> recipeChance = new List<int>();
            int totalChance = 0;

            for (int x = 0; x < recipes.Count; x++)
            {
                totalChance = (int)(totalChance + Chance(current, recipes[x]));
            }

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

            List<Recipe> randomRecipe = RandomRecipe(rec, recipes, 5);

            int counter = 0; //for gridplacement
            foreach (var recipe in randomRecipe)
            {

				var image = new Image
				{
					HeightRequest = 100,
					Source = recipe.Imagelink,
					VerticalOptions = LayoutOptions.End,
					Aspect = Aspect.AspectFill
				};
				image.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => recipe_clicked(recipe)) });

				var bview = new BoxView { BackgroundColor = Color.WhiteSmoke };
				bview.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => recipe_clicked(recipe)) });

				Grid innergrid = new Grid();
				innergrid.RowDefinitions.Add(new RowDefinition { Height = 20 });
				innergrid.RowDefinitions.Add(new RowDefinition { Height = 80 });

				string beschrijving;
				if (recipe.Beschrijving.ToString().Length > 120)
				{
					beschrijving = recipe.Beschrijving.ToString().Substring(0, 120) + "...";
				}
				else
				{
					beschrijving = recipe.Beschrijving.ToString();
				}

				grid.Children.Add(bview, 1, counter);
				innergrid.Children.Add(new Label { Text = recipe.Title, TextColor = Color.Red, FontAttributes = FontAttributes.Bold }, 0, 0);
				innergrid.Children.Add(new Label { Text = beschrijving, TextColor = Color.Black }, 0, 1);
				grid.Children.Add(innergrid, 1, counter);
				grid.Children.Add(image, 0, counter);

				counter++;
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
