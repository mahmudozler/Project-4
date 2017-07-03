using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace RecipeApp
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            press_register.Command = new Command(register_user);

            if(Global.status == "logged_in") {
                register_page.Children.Clear();
                StackLayout stack = new StackLayout { BackgroundColor=Color.FromHex("#dedede"),Padding= new Thickness(10) };
                register_page.Children.Add(stack);
                stack.Children.Add(new Label { Text = "You are already logged in", TextColor = Color.FromHex("#2b2b2b"), FontSize=17 });
                //register_page.Children.Add(new Button { Text = "Your account", Command = new Command(async() => await Navigation.PopAsync()) });
            }
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

        private async void register_user() {
            var client = new HttpClient();

            //check if user is in db
            var user_search = await client.GetStringAsync("http://145.24.222.221/login.php?user=" + EntryInput.Text + "&pass=" + PasswordInput2.Text);
            var search_result = JsonConvert.DeserializeObject<System.Collections.Generic.List<RegisterResponse>>(user_search);
            if (search_result.Count == 0) { //if chosen name not in db already, register new account
				await client.GetAsync("http://145.24.222.221/register.php?user=" + EntryInput.Text + "&pass=" + PasswordInput2.Text);

				// Action after succesfull registration
				await register_form.FadeTo(0, 1000);
				register_page.Children.Clear();
                var to_login_text = new Label { Text = "Congratulations! To login with your new account press the button below.", TextColor = Color.FromHex("#2b2b2b") };
				var to_login = new Button { Text = "login" };
				Navigation.InsertPageBefore(new AccountPage(), this);
				to_login.Command = new Command(() => Navigation.PopAsync());
				register_page.Children.Add(to_login_text);
				register_page.Children.Add(to_login);
            }
            else 
            {
				register_status.TextColor = Color.Red;
				register_status.Text = "register has failed";

            }

        }

	}
}
