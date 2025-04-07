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
using App25.Models;

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

        private SKBitmap backBitmap;

        private float backX;
        private float backY;
        private float backlength;

        private bool isBackPressed;

        private float emblemLength;
        private SKBitmap crownBitmap;

        private float rankHeaderX, unameHeaderX, scoreHeaderX;

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

            emblemLength = 125;
            isBackPressed = false;

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

            string backbutton = "App25.assets.others.buttons.backbutton.backbutton.png";
            backBitmap = _bitmapLoader.LoadBitmapFromResource(backbutton, this.GetType());
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


            float backBitmapLength = 120;
            float maxBackLength = 130;

            backlength = isBackPressed ? maxBackLength : backBitmapLength;
            backX = isBackPressed ? 30 - (maxBackLength - backBitmapLength) / 2 : 30;
            backY = titleY + titleHeight / 2 - (isBackPressed ? maxBackLength : backBitmapLength) / 2;

            canvas.DrawBitmap(backBitmap, new SKRect(backX, backY, backX + backlength, backY + backlength));

            var backChar = new SKPaint
            {
                Typeface = _font.LoadCustomfont(),
                TextSize = 80,
                Color = isBackPressed ? SKColors.White : SKColor.Parse("005500"),
                TextAlign = SKTextAlign.Center
            };

            float charX = backX + backlength / 2;
            float charY = backY + backlength / 2 + backChar.TextSize / 3;
            canvas.DrawText("<", charX, charY, backChar);

            // LeaderBoard
            //1414x106

            rowWidth = 2020;
            rowHeight = 159;
            rowX = width / 2 - rowWidth / 2;
            rowY = titleY + titleHeight + 100;

            float headerY = rowY - 15;

            rankHeaderX = rowX + rowWidth / 9;
            unameHeaderX = rowX + rowWidth / 3 + rowWidth / 8;
            scoreHeaderX = rowX + rowWidth - rowWidth / 8;

            var tableHeader = new SKPaint
            {
                Typeface = _font.LoadCustomfont(),
                TextAlign = SKTextAlign.Center,
                TextSize = 60
            };
            canvas.DrawText("Rank", rankHeaderX, headerY, tableHeader);
            canvas.DrawText("Username", unameHeaderX, headerY, tableHeader);
            canvas.DrawText("Score", scoreHeaderX, headerY, tableHeader);

            if (users.Count > 0 && users[0].HighestScore > 0)
            {

                RowRenderer(canvas, 0, 0, firstRowBitmap, firstEmbBitmap, "504416");

                if (users.Count > 1)
                {
                    if (users[1].HighestScore > 0)
                    {
                        RowRenderer(canvas, 1, 220, secondRowBitmap, secondEmbBitmap, "333333");
                    }
                }
                if (users.Count > 2)
                {
                    if (users[2].HighestScore > 0)

                    {
                        RowRenderer(canvas, 2, 440, thirdRowBitmap, thirdEmbBitmap, "552200");
                    }
                }
                if (users.Count > 3)
                {
                    if (users[3].HighestScore > 0)
                    {
                        RowRenderer(canvas, 3, 660, normalRowBitmap, null, "333333");
                    }
                }

                if (users.Count > 4)
                {
                    if (users[4].HighestScore > 0)
                    {
                        RowRenderer(canvas, 4, 880, normalRowBitmap, null, "333333");
                    }
                    //canvas.DrawText(users[0].Username, rowX, rowY, new SKPaint { Color = SKColors.Black, TextSize = 100 });
                }
            }
        }
        private void RowRenderer(SKCanvas canvas, int user, float heightDiff, SKBitmap rowBitmap, SKBitmap embBitmap, string fontColor)
        {
            float rowYSp = rowY + heightDiff;

            float embX = rankHeaderX - emblemLength / 2;
            float embY = rowYSp + rowHeight / 2 - emblemLength / 2;

            canvas.DrawBitmap(rowBitmap, new SKRect(rowX, rowYSp, rowX + rowWidth, rowYSp + rowHeight));
            if (user == 0)
            {
                //106*75
                canvas.Save();
                float crownWidth = 169;
                float crownHeight = 120;

                float cx = rowX;
                float cy = rowYSp;

                canvas.Translate(cx, cy);
                canvas.RotateDegrees(-45f);


                canvas.DrawBitmap(crownBitmap, new SKRect(-crownWidth / 2, -crownHeight / 2, crownWidth / 2, crownHeight / 2));
                canvas.Restore();
            }
            if (embBitmap != null)
            {
                canvas.DrawBitmap(embBitmap, new SKRect(embX, embY, embX + emblemLength, embY + emblemLength));
            }
            var tableData = new SKPaint
            {
                Typeface = _font.LoadCustomfont(),
                TextAlign = SKTextAlign.Center,
                TextSize = 80
            };

            tableData.Color = SKColor.Parse($"{fontColor}");
            canvas.DrawText(users[user].Username, unameHeaderX, rowYSp + rowHeight / 2 + tableData.TextSize / 3, tableData);
            canvas.DrawText(users[user].HighestScore.ToString(), scoreHeaderX, rowYSp + rowHeight / 2 + tableData.TextSize / 3, tableData);
            canvas.DrawText($"{user + 1}", rankHeaderX, rowYSp + rowHeight / 2 + tableData.TextSize / 3, new SKPaint
            {
                Typeface = _font.LoadCustomfont(),
                TextAlign = SKTextAlign.Center,
                TextSize = 100,
                Color = tableData.Color
            });


        }

        private async Task getUserScore()
        {
            users = await _lbViewModel.getLBData();

        }

        private void OnTouch(object sender, SKTouchEventArgs e)
        {
            bool isBackHovered = e.Location.X >= backX &&
                                e.Location.X <= backX + backlength &&
                                e.Location.Y >= backY &&
                                e.Location.Y <= backY + backlength;

            if (e.ActionType == SKTouchAction.Pressed && isBackHovered)
            {
                _soundEffect.Play();
                isBackPressed = true;
                canvasView.InvalidateSurface();
                Navigation.PopAsync();
            }


            if (e.ActionType == SKTouchAction.Cancelled || e.ActionType == SKTouchAction.Released || (e.ActionType == SKTouchAction.Moved && !isBackHovered))
            {
                isBackPressed = false;
                canvasView.InvalidateSurface();
            }

            e.Handled = true;
        }

    }
}