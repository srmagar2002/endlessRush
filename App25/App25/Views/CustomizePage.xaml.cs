using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using System.Reflection;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using App25.ViewModels;
using App25.Services;

namespace App25.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class CustomizePage : ContentPage
    {
        private BitmapLoader _bitmapLoader;
        private SKBitmap titlebitmap, applyBitmap;
        private CustomizeViewModel _customizeViewModel;
        private readonly PixelFont _pixelFont;
        private SKTypeface font;

        private ButtonSoundEffect _buttonsoundeffect;

        private float applyX { get; set; }
        private float applyY { get; set; }
        private float applyWidth { get; set; }
        private float applyHeight { get; set; }
        private bool applyPressed;
        public ObservableCollection<SkinItem> Backgrounds { get; set; }
        public ObservableCollection<SkinItem> Characters { get; set; }
        public ObservableCollection<SkinItem> Obstacles { get; set; }

        private SkinItem selectedBackground;
        private SkinItem selectedCharacter;
        private SkinItem selectedObstacle;


        public CustomizePage()
        {
            _customizeViewModel = new CustomizeViewModel();
            _bitmapLoader = new BitmapLoader();
            _pixelFont = new PixelFont();
            _buttonsoundeffect = new ButtonSoundEffect();
            applyPressed = false;

            InitializeComponent();


            // Load Skins
            Backgrounds = new ObservableCollection<SkinItem>
        {
            {new SkinItem("Day",ImageSource.FromResource("App25.assets.backgrounds.bg1.background1.png"), "bg1")},
            { new SkinItem("Cave", ImageSource.FromResource("App25.assets.backgrounds.bg2.background1.png"), "bg2")},
            { new SkinItem("Evening", ImageSource.FromResource("App25.assets.backgrounds.bg3.background1.png"), "bg3")}
        };

            Characters = new ObservableCollection<SkinItem>
        {
            {new SkinItem("Man", ImageSource.FromResource("App25.assets.characters.char1.running1.png"), "char1")},
            {new SkinItem("Ninja", ImageSource.FromResource("App25.assets.characters.char2.jump.png"), "char2")},
            {new SkinItem("Detective", ImageSource.FromResource("App25.assets.characters.char3.running1.png"),"char3")},
            {new SkinItem("Gato", ImageSource.FromResource("App25.assets.characters.char4.running1.png"),"char4")},
         };

            Obstacles = new ObservableCollection<SkinItem>
        {
             { new SkinItem("Hurdles", ImageSource.FromResource("App25.assets.obstacles.obstacle1.png"), "obstacle1") },
             { new SkinItem("Pipes", ImageSource.FromResource("App25.assets.obstacles.obstacle2.png"), "obstacle2") },
             { new SkinItem("Axe", ImageSource.FromResource("App25.assets.obstacles.obstacle3.png"), "obstacle3") },
        };

            // Binding
            BindingContext = this;

            // Default selections

            foreach (var background in Backgrounds)
            {
                if (background.Code == CurrentUser.User.BackgroundAsset)
                {
                    selectedBackground = background;
                    BackgroundPreview.Source = background.Image;
                    BackgroundPreviewText.Text = background.Name;
                    break;
                }
            }
            foreach (var character in Characters)
            {
                if (character.Code == CurrentUser.User.CharacterAsset)
                {
                    selectedCharacter = character;
                    CharacterPreview.Source = character.Image;
                    CharacterPreviewText.Text = character.Name;
                    break;
                }
            }
            foreach (var obstacle in Obstacles)
            {
                if (obstacle.Code == CurrentUser.User.ObstacleAsset)
                {
                    selectedObstacle = obstacle;
                    ObstaclePreview.Source = obstacle.Image;
                    ObstaclePreviewText.Text = obstacle.Name;
                    break;
                }
            }

            BackgroundSelection.SelectionChanged += OnBackgroundSelected;
            CharacterSelection.SelectionChanged += OnCharacterSelected;
            ObstacleSelection.SelectionChanged += OnObstacleSelected;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _buttonsoundeffect.SetVolume(CurrentUser.User.SoundEffectsVol);
        }
        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            font = _pixelFont.LoadCustomfont();

            SKCanvas canvas = e.Surface.Canvas;
            int width = e.Info.Width;
            int height = e.Info.Height;


            titlebitmap = _bitmapLoader.LoadBitmapFromResource("App25.assets.others.titleBoard.png", typeof(CustomizePage));
            Console.WriteLine($"The Width={titlebitmap.Width} The Height={titlebitmap.Height}");

            float titleX = width / 2 - titlebitmap.Width / 2;
            float titleY = 10;

            if (titlebitmap != null)
            {
                canvas.DrawBitmap(titlebitmap, new SKRect(titleX, titleY, titleX + titlebitmap.Width, titleY + titlebitmap.Height));

                var titletext = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = SKColor.Parse("2d1650"),
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center,
                    TextSize = 70,
                    Typeface = font
                };

                float titletextX = titleX + titlebitmap.Width / 2;
                float titletextY = titleY + titlebitmap.Height / 2 + (titletext.TextSize / 3);

                if (BackgroundSelection.IsVisible)
                {
                    canvas.DrawText("Select a Background", titletextX, titletextY, titletext);
                }
                else if (CharacterSelection.IsVisible)
                {
                    canvas.DrawText("Select a Character", titletextX, titletextY, titletext);
                }
                else if (ObstacleSelection.IsVisible)
                {
                    canvas.DrawText("Select an Obstacle", titletextX, titletextY, titletext);
                }
                else
                {
                    canvas.DrawText("customize", titletextX, titletextY, titletext);
                }
            }


        }


        private void OnButtonPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            font = _pixelFont.LoadCustomfont();

            SKCanvas canvas = e.Surface.Canvas;
            int width = e.Info.Width;
            int height = e.Info.Height;
            applyBitmap = _bitmapLoader.LoadBitmapFromResource("App25.assets.others.buttons.applybutton.apply.png", typeof(CustomizePage));
            applyWidth = applyBitmap.Width;
            applyHeight = applyBitmap.Height;
            applyX = width / 2 - applyWidth / 2;
            applyY = height / 2 - applyHeight / 2;



            var applytext = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = applyPressed ? SKColors.White : SKColor.Parse("005500"),
                IsAntialias = true,
                TextAlign = SKTextAlign.Center,
                TextSize = 80,
                Typeface = font
            };
            float applytextX = applyX + applyBitmap.Width - applyBitmap.Width / 2;
            float applytextY = applyY + applyBitmap.Height / 2 + (applytext.TextSize / 3) - (applyPressed ? 10 : 0);


            canvas.DrawBitmap(applyBitmap, new SKRect(applyX, applyY, applyX + applyWidth, applyY + applyHeight));
            canvas.DrawText("apply", applytextX, applytextY, applytext);


        }

        private void OnTouch(object sender, SKTouchEventArgs e)
        {
            bool isInsideApplyButton = IsInsideApplyButton(e.Location.X, e.Location.Y);

            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    if (isInsideApplyButton)
                    {
                        _buttonsoundeffect.Play();
                        ApplySelection(sender, e);
                        applyPressed = true;
                        buttonSKCanvas.InvalidateSurface();
                    }
                    break;

                case SKTouchAction.Released:
                case SKTouchAction.Cancelled:
                    applyPressed = false;
                    buttonSKCanvas.InvalidateSurface();
                    break;

                case SKTouchAction.Moved:
                    if (!isInsideApplyButton && applyPressed)
                    {
                        applyPressed = false;
                        buttonSKCanvas.InvalidateSurface();
                    }
                    break;
            }

            e.Handled = true;
        }


        private bool IsInsideApplyButton(float x, float y)
        {
            return x >= applyX && x <= applyX + applyWidth && y >= applyY && y <= applyY + applyHeight;
        }

        private void OnBackgroundPreviewTapped(object sender, EventArgs e)
        {
            _buttonsoundeffect.Play();
            BackgroundSelection.IsVisible = true;
            canvasView.InvalidateSurface();

        }
        private void OnBackgroundSelectionChanged(object sender, EventArgs e)
        {
            _buttonsoundeffect.Play();
            BackgroundSelection.IsVisible = false;
            canvasView.InvalidateSurface();

        }

        private void OnCharacterPreviewTapped(object sender, EventArgs e)
        {
            _buttonsoundeffect.Play();
            CharacterSelection.IsVisible = true;
            canvasView.InvalidateSurface();
        }

        private void OnCharacterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _buttonsoundeffect.Play();
            CharacterSelection.IsVisible = false;
            canvasView.InvalidateSurface();
        }

        private void OnObstaclePreviewTapped(object sender, EventArgs e)
        {
            _buttonsoundeffect.Play();
            ObstacleSelection.IsVisible = true;
            canvasView.InvalidateSurface();
        }

        private void OnObstacleSelectionChanged(object sender, EventArgs e)
        {
            _buttonsoundeffect.Play();
            ObstacleSelection.IsVisible = false;
            canvasView.InvalidateSurface();
        }


        private void OnGridTap(object sender, EventArgs e)
        {

            if (BackgroundSelection.IsVisible)
            {
                _buttonsoundeffect.Play();
                BackgroundSelection.IsVisible = false;
                canvasView.InvalidateSurface();
            }
            if (CharacterSelection.IsVisible)
            {
                _buttonsoundeffect.Play();
                CharacterSelection.IsVisible = false;
                canvasView.InvalidateSurface();
            }
            if (ObstacleSelection.IsVisible)
            {
                _buttonsoundeffect.Play();
                ObstacleSelection.IsVisible = false;
                canvasView.InvalidateSurface();
            }

        }


        private void OnBackgroundSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedItem = (SkinItem)e.CurrentSelection[0];
                selectedBackground = selectedItem;
                BackgroundPreview.Source = selectedBackground.Image;
                BackgroundPreviewText.Text = selectedItem.Name;
            }
        }


        private void OnCharacterSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedItem = (SkinItem)e.CurrentSelection[0];
                selectedCharacter = selectedItem;
                CharacterPreview.Source = selectedCharacter.Image;
                CharacterPreviewText.Text = selectedItem.Name;
            }
        }

        private void OnObstacleSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedItem = (SkinItem)e.CurrentSelection[0];
                selectedObstacle = selectedItem;
                ObstaclePreview.Source = selectedObstacle.Image;
                ObstaclePreviewText.Text = selectedItem.Name;
            }
        }

        private async void ApplySelection(object sender, EventArgs e)
        {
            // Save selections (You may want to store these in local storage or pass them to the game)
            Application.Current.Properties["SelectedBackground"] = selectedBackground;
            Application.Current.Properties["SelectedCharacter"] = selectedCharacter;
            Application.Current.Properties["SelectedObstacle"] = selectedObstacle;

            CurrentUser.User.BackgroundAsset = selectedBackground.Code;
            CurrentUser.User.ObstacleAsset = selectedObstacle.Code;
            CurrentUser.User.CharacterAsset = selectedCharacter.Code;

            await _customizeViewModel.UpdateUserAssets(selectedBackground.Code, selectedCharacter.Code, selectedObstacle.Code);

            await Application.Current.SavePropertiesAsync();

            AudioLoader.Instance.NonGamePageNavigation = true;
            await DisplayAlert("Selection Saved", "Your customization has been saved!", "OK");
            AudioLoader.Instance.Pause();
            await Navigation.PopAsync();
        }

        // Helper Model
        public class SkinItem
        {
            public string Name { get; set; }
            public ImageSource Image { get; set; }
            public string Code { get; set; }

            public SkinItem(string name, ImageSource image, string code)
            {
                Name = name;
                Image = image;
                Code = code;
            }
        }


    }
}


