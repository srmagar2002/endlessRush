using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using System.IO;
using Xamarin.Forms.Xaml;
using App25.ViewModels;
using App25.Services;

namespace App25.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class GamePage : ContentPage
    {
        BitmapLoader _bitmapLoader;
        AuthViewModel _authViewModel;

        private readonly ButtonSoundEffect buttonSoundEffect;

        private readonly PixelFont _pixelFont;

        private readonly AudioLoader _audioLoader;
        private SKTypeface _typeface { get; set; }

        private SKCanvasView canvasView;
        private SKPaint dinoPaint, obstaclePaint, groundPaint, pausedPaint, pausedtextPaint;

        private SKBitmap run1Bitmap, run2Bitmap, jumpBitmap;
        private SKBitmap groundBitmap, bgbitmap;
        private SKBitmap obsBitmap;
        private float buttonX { get; set; }
        private float buttonY, buttonWidth, buttonHeight;


        private SKBitmap pauseIconBitmap, pauseWhiteIconBitmap, resumeIconBitmap;

        private SKBitmap quitBitmap, restartBitmap, resumeBitmap;
        private float pausedButtonWidth { get; set; }
        private float pausedButtonHeight { get; set; }

        private float pausedButtonX { get; set; }
        private float pausedButtonY { get; set; }

        private bool isPausedPressed { get; set; } = false;
        private bool isResumePressed { get; set; } = false;
        private bool isPaused { get; set; }

        private float resumeButtonX { get; set; }
        private float resumeButtonY { get; set; }

        private float exitButtonX { get; set; }
        private float exitButtonY { get; set; }


        private float dinoX, dinoY, dinoVelocity, gravity;
        private float dinoJumpVelocity;
        private float dinoHeight, dinoWidth, obstacleHeight, obstacleWidth, obstacleMinHeight;
        private float obstacleX, obstacleY;
        private float groundY { get; set; }
        private float gameSpeed;

        private bool isJumping;
        private bool isRunning;
        private int runFrame;

        private Random random;
        private int score;
        private int obstaclePassed;
        private SKPaint scorePaint;
        private bool gameOver;

        private List<Obstacle> obstacles;
        private int maxObstacles;
        private float obstacleCooldown;
        private float maxObstacleCooldown;

        private float groundOffset = 0;
        private float bgOffset = 0;


        public GamePage()
        {
            _bitmapLoader = new BitmapLoader();
            _authViewModel = new AuthViewModel();
            canvasView = new SKCanvasView();
            buttonSoundEffect = new ButtonSoundEffect();

            _audioLoader = new AudioLoader();
            _pixelFont = new PixelFont();

            canvasView.PaintSurface += OnPaintSurface;
            canvasView.EnableTouchEvents = true;
            canvasView.Touch += OnTouch;

            dinoPaint = new SKPaint { Color = SKColors.Green };
            obstaclePaint = new SKPaint { Color = SKColors.Red };
            groundPaint = new SKPaint { Color = SKColors.Gray };
            scorePaint = new SKPaint { Color = SKColors.Black, TextSize = 30 };

            LoadAssets();

            pausedPaint = new SKPaint { Color = SKColors.LightBlue, IsAntialias = true, Style = SKPaintStyle.Fill };
            pausedtextPaint = new SKPaint { Color = SKColors.Black, IsAntialias = true, Style = SKPaintStyle.Fill, TextSize = 40 };

            //330x160
            buttonHeight = 160;
            buttonWidth = 330;

            isPaused = false;

            dinoX = 100;
            dinoY = 0;
            dinoVelocity = 0;


            dinoHeight = 200;
            dinoWidth = 100;

            obstacleHeight = 210;
            obstacleMinHeight = 150;

            obstacleWidth = 120;


            gravity = 2.5f;
            dinoJumpVelocity = -37;
            obstacleX = 2000;
            obstacleY = 0;
            gameSpeed = 18;
            isJumping = false;
            isRunning = true;
            runFrame = 0;

            random = new Random();
            score = 0;
            obstaclePassed = 0;
            gameOver = false;


            obstacles = new List<Obstacle>();
            maxObstacles = 5;
            obstacleCooldown = 0;
            maxObstacleCooldown = 2000;


            Content = canvasView;


            GameTick();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetNavBarIsVisible(this, false);
            buttonSoundEffect.SetVolume(CurrentUser.User.SoundEffectsVol);
            GamePageTheme();
        }


        private void GamePageTheme()
        {
            AudioLoader.Instance.LoadAudio(this.GetType());
            AudioLoader.Instance.Play();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            AudioLoader.Instance.Stop();
        }
        public void GameTick()

        {
            Device.StartTimer(TimeSpan.FromMilliseconds(16), () => //1sec 1000ms
            {
                UpdateGame();
                canvasView.InvalidateSurface();
                return !gameOver;
            });
        }

        public void LoadAssets()
        {
            var theuser = CurrentUser.User;

            string bgAssets = "App25.assets.backgrounds." + theuser.BackgroundAsset;

            groundBitmap = _bitmapLoader.LoadBitmapFromResource(bgAssets + ".ground1.png", this.GetType());
            bgbitmap = _bitmapLoader.LoadBitmapFromResource(bgAssets + ".background1.png", this.GetType());

            string obsAssets = "App25.assets.obstacles";

            obsBitmap = _bitmapLoader.LoadBitmapFromResource(obsAssets + "." + theuser.ObstacleAsset + ".png", this.GetType());

            string charAssets = "App25.assets.characters." + theuser.CharacterAsset;

            run1Bitmap = _bitmapLoader.LoadBitmapFromResource(charAssets + ".running1.png", this.GetType());
            run2Bitmap = _bitmapLoader.LoadBitmapFromResource(charAssets + ".running2.png", this.GetType());
            jumpBitmap = _bitmapLoader.LoadBitmapFromResource(charAssets + ".jump.png", this.GetType());


            string pauseIconAssets = "App25.assets.others.buttons.pauseAndresumeIcon.pauseIcon.png";
            string pauseWhiteIconAssets = "App25.assets.others.buttons.pauseAndresumeIcon.pauseWhiteIcon.png";
            string resumeIconAssets = "App25.assets.others.buttons.pauseAndresumeIcon.resumeIcon.png";

            pauseIconBitmap = _bitmapLoader.LoadBitmapFromResource(pauseIconAssets, this.GetType());
            pauseWhiteIconBitmap = _bitmapLoader.LoadBitmapFromResource(pauseWhiteIconAssets, this.GetType());
            resumeIconBitmap = _bitmapLoader.LoadBitmapFromResource(resumeIconAssets, this.GetType());

            string quitAssets = "App25.assets.others.buttons.quitbutton.quit.png";
            string restartAssets = "App25.assets.others.buttons.restartbutton.restart.png";
            string resumeAssets = "App25.assets.others.buttons.resumebutton.resume.png";

            quitBitmap = _bitmapLoader.LoadBitmapFromResource(quitAssets, this.GetType());
            restartBitmap = _bitmapLoader.LoadBitmapFromResource(restartAssets, this.GetType());
            resumeBitmap = _bitmapLoader.LoadBitmapFromResource(resumeAssets, this.GetType());
        }





        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            _typeface = _pixelFont.LoadCustomfont();
            SKCanvas canvas = e.Surface.Canvas;
            SKImageInfo info = e.Info;
            int width = e.Info.Width;
            int height = e.Info.Height;

            if (CurrentUser.User.CharacterAsset == "char4")
            {
                dinoWidth = 375;
                dinoHeight = 150;
                obstacleHeight = 150;
                obstacleMinHeight = 80;
                dinoJumpVelocity = -45;
            }

            canvas.Clear(SKColors.White);

            groundY = height - 50;
            obstacleY = groundY - obstacleHeight;

            if (dinoY <= 0)
            {
                dinoY = groundY - dinoHeight;
            }

            dinoY = Math.Min(dinoY, groundY - dinoHeight);


            // Draw Background
            if (bgbitmap != null)
            {
                float bgWidth = width;

                // Draw two background images side by side for seamless looping
                canvas.DrawBitmap(bgbitmap, new SKRect(bgOffset, 0, bgOffset + bgWidth, height));
                canvas.DrawBitmap(bgbitmap, new SKRect(bgOffset + bgWidth - 10, 0, bgOffset + 20 + 2 * bgWidth, height));
            }




            // Draw ground
            //canvas.DrawRect(0, groundY, width, 50, groundPaint);
            //  canvas.DrawBitmap(groundBitmap, new SKRect(0, groundY, width, groundY + 50));

            if (groundBitmap == null)
            {
                canvas.DrawRect(0, groundY, width, 50, groundPaint);
            }
            else
            {
                float groundWidth = width;

                // Draw two ground images to create a seamless loop
                canvas.DrawBitmap(groundBitmap, new SKRect(groundOffset, groundY, groundOffset + groundWidth, groundY + 50));
                canvas.DrawBitmap(groundBitmap, new SKRect(groundOffset + groundWidth, groundY, groundOffset + 2 * groundWidth, groundY + 50));
            }

            // Draw dino
            //canvas.DrawRect(dinoX, dinoY, dinoWidth, dinoHeight, dinoPaint);

            if (jumpBitmap == null || run1Bitmap == null || run2Bitmap == null)
            {
                canvas.DrawRect(dinoX, dinoY, dinoWidth, dinoHeight, dinoPaint);
            }
            else
            {
                if (isJumping)
                {
                    canvas.DrawBitmap(jumpBitmap, new SKRect(dinoX, dinoY, dinoX + dinoWidth, dinoY + dinoHeight));
                }
                else
                {
                    SKBitmap currentRunBitmap = ((runFrame / 10) % 2 == 0) ? run1Bitmap : run2Bitmap;
                    canvas.DrawBitmap(currentRunBitmap, new SKRect(dinoX, dinoY, dinoX + dinoWidth, dinoY + dinoHeight));
                }
            }

            // Draw obstacle
            //canvas.DrawRect(obstacleX, obstacleY, obstacleWidth, obstacleHeight, obstaclePaint);

            foreach (Obstacle obstacle in obstacles)
            {
                canvas.DrawBitmap(obsBitmap, new SKRect(obstacle.X, obstacle.Y, obstacle.X + obstacle.Width, obstacle.Y + obstacle.Height));
                //canvas.DrawRect(obstacle.X, obstacle.Y, obstacle.Width, obstacle.Height, obstaclePaint);
            }

            //Draw score
            var yourScorePaint = new SKPaint
            {
                TextAlign = SKTextAlign.Center,
                TextSize = 70,
                Color = SKColors.White,
                Typeface = _typeface
            };

            if (!isPaused && !gameOver)
            {
                canvas.DrawText($"Score: {score}", 250, 80, yourScorePaint);
            }


            //Draw pause button
            buttonX = width - buttonWidth;
            buttonY = 0;

            pausedButtonHeight = 100;
            pausedButtonWidth = 100;
            pausedButtonX = width - (pausedButtonWidth + pausedButtonWidth / 2);
            pausedButtonY = pausedButtonHeight / 2;


            if (!isPaused)
            {
                if (isPausedPressed)
                {
                    canvas.DrawBitmap(pauseWhiteIconBitmap, new SKRect(pausedButtonX, pausedButtonY, pausedButtonX + pausedButtonWidth, pausedButtonY + pausedButtonHeight));
                }
                else
                {
                    canvas.DrawBitmap(pauseIconBitmap, new SKRect(pausedButtonX, pausedButtonY, pausedButtonX + pausedButtonWidth, pausedButtonY + pausedButtonHeight));
                }

            }
            else
            {
                //canvas.DrawBitmap(resumeIconBitmap, new SKRect(pausedButtonX, pausedButtonY, pausedButtonX + pausedButtonWidth, pausedButtonY + pausedButtonHeight));
                DrawPauseOverlay(canvas, info);

                resumeButtonX = width / 2 - (buttonWidth / 2);
                resumeButtonY = height / 2 + 100;

                var resumeTextPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = isResumePressed ? SKColors.White : SKColor.Parse("504416"),
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center,
                    TextSize = 70,
                    Typeface = _typeface,
                };

                float resumeTextX = resumeButtonX + buttonWidth / 2;
                float resumeTextY = resumeButtonY + buttonHeight / 2 + (resumeTextPaint.TextSize / 3);

                if (resumeBitmap != null)
                {
                    canvas.DrawBitmap(resumeBitmap, new SKRect(resumeButtonX, resumeButtonY, resumeButtonX + buttonWidth, resumeButtonY + buttonHeight));
                }
                else
                {
                    canvas.DrawRoundRect(resumeButtonX, resumeButtonY, buttonWidth, buttonHeight, 20, 20, pausedPaint);
                }
                canvas.DrawText("resume", resumeTextX, resumeTextY, resumeTextPaint);

                exitButtonX = width / 2 - (buttonWidth / 2);
                exitButtonY = resumeButtonY + 250;

                var quitTextPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = SKColor.Parse("800000"),
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center,
                    TextSize = 70,
                    Typeface = _typeface,
                };

                float quitTextX = exitButtonX + buttonWidth / 2;
                float quitTextY = exitButtonY + buttonHeight / 2 + (quitTextPaint.TextSize / 3);

                if (quitBitmap != null)
                {
                    canvas.DrawBitmap(quitBitmap, new SKRect(exitButtonX, exitButtonY, exitButtonX + buttonWidth, exitButtonY + buttonHeight));
                }
                else
                {
                    canvas.DrawRoundRect(exitButtonX, exitButtonY, buttonWidth, buttonHeight, 20, 20, pausedPaint);
                }

                canvas.DrawText("quit", quitTextX, quitTextY, quitTextPaint);

                var pausedScorePaint = new SKPaint
                {
                    TextAlign = SKTextAlign.Center,
                    TextSize = 70,
                    Color = SKColors.White,
                    Typeface = _typeface
                };


                canvas.DrawText($"Your Score: {score}", resumeTextX, resumeTextY - 230, pausedScorePaint);


                var gamePausedPaint = new SKPaint
                {
                    Color = SKColor.Parse("ccff00"),
                    TextSize = 300,
                    Style = SKPaintStyle.Fill,
                    TextAlign = SKTextAlign.Center,
                    Typeface = _typeface
                };
                canvas.DrawText("Game Paused", width / 2, height / 2 - 250, gamePausedPaint);

            }

            if (gameOver)
            {
                DrawPauseOverlay(canvas, info);


                resumeButtonX = width / 2 - (buttonWidth / 2);
                resumeButtonY = height / 2 + 100;

                var resumeTextPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = isResumePressed ? SKColors.White : SKColor.Parse("668000"),
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center,
                    TextSize = 70,
                    Typeface = _typeface,
                };

                float resumeTextX = resumeButtonX + buttonWidth / 2;
                float resumeTextY = resumeButtonY + buttonHeight / 2 + (resumeTextPaint.TextSize / 3);

                if (resumeBitmap != null)
                {
                    canvas.DrawBitmap(restartBitmap, new SKRect(resumeButtonX, resumeButtonY, resumeButtonX + buttonWidth, resumeButtonY + buttonHeight));
                }
                else
                {
                    canvas.DrawRoundRect(resumeButtonX, resumeButtonY, buttonWidth, buttonHeight, 20, 20, pausedPaint);
                }
                canvas.DrawText("restart", resumeTextX, resumeTextY, resumeTextPaint);

                exitButtonX = width / 2 - (buttonWidth / 2);
                exitButtonY = resumeButtonY + 250;

                var quitTextPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = SKColor.Parse("800000"),
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center,
                    TextSize = 70,
                    Typeface = _typeface,
                };

                float quitTextX = exitButtonX + buttonWidth / 2;
                float quitTextY = exitButtonY + buttonHeight / 2 + (quitTextPaint.TextSize / 3);

                if (quitBitmap != null)
                {
                    canvas.DrawBitmap(quitBitmap, new SKRect(exitButtonX, exitButtonY, exitButtonX + buttonWidth, exitButtonY + buttonHeight));
                }
                else
                {
                    canvas.DrawRoundRect(exitButtonX, exitButtonY, buttonWidth, buttonHeight, 20, 20, pausedPaint);
                }

                canvas.DrawText("exit", quitTextX, quitTextY, quitTextPaint);

                var goScorePaint = new SKPaint
                {
                    TextAlign = SKTextAlign.Center,
                    TextSize = 70,
                    Color = SKColors.White,
                    Typeface = _typeface
                };


                canvas.DrawText($"Your Score: {score}", resumeTextX, resumeTextY - 270, goScorePaint);
                canvas.DrawText($"Obstacle Jumped: {obstaclePassed}", resumeTextX, resumeTextY - 200, goScorePaint);

                var gamePausedPaint = new SKPaint
                {
                    Color = SKColor.Parse("ff0000"),
                    TextSize = 300,
                    Style = SKPaintStyle.Fill,
                    TextAlign = SKTextAlign.Center,
                    Typeface = _typeface
                };
                canvas.DrawText("Game Over", width / 2, height / 2 - 250, gamePausedPaint);

            }
        }

        private void DrawPauseOverlay(SKCanvas canvas, SKImageInfo info)
        {
            using (var paint = new SKPaint())
            {

                paint.ImageFilter = SKImageFilter.CreateBlur(10, 20); // Adjust blur intensity
                paint.Color = SKColors.Black.WithAlpha(150); // Semi-transparent dark overlay


                canvas.DrawRect(new SKRect(0, 0, info.Width, info.Height), paint);
            }
        }

        private void UpdateGame()
        {
            //Console.WriteLine($"Dino: ({dinoX}, {dinoY}), Obstacles: {obstacles.Count}");

            if (gameOver)
            {
                return;
            }

            if (isPaused) return;

            //Background Movement
            bgOffset -= gameSpeed * 0.1f;

            // Reset the background position when it moves completely off-screen
            if (bgbitmap != null && bgOffset <= -canvasView.CanvasSize.Width)
            {
                bgOffset = 0;
            }


            //Ground Movement
            groundOffset -= gameSpeed;

            // Reset the ground position when it moves completely off-screen
            if (groundBitmap != null && groundOffset <= -groundBitmap.Width)
            {
                groundOffset = 0;
            }

            // Dino physics
            dinoVelocity += gravity;
            dinoY += dinoVelocity;


            if (dinoY >= groundY - dinoHeight)
            {
                dinoY = groundY - dinoHeight;
                dinoVelocity = 0;
                isJumping = false;
                isRunning = true;
            }



            // Obstacle movement


            obstacleCooldown -= 16;
            if (obstacles.Count == 0 || obstacleCooldown <= 0)
            {
                SpawnObstacle();
                obstacleCooldown = random.Next((int)(maxObstacleCooldown * 0.30), (int)maxObstacleCooldown);
            }



            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                Obstacle obstacle = obstacles[i];

                //Console.WriteLine($"Obstacle at ({obstacle.X}, {obstacle.Y}) Ob-Area {obstacle.Height}x{obstacle.Width}");

                if (dinoX < obstacle.X + obstacle.Width &&
                  dinoX + dinoWidth > obstacle.X &&
                  dinoY < obstacle.Y + obstacle.Height &&
                  dinoY + dinoHeight > obstacle.Y)
                {
                    //Console.WriteLine("Collision detected!");
                    gameOver = true;
                    _audioLoader.Stop();
                    OnGameOver(score);
                }

                obstacle.X -= gameSpeed;

                if (obstacle.X < -obstacle.Width)
                {

                    obstacles.RemoveAt(i);
                    if (gameSpeed != 60)
                    {
                        gameSpeed += 1f;
                    }
                    obstaclePassed++;
                    score = obstaclePassed * 100 + ((int)gameSpeed * 10);
                }
            }

            if (gameSpeed >= 55)
            {
                score++;
            }

            //Running animation frame update
            if (!isJumping)
            {
                runFrame++;
            }
        }

        private void SpawnObstacle()
        {
            if (canvasView.CanvasSize.Width > 0)
            {
                float x = canvasView.CanvasSize.Width;
                float height = random.Next((int)obstacleMinHeight, (int)obstacleHeight);
                float width = obstacleWidth;
                float y = groundY - height;

                obstacles.Add(new Obstacle(x, y, width, height));
                //Console.WriteLine("Obstacle Spawned");
            }
        }


        private void OnTouch(object sender, SKTouchEventArgs e)
        {
            //Console.WriteLine(canvasView.Width);
            //Console.WriteLine(canvasView.Height);

            bool pauseButtonTouched =
                    e.Location.X >= pausedButtonX &&
                    e.Location.X <= pausedButtonX + pausedButtonWidth &&
                    e.Location.Y >= pausedButtonY &&
                    e.Location.Y < pausedButtonY + pausedButtonHeight;


            bool resumeButtonTouched =
                    e.Location.X >= resumeButtonX &&
                    e.Location.X <= resumeButtonX + buttonWidth &&
                    e.Location.Y >= resumeButtonY &&
                    e.Location.Y <= resumeButtonY + buttonHeight;

            bool exitButtonTouched =
                    e.Location.X >= exitButtonX &&
                    e.Location.X <= exitButtonX + buttonWidth &&
                    e.Location.Y >= exitButtonY &&
                    e.Location.Y <= exitButtonY + buttonHeight;
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    if (dinoY >= groundY - dinoHeight && !gameOver && !isPaused && !pauseButtonTouched)
                    {
                        dinoVelocity = dinoJumpVelocity;
                        isJumping = true;
                        isRunning = false;
                    }


                    if (!gameOver)
                    {
                        // Pause functionality
                        if (!isPaused && pauseButtonTouched)
                        {
                            buttonSoundEffect.Play();
                            isPaused = true;
                            isPausedPressed = true;
                            canvasView.InvalidateSurface();
                            Console.WriteLine("Game Paused");
                        }

                        // Resume functionality
                        else if (isPaused)
                        {
                            if (resumeButtonTouched)
                            {
                                buttonSoundEffect.Play();
                                isPaused = false;
                                isResumePressed = true;
                                canvasView.InvalidateSurface();
                                Console.WriteLine("Game Resumed");
                            }
                            else if (exitButtonTouched)
                            {
                                buttonSoundEffect.Play();
                                AudioLoader.Instance.Stop();
                                ExitGame();
                            }
                        }
                    }
                    if (gameOver)
                    {
                        if (resumeButtonTouched)
                        {
                            buttonSoundEffect.Play();
                            RestartGame();
                        }
                        else if (exitButtonTouched)
                        {
                            buttonSoundEffect.Play();
                            AudioLoader.Instance.Stop();
                            ExitGame();
                        }
                    }

                    break;

                case SKTouchAction.Released:
                case SKTouchAction.Cancelled:
                    isPausedPressed = false;
                    isResumePressed = false;
                    break;

                case SKTouchAction.Moved:
                    if (!pauseButtonTouched && isPausedPressed)
                    {
                        isPausedPressed = false;
                        isResumePressed = false;
                    }
                    break;
            }

            e.Handled = true;
        }

        private void OnGameOver(int score)
        {
            Device.BeginInvokeOnMainThread(async () =>
           {
               //Console.WriteLine("here");
               if (CurrentUser.User != null)
               {
                   await _authViewModel.UpdateHighestScore(score);

               }
               //await DisplayAlert("Game Over", $"Your score: {score}", "Restart");
           });
        }
        private void RestartGame()
        {


            dinoX = 100;
            dinoY = canvasView.CanvasSize.Height - dinoHeight;
            dinoVelocity = 0;
            obstacleX = 2000;
            gameSpeed = 20;
            score = 0;
            gameOver = false;
            isJumping = false;
            isRunning = true;
            runFrame = 0;
            obstaclePassed = 0;
            GamePageTheme();
            groundOffset = 0;
            bgOffset = 0;

            obstacles.Clear();

            Device.BeginInvokeOnMainThread(() =>
            {
                GameTick();
            });


            Console.WriteLine("Game restarted.");

        }

        private void ExitGame()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PopAsync();

            });
        }
    }

    public class Obstacle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Obstacle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}