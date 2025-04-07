using App25.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using App25.Services;
using App25.Models;
namespace App25.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly ButtonSoundEffect _buttonSoundEffect = new ButtonSoundEffect();
        private SKBitmap _backgroundBitmap;
        private readonly BitmapLoader _bitmapLoader;
        private readonly AuthViewModel _viewModel;
        public static ImageSource logo = ImageSource.FromResource("App25.assets.others.ERlogo3D.png");

        public LoginPage()
        {
            _bitmapLoader = new BitmapLoader();
            InitializeComponent();
            LoadBackground();
            _viewModel = new AuthViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CurrentUser.User = null;
        }

        private void LoadBackground() //loads background
        {
            string backgroundImage = "App25.assets.others.sunset1.png";
            _backgroundBitmap = _bitmapLoader.LoadBitmapFromResource(backgroundImage, this.GetType());
        }

        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            float width = e.Info.Width;
            float height = e.Info.Height;

            /*            string test = "";
                        if (CurrentUser.User == null)
                        {
                            test = "hello";
                        }
                        else
                        {
                            test = CurrentUser.User.Username.ToString();
                        }
            */
            canvas.DrawBitmap(_backgroundBitmap, new SKRect(0, 0, width, height));

            //canvas.DrawText(test, 10, 100, new SKPaint { TextSize = 70, Color = SKColors.White });
        }
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            _buttonSoundEffect.Play();
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;

            bool isValid = await _viewModel.LoginUser(username, password);

            if (isValid)
            {
                await DisplayAlert("Success", "Login successful", "OK");
                await Shell.Current.GoToAsync("about");
                usernameEntry.Text = string.Empty;
                passwordEntry.Text = string.Empty;
            }
            else
                await DisplayAlert("Error", "Invalid username or password", "OK");
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            _buttonSoundEffect.Play();
            usernameEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;
            await Shell.Current.GoToAsync("register");
        }


    }
}