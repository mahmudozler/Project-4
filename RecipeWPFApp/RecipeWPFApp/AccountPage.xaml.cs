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
    }
}
