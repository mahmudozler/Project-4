﻿﻿﻿using System;
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
        // login references & initiation to be accesed later 
        Entry log_name = new Entry { Placeholder = "Fill in username" };
        Entry log_pass = new Entry { Placeholder = "Fill in password",IsPassword=true };
        Button login_press = new Button{ Text = "login" };
        Button logout_press = new Button { Text = "logout", Margin = new Thickness(0, 0, 0, 35) };
        Label login_response = new Label { };

		public AccountPage ()
		{
			InitializeComponent ();
            if (Global.status == "logged_in")
            {
                //initialise account page when user logs in
                init_accountpage();
            }
            else if(Global.status == "logged_out") 
            {
                //initialise loginform
                init_loginform();
            }
        }

        //Validate login attempt
		private async void getUser()
		{
			var client = new HttpClient();
			var data = await client.GetStringAsync("http://145.24.222.221/login.php?user=" + log_name.Text + "&pass=" + log_pass.Text);
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

                Navigation.InsertPageBefore(new AccountPage(), this);
                await Navigation.PopAsync();
                //new NavigationPage(CreateWindow.Create(3));
			}
		}

        private void logout() {
            // set status to logged out 
            Global.status = "logged_out";

            // unset the logged in user data
            Global.username = default(string);
            Global.password = default(string);
            Global.admin = default(int);

            // initialise login form again 
            init_loginform();
        }

        private void init_loginform() {
			// Show login form to log in 
			accountpage.Children.Clear();
			accountpage.Children.Add(new Label { Text = "Manage account", FontAttributes = FontAttributes.Bold, FontSize = 25, HorizontalTextAlignment = TextAlignment.Center });
			accountpage.Children.Add(new BoxView { Margin = new Thickness(0, 0, 0, 20), HeightRequest = 1, BackgroundColor = Color.LightGray });

			accountpage.Children.Add(new Label { Text = "Username", TextColor = Color.FromHex("#2b2b2b"), Margin = new Thickness(5, 0, 0, 0), FontSize = 17 });
			accountpage.Children.Add(log_name);
			accountpage.Children.Add(new Label { Text = "Password", TextColor = Color.FromHex("#2b2b2b"), Margin = new Thickness(5, 0, 0, 0), FontSize = 17 });
			accountpage.Children.Add(log_pass);

			accountpage.Children.Add(login_press);
			login_press.Command = new Command(getUser);
			accountpage.Children.Add(login_response);
        }

        private void init_accountpage() {
			// showing account details of logged in user 
			accountpage.Children.Clear();
			accountpage.Children.Add(new Label { Text = "Your account", FontAttributes = FontAttributes.Bold, FontSize = 25, HorizontalTextAlignment = TextAlignment.Center });
			accountpage.Children.Add(new BoxView { Margin = new Thickness(0, 0, 0, 20), HeightRequest = 1, BackgroundColor = Color.LightGray });
			accountpage.Children.Add(new Label { Text = "Username: " + Global.username });
			accountpage.Children.Add(new Label { Text = "Account type: " + Global.admin.ToString() });
            accountpage.Children.Add(logout_press);
            logout_press.Command = new Command(logout);
            accountpage.Children.Add(new Label { Text = "Your bookmarks", TextColor = Color.FromHex("A81313"),FontSize=20,FontAttributes=FontAttributes.Bold,HorizontalTextAlignment = TextAlignment.Center });
            var bookmarkline = new BoxView { Margin = new Thickness(0, 0, 0, 10), HeightRequest = 1, BackgroundColor = Color.LightGray };
            accountpage.Children.Add(bookmarkline);

            //show the user's bookmarks
            SetUserBookmarks();
        }

        private async Task<Recipe> getRecipeById(string ID) {
            var client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/recipe.php?id=" + ID);
            response = response.Substring(1, response.Length - 2);
            var recipe = JsonConvert.DeserializeObject<Recipe>(response);
            return recipe;
        }

        private async void SetUserBookmarks() {
            var client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/bookmark.php?user=" + Global.username);
            var bookmarks_fetch = JsonConvert.DeserializeObject<List<BookmarkItem>>(response);
            var list_bookmarks = new System.Collections.Generic.List<Recipe>();

            // fetch recipe object from bookmarks id list
            foreach (var recipe in bookmarks_fetch) {
                var rec = await getRecipeById(recipe.recept);
                list_bookmarks.Add(rec);
            }

            // set bookmarks recipe in list
            if (list_bookmarks.Count > 0)
            {
                foreach (var recipe in list_bookmarks)
                {
                    accountpage.Children.Add(new Button { Text = recipe.Title, Margin = new Thickness(20, 0), Command = new Command(() => Navigation.PushAsync(new MainRecipePage(recipe))), BackgroundColor = Color.FromHex("BD3E3E"), TextColor = Color.White, FontAttributes = FontAttributes.Italic });
                }
            }
            else 
            {
                accountpage.Children.Add(new Label { Text = "You have no bookmarks yet, go add some!" });
            }
		}
    }
}