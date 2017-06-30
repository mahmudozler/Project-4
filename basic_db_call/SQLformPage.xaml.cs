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


namespace SQLform
{
    public partial class SQLformPage : ContentPage
    {
        public SQLformPage()
        {
            init();

            InitializeComponent();
        }

        /*private async void ButtonClick_1(object sender, System.EventArgs e)
		{
            //await getData();
            var x = await getData();
            button.Text = x;

		}*/

		public async void init()
		{
            //get the data from JSON link in record list
			var x = await getData();
            var records = JsonConvert.DeserializeObject<List<RootObject>>(x);
            foreach (var repice in records)
            {
                lay.Children.Add(new Label { Text = repice.Title,TextColor = Color.Red,  });
                lay.Children.Add(new BoxView{ HeightRequest = 1, BackgroundColor = Color.Gray });
                lay.Children.Add(new Label { Text = repice.Bereidingswijze, TextColor = Color.Red });
                lay.Children.Add(new Image { Source = "http://images.media-allrecipes.com/userphotos/600x600/402350.jpg" });
                lay.Children.Add(new Button { Text = "See recipe", Command = new Command(() => button_clicked((RootObject)repice)) });
            }

		}

        public async Task<String> getData() {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://infpr04.esy.es/recipe.php");
            //var result = response.Content.ReadAsStringAsync();

            return response;
        }

        public void button_clicked(RootObject repice) {
            App.Current.MainPage = new detailPage(repice);
        }

		public class RootObject
		{
			public string ID { get; set; }
			public string Title { get; set; }
			public object Beschrijving { get; set; }
			public string Ingredienten { get; set; }
			public object Voorbereiding { get; set; }
			public string Bereidingswijze { get; set; }
			public string Categorie { get; set; }
			public string Imagelink { get; set; }
			public object Valid { get; set; }
		}


    }
}
