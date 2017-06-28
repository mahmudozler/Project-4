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
            string text = textbox.Text;
            string category = this.CategoryPicker.Items[this.CategoryPicker.SelectedIndex];
            string inputstring = "input=";

            if (category == "All")
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
				var image = new Image { Source = records.GetCurrent().Visit(item => item.Imagelink, () => "") };
				image.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => recipe_clicked(records.GetCurrent().Visit(item => item.Title, () => "none"))) });
                grid.Children.Add(new Label { Text = records.GetCurrent().Visit(item => item.Title, () => ""), TextColor = Color.Red, FontAttributes = FontAttributes.Bold }, 1, records.Current);
                grid.Children.Add(new BoxView { HeightRequest = 1, BackgroundColor = Color.WhiteSmoke }, 1, records.Current);
                grid.Children.Add(new Label { Text = records.GetCurrent().Visit(item => (string)item.Beschrijving, () => ""), TextColor = Color.Black }, 1, records.Current);
                grid.Children.Add(image, 0, records.Current);
            }


        }

        private async Task<String> getData(string str)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://infpr04.esy.es/search.php?" + str);
            return response;
        }

		public void recipe_clicked(string recipe)
		{
			//await Navigation.PushAsync(new MainRecipePage(recipe));
            searchlabel.Text = recipe;
		}
    }
}
