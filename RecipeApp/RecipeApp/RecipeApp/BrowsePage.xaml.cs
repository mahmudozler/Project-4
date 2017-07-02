﻿﻿﻿using Xamarin.Forms;
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
            grid.Children.Clear();

            Entry textbox = (Entry)sender;
            string text;

            if (textbox.Text != null) { text = textbox.Text; } else { text = ""; }
            string category = this.CategoryPicker.Items[this.CategoryPicker.SelectedIndex];
            string inputstring = "input=";


            if(text.Contains(" "))
            {
                string[] tempstr = text.Split(' ');

				if (category == "All")
				{
                    foreach(string str in tempstr)
                    {
						inputstring += str+ "+";                        
                    }
				}
				else
				{
					foreach (string str in tempstr)
					{
						inputstring += str+"+";
					}
					inputstring += text + "&category=" + category;
				}
            }
            else
            {
				if (category == "All")
				{
					inputstring += text;
				}
				else
				{
					inputstring += text + "&category=" + category;
				}
            }

            var response = await getData(inputstring);
            List<Recipe> records = JsonConvert.DeserializeObject<List<Recipe>>(response);

            while (records.GetNext().Visit(item => true, () => false))
            {
				var image = new Image
				{
					index = records.Current,
					Source = records.GetCurrent().Visit(item => item.Imagelink, () => ""),HeightRequest = 100,VerticalOptions = LayoutOptions.End,Aspect = Aspect.AspectFill };
                TapGestureRecognizer tapgr = new TapGestureRecognizer();
                tapgr.Tapped += async (s, events) => { Image img = (Image)s; await Navigation.PushAsync(new MainRecipePage(records[img.index])); };
                image.GestureRecognizers.Add(tapgr);

                var bview = new BoxView { index = records.Current, HeightRequest = 1, BackgroundColor = Color.WhiteSmoke };
				TapGestureRecognizer tapgrtwo = new TapGestureRecognizer();
                tapgrtwo.Tapped += async (s, events) => { BoxView bv = (BoxView)s; await Navigation.PushAsync(new MainRecipePage(records[bv.index])); };
                bview.GestureRecognizers.Add(tapgrtwo);

                Grid innergrid = new Grid();
                innergrid.RowDefinitions.Add(new RowDefinition{Height=20});
                innergrid.RowDefinitions.Add(new RowDefinition { Height = 80});

				grid.Children.Add(bview, 1, records.Current);
                innergrid.Children.Add(new Label { Text = records.GetCurrent().Visit(item => item.Title, () => ""), TextColor = Color.Red, FontAttributes = FontAttributes.Bold }, 0, 0);
                innergrid.Children.Add(new Label { Text = records.GetCurrent().Visit(item => (string)item.Beschrijving, () => ""), TextColor = Color.Black }, 0, 1);
                grid.Children.Add(innergrid, 1, records.Current);
                grid.Children.Add(image, 0, records.Current);
            }

        }

        public class Image : Xamarin.Forms.Image
        {
            public int index { get; set; } = 0;

            public Image() : base()
            {}

        }

        public class BoxView : Xamarin.Forms.BoxView
		{
			public int index { get; set; } = 0;

            public BoxView() : base()
			{ }

		}


        private async Task<String> getData(string str)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://infpr04.heliohost.org/search.php?" + str);
            return response;
        }
    }
}
