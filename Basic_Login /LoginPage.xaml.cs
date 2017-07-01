using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace SQLform
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
		
			InitializeComponent();
            if(Global.status == "logged_in") { login_page.Children.Clear(); };
            tolog.Command = new Command(() => Navigation.PushAsync(new AccountPage()));
            login.Command = new Command(getUser);
        }


        // User login check
        private async void getUser()
        {
            var client = new HttpClient();
            var data = await client.GetStringAsync("http://infpr04.esy.es/login.php?user=" + log_name.Text + "&pass=" + log_pass.Text);
            var user = JsonConvert.DeserializeObject<List<User>>(data);

            //Check if valid login
            if (user.Count == 0)
            {
                log_status.TextColor = Color.Red;
                log_status.Text = "Invalid login";
            }
            else
            {
                // If login matches user in db change status to logged in 
                Global.status = "logged_in";

                // User variables accesible throughout whole application 
                Global.username = user[0].username;
                Global.password = user[0].password;
                Global.admin = Int32.Parse(user[0].admin);

                log_status.TextColor = Color.Green;
                log_status.Text = "Succesfull login!";

                login_page.Children.Clear();

                login_page.Children.Add(new Button { Text = "go to account", Command = new Command(() => Application.Current.MainPage = new AccountPage())});
            }
        }

		public class User
		{
			public string username { get; set; }
			public string password { get; set; }
			public string admin { get; set; }
		}
    }
}
