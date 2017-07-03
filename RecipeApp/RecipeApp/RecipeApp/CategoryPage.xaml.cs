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
    public partial class CategoryPage : ContentPage
    {
        public CategoryPage(string category)
        {
            //var category_name = _category;
            InitialiseRecipes(category);
            InitializeComponent();
        }

        public async void InitialiseRecipes(string category)
        {
            //get the data from JSON link in record list
            var response = await getData(category);
            var records = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);
            var counter = 0;
            foreach (var recipe in records)
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
                if(recipe.Beschrijving.ToString().Length > 120) {
                    beschrijving = recipe.Beschrijving.ToString().Substring(0, 120) + "...";
                } else {
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

        public async Task<String> getData(string category)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("145.24.222.221/search.php?input=&category=" + category);
            return response;
        }

        public void recipe_clicked(Recipe recipe)
        {
            Navigation.PushAsync(new MainRecipePage(recipe));
        }
    }
}
