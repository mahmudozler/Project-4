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
			if ((string)Application.Current.Properties["status"] == "logged_in")
			{
				var username = Application.Current.Properties["username"].ToString();
				var password = Application.Current.Properties["password"].ToString();
				var admin = Application.Current.Properties["admin"].ToString();

				InitializeComponent();
				user.Text = "user: " + username;
				pass.Text = "password: " + password;
				ad.Text = "admin: " + admin;
			}
			else
			{
				status.Text = "Log in first to check account details";
			}
        }
    }
}
