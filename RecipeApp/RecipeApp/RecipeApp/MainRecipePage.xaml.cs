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
        public MainRecipePage(Recipe recipe)
        {
            InitializeComponent();

			// Fill in all fields with recipe data on initialization from parameter
            recipe_name.Text = recipe.Title;
            recipe_image.Source = recipe.Imagelink;
            recipe_beschrijving.Text = recipe.Beschrijving.ToString();
            recipe_instructions.Text = recipe.Bereidingswijze;

            var IngredientList = recipe.Ingredienten.Split(',').ToList();  //split string between to get ingredients seperate
            foreach (var ingredient in IngredientList)
            {
                recipe_ingredients.Children.Add(new Label { Text = ingredient, TextColor = Color.Black });
            }
        }

    }
}