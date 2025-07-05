using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.UI.Windowing;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Devices;
using ProjectTSSI.Handlers;
using ProjectTSSI.Models.CustomHandlers;
using ProjectTSSI.Global;
using System.Text.Json;
using Windows.System;
using Windows.UI.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using ProjectTSSI.Models;
using ProjectTSSI.Models.Interfaces;
using ProjectTSSI.Handlers.Windows;


namespace ProjectTSSI
{
    public partial class LoginWS : ContentPage, INotifyPropertyChanged, IViewModelNeedCustom
    {
        #region Properties
        public Dictionary<string, INotifyPropertyChanged> Configurations { get; set; } = new();
        private List<Border> listBordersAnimation { get; set; } = new();
        public event PropertyChangedEventHandler PropertyChanged;
        private DispatcherTimer _checkCoreWindowTimer = new();
        private readonly List<string> _listImages = new List<string>
        {
            "login_image_one.png",
            "login_image_two.png",
            "login_image_three.png"
        };
        private bool _isDebugMode = false;
        public bool IsDebugMode
        {
            get => _isDebugMode;
            set
            {
                _isDebugMode = value;
                OnPropertyChanged(nameof(IsDebugMode));
            }
        }
        private double _screenWidth;
        private double _screenHeight;
        private double _screenDensity;
        private string loginUserText = "";
        private string loginPasswordText = "";

        public string LoginUserText
        {
            get => loginUserText;
            set
            {
                if (loginUserText != value)
                {
                    loginUserText = value;
                    OnPropertyChanged();
                    ActiveButtonLogin();
                }
            }
        }

        public string LoginPasswordText
        {
            get => loginPasswordText;
            set
            {
                if (loginPasswordText != value)
                {
                    loginPasswordText = value;
                    OnPropertyChanged();
                    ActiveButtonLogin();
                }
            }
        }

        private int _indexImages = 0;
        private int _elementOfErrorMail = 0;
        private bool _isButtonLoginActive = false;

        public bool IsButtonLoginActive
        {
            get => _isButtonLoginActive;
            set
            {
                if (_isButtonLoginActive != value)
                {
                    _isButtonLoginActive = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _correctMail = false;
        private bool _correctPassword = false;
        #endregion
        #region Init Component
        public LoginWS()
        {
            GlobalMethods.LoadJsonConfigurations(Configurations, "LoginWSConfigs.json");
            OnPropertyChanged(nameof(Configurations));
            InitializeComponent();
            BindingContext = this;
            this.Loaded += OnPageLoaded;
            this.Unloaded += OnPageUnloaded;
        }
        public void OnPageLoaded(object sender, EventArgs e)
        {
            var mauiWindow = Microsoft.Maui.Controls.Application.Current?.Windows?.FirstOrDefault();
            if (mauiWindow?.Handler?.PlatformView is Microsoft.UI.Xaml.Window nativeWindow)
            {
                var accelerator = new Microsoft.UI.Xaml.Input.KeyboardAccelerator
                {
                    Key = VirtualKey.S,
                    Modifiers = VirtualKeyModifiers.Control | VirtualKeyModifiers.Shift
                };

                accelerator.Invoked += Accelerator_Invoked;
                var xamlRoot = nativeWindow.Content?.XamlRoot;
                if (nativeWindow.Content is Microsoft.UI.Xaml.FrameworkElement fe)
                    fe.KeyboardAccelerators.Add(accelerator);
            }
        }
        public void OnPageUnloaded(object sender, EventArgs e)
        {
            var mauiWindow = Microsoft.Maui.Controls.Application.Current?.Windows?.FirstOrDefault();
            if (mauiWindow?.Handler?.PlatformView is Microsoft.UI.Xaml.Window nativeWindow)
            {
                var xamlRoot = nativeWindow.Content?.XamlRoot;
                if (nativeWindow.Content is Microsoft.UI.Xaml.FrameworkElement fe)
                {
                    var accelerator = fe.KeyboardAccelerators.FirstOrDefault(a => a.Key == VirtualKey.S && a.Modifiers == (VirtualKeyModifiers.Control | VirtualKeyModifiers.Shift));
                    if (accelerator != null)
                    {
                        accelerator.Invoked -= Accelerator_Invoked;
                        fe.KeyboardAccelerators.Remove(accelerator);
                    }
                }
            }
            _checkCoreWindowTimer.Stop();
            listBordersAnimation.ForEach(b => { b.AbortAnimation("ColorAnimation"); absoluteLayoutCircles.Children.Remove(b); });
            listBordersAnimation.Clear();
        }
        public void Accelerator_Invoked(object sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
            this.IsDebugMode = !this.IsDebugMode;
            GlobalConstants.IsDebugMode = this.IsDebugMode;
        }
        protected override async void OnAppearing()
        {
            try
            {
                await SetConstants();
                setSizesItems();
                ImagesAnimations();
                await FadeInAll();
            }
            catch (System.Exception ex)
            {
                Logger.Log($"Error:{ex.Message}");
            }
            base.OnAppearing();

        }
        private async Task SetConstants()
        {
            var displayInfor = DeviceDisplay.MainDisplayInfo;
            _screenWidth = displayInfor.Width;
            _screenHeight = displayInfor.Height;
            _screenDensity = displayInfor.Density;
            GlobalConstants.UpdateScreenMetrics(_screenWidth, _screenHeight, _screenDensity);
        }
        private void setSizesItems()
        {
            GlobalMethods.LoadJsonConfigurations(Configurations, "LoginWSConfigs.json");
            OnPropertyChanged(nameof(Configurations));
            GenerateCirclesAnimation();
        }
        #endregion
        public void GenerateCirclesAnimation()
        {
            Random random = new Random();
            List<Rect> listRectsCircles = new List<Rect>
            {
                    new Rect(0.3, -.16, 100, 100),
                    new Rect(0.5, -.13, 100, 100),
                    new Rect(0.9, -.15, 100, 100),
                    new Rect(1, -.17, 100, 100),
                    new Rect(1.05, 0.9, 100, 100),
                    new Rect(0.1, 1.135, 100, 100),
                    new Rect(1.06, 0.7, 100, 100),
                    new Rect(0.7, 1.17, 100, 100),
                    new Rect(-0.065, 0.5, 100, 100),
                    new Rect(0.7, -.14, 100, 100),
                    new Rect(0.5, 1.16, 100, 100),
                    new Rect(0.9, 1.15, 100, 100),
                    new Rect(0.3, 1.14, 100, 100),
                    new Rect(1.04, 0.5, 100, 100),
                    new Rect(0.1, 1.13, 100, 100),
                    new Rect(-0.05, 1, 100, 100),
                    new Rect(-0.04, 0.7, 100, 100),
                    new Rect(1.035, 0.3, 100, 100),
            };
            int cuantityRectsCircle = listRectsCircles.Count;
            _checkCoreWindowTimer.Interval = TimeSpan.FromSeconds(1);
            _checkCoreWindowTimer.Tick += async (s, e) =>
            {
                if (listBordersAnimation.Count <= cuantityRectsCircle)
                {
                    int randomNumber = random.Next(0, listRectsCircles.Count);
                    int randomRed = random.Next(0, 256);
                    int randomBlue = random.Next(0, 256);
                    int randomGreen = random.Next(0, 256);
                    Border borderAdd = GenerateCircles(listRectsCircles[randomNumber], randomRed, randomBlue, randomGreen);
                    await borderAdd.FadeTo(1, 1000, Easing.Linear);
                    listBordersAnimation.Add(borderAdd);
                }
                else
                {
                    int randomDelete = random.Next(0, listBordersAnimation.Count - 1);
                    await listBordersAnimation[randomDelete].FadeTo(0, 1000);
                    listBordersAnimation[randomDelete].AbortAnimation("ColorAnimation");
                    absoluteLayoutCircles.Children.Remove(listBordersAnimation[randomDelete]);
                    listBordersAnimation.RemoveAt(randomDelete);
                }
            };
            _checkCoreWindowTimer.Start();
        }
        private Border GenerateCircles(Rect rect, int red, int blue, int green)
        {
            var border = new Border
            {
                Stroke = Colors.White,
                StrokeThickness = 2,
                Background = Colors.White,
                WidthRequest = (int)_screenWidth * 0.01,
                HeightRequest = (int)_screenHeight * 0.0177,
                StrokeShape = new RoundRectangle { CornerRadius = 0 },
                Rotation = 45
            };

            AbsoluteLayout.SetLayoutBounds(border, rect);
            AbsoluteLayout.SetLayoutFlags(border, AbsoluteLayoutFlags.PositionProportional);
            var animation = new Animation();
            animation.Add(0, 0.5, new Animation(v =>
            {
                border.Background = new SolidColorBrush(Color.FromRgba((red / 255.0) * v, (green / 255.0) * v, (blue / 255.0) * v, 0.15));
            }, 0, 1));

            animation.Add(0.5, 1, new Animation(v =>
            {
                border.Background = new SolidColorBrush(Color.FromRgba((red / 255.0) * (1 - v), (green / 255.0) * (1 - v), (blue / 255.0) * (1 - v), 0.15));
            }, 0, 1));
            animation.Commit(border, "ColorAnimation", 16, 6000, Easing.SinInOut, null, () => true);
            border.Opacity = 0;
            border.Stroke = Colors.Transparent;
            absoluteLayoutCircles.Children.Add(border);
            return border;
        }


        private async Task FadeInAll()
        {
            var fadeInAnimation1 = new Animation(v => StackLoginPart1.Opacity = v, 0, 1);
            fadeInAnimation1.Commit(this, "FadeIn1", 16, 2000, Easing.Linear);

            var fadeInAnimation2 = new Animation(v => StackLoginPart2.Opacity = v, 0, 1);
            fadeInAnimation2.Commit(this, "FadeIn2", 16, 2000, Easing.Linear);

            var fadeInAnimation3 = new Animation(v => StackLoginPart3.Opacity = v, 0, 1);
            fadeInAnimation3.Commit(this, "FadeIn3", 16, 2000, Easing.Linear);

            await Task.Delay(2000);

        }
        private async void ImagesAnimations()
        {
            while (true)
            {
                animationImagesLogin.Source = _listImages[_indexImages];
                switch (_indexImages)
                {
                    case 0:
                        _indexImages += 1;
                        break;

                    case 1:
                        _indexImages += 1;
                        break;

                    case 2:
                        _indexImages = 0;
                        break;
                }
                await animationImagesLogin.FadeTo(1, 2000);
                await Task.Delay(10000);
                await animationImagesLogin.FadeTo(0, 2000);
            }
        }
        private async void OnFocusedUser(object sender, FocusEventArgs e)
        {
            iconUser.Source = "user_icon_active.png";
            await iconUser.ScaleTo(1.4, 300, Easing.CubicIn);
            await iconUser.ScaleTo(1.0, 250, Easing.CubicOut);
        }

        private async void ActiveButtonLogin()
        {
            if (!string.IsNullOrWhiteSpace(LoginUserText))
            {
                Regex regexMail = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                if (regexMail.IsMatch(LoginUserText))
                {
                    _correctMail = true;
                    ErrorMail.Children.Clear();
                    _elementOfErrorMail = 0;
                }
                else
                {
                    _correctMail = false;
                    if (_elementOfErrorMail == 0)
                    {
                        // Crear el AbsoluteLayout principal
                        var innerAbsoluteLayout = new AbsoluteLayout
                        {
                            VerticalOptions = LayoutOptions.Fill,
                            HeightRequest = (int)((GlobalConstants.ScreenHeight * 0.034) / GlobalConstants.ScreenDensity)
                        };
                        AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, 23, 1, 0.1));
                        AbsoluteLayout.SetLayoutFlags(innerAbsoluteLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.WidthProportional);
                        // Crear el Border
                        var border = new Border
                        {
                            BackgroundColor = Color.FromArgb("#CD5C5C"),
                            VerticalOptions = LayoutOptions.Fill,
                            HorizontalOptions = LayoutOptions.Fill,
                            StrokeThickness = 0,
                            Padding = new Microsoft.Maui.Thickness(0)
                        };
                        AbsoluteLayout.SetLayoutBounds(border, new Rect(0, 17, 1, 1));
                        AbsoluteLayout.SetLayoutFlags(border, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                        // Definir la forma del borde con esquinas redondeadas en la parte inferior
                        border.StrokeShape = new RoundRectangle
                        {
                            CornerRadius = new Microsoft.Maui.CornerRadius(0, 0, 5, 5)
                        };

                        // Crear el Label
                        var label = new Label
                        {
                            Text = "El Correo Proporcionado No Es Valido",
                            FontFamily = "PoppinsRegular",
                            FontSize = (int)((GlobalConstants.ScreenHeight * 0.0125) / GlobalConstants.ScreenDensity),
                            TextColor = Colors.White,
                            HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Fill,
                            VerticalOptions = LayoutOptions.End
                        };
                        AbsoluteLayout.SetLayoutBounds(label, new Rect(0.5, 0.5, 1, 1));
                        AbsoluteLayout.SetLayoutFlags(label, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                        // Agregar el Label al Border
                        border.Content = label;

                        // Agregar el Border al AbsoluteLayout
                        innerAbsoluteLayout.Children.Add(border);

                        // Agregar el AbsoluteLayout principal al AbsoluteLayout con nombre ErrorMail
                        ErrorMail.Children.Add(innerAbsoluteLayout);
                        _elementOfErrorMail = 1;
                        // Ejecutar la animación para mover el AbsoluteLayout 
                        var taskCompletionSource = new TaskCompletionSource<bool>();
                        innerAbsoluteLayout.Animate("moveAnimation", new Animation(v => AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, v, 1, 0.1)), 23, 50), length: 200, easing: Easing.Linear, finished: (v, c) => taskCompletionSource.SetResult(true));
                        await taskCompletionSource.Task;
                        await innerAbsoluteLayout.ScaleTo(1.01, 200, Easing.CubicIn);
                        await innerAbsoluteLayout.ScaleTo(1.0, 200, Easing.CubicOut);
                    }
                }
            }
            else
            {
                ErrorMail.Children.Clear();
                _elementOfErrorMail = 0;
            }
            if (!string.IsNullOrWhiteSpace(LoginPasswordText))
            {
                _correctPassword = true;
                ErrorPassword.Children.Clear();
            }
            else
            {
                _correctPassword = false;

            }

            if (!string.IsNullOrWhiteSpace(LoginUserText) && !string.IsNullOrWhiteSpace(LoginPasswordText) && _correctMail && _correctPassword)
            {
                IsButtonLoginActive = true;
                buttonLogin.BackgroundColor = Color.FromArgb("#6f69cf");
            }
            else if (string.IsNullOrWhiteSpace(LoginPasswordText) || string.IsNullOrWhiteSpace(LoginUserText) || !_correctMail || _correctPassword)
            {
                IsButtonLoginActive = false;
                buttonLogin.BackgroundColor = Color.FromArgb("#b9b5ff");
            }
        }

        private async void OnUnfocusedUser(object sender, FocusEventArgs e)
        {
            iconUser.Source = "user_icon_innactive.png";
            await iconUser.ScaleTo(0.6, 300, Easing.CubicIn);
            await iconUser.ScaleTo(1.0, 250, Easing.CubicOut);
        }

        private async void OnFocusedPassword(object sender, FocusEventArgs e)
        {
            iconPassword.Source = "password_icon_active.png";
            await iconPassword.ScaleTo(1.4, 250, Easing.CubicIn);
            await iconPassword.ScaleTo(1.0, 250, Easing.CubicOut);
        }

        private async void OnUnfocusedPassword(object sender, FocusEventArgs e)
        {
            iconPassword.Source = "password_icon_innactive.png";
            await iconPassword.ScaleTo(0.6, 300, Easing.CubicIn);
            await iconPassword.ScaleTo(1.0, 250, Easing.CubicOut);
        }

        private async void OnClickHandler(object sender, EventArgs e)
        {
            if (sender is CustomLabel label)
            {
                if (label.AutomationId == "label_register")
                {
                    try
                    {
                        await AnimationStackFadeReg();
                        await SwapPositionsBorders();
                        await Task.Delay(200);
                        var page = GlobalConstants.ServiceProvider.GetRequiredService<RegisterWS>();
                        Microsoft.Maui.Controls.Application.Current.MainPage = page;

                    }
                    catch (System.Exception ex)
                    {
                        Logger.Log($"ERROR:{ex.Message}");
                    }
                }
                else if (label.AutomationId == "label_enter_without")
                {
                    LoaderPrincipalPage.BackgroundColor = Color.FromArgb("#7e7e7e");
                    LoaderPrincipalPage.Opacity = 0.5;
                    LoaderPrincipalPage.VerticalOptions = LayoutOptions.Fill;
                    LoaderPrincipalPage.HorizontalOptions = LayoutOptions.Fill;

                    var gifLoader = new FFImageLoading.Maui.CachedImage
                    {
                        Source = "loader_principal_page.gif", // Reemplaza con el nombre de tu GIF en Resources
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = (int)((GlobalConstants.ScreenWidth * 0.065) / GlobalConstants.ScreenDensity),
                        HeightRequest = (int)((GlobalConstants.ScreenHeight * 0.065) / GlobalConstants.ScreenDensity),
                        Aspect = Aspect.AspectFit
                    };

                    // Configurar LayoutBounds y LayoutFlags
                    AbsoluteLayout.SetLayoutBounds(gifLoader, new Rect(0.5, 0.5, 64, 64)); // Centrar en la pantalla
                    AbsoluteLayout.SetLayoutFlags(gifLoader, AbsoluteLayoutFlags.PositionProportional);
                    LoaderPrincipalPage.Children.Add(gifLoader);
                    LoaderPrincipalPage.IsVisible = true;
                    await Task.Delay(10000);
                    Microsoft.Maui.Controls.Application.Current.MainPage = new HomePage();
                }
            }
            if (sender is CustomButton button)
            {
                Logger.Log($"Es un custom Button");
                if (button.AutomationId == "button_login" && IsButtonLoginActive)
                {
                    LoaderPrincipalPage.BackgroundColor = Color.FromArgb("#7e7e7e");
                    LoaderPrincipalPage.Opacity = 0.5;
                    LoaderPrincipalPage.VerticalOptions = LayoutOptions.Fill;
                    LoaderPrincipalPage.HorizontalOptions = LayoutOptions.Fill;

                    var gifLoader = new FFImageLoading.Maui.CachedImage
                    {
                        Source = "loader_principal_page.gif", // Reemplaza con el nombre de tu GIF en Resources
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = (int)((GlobalConstants.ScreenWidth * 0.065) / GlobalConstants.ScreenDensity),
                        HeightRequest = (int)((GlobalConstants.ScreenHeight * 0.065) / GlobalConstants.ScreenDensity),
                        Aspect = Aspect.AspectFit
                    };

                    // Configurar LayoutBounds y LayoutFlags
                    AbsoluteLayout.SetLayoutBounds(gifLoader, new Rect(0.5, 0.5, 64, 64)); // Centrar en la pantalla
                    AbsoluteLayout.SetLayoutFlags(gifLoader, AbsoluteLayoutFlags.PositionProportional);
                    LoaderPrincipalPage.Children.Add(gifLoader);
                    LoaderPrincipalPage.IsVisible = true;
                    await Task.Delay(10000);
                    Microsoft.Maui.Controls.Application.Current.MainPage = new HomePage();
                }
            }
        }

        private async Task AnimationStackFadeReg()
        {
            var fadeOutAnimation1 = new Animation(v => StackLoginPart1.Opacity = v, 1, 0);
            fadeOutAnimation1.Commit(this, "FadeOut1", 16, 300, Easing.Linear);

            var fadeOutAnimation2 = new Animation(v => StackLoginPart2.Opacity = v, 1, 0);
            fadeOutAnimation2.Commit(this, "FadeOut2", 16, 300, Easing.Linear);

            var fadeOutAnimation3 = new Animation(v => StackLoginPart3.Opacity = v, 1, 0);
            fadeOutAnimation3.Commit(this, "FadeOut3", 16, 300, Easing.Linear);
            await Task.Delay(300);
            if (BorderLayoutLoginPart1.Content is Layout layout1)
            {
                layout1.Children.Clear();
            }
        }

        private async Task SwapPositionsBorders()
        {
            var layoutBounds1 = AbsoluteLayout.GetLayoutBounds(BorderLayoutLoginPart1);
            var layoutBounds2 = AbsoluteLayout.GetLayoutBounds(BorderLayoutLoginPart2);
            var swapAnimation = new Animation();
            swapAnimation.Add(0, 1, new Animation(v => AbsoluteLayout.SetLayoutBounds(BorderLayoutLoginPart1, new Rect(v, layoutBounds1.Y, layoutBounds1.Width, layoutBounds1.Height)), layoutBounds1.X, layoutBounds2.X));
            swapAnimation.Add(0, 1, new Animation(v => AbsoluteLayout.SetLayoutBounds(BorderLayoutLoginPart2, new Rect(v, layoutBounds2.Y, layoutBounds2.Width, layoutBounds2.Height)), layoutBounds2.X, layoutBounds1.X));
            swapAnimation.Commit(this, "Swap", 16, 300, Easing.Linear);
            await Task.Delay(300);
            // Cambiar las esquinas redondeadas 
            BorderLayoutLoginPart1.StrokeShape = new RoundRectangle { CornerRadius = new Microsoft.Maui.CornerRadius(0, 18, 0, 18) };
            BorderLayoutLoginPart2.StrokeShape = new RoundRectangle { CornerRadius = new Microsoft.Maui.CornerRadius(18, 0, 18, 0) };
            await Task.Delay(600);
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
