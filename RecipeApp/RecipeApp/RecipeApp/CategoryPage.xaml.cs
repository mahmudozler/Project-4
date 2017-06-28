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
			System.Collections.Generic.List<Recipe> records = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);
			foreach (var repice in records)
			{
				category_results.Children.Add(new Label { Text = repice.Title, TextColor = Color.Red, FontSize = 20 });
				category_results.Children.Add(new BoxView { HeightRequest = 1, BackgroundColor = Color.Gray });
                category_results.Children.Add(new Label { Text = repice.Beschrijving.ToString(), TextColor = Color.Red });
				category_results.Children.Add(new Image { Source = repice.Imagelink });
				//results.Children.Add(new Button { Text = "See recipe", Command = new Command(() => button_clicked((RootObject)repice)) });
			}

		}

		public async Task<String> getData(string category)
		{
			HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://infpr04.esy.es/search.php?input=&category=" + category);
			return response;
		}
    }
}
