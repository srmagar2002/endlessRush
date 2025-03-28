using App25.Services;
using App25.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp.Views.Forms;
using App25.Data;

namespace App25.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaderboardPage : ContentPage
    {
        private readonly LBViewModel _lbViewModel;

        private SKCanvasView canvasView;
        private readonly BitmapLoader _bitmapLoader;
        private readonly PixelFont _font;
        private ButtonSoundEffect _soundEffect;

        private SKBitmap lbtitleboardbitmap;
        private SKBitmap firstRowBitmap, secondRowBitmap, thirdRowBitmap, normalRowBitmap; //Row Background
        private SKBitmap firstEmbBitmap, secondEmbBitmap, thirdEmbBitmap; //Emblem
        private SKBitmap crownBitmap;

        private SKBitmap testBitmap;

        private List<UserScoreDTO> users;

        private float rowWidth, rowHeight, rowX, rowY;
        private float titleWidth, titleHeight, titleX, titleY;
        public LeaderboardPage()
        {

            canvasView = new SKCanvasView();
            canvasView.PaintSurface += CanvasPaintSurface;
            canvasView.EnableTouchEvents = true;
            canvasView.Touch += OnTouch;

            _bitmapLoader = new BitmapLoader();
            _font = new PixelFont();
            _soundEffect = new ButtonSoundEffect();
            _lbViewModel = new LBViewModel();
            _font = new PixelFont();

            LoadAsset();
            LoadLBUsers();

            Content = canvasView;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetNavBarIsVisible(this, false);
            _soundEffect.SetVolume(CurrentUser.User.SoundEffectsVol);
            canvasView.InvalidateSurface();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            AudioLoader.Instance.NonGamePageNavigation = true;
            AudioLoader.Instance.Pause();
        }

        private async void LoadLBUsers()
        {
            await getUserScore();
        }
        private void LoadAsset()
        {
            string titleboard = "App25.assets.others.lbaccessories.lbtitleBoard.png";
            lbtitleboardbitmap = _bitmapLoader.LoadBitmapFromResource(titleboard, this.GetType());

            string firstRow = "App25.assets.others.lbaccessories.lbfirstrow.png";
            string secondRow = "App25.assets.others.lbaccessories.lbsecondrow.png";
            string thirdRow = "App25.assets.others.lbaccessories.lbthirdrow.png";
            string normalRow = "App25.assets.others.lbaccessories.lbnormalrow.png";
            firstRowBitmap = _bitmapLoader.LoadBitmapFromResource(firstRow, this.GetType());
            secondRowBitmap = _bitmapLoader.LoadBitmapFromResource(secondRow, this.GetType());
            thirdRowBitmap = _bitmapLoader.LoadBitmapFromResource(thirdRow, this.GetType());
            normalRowBitmap = _bitmapLoader.LoadBitmapFromResource(normalRow, this.GetType());

            string firstEmb = "App25.assets.others.lbaccessories.lb_empty_firstemb.png";
            string secondEmb = "App25.assets.others.lbaccessories.lb_empty_secondemb.png";
            string thirdEmb = "App25.assets.others.lbaccessories.lb_empty_thirdemb.png";
            firstEmbBitmap = _bitmapLoader.LoadBitmapFromResource(firstEmb, this.GetType());
            secondEmbBitmap = _bitmapLoader.LoadBitmapFromResource(secondEmb, this.GetType());
            thirdEmbBitmap = _bitmapLoader.LoadBitmapFromResource(thirdEmb, this.GetType());

            string crown = "App25.assets.others.lbaccessories.crown.png";
            crownBitmap = _bitmapLoader.LoadBitmapFromResource(crown, this.GetType());

            string homebackgroundassets = "App25.assets.others.homebackground.png";
            testBitmap = _bitmapLoader.LoadBitmapFromResource(homebackgroundassets, this.GetType());

        }
        private void CanvasPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            SKImageInfo info = e.Info;
            float width = info.Width;
            float height = info.Height;

            //title;
            //508x91
            //837x150
            titleWidth = 884;
            titleHeight = 159;
            titleX = width / 2 - titleWidth / 2;
            titleY = 10;

            canvas.DrawBitmap(testBitmap, new SKRect(0, 0, width, height));

            canvas.DrawBitmap(lbtitleboardbitmap, new SKRect(titleX, titleY, titleX + titleWidth, titleY + titleHeight));

            var titleText = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColor.Parse("504416"),
                IsAntialias = true,
                TextAlign = SKTextAlign.Center,
                TextSize = 70,
                Typeface = _font.LoadCustomfont(),

            };
            float titleTextX = titleX + titleWidth / 2;
            float titleTextY = titleY + titleHeight / 2 + titleText.TextSize / 3;

            canvas.DrawText("leaderboard", titleTextX, titleTextY, titleText);

            // LeaderBoard
            //1414x106

            rowWidth = 2020;
            rowHeight = 159;
            rowX = width / 2 - rowWidth / 2;
            rowY = titleY + titleHeight + 100;

            float headerY = rowY - 15;

            float rankHeaderX = rowX + rowWidth / 12;
            float unameHeaderX = rowX + rowWidth / 3 + rowWidth / 13;
            float scoreHeaderX = rowX + rowWidth - rowWidth / 6;

            var tableHeader = new SKPaint
            {
                Typeface = _font.LoadCustomfont(),
                TextSize = 60
            };
            canvas.DrawText("Rank", rankHeaderX, headerY, tableHeader);
            canvas.DrawText("Username", unameHeaderX, headerY, tableHeader);
            canvas.DrawText("Score", scoreHeaderX, headerY, tableHeader);

            if (users.Count > 0 && users[0].HighestScore > 0)
            {
                canvas.DrawBitmap(firstRowBitmap, new SKRect(rowX, rowY, rowX + rowWidth, rowY + rowHeight));

                var tableData = new SKPaint
                {

                };

                //canvas.DrawText(users[0].Username, rowX, rowY, new SKPaint { Color = SKColors.Black, TextSize = 100 });
            }


        }


        private async Task getUserScore()
        {
            users = await _lbViewModel.getLBData();
            Console.WriteLine("HEllo this is me " + users[0].Username + " " + users.Count + " " + users[1].HighestScore);
        }

        private void OnTouch(object sender, SKTouchEventArgs e)
        {
            if (e.ActionType == SKTouchAction.Pressed && e.Location.X >= 0 && e.Location.X <= 100 && e.Location.Y >= 0 && e.Location.Y <= 100)
            {
                Shell.Current.GoToAsync("about");
            }
        }



    }
}