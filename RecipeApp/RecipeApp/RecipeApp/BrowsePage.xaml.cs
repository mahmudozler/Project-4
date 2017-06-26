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
    }
}
