using App25.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App25.Services;

namespace App25.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private SKBitmap _backgroundBitmap;
        private readonly BitmapLoader _bitmapLoader;
        private readonly AuthViewModel _viewModel;
        public static ImageSource logo = ImageSource.FromResource("App25.assets.others.ERlogo3D.png");

        public RegisterPage()
        {

            _bitmapLoader = new BitmapLoader();
            InitializeComponent();
            LoadBackground();
            _viewModel = new AuthViewModel();
        }

        private void LoadBackground()
        {
            string backgroundImage = "App25.assets.others.sunset2.png";
            _backgroundBitmap = _bitmapLoader.LoadBitmapFromResource(backgroundImage, this.GetType());
        }
        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            float width = e.Info.Width;
            float height = e.Info.Height;
            canvas.DrawBitmap(_backgroundBitmap, new SKRect(0, 0, width, height));
        }
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;
            string confirmPassword = confirmPasswordEntry.Text;

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match!", "OK");
                return;
            }

            bool isRegistered = await _viewModel.RegisterUser(username, password);

            if (isRegistered)
            {
                await DisplayAlert("Success", "Registration successful!", "OK");
                await Shell.Current.GoToAsync("//LoginPage");  // Go back to Login page
            }
            else
            {
                await DisplayAlert("Error", "Registration failed!", "OK");
            }
        }

        private async void OnBackToLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }


    }
}