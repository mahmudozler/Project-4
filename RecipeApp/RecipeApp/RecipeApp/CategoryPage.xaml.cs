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
			foreach (var recipe in records)
			{
                var Clicklabel = new Label { Text = recipe.Title, TextColor = Color.Red, FontSize = 20 };
                Clicklabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => recipe_clicked(recipe)) });
				category_results.Children.Add(Clicklabel);
				category_results.Children.Add(new BoxView { HeightRequest = 1, BackgroundColor = Color.Gray });
                category_results.Children.Add(new Label { Text = recipe.Beschrijving.ToString(), TextColor = Color.Red });
				category_results.Children.Add(new Image { Source = recipe.Imagelink });
				//results.Children.Add(new Button { Text = "See recipe", Command = new Command(() => button_clicked((RootObject)repice)) });
			}

		}

		public async Task<String> getData(string category)
		{
			HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://infpr04.esy.es/search.php?input=&category=" + category);
			return response;
		}

		public void recipe_clicked(Recipe recipe)
		{
            Navigation.PushAsync(new MainRecipePage(recipe));
		}
    }
}
