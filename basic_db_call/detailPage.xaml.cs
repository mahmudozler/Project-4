using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SQLform
{
    public partial class detailPage : ContentPage
    {
        public detailPage(SQLform.SQLformPage.RootObject recipe)
        //public detailPage()
        {
            InitializeComponent();
            main_recipe.Children.Add(new Label { Text = recipe.Title });
            main_recipe.Children.Add(new BoxView { BackgroundColor = Color.Gray });
            main_recipe.Children.Add(new Label { Text = recipe.Bereidingswijze });
            main_recipe.Children.Add(new Button
            {
                Text = "go back",
                Command = new Command(async o => { await App.Current.MainPage.Navigation.PopAsync(); })
            });
        }

        public void goBack() {
            App.Current.MainPage.Navigation.PopAsync();
        }

    }
}
