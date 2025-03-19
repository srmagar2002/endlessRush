using System;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using App25.Services;
using System.ComponentModel;
using App25.ViewModels;

namespace App25.Views
{
    //37 image used
    public partial class AboutPage : ContentPage, INotifyPropertyChanged
    {
        private AboutViewModel _aboutViewModel;
        private BitmapLoader _bitmapLoader;

        private readonly ButtonSoundEffect _buttonSoundEffect;
        private readonly AudioLoader _audioLoader = new AudioLoader();
        private SKCanvasView canvasView;
        private SKBitmap homebackgroundBitmap, EndlessRunLogoBitmap, startButtonBitmap, settingButtonBitmap, charButtonBitmap, lbBitmap;
        private SKBitmap startButtonWhiteBitmap, settingButtonWhiteBitmap, charButtonWhiteBitmap, lbWhiteBitmap;
        private SKBitmap extraBitmap;
        private SKBitmap logoutBitmap;

        private SKBitmap musicBitmap, noMusicBitmap, soundBitmap, noSoundBitmap, thumbBitmap;

        private float buttonX, buttonY, buttonWidth, buttonHeight;
        private bool isStartPressed { get; set; }

        private bool isSettingsOn { get; set; } = false;

        private float settingYOffset = 0;
        private float settingAnimationSpeed = 250f;
        private float settingYlimit { get; set; }

        private float settingWindowX;
        private float settingWindowY;
        private float settingWindowWidth;
        private float settingWindowHeight;
        private float settingInitialY;

        private float _musictrackstart;
        private float _musictrackend;
        private float _musictrackY;
        private float _trackheight = 50;
        private float _trackwidth;

        private bool wasmusicSLiderPressed = false;

        private float _musicthumbX;
        private float _thumbSize = 70;

        public float _musicSliderValue;
        public event PropertyChangedEventHandler PropertyChanged;

        public float MusicSliderValue
        {
            get => _musicSliderValue;
            set
            {
                if (_musicSliderValue != value)
                {
                    _musicSliderValue = value;
                    OnPropertyChanged(nameof(MusicSliderValue));
                    OnSliderValueChanged(_musicSliderValue);
                }
            }
        }

        private float _effecttrackstart;
        private float _effecttrackend;
        private float _effecttrackY;

        private bool wasEffectSliderPressed = false;

        private float _effectThumbX;

        public float _effectSliderValue;
        public float EffectSliderValue
        {
            get => _effectSliderValue;
            set
            {
                if (_effectSliderValue != value)
                {
                    _effectSliderValue = value;
                    OnPropertyChanged(nameof(EffectSliderValue));
                    OnEffectValueChanged(_effectSliderValue);
                }
            }
        }

        private float iconSize = 125;

        public float logoutX;
        public float logoutY;
        public float logoutWidth;
        public float logoutHeight;

        public bool isLogoutPressed { get; set; } = false;


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
            _aboutViewModel = new AboutViewModel();
            canvasView = new SKCanvasView();

            canvasView.PaintSurface += OnPaintSurface;
            canvasView.EnableTouchEvents = true;
            canvasView.Touch += OnTouch;

            _audioLoader = new AudioLoader();
            _buttonSoundEffect = new ButtonSoundEffect();

            _pixelFont = new PixelFont();
            font = _pixelFont.LoadCustomfont();
            _bitmapLoader = new BitmapLoader();

            MusicSliderValue = (float)CurrentUser.User.Music;
            EffectSliderValue = (float)CurrentUser.User.SoundEffectsVol;

            LoadAssets();
            Content = canvasView;
            HomePageTick();


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetNavBarIsVisible(this, false); // Turns off the default navigation bar
            canvasView.InvalidateSurface();

            ButtonSoundEffect.Instance.SetVolume(CurrentUser.User.SoundEffectsVol);
            MenuMenuTheme();
        }

        private void MenuMenuTheme()
        {
            if (!AudioLoader.Instance.NonGamePageNavigation)
            {
                AudioLoader.Instance.LoadAudio(this.GetType());
            }

            AudioLoader.Instance.Play();
            AudioLoader.Instance.SetVolume(CurrentUser.User.Music);
        }

        private void HomePageTick() // tick for the logo animation
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(16), () =>
            {

                animationTime += animationSpeed;
                logoYOffset = (float)(Math.Sin(animationTime) * animationAmplitude);

                if (isSettingsOn && settingWindowY >= settingYlimit)
                {
                    settingYOffset += settingAnimationSpeed;
                }

                if (!isSettingsOn && settingWindowY <= settingInitialY)
                {
                    settingYOffset -= settingAnimationSpeed;
                }
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

            string logoutasset = "App25.assets.others.buttons.quitbutton.quit.png";
            logoutBitmap = _bitmapLoader.LoadBitmapFromResource(logoutasset, this.GetType());

            //icon assets

            string musicasset = "App25.assets.others.icons.music.music.png";
            musicBitmap = _bitmapLoader.LoadBitmapFromResource(musicasset, this.GetType());
            string nomusicasset = "App25.assets.others.icons.nomusic.nomusic.png";
            noMusicBitmap = _bitmapLoader.LoadBitmapFromResource(nomusicasset, this.GetType());

            string soundasset = "App25.assets.others.icons.sound.sound.png";
            soundBitmap = _bitmapLoader.LoadBitmapFromResource(soundasset, this.GetType());
            string nosoundasset = "App25.assets.others.icons.nosound.nosound.png";
            noSoundBitmap = _bitmapLoader.LoadBitmapFromResource(nosoundasset, this.GetType());

            string thumbAsset = "App25.assets.others.icons.thumb.png";
            thumbBitmap = _bitmapLoader.LoadBitmapFromResource(thumbAsset, this.GetType());
        }

        private void OnEffectValueChanged(float volume)
        {
            _buttonSoundEffect.SetVolume(volume);
        }
        private void OnSliderValueChanged(float volume)
        {
            _audioLoader.SetVolume(volume);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            int width = e.Info.Width;
            int height = e.Info.Height;
            settingYlimit = height / 3;
            settingInitialY = height;

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

            //Settings Window//

            if (isSettingsOn)
            {
                using (var paint = new SKPaint())
                {

                    paint.ImageFilter = SKImageFilter.CreateBlur(10, 20); // Adjust blur intensity
                    paint.Color = SKColors.Black.WithAlpha(150); // Semi-transparent dark overlay


                    canvas.DrawRect(new SKRect(0, 0, width, height), paint);
                }
            }


            settingWindowX = width / 5;
            settingWindowY = height - settingYOffset;
            settingWindowWidth = width - 2 * (width / 5);
            settingWindowHeight = height - (height / 3) + 30;

            var settingStyle = new SKPaint { Color = SKColor.Parse("008de5") };

            canvas.DrawRect(settingWindowX, settingWindowY, settingWindowWidth, settingWindowHeight, settingStyle);

            canvas.DrawRect(settingWindowX + 50, settingWindowY + 50, settingWindowWidth - 100, settingWindowHeight, new SKPaint { Color = SKColor.Parse("98e0ff") });


            _musictrackstart = settingWindowX + settingWindowWidth / 5 + 100;
            _musictrackend = _musictrackstart + settingWindowWidth - 2 * (settingWindowWidth / 5);
            _musictrackY = settingWindowY + settingWindowHeight / 4;
            _trackwidth = _musictrackend - _musictrackstart;

            float _musicIncrementTrackX = _musictrackstart;
            float _musicIncrementTrackY = _musictrackY;
            float _musicIncrementTrackWidth = _musicthumbX - _musicIncrementTrackX;
            float _musicIncremenTracktHeight = _trackheight;


            float squarehalfSize = _thumbSize / 2;

            float _musicthumbY = (_musictrackY + _trackheight / 2) - squarehalfSize;

            using (var trackPaint = new SKPaint { Color = SKColor.Parse("504416"), IsAntialias = true })
            //using (var thumbPaint = new SKPaint { Color = SKColors.Blue, IsAntialias = true })
            using (var itrackPaint = new SKPaint
            {
                Shader = SKShader.CreateLinearGradient(
                    new SKPoint(_musicIncrementTrackX, _musicIncrementTrackY),
                    new SKPoint(_musicIncrementTrackX, _musicIncrementTrackY + _musicIncremenTracktHeight),
                    new SKColor[] { SKColor.Parse("#FFD42A"), SKColor.Parse("#504416") },
                    new float[] { 0.7f, 1 },
                    SKShaderTileMode.Clamp)
            })
            {
                canvas.DrawRoundRect(_musictrackstart, _musictrackY, _trackwidth, _trackheight, 5, 5, trackPaint);

                canvas.DrawRoundRect(_musicIncrementTrackX, _musicIncrementTrackY, _musicIncrementTrackWidth, _musicIncremenTracktHeight, 5, 5, itrackPaint);

                //    canvas.DrawRect(_musicthumbX - squarehalfSize, _musicthumbY, _thumbSize, _thumbSize, thumbPaint);
                canvas.DrawBitmap(thumbBitmap, new SKRect(_musicthumbX - squarehalfSize, _musicthumbY, _musicthumbX - squarehalfSize + _thumbSize, _musicthumbY + _thumbSize));

            }

            float musicTrackIconSize = iconSize;
            float musicTrackIconY = (_musictrackY + _trackheight / 2) - musicTrackIconSize / 2;
            float musicTrackIconX = settingWindowX + (_musictrackstart - settingWindowX) / 2 - musicTrackIconSize / 2 + 50;

            //    canvas.DrawRect(musicTrackIconX, musicTrackIconY, musicTrackIconSize, musicTrackIconSize, new SKPaint { Color = SKColors.Blue });
            if (MusicSliderValue == 0)
            {
                canvas.DrawBitmap(noMusicBitmap, new SKRect(musicTrackIconX, musicTrackIconY, musicTrackIconX + musicTrackIconSize, musicTrackIconY + musicTrackIconSize));

            }
            else if (MusicSliderValue > 0)
            {
                canvas.DrawBitmap(musicBitmap, new SKRect(musicTrackIconX, musicTrackIconY, musicTrackIconX + musicTrackIconSize, musicTrackIconY + musicTrackIconSize));
            }
            //effect

            _effecttrackstart = _musictrackstart;
            _effecttrackend = _musictrackend;
            _effecttrackY = settingWindowY + settingWindowHeight / 2;

            float _effectIncrementTrackX = _effecttrackstart;
            float _effectIncrementTrackY = _effecttrackY;
            float _effectIncrementTrackWidth = _effectThumbX - _effectIncrementTrackX;
            float _effectIncrementTrackHeight = _trackheight;

            float _effectthumbY = (_effecttrackY + _trackheight / 2) - squarehalfSize;

            using (var effecttrackPaint = new SKPaint { Color = SKColor.Parse("504416"), IsAntialias = true })
            //using (var effectthumbPaint = new SKPaint { Color = SKColors.Blue, IsAntialias = true })
            using (var effectItrackPaint = new SKPaint
            {
                Shader = SKShader.CreateLinearGradient(
                    new SKPoint(_effectIncrementTrackX, _effectIncrementTrackY),
                    new SKPoint(_effectIncrementTrackX, _effectIncrementTrackY + _effectIncrementTrackHeight),
                    new SKColor[] { SKColor.Parse("#FFD42A"), SKColor.Parse("#504416") },
                    new float[] { 0.7f, 1 },
                    SKShaderTileMode.Clamp)
            })
            {

                canvas.DrawRoundRect(_effecttrackstart, _effecttrackY, _trackwidth, _trackheight, 5, 5, effecttrackPaint);

                canvas.DrawRoundRect(_effectIncrementTrackX, _effectIncrementTrackY, _effectIncrementTrackWidth, _effectIncrementTrackHeight, 5, 5, effectItrackPaint);
                //  canvas.DrawRect(_effectThumbX - squarehalfSize, _effectthumbY, _thumbSize, _thumbSize, effectthumbPaint);

                canvas.DrawBitmap(thumbBitmap, new SKRect(_effectThumbX - squarehalfSize, _effectthumbY, _effectThumbX - squarehalfSize + _thumbSize, _effectthumbY + _thumbSize));

            }
            float effectTrackIconSize = iconSize;
            float effectTrackIconY = (_effecttrackY + _trackheight / 2) - effectTrackIconSize / 2;
            float effectTrackIconX = settingWindowX + (_effecttrackstart - settingWindowX) / 2 - effectTrackIconSize / 2 + 50;

            //canvas.DrawRect(effectTrackIconX, effectTrackIconY, effectTrackIconSize, effectTrackIconSize, new SKPaint { Color = SKColors.Brown });

            if (EffectSliderValue == 0)
            {
                canvas.DrawBitmap(noSoundBitmap, new SKRect(effectTrackIconX, effectTrackIconY, effectTrackIconX + effectTrackIconSize, effectTrackIconY + effectTrackIconSize));
            }
            else if (EffectSliderValue > 0)
            {
                canvas.DrawBitmap(soundBitmap, new SKRect(effectTrackIconX, effectTrackIconY, effectTrackIconX + effectTrackIconSize, effectTrackIconY + effectTrackIconSize));
            }

            //330x160

            float smallbuttonWidth = 330;
            float smallbuttonHeight = 160;
            float maxbuttonWidth = 340;
            float maxbuttonHeight = 165;
            float initLogoutY = effectTrackIconY + 250;
            logoutWidth = isLogoutPressed ? maxbuttonWidth : smallbuttonWidth; //+10
            logoutHeight = isLogoutPressed ? maxbuttonHeight : smallbuttonHeight; // +5

            logoutX = isLogoutPressed ? effectTrackIconX - (maxbuttonWidth - smallbuttonWidth) / 2 : effectTrackIconX;
            logoutY = isLogoutPressed ? initLogoutY - (maxbuttonHeight - smallbuttonHeight) / 2 : initLogoutY;


            canvas.DrawBitmap(logoutBitmap, new SKRect(logoutX, logoutY, logoutX + logoutWidth, logoutY + logoutHeight));


            var logoutText = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = isLogoutPressed ? SKColors.White : SKColor.Parse("800000"),
                IsAntialias = true,
                TextAlign = SKTextAlign.Center,
                TextSize = 70,
                Typeface = font,
            };
            float logoutTextX = logoutX + logoutWidth / 2;
            float logoutTextY = logoutY + logoutHeight / 2 + logoutText.TextSize / 3;
            canvas.DrawText("logout", logoutTextX, logoutTextY, logoutText);


        }

        private void MusicSliderUpdater(object sender, SKTouchEventArgs e)
        {
            _musicthumbX = Math.Max(_musictrackstart, Math.Min(e.Location.X, _musictrackend));
            MusicSliderValue = (_musicthumbX - _musictrackstart) / (_musictrackend - _musictrackstart);
            canvasView.InvalidateSurface();
        }
        private void EffectSliderUpdater(object sender, SKTouchEventArgs e)
        {
            _effectThumbX = Math.Max(_effecttrackstart, Math.Min(e.Location.X, _effecttrackend));
            EffectSliderValue = (_effectThumbX - _effecttrackstart) / (_effecttrackend - _effecttrackstart);
            canvasView.InvalidateSurface();
        }

        private void OnTouch(object sender, SKTouchEventArgs e) // touch event for the buttons
        {
            //Console.WriteLine($"{buttonX} , {buttonY}, {buttonWidth}, {buttonHeight}");


            if (isSettingsOn)
            {
                bool musicsliderHover = e.Location.X >= _musictrackstart &&
                                        e.Location.X <= _musictrackend &&
                                        e.Location.Y >= _musictrackY &&
                                        e.Location.Y <= _musictrackY + _trackheight;

                bool effectsliderHover = e.Location.X >= _effecttrackstart &&
                                         e.Location.X <= _effecttrackend &&
                                         e.Location.Y >= _effecttrackY &&
                                         e.Location.Y <= _effecttrackY + _trackheight;
                bool isLogoutHover = e.Location.X >= logoutX &&
                                      e.Location.X <= logoutX + logoutWidth &&
                                      e.Location.Y >= logoutY &&
                                      e.Location.Y <= logoutY + logoutHeight;

                if (e.ActionType == SKTouchAction.Pressed)
                {
                    if (musicsliderHover)
                    {
                        MusicSliderUpdater(sender, e);
                        wasmusicSLiderPressed = true;
                    }

                    if (effectsliderHover)
                    {
                        EffectSliderUpdater(sender, e);
                        wasEffectSliderPressed = true;
                    }

                    if (isLogoutHover)
                    {
                        isLogoutPressed = true;
                        canvasView.InvalidateSurface();
                        Logout();

                    }
                }

                if (e.ActionType == SKTouchAction.Moved)
                {
                    if (wasmusicSLiderPressed)
                    {
                        MusicSliderUpdater(sender, e);
                    }

                    if (wasEffectSliderPressed)
                    {
                        EffectSliderUpdater(sender, e);
                    }

                    if (isLogoutPressed && !isLogoutHover)
                    {
                        isLogoutPressed = false;
                        canvasView.InvalidateSurface();
                    }
                }

                if (e.ActionType == SKTouchAction.Released || e.ActionType == SKTouchAction.Cancelled)
                {
                    wasmusicSLiderPressed = false;
                    wasEffectSliderPressed = false;
                    isLogoutPressed = false;
                    canvasView.InvalidateSurface();
                }

                e.Handled = true;

            }

            bool settingWindowPressed =
                                          e.Location.X >= settingWindowX &&
                                          e.Location.X <= settingWindowX + settingWindowWidth &&
                                          e.Location.Y >= settingWindowY &&
                                          e.Location.Y <= settingWindowY + settingWindowHeight;

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

                    if (!isSettingsOn)
                    {
                        if (startButtonPressed)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                _buttonSoundEffect.Play();
                                _audioLoader.Stop();
                                AudioLoader.Instance.NonGamePageNavigation = false;
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
                                isSettingsOn = true;
                            });

                            Console.WriteLine(CurrentUser.User.Music);
                            float userMusicVol = (float)CurrentUser.User.Music;
                            _musicthumbX = (userMusicVol * (_musictrackend - _musictrackstart)) + _musictrackstart;
                            float userSoundVol = (float)CurrentUser.User.SoundEffectsVol;
                            _effectThumbX = (userSoundVol * (_effecttrackend - _effecttrackstart)) + _effecttrackstart;

                            isSettingPressed = true;
                            canvasView.InvalidateSurface();
                        }


                        if (lbButtonPressed)
                        {
                            _buttonSoundEffect.Play();
                            Device.BeginInvokeOnMainThread(async () =>
                            {

                                await Shell.Current.GoToAsync("lb");
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
                    }
                    if (isSettingsOn)
                    {
                        if (!settingWindowPressed)
                        {
                            UpdateVol();
                            _buttonSoundEffect.Play();
                            isSettingsOn = false;
                        }
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

        private async void UpdateVol()
        {
            await _aboutViewModel.UpdateVol(MusicSliderValue, EffectSliderValue);
        }

        private async void Logout()
        {
            await Shell.Current.GoToAsync("//LoginPage");
            AudioLoader.Instance.Stop();

        }

    }
}
