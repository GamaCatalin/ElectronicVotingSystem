using EVT_FrontendApp.ViewModels;
using EVT_FrontendApp.Views;
using System;
using System.Collections.Generic;
using System.Net;
using EVT_FrontendApp.Services;
using Xamarin.Forms;

namespace EVT_FrontendApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
        }
    }
}
