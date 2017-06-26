using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetail : MasterDetailPage
    {
        public MasterDetail()
        {
            InitializeComponent();

            Detail = new NavigationPage(new MainPage());
        }

        private void MenuItem1_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new MainPage());

            IsPresented = false;
        }

        private void MenuItem2_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new BrowsePage());

            IsPresented = false;
        }

        private void MenuItem3_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new AccountPage());

            IsPresented = false;
        }
    }
}