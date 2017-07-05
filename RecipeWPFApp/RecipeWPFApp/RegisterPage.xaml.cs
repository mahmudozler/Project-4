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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();

            press_register.Click += register_user;

            if (Global.status == "logged_in")
            {
                register_page.Children.Clear();
                StackPanel stack = new StackPanel { Background = Brushes.WhiteSmoke };
                register_page.Children.Add(stack);
                stack.Children.Add(new TextBlock { Text = "You are already logged in", Foreground = Brushes.DarkGray, FontSize = 17 });
                //register_page.Children.Add(new Button { Text = "Your account", Command = new Command(async() => await Navigation.PopAsync()) });
            }
        }

        private void Entry_TextChanged(object sender, TextCompositionEventArgs e)
        {
            if (EntryInput.Text.Length > 20)
            {
                e.Handled = true;
            }
        }

        private void Password_TextChanged(object sender, TextCompositionEventArgs e)
        {
            if (PasswordInput1.Password.Length > 20)
            {
                e.Handled = true;
            }
        }

        private void Password2_TextChanged(object sender, TextCompositionEventArgs e)
        {
            if (PasswordInput2.Password.Length > 20)
            {
                e.Handled = true;
            }
        }

        private async void register_user(object system, RoutedEventArgs e)
        {
            var client = new HttpClient();
            if (PasswordInput1.Password == PasswordInput2.Password)
            {
                //check if user is in db
                var user_search = await client.GetStringAsync("http://145.24.222.221/register.php?user=" + EntryInput.Text + "&pass=" + PasswordInput2.Password);
                var search_result = JsonConvert.DeserializeObject<System.Collections.Generic.List<RegisterResponse>>(user_search);
                if (search_result[0].status)
                { //if chosen name not in db already, register new account

                    register_page.Children.Clear();
                    var to_login_text = new TextBlock { Text = "Congratulations! To login with your new account press the button below.", Foreground = Brushes.DarkGray };
                    var to_login = new Button { Content = "login" };
                    this.NavigationService.Navigate(new AccountPage());
                    register_page.Children.Add(to_login_text);
                    register_page.Children.Add(to_login);
                }
                else
                {
                    register_status.Foreground = Brushes.Red;
                    register_status.Text = "register has failed";

                }
            }
            else
            {
                register_status.Foreground = Brushes.Red;
                register_status.Text = "passwords are not the same";
            }
        }
    }
}
