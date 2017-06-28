using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RecipeApp
{
    public partial class MainPage : ContentPage
	{
        public MainPage()
        {
            InitializeComponent();;
        }

        private async void MainRecipe_Tapped(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new MainRecipePage());
        }

		private async void Category_Tapped(object sender, EventArgs e)
		{
            var CategoryObject = (Image)sender;
            //main_recip.Text = test.ClassId;
            await Navigation.PushAsync(new CategoryPage(CategoryObject.ClassId));
		}
    }
}
