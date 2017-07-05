using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace RecipeWPFApp
{
    /// <summary>
    /// Interaction logic for AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        TextBox log_name = new TextBox();
        PasswordBox log_pass = new PasswordBox();
        Button login_press = new Button { Content = "login" };
        Button logout_press = new Button { Content = "logout", Margin = new Thickness(0, 0, 0, 35) };
        TextBlock login_response = new TextBlock();

        public AccountPage()
        {
            InitializeComponent();

            if (Global.status == "logged_in")
            {
                //initialise account page when user logs in
                init_accountpage();
            }
            else if (Global.status == "logged_out")
            {
                //initialise loginform
                init_loginform();
            }
        }

        private void init_loginform()
        {
            // Show login form to log in 
            accountpage.Children.Clear();
            accountpage.Children.Add(new TextBlock { Text = "Manage account", FontWeight = FontWeights.Bold, FontSize = 25, HorizontalAlignment = HorizontalAlignment.Center });
            accountpage.Children.Add(new Rectangle { Margin = new Thickness(0, 0, 0, 20), Height = 1, Fill = Brushes.LightGray });

            accountpage.Children.Add(new TextBlock { Text = "Username", Foreground = Brushes.DarkGray, Margin = new Thickness(5, 0, 0, 0), FontSize = 17 });
            accountpage.Children.Add(log_name);
            accountpage.Children.Add(new TextBlock { Text = "Password", Foreground = Brushes.DarkGray, Margin = new Thickness(5, 0, 0, 0), FontSize = 17 });
            accountpage.Children.Add(log_pass);

            accountpage.Children.Add(login_press);
            login_press.MouseLeftButtonDown += new MouseButtonEventHandler(getUser);
            accountpage.Children.Add(login_response);
        }

        private void init_accountpage()
        {
            // showing account details of logged in user 
            accountpage.Children.Clear();
            accountpage.Children.Add(new TextBlock { Text = "Your account", FontWeight = FontWeights.Bold, FontSize = 25, HorizontalAlignment = HorizontalAlignment.Center });
            accountpage.Children.Add(new Rectangle { Margin = new Thickness(0, 0, 0, 20), Height = 1, Fill = Brushes.LightGray });
            accountpage.Children.Add(new TextBlock { Text = "Username: " + Global.username });
            accountpage.Children.Add(new TextBlock { Text = "Account type: " + Global.admin.ToString() });
            accountpage.Children.Add(logout_press);
            logout_press.MouseLeftButtonDown += new MouseButtonEventHandler(logout);
            accountpage.Children.Add(new TextBlock { Text = "Your bookmarks", Foreground = Brushes.IndianRed, FontSize = 20, FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Center });
            var bookmarkline = new Rectangle { Margin = new Thickness(0, 0, 0, 10), Height = 1, Fill = Brushes.LightGray };
            accountpage.Children.Add(bookmarkline);

            //show the user's bookmarks
            SetUserBookmarks();
        }

        private async Task<Recipe> getRecipeById(string ID)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/recipe.php?id=" + ID);
            response = response.Substring(1, response.Length - 2);
            var recipe = JsonConvert.DeserializeObject<Recipe>(response);
            return recipe;
        }

        private async void SetUserBookmarks()
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/bookmark.php?user=" + Global.username);
            var bookmarks_fetch = JsonConvert.DeserializeObject<List<BookmarkItem>>(response);
            var list_bookmarks = new System.Collections.Generic.List<Recipe>();

            // fetch recipe object from bookmarks id list
            foreach (var recipe in bookmarks_fetch)
            {
                var rec = await getRecipeById(recipe.recept);
                list_bookmarks.Add(rec);
            }

            // set bookmarks recipe in list
            if (list_bookmarks.Count > 0)
            {
                foreach (var recipe in list_bookmarks)
                {
                    var button = new Button { Uid = recipe.ID, Content = recipe.Title, Margin = new Thickness(20, 20, 0, 0), Background = Brushes.IndianRed, Foreground = Brushes.White, FontStyle = FontStyles.Italic };
                    button.MouseLeftButtonDown += new MouseButtonEventHandler(bookmark_MouseLeftButtonDown);
                    accountpage.Children.Add(button);
                }
            }
            else
            {
                accountpage.Children.Add(new TextBlock { Text = "You have no bookmarks yet, go add some!" });
            }
        }

        private async void bookmark_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var recipeObject = (Image)sender;
            var response = await getData(recipeObject.Uid);
            var records = JsonConvert.DeserializeObject<System.Collections.Generic.List<Recipe>>(response);
            this.NavigationService.Navigate(new MainRecipePage(records[0]));
        }

        //Validate login attempt
        private async void getUser(object sender, MouseButtonEventArgs e)
        {
            var client = new HttpClient();
            var data = await client.GetStringAsync("http://145.24.222.221/login.php?user=" + log_name.Text + "&pass=" + log_pass.Password);
            var user = JsonConvert.DeserializeObject<List<User>>(data);

            //Check if valid login
            if (user.Count == 0)
            {
                login_response.Foreground = Brushes.Red;
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

                login_response.Foreground = Brushes.Green;
                login_response.Text = "Succesfull login!";

                InitializeComponent();
                //new NavigationPage(CreateWindow.Create(3));
            }
        }

        private void logout(object sender, MouseButtonEventArgs e)
        {
            // set status to logged out 
            Global.status = "logged_out";

            // unset the logged in user data
            Global.username = default(string);
            Global.password = default(string);
            Global.admin = default(int);

            // initialise login form again 
            init_loginform();
        }

        private async Task<String> getData(string id)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://145.24.222.221/recipe.php?id=" + id);
            return response;
        }
    }
}

