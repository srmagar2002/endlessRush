using App25.ViewModels;
using App25.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace App25
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("register", typeof(RegisterPage));
            Routing.RegisterRoute("about", typeof(AboutPage));
            Routing.RegisterRoute("game", typeof(GamePage));
            Routing.RegisterRoute("customize", typeof(CustomizePage));
            Routing.RegisterRoute("lb", typeof(LeaderboardPage));
            Routing.RegisterRoute("test", typeof(Test));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
