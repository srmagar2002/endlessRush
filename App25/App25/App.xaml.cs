﻿using App25.Services;
using App25.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App25
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
