using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace RecipeApp
{
    public partial class MainPage : ContentPage
	{
        public MainPage()
        {
            InitializeComponent();;
        }

        private async void MainRecipe_Tapped(object sender, EventArgs e)
            //display main recipe
        {
            var response = await getData();
            var featured_item = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);
            await Navigation.PushAsync(new MainRecipePage(featured_item[0]));
        }

		private async void Category_Tapped(object sender, EventArgs e)
            //display all items in selected category
		{
            var CategoryObject = (Image)sender;
            await Navigation.PushAsync(new CategoryPage(CategoryObject.ClassId));
		}

		public async Task<String> getData()
            //send request to server
		{
			HttpClient client = new HttpClient();
			var response = await client.GetStringAsync("http://145.24.222.221/recipe.php?id=23");
			return response;
		}
    }
}
