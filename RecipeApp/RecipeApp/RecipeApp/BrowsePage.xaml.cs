using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace RecipeApp
{
    public partial class BrowsePage : ContentPage
    {
        public BrowsePage()
        {
            InitializeComponent();
            this.CategoryPicker.SelectedIndex = 0;
        }

        //public void searchResult() {
        async void Search(object sender, EventArgs e)
			{
				var answer = await DisplayAlert("Question?", "Would you like some salt?", "Yes", "No");
			}
    }
}
