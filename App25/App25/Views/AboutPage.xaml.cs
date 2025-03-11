using System;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using App25.Services;

namespace App25.Views
{
    //37 image used
    public partial class AboutPage : ContentPage
    {
        BitmapLoader _bitmapLoader;
        private readonly ButtonSoundEffect _buttonSoundEffect;
        private readonly AudioLoader _audioLoader = new AudioLoader();
        private SKCanvasView canvasView;
        private SKCanvasView settingView;
        private SKBitmap homebackgroundBitmap, EndlessRunLogoBitmap, startButtonBitmap, settingButtonBitmap, charButtonBitmap, lbBitmap;
        private SKBitmap startButtonWhiteBitmap, settingButtonWhiteBitmap, charButtonWhiteBitmap, lbWhiteBitmap;
        private SKBitmap extraBitmap;
        private float buttonX, buttonY, buttonWidth, buttonHeight;
        private bool isStartPressed { get; set; }

        private readonly PixelFont _pixelFont;
        private readonly SKTypeface font;

        private float settingbuttonWidth { get; set; }
        private float settingbuttonHeight { get; set; }
        private float settingbuttonX { get; set; }
        private float settingbuttonY { get; set; }
        private bool isSettingPressed { get; set; }

        private float charbuttonWidth { get; set; }
        private float charbuttonHeight { get; set; }
        private float charbuttonX { get; set; }
        private float charbuttonY { get; set; }
        private bool isCharPressed { get; set; }

        private float lbbuttonWidth { get; set; }
        private float lbbuttonHeight { get; set; }
        private float lbbuttonX { get; set; }
        private float lbbuttonY { get; set; }
        private bool isLBPressed { get; set; }

        private float logoYOffset = 0;
        private float animationTime = 0;
        private const float animationSpeed = 0.03f; // Adjust for speed of movement
        private const float animationAmplitude = 25f;
        public AboutPage()
        {
            canvasView = new SKCanvasView();

            canvasView.PaintSurface += OnPaintSurface;
            canvasView.EnableTouchEvents = true;
            canvasView.Touch += OnTouch;


            settingView = new SKCanvasView();
            settingView.PaintSurface += SettingView_PaintSurface; ;


            _audioLoader = new AudioLoader();
            _buttonSoundEffect = new ButtonSoundEffect();

            _pixelFont = new PixelFont();
            font = _pixelFont.LoadCustomfont();
            _bitmapLoader = new BitmapLoader();

            LoadAssets();
            Content = canvasView;
            HomePageTick();


        }

        private void SettingView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetNavBarIsVisible(this, false); // Turns off the default navigation bar
            canvasView.InvalidateSurface();
            MenuMenuTheme();
        }

        private void MenuMenuTheme()
        {
            _audioLoader.LoadAudio(3);
            _audioLoader.Play();
        }

        private void HomePageTick() // tick for the logo animation
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(33), () =>
            {
                animationTime += animationSpeed;
                logoYOffset = (float)(Math.Sin(animationTime) * animationAmplitude);
                canvasView.InvalidateSurface();
                return true;
            });
        }
        private void LoadAssets() // loads image assets
        {
            string homebackgroundassets = "App25.assets.others.homebackground.png";
            homebackgroundBitmap = _bitmapLoader.LoadBitmapFromResource(homebackgroundassets, this.GetType());

            string erlogoassets = "App25.assets.others.ERlogo3D.png";
            EndlessRunLogoBitmap = _bitmapLoader.LoadBitmapFromResource(erlogoassets, this.GetType());

            string startButtonassets = "App25.assets.others.buttons.startbutton.startbutton.png";
            string startButtonWhiteassets = "App25.assets.others.buttons.startbutton.startbuttonactive.png";
            startButtonBitmap = _bitmapLoader.LoadBitmapFromResource(startButtonassets, this.GetType());
            startButtonWhiteBitmap = _bitmapLoader.LoadBitmapFromResource(startButtonWhiteassets, this.GetType());

            string settingButtonassets = "App25.assets.others.buttons.settingbutton.settingbutton.png";
            string settingButtonWhiteassets = "App25.assets.others.buttons.settingbutton.settingbuttonactive.png";
            settingButtonBitmap = _bitmapLoader.LoadBitmapFromResource(settingButtonassets, this.GetType());
            settingButtonWhiteBitmap = _bitmapLoader.LoadBitmapFromResource(settingButtonWhiteassets, this.GetType());

            string charButtonassets = "App25.assets.others.buttons.charbutton.customizebutton.png";
            string charButtonWhiteassets = "App25.assets.others.buttons.charbutton.customizebuttonactive.png";
            charButtonBitmap = _bitmapLoader.LoadBitmapFromResource(charButtonassets, this.GetType());
            charButtonWhiteBitmap = _bitmapLoader.LoadBitmapFromResource(charButtonWhiteassets, this.GetType());

            string lbButtonassets = "App25.assets.others.buttons.leaderboardbutton.lb.png";
            string lbWhiteassets = "App25.assets.others.buttons.leaderboardbutton.lbwhite.png";
            lbBitmap = _bitmapLoader.LoadBitmapFromResource(lbButtonassets, this.GetType());
            lbWhiteBitmap = _bitmapLoader.LoadBitmapFromResource(lbWhiteassets, this.GetType());

            string extraasset = "App25.assets.others.Extra.png";
            extraBitmap = _bitmapLoader.LoadBitmapFromResource(extraasset, this.GetType());

        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            int width = e.Info.Width;
            int height = e.Info.Height;

            canvas.Clear(SKColors.White);

            buttonWidth = 600;
            buttonHeight = 300;
            buttonX = (width - buttonWidth) / 2;
            buttonY = height / 2 + 100;

            if (homebackgroundBitmap != null)
            {
                canvas.DrawBitmap(homebackgroundBitmap, new SKRect(0, 0, width, height));
            }
            else
            {
                canvas.Clear(SKColors.SkyBlue);
            }

            if (EndlessRunLogoBitmap != null)
            {
                float logoWidth = 1680;
                float logoHeight = 800;
                float logoX = (width - logoWidth) / 2;
                float logoY = 50 + logoYOffset;
                canvas.DrawBitmap(EndlessRunLogoBitmap, new SKRect(logoX, logoY, logoX + logoWidth, logoY + logoHeight));
            }

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Blue,
                IsAntialias = true,

            })
            {
                if (startButtonBitmap != null)
                {
                    if (isStartPressed)
                    {
                        canvas.DrawBitmap(startButtonWhiteBitmap, new SKRect(buttonX, buttonY, buttonX + buttonWidth, buttonY + buttonHeight));
                    }
                    else
                    {
                        canvas.DrawBitmap(startButtonBitmap, new SKRect(buttonX, buttonY, buttonX + buttonWidth, buttonY + buttonHeight));
                    }
                }
                else
                {
                    canvas.DrawRoundRect(buttonX, buttonY, buttonWidth, buttonHeight, 20, 20, paint);
                }
            }



            var textPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = isStartPressed ? SKColors.White : SKColor.Parse("002255"),
                IsAntialias = true,
                TextAlign = SKTextAlign.Center,
                TextSize = 140,
                Typeface = font
            };

            float textX = buttonX + buttonWidth / 2;
            float textY = buttonY + buttonHeight / 2 + (textPaint.TextSize / 3);
            canvas.DrawText("START", textX, textY, textPaint);


            canvas.DrawText($"Hi, {CurrentUser.User.Username}", 10, 80, new SKPaint { Color = SKColors.Black, TextSize = 70, Typeface = font });


            //setting button
            var settingtextPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = isSettingPressed ? SKColors.White : SKColor.Parse("333333"),
                IsAntialias = true,
                TextAlign = SKTextAlign.Center,
                TextSize = 70,
                Typeface = font
            };
            settingbuttonX = buttonX - width / 3;
            settingbuttonY = buttonY + height / 3 - 200;
            settingbuttonWidth = 560;
            settingbuttonHeight = 160;
            float settingtextX = settingbuttonX + settingbuttonWidth - 40 - settingbuttonWidth / 3;
            float settingtextY = settingbuttonY + settingbuttonHeight / 2 + (settingtextPaint.TextSize / 3);

            if (settingButtonBitmap != null)
            {
                if (isSettingPressed)
                {
                    canvas.DrawBitmap(settingButtonWhiteBitmap, new SKRect(settingbuttonX, settingbuttonY, settingbuttonX + settingbuttonWidth, settingbuttonY + settingbuttonHeight));
                }
                else
                {
                    canvas.DrawBitmap(settingButtonBitmap, new SKRect(settingbuttonX, settingbuttonY, settingbuttonX + settingbuttonWidth, settingbuttonY + settingbuttonHeight));
                }
            }
            else
            {
                canvas.DrawRoundRect(settingbuttonX, settingbuttonY, settingbuttonWidth, settingbuttonHeight, 20, 20, new SKPaint { Color = SKColors.Gray });
            }
            canvas.DrawText("settings", settingtextX, settingtextY, settingtextPaint);

            //character select

            var chartextPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = isCharPressed ? SKColors.White : SKColor.Parse("2d1650"),
                IsAntialias = true,
                TextAlign = SKTextAlign.Center,
                TextSize = 70,
                Typeface = font
            };

            charbuttonWidth = 560;
            charbuttonHeight = 160;
            charbuttonX = width / 2 - charbuttonWidth / 2;
            charbuttonY = buttonY + height / 3 - 100;
            float chartextX = charbuttonX + charbuttonWidth - 40 - charbuttonWidth / 3;
            float chartextY = charbuttonY + charbuttonHeight / 2 + (chartextPaint.TextSize / 3);

            if (charButtonBitmap != null || charButtonWhiteBitmap != null)
            {
                if (isCharPressed)
                {
                    canvas.DrawBitmap(charButtonWhiteBitmap, new SKRect(charbuttonX, charbuttonY, charbuttonX + charbuttonWidth, charbuttonY + charbuttonHeight));
                }
                else
                {
                    canvas.DrawBitmap(charButtonBitmap, new SKRect(charbuttonX, charbuttonY, charbuttonX + charbuttonWidth, charbuttonY + charbuttonHeight));
                }
            }
            else
            {
                canvas.DrawRoundRect(charbuttonX, charbuttonY, charbuttonWidth, charbuttonHeight, 20, 20, new SKPaint { Color = SKColors.Black });
            }
            canvas.DrawText("customize", chartextX, chartextY, chartextPaint);

            //leaderboard

            var lbtextPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = isLBPressed ? SKColors.White : SKColor.Parse("504416"),
                IsAntialias = true,
                TextAlign = SKTextAlign.Center,
                TextSize = 68,
                Typeface = font
            };

            lbbuttonWidth = 560;
            lbbuttonHeight = 160;
            lbbuttonX = width - width / 4;
            lbbuttonY = buttonY + height / 3 - 200;
            float lbtextX = lbbuttonX + lbbuttonWidth - 50 - lbbuttonWidth / 3;
            float lbtextY = lbbuttonY + lbbuttonHeight / 2 + (lbtextPaint.TextSize / 3);

            if (lbBitmap != null)
            {
                if (isLBPressed)
                {
                    canvas.DrawBitmap(lbWhiteBitmap, new SKRect(lbbuttonX, lbbuttonY, lbbuttonX + lbbuttonWidth, lbbuttonY + lbbuttonHeight));
                }
                else
                {
                    canvas.DrawBitmap(lbBitmap, new SKRect(lbbuttonX, lbbuttonY, lbbuttonX + lbbuttonWidth, lbbuttonY + lbbuttonHeight));
                }
            }
            else
            {
                canvas.DrawRoundRect(lbbuttonX, lbbuttonY, lbbuttonWidth, lbbuttonHeight, 20, 20, new SKPaint { Color = SKColors.Black });
            }
            canvas.DrawText("leaderboard", lbtextX, lbtextY, lbtextPaint);


            float extraWidth = 960;
            float extraHeight = 480;
            float extraX = width - (extraWidth / 5 + extraWidth / 2);
            float extraY = extraHeight / 10;

            canvas.DrawBitmap(extraBitmap, new SKRect(extraX, extraY, extraWidth + extraX, extraY + extraHeight));

            var scoretextPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 120,
                Typeface = font
            };
            var scorelabelPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 50,
                Typeface = font
            };
            float scoretextX = extraX + ((width - extraX) / 2) - extraWidth / 10;
            float scoretextY = extraY + extraHeight / 2 + scoretextPaint.TextSize;

            canvas.DrawText("Highest Score:", scoretextX - 40, scoretextY - scoretextPaint.TextSize, scorelabelPaint);

            canvas.DrawText($"{CurrentUser.User.HighestScore}", scoretextX, scoretextY, scoretextPaint);



        }

        private void OnTouch(object sender, SKTouchEventArgs e) // touch event for the buttons
        {
            //Console.WriteLine($"{buttonX} , {buttonY}, {buttonWidth}, {buttonHeight}");

            bool startButtonPressed = e.Location.X >= buttonX && e.Location.X <= buttonX + buttonWidth &&
                                      e.Location.Y >= buttonY && e.Location.Y <= buttonY + buttonHeight;

            bool customizeButtonPressed = e.Location.X >= charbuttonX && e.Location.X <= charbuttonX + charbuttonWidth &&
                                          e.Location.Y >= charbuttonY && e.Location.Y <= charbuttonY + charbuttonHeight;

            bool settingButtonPressed = e.Location.X >= settingbuttonX && e.Location.X <= settingbuttonX + settingbuttonWidth &&
                                        e.Location.Y >= settingbuttonY && e.Location.Y <= settingbuttonY + settingbuttonHeight;

            bool lbButtonPressed = e.Location.X >= lbbuttonX && e.Location.X <= lbbuttonX + lbbuttonWidth &&
                                   e.Location.Y >= lbbuttonY && e.Location.Y <= lbbuttonY + lbbuttonHeight;

            bool logout_Deletelater = e.Location.X >= 0 && e.Location.X <= 100 &&
                                     e.Location.Y >= 0 && e.Location.Y <= 100;
            bool test_Deletelater = e.Location.X >= 0 && e.Location.X <= 100 &&
                                   e.Location.Y >= 100 && e.Location.Y <= 200;

            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    if (startButtonPressed)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            _buttonSoundEffect.Play();
                            _audioLoader.Stop();
                            await Shell.Current.GoToAsync("game");
                        });

                        isStartPressed = true;
                        canvasView.InvalidateSurface();
                    }

                    if (customizeButtonPressed)
                    {
                        _buttonSoundEffect.Play();
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Shell.Current.GoToAsync("customize");

                        });
                        isCharPressed = true;
                        canvasView.InvalidateSurface();

                    }

                    if (settingButtonPressed)
                    {
                        _buttonSoundEffect.Play();
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //await Shell.Current.GoToAsync("setting");
                            await DisplayAlert("Settings", "Coming Soon", "OK");
                        });
                        isSettingPressed = true;
                        canvasView.InvalidateSurface();
                    }


                    if (lbButtonPressed)
                    {
                        _buttonSoundEffect.Play();
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //await  Shell.Current.GoToAsync("leaderboard");
                            await DisplayAlert("Leaderboard", "Coming Soon", "OK");
                        });
                        isLBPressed = true;
                        canvasView.InvalidateSurface();
                    }

                    if (logout_Deletelater)
                    {
                        _buttonSoundEffect.Play();
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            _audioLoader.Stop();
                            await Shell.Current.GoToAsync("//LoginPage");
                        });
                    }

                    if (test_Deletelater)
                    {
                        _buttonSoundEffect.Play();
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Shell.Current.GoToAsync("test");
                        });
                    }
                    break;

                case SKTouchAction.Released:
                case SKTouchAction.Cancelled:
                    isStartPressed = false;
                    isCharPressed = false;
                    isLBPressed = false;
                    isSettingPressed = false;
                    canvasView.InvalidateSurface();
                    break;

                case SKTouchAction.Moved:
                    if (!startButtonPressed && isStartPressed)
                    {
                        isStartPressed = false;
                        canvasView.InvalidateSurface();
                    }
                    if (!customizeButtonPressed && isCharPressed)
                    {
                        isCharPressed = false;
                        canvasView.InvalidateSurface();
                    }
                    if (!settingButtonPressed && isSettingPressed)
                    {
                        isSettingPressed = false;
                        canvasView.InvalidateSurface();
                    }
                    if (!lbButtonPressed && isLBPressed)
                    {
                        isLBPressed = false;
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
            e.Handled = true;
        }


    }
}
