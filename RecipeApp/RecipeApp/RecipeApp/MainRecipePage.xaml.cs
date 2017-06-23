﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainRecipePage : ContentPage
    {
        public MainRecipePage()
        {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);

            IngredientsLabel.Text = "1. water\n2. zout";
        }
    }
}