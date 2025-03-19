using App25.Services;
using App25.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App25.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaderboardPage : ContentPage
    {

        private readonly BitmapLoader _bitmapLoader;
        private readonly PixelFont _font;
        private ButtonSoundEffect _soundEffect;

        private readonly LBViewModel _lbViewModel;
        public LeaderboardPage()
        {
            _bitmapLoader = new BitmapLoader();
            _font = new PixelFont();
            _soundEffect = new ButtonSoundEffect();
            _lbViewModel = new LBViewModel();

            TestMethod();

            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _soundEffect.SetVolume(CurrentUser.User.SoundEffectsVol);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            AudioLoader.Instance.NonGamePageNavigation = true;
            AudioLoader.Instance.Pause();
        }
        private async Task TestMethod()
        {
            string hello = "";
            var users = await _lbViewModel.getLBData();

            foreach (var user in users)
            {
                hello += $"user:{user.Username}, score:{user.HighestScore} \n";
            }

            noway.Text = hello;
        }

    }
}