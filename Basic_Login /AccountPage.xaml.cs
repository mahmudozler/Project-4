using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SQLform
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
			InitializeComponent();

            if (Global.status == "logged_in")
			{

				InitializeComponent();
				user.Text = "user: " + Global.username;
				pass.Text = "password: " + Global.password;
				ad.Text = "admin: " + Global.admin;
			}
			else
			{
				status.Text = "Log in first to check account details";
			}
        }
    }
}
