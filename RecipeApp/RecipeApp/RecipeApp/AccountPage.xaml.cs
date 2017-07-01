using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountPage : ContentPage
	{
		public AccountPage ()
		{
			InitializeComponent ();
        }

        //Validate login attempt
		private async void getUser()
		{
			var client = new HttpClient();
			var data = await client.GetStringAsync("http://infpr04.esy.es/login.php?user=" + log_name.Text + "&pass=" + log_pass.Text);
			var user = JsonConvert.DeserializeObject<List<User>>(data);

			//Check if valid login
			if (user.Count == 0)
			{
				login_response.TextColor = Color.Red;
				login_response.Text = "Invalid login";
			}
			else
			{
				// If login matches user in db change status to logged in 
				Global.status = "logged_in";

				// User variables accesible throughout whole application 
				Global.username = user[0].username;
				Global.password = user[0].password;
				Global.admin = Int32.Parse(user[0].admin);

				login_response.TextColor = Color.Green;
				login_response.Text = "Succesfull login!";

				//login_page.Children.Clear();

				//login_page.Children.Add(new Button { Text = "go to account", Command = new Command(() => Application.Current.MainPage = new AccountPage()) });
			}
		}
    }
}