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
    public partial class BrowsePage : ContentPage
    {
        List<Recipe> recipes = new List<Recipe>();

        public BrowsePage()
        {
            InitializeComponent();
            this.CategoryPicker.SelectedIndex = 0;
        }

        private async void Search(object sender, EventArgs e)
		{
            lay.Children.Clear();

            Entry textbox = (Entry)sender;
            string text = textbox.Text;
            string category = this.CategoryPicker.Items[this.CategoryPicker.SelectedIndex];
            string inputstring = "input=";

            if(category == "All")
            {
                inputstring += text;
            }
            else
            {
                inputstring += text + "&category=" + category;
            }

            var response = await getData(inputstring);
            var records = JsonConvert.DeserializeObject<List<Recipe>>(response);
            while (records.GetNext().Visit(item => true, () => false))
			{
                lay.Children.Add(new Label { Text = records.GetCurrent().Visit(item => item.Title, ()=>""), TextColor = Color.Red, });
				lay.Children.Add(new BoxView { HeightRequest = 1, BackgroundColor = Color.Gray });
                lay.Children.Add(new Label { Text = records.GetCurrent().Visit(item => (string)item.Beschrijving, () => ""), TextColor = Color.Red });
                lay.Children.Add(new Image { Source = records.GetCurrent().Visit(item => item.Imagelink, () => "") });
			}


		}

        private async Task<String> getData(string str)
		{
			HttpClient client = new HttpClient();
			var response = await client.GetStringAsync("http://infpr04.esy.es/search.php?"+str);
			return response;
		}
    }
}
