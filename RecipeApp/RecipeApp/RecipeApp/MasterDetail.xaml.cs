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

            Detail = new NavigationPage(CreateWindow.Create(1));
        }

        private void MenuItem1_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(CreateWindow.Create(1));

            IsPresented = false;
        }

        private void MenuItem2_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(CreateWindow.Create(2));

            IsPresented = false;
        }

        private void MenuItem3_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(CreateWindow.Create(3));

            IsPresented = false;
        }
    }
}