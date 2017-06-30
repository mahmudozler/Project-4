using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace RecipeApp
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }
        void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 20)
            {
                EntryInput.Text = EntryInput.Text.Remove(20);
            }
        }
        void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 20)
            {
                PasswordInput1.Text = PasswordInput1.Text.Remove(20);
            }
        }
        void Password2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 20)
            {
                PasswordInput2.Text = PasswordInput2.Text.Remove(20);
            }
        }
    }
    
}
