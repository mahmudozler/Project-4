﻿using Xamarin.Forms;
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
        public Recipe recipe;

        public MainRecipePage(Recipe recipe)
        {
            this.recipe = recipe;
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

            // Bookmarkbutton if a user is logged in
            bookmark_button.Command = new Command(() => add_bookmark()); //set click action

            if(Global.status == "logged_out") {
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
            var response = await client.GetStringAsync("http://145.24.222.221/search.php?input=%category=all");

            return response;
        }

        private async void add_bookmark() {
            //Generate list with ids of all bookmarks
            HttpClient client = new HttpClient();
            var bookmark_id_list = await Bookmark_check();

            if (bookmark_id_list.Contains(recipe.ID) == false) 
            {
				// if recipe not bookmarked yet Add to bookmark 
				var response = await client.GetStringAsync("http://145.24.222.221/bookmark.php?user=" + Global.username + "&add=" + recipe.ID);
                bookmark_button.Text = "Remove Bookmark";
                bookmark_button.BackgroundColor = Color.FromHex("#9E3636");
            }
            else 
            {
                // Remove bookmark and add bookmark button
                var response = await client.GetStringAsync("http://145.24.222.221/bookmark.php?user=" + Global.username + "&remove=" + recipe.ID);
                bookmark_button.Text = "Bookmark this Recipe";
                bookmark_button.BackgroundColor = Color.FromHex("#e04021");
            }
        }

        private async Task<System.Collections.Generic.List<string>> Bookmark_check() {
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
            if(bookmark_id_list.Contains(recipe.ID)) {
				//if recipe laready bookmarked
				bookmark_button.Text = "Remove Bookmark";
				bookmark_button.BackgroundColor = Color.FromHex("#9E3636");
            }
        }

        public void recipe_clicked(Recipe recipe)
        {
            Navigation.PushAsync(new MainRecipePage(recipe));
        }

        private async void insert_rating()
            //check for boundaries and insert rating
        {
            try
            {
				if(Convert.ToInt16(rate_form_rating.Text) <= 10 && Convert.ToInt16(rate_form_rating.Text) > 0)
				{
					var client = new HttpClient();
					var response = await client.GetStringAsync("http://145.24.222.221/rate.php?user="+ Global.username +"&add="+ recipe.ID +"&val="+rate_form_rating.Text);
					this.init_rate_form();
				}
				else
				{
                    await DisplayAlert("Error", "Wrong input most likely", "Cancel");
					
				}     
            }
            catch {await DisplayAlert("Error", "Wrong input most likely", "Cancel");}
        }


		private async void remove_rating()
            //remove rating from database
		{
			var client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/rate.php?user=" + Global.username + "&remove=" + recipe.ID + "&val=" + rate_form_rating.Text);
            this.rate_form_button.Text = "Rate";
            this.init_rate_form();
		}

        private async void init_rate_form()
            //initialize rating form (checks whether account is logged in and whether user has already rated)
        {
            string totalrate;
            var client = new HttpClient();

			var average = await client.GetStringAsync("http://145.24.222.221/rate.php?recipe=" + recipe.ID);
			var averagejson = JsonConvert.DeserializeObject<System.Collections.Generic.List<Average>>(average);


            if(Global.status == "logged_in")
            {
                var userrating = await client.GetStringAsync("http://145.24.222.221/rate.php?user=" + Global.username);
                var userratinglist = JsonConvert.DeserializeObject<System.Collections.Generic.List<UserRating>>(userrating);

                if (userratinglist.Count > 0)
                {
                    bool Isset = false;
                    foreach(UserRating ur in userratinglist)
                    {
						if (ur.recept == recipe.ID)
						{
                            Isset = true;
							rate_form_rating.IsEnabled = false;
                            rate_form_rating.Text = ur.beoordeling;
							if (averagejson[0].beoordeling != null) { totalrate = averagejson[0].beoordeling.Substring(0, 4); } else { totalrate = "N/R"; };
							rate_form_resultlabel.Text = "Average Score: " + totalrate;
							rate_form_button.Text = "Remove Rating";
							rate_form_button.Command = new Command(() => {remove_rating(); });
							break;
						}
                    }
                    if(!Isset)
                    {
						rate_form_rating.IsEnabled = true;
                        rate_form_rating.Text = "";
                        rate_form_rating.Placeholder = "Score 1-10";
						rate_form_button.Command = new Command(() => { insert_rating(); });
						if (averagejson[0].beoordeling != null) { totalrate = averagejson[0].beoordeling.Substring(0, 4); } else { totalrate = "N/R"; };
						rate_form_resultlabel.Text = "Average Score: " + totalrate;               
                    }
                }
				else
				{
					rate_form_rating.IsEnabled = true;
                    rate_form_rating.Text = "";
                    rate_form_rating.Placeholder = "Score 1-10";
					rate_form_button.Command = new Command(() => { insert_rating(); });
					if (averagejson[0].beoordeling != null) { totalrate = averagejson[0].beoordeling.Substring(0, 4); } else { totalrate = "N/R"; };
					rate_form_resultlabel.Text = "Average Score: " + totalrate;
				}
            }
            else
            {

                rate_form_rating.IsEnabled = false;
                rate_form_rating.Text = "Log in to rate";
                rate_form_button.Command = new Command(async () => {await DisplayAlert("Error","You're not logged in yet", "Cancel");});
                if (averagejson[0].beoordeling != null) { totalrate = averagejson[0].beoordeling.Substring(0, 4); } else { totalrate = "N/R"; };
                rate_form_resultlabel.Text = "Average Score: " + totalrate;
            }
        }

    }
}
