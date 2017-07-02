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
            var user_search = await client.GetStringAsync("http://infpr04.heliohost.org/login.php?user=" + EntryInput.Text + "&pass=" + PasswordInput2.Text);
            var search_result = JsonConvert.DeserializeObject<System.Collections.Generic.List<RegisterResponse>>(user_search);
            if (search_result.Count == 0) {
				await client.GetAsync("http://infpr04.heliohost.org/register.php?user=" + EntryInput.Text + "&pass=" + PasswordInput2.Text);
				//response = response.Substring(1, response.Length - 2); //remove array brackets, because just 1 value
				//var register_response = JsonConvert.DeserializeObject<RegisterResponse>(response);
				register_status.TextColor = Color.Green;
				register_status.Text = "succesfully registered, you can login now";
            }
            else 
            {
				register_status.TextColor = Color.Red;
				register_status.Text = "register has failed";
            }

        }

	}
}
