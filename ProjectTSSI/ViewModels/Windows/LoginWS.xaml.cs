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


namespace ProjectTSSI
{
    public partial class LoginWS : ContentPage, INotifyPropertyChanged
    {
        private readonly List<string> _listImages = new List<string>
        {
            "login_image_one.png",
            "login_image_two.png",
            "login_image_three.png"
        };
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
        public LoginWS()
        {
            InitializeComponent();
            BindingContext = this; // Establecer el contexto de enlace
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ImagesAnimations();
            await FadeInAll();
            SetWindowRestrictions();
        }
        private async Task FadeInAll(){
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
                        animationImagesLogin.HeightRequest = 350;
                        animationImagesLogin.WidthRequest = 280;
                        AbsoluteLayout.SetLayoutBounds(animationImagesLogin, new Rect(70, -3, 280, 350));
                        _indexImages += 1;
                        break;

                    case 1:
                        animationImagesLogin.HeightRequest = 350;
                        animationImagesLogin.WidthRequest = 300;
                        AbsoluteLayout.SetLayoutBounds(animationImagesLogin, new Rect(0, 8, 280, 350));
                        _indexImages += 1;
                        break;

                    case 2:
                        animationImagesLogin.HeightRequest = 355;
                        animationImagesLogin.WidthRequest = 340;
                        AbsoluteLayout.SetLayoutBounds(animationImagesLogin, new Rect(0, -6, 280, 350));
                        _indexImages = 0;
                        break;
                }
                await animationImagesLogin.FadeTo(1, 2000);
                await Task.Delay(10000);
                await animationImagesLogin.FadeTo(0, 2000);
            }
        }
        private void SetWindowRestrictions(){
           // Obtén la ventana actual de la aplicación
            var mauiWindow = Application.Current.Windows[0].Handler.PlatformView as MauiWinUIWindow;
            var appWindow = mauiWindow?.AppWindow;

            if (appWindow != null){
                 // Configura el tamaño de la ventana
                appWindow.Resize(new Windows.Graphics.SizeInt32(1900, 1050));

                /// Configura el tamaño mínimo de la ventana
                var presenter = appWindow.Presenter as OverlappedPresenter;
                if (presenter != null){
                    presenter.IsResizable = false; // Bloquea el redimensionamiento de la ventana
                    presenter.IsMaximizable = false; //Bloquea la maximizacion
                }
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
            if(!string.IsNullOrWhiteSpace(LoginUserText)){
                Regex regexMail = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"); 
                if(regexMail.IsMatch(LoginUserText)){
                    _correctMail = true;
                    ErrorMail.Children.Clear();
                    _elementOfErrorMail = 0;
                } 
                else{
                    _correctMail = false;
                    if(_elementOfErrorMail == 0){
                        // Crear el AbsoluteLayout principal
                    var innerAbsoluteLayout = new AbsoluteLayout{
                        HeightRequest = 35,
                    };
                    AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, 23, 1, 0.1));
                    AbsoluteLayout.SetLayoutFlags(innerAbsoluteLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.WidthProportional);
                    // Crear el Border
                    var border = new Border{
                        BackgroundColor = Color.FromArgb("#CD5C5C"),
                        HeightRequest = 35,
                        StrokeThickness = 0,
                        Padding = new Thickness(0)
                    };
                    AbsoluteLayout.SetLayoutBounds(border, new Rect(0, 17, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(border, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                    // Definir la forma del borde con esquinas redondeadas en la parte inferior
                    border.StrokeShape = new RoundRectangle{
                        CornerRadius = new CornerRadius(0, 0, 5, 5)
                    };

                    // Crear el Label
                    var label = new Label{
                        Text = "El Correo Proporcionado No Es Valido",
                        FontFamily = "PoppinsRegular",
                        FontSize = 11,
                        TextColor = Colors.White,
                        HorizontalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
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
            else{
                ErrorMail.Children.Clear();
                _elementOfErrorMail = 0;
            }
            if(!string.IsNullOrWhiteSpace(LoginPasswordText)){
                _correctPassword = true;
                ErrorPassword.Children.Clear();
            } 
            else{
                _correctPassword = false;
                
            } 

            if(!string.IsNullOrWhiteSpace(LoginUserText) && !string.IsNullOrWhiteSpace(LoginPasswordText) && _correctMail && _correctPassword){
                IsButtonLoginActive = true;
                buttonLogin.BackgroundColor = Color.FromArgb("#6f69cf");
            }
            else if(string.IsNullOrWhiteSpace(LoginPasswordText) || string.IsNullOrWhiteSpace(LoginUserText) || !_correctMail || _correctPassword){
                IsButtonLoginActive = false;
                buttonLogin.BackgroundColor = Color.FromArgb("#b9b5ff");
            }
        }

        private async void OnUnfocusedUser(object sender, FocusEventArgs e)
        {
            iconUser.Source = "user_icon_inactive.png";
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
            iconPassword.Source = "password_icon_inactive.png";
            await iconPassword.ScaleTo(0.6, 300, Easing.CubicIn);
            await iconPassword.ScaleTo(1.0, 250, Easing.CubicOut);
        }

        private async void OnClickHandler(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                if (label.AutomationId == "LabelRegister"){
                    try{
                        await AnimationStackFadeReg();
                        await SwapPositionsBorders();
                        await Task.Delay(200);
                        Application.Current.MainPage = new RegisterWS();
                    }
                    catch (System.Exception ex){
                        DisplayAlert("ERROR",ex.Message,"OK");
                    }
                }
                else if (label.AutomationId == "LabelEnterWithOut")
                {
                    LoaderPrincipalPage.BackgroundColor = Color.FromArgb("#7e7e7e");
                    LoaderPrincipalPage.Opacity = 0.5;
                    LoaderPrincipalPage.VerticalOptions = LayoutOptions.FillAndExpand;
                    LoaderPrincipalPage.HorizontalOptions = LayoutOptions.FillAndExpand;

                    var gifLoader = new FFImageLoading.Maui.CachedImage
                    {
                        Source = "loader_principal_page.gif", // Reemplaza con el nombre de tu GIF en Resources
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 64,
                        HeightRequest = 64,
                        Aspect = Aspect.AspectFit
                    };

                    // Configurar LayoutBounds y LayoutFlags
                    AbsoluteLayout.SetLayoutBounds(gifLoader, new Rect(0.5, 0.5, 64, 64)); // Centrar en la pantalla
                    AbsoluteLayout.SetLayoutFlags(gifLoader, AbsoluteLayoutFlags.PositionProportional);
                    LoaderPrincipalPage.Children.Add(gifLoader);
                    LoaderPrincipalPage.IsVisible = true;
                    await Task.Delay(10000);
                    Application.Current.MainPage = new PrincipalPage();
                }
            }
            if(sender is Button button){
                if(button.AutomationId == "ButtonToLogin" && IsButtonLoginActive){
                    LoaderPrincipalPage.BackgroundColor = Color.FromArgb("#7e7e7e");
                    LoaderPrincipalPage.Opacity = 0.5;
                    LoaderPrincipalPage.VerticalOptions = LayoutOptions.FillAndExpand;
                    LoaderPrincipalPage.HorizontalOptions = LayoutOptions.FillAndExpand;

                    var gifLoader = new FFImageLoading.Maui.CachedImage
                    {
                        Source = "loader_principal_page.gif", // Reemplaza con el nombre de tu GIF en Resources
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 64,
                        HeightRequest = 64,
                        Aspect = Aspect.AspectFit
                    };

                    // Configurar LayoutBounds y LayoutFlags
                    AbsoluteLayout.SetLayoutBounds(gifLoader, new Rect(0.5, 0.5, 64, 64)); // Centrar en la pantalla
                    AbsoluteLayout.SetLayoutFlags(gifLoader, AbsoluteLayoutFlags.PositionProportional);
                    LoaderPrincipalPage.Children.Add(gifLoader);
                    LoaderPrincipalPage.IsVisible = true;
                    await Task.Delay(10000);
                    Application.Current.MainPage = new PrincipalPage();
                }
            }
        }

        private async Task AnimationStackFadeReg(){
            var fadeOutAnimation1 = new Animation(v => StackLoginPart1.Opacity = v, 1, 0);
            fadeOutAnimation1.Commit(this, "FadeOut1", 16, 300, Easing.Linear);
    
            var fadeOutAnimation2 = new Animation(v => StackLoginPart2.Opacity = v, 1, 0);
            fadeOutAnimation2.Commit(this, "FadeOut2", 16, 300, Easing.Linear);

            var fadeOutAnimation3 = new Animation(v => StackLoginPart3.Opacity = v, 1, 0);
            fadeOutAnimation3.Commit(this, "FadeOut3", 16, 300, Easing.Linear);
            await Task.Delay(300);
            if (BorderLayoutLoginPart1.Content is Layout layout1){
                layout1.Children.Clear();
            }
        }

        private async Task SwapPositionsBorders(){
            var layoutBounds1 = AbsoluteLayout.GetLayoutBounds(BorderLayoutLoginPart1);
            var layoutBounds2 = AbsoluteLayout.GetLayoutBounds(BorderLayoutLoginPart2);
            var swapAnimation = new Animation();
            swapAnimation.Add(0, 1, new Animation(v => AbsoluteLayout.SetLayoutBounds(BorderLayoutLoginPart1, new Rect(v, layoutBounds1.Y, layoutBounds1.Width, layoutBounds1.Height)), layoutBounds1.X, layoutBounds2.X));
            swapAnimation.Add(0, 1, new Animation(v => AbsoluteLayout.SetLayoutBounds(BorderLayoutLoginPart2, new Rect(v, layoutBounds2.Y, layoutBounds2.Width, layoutBounds2.Height)), layoutBounds2.X, layoutBounds1.X));
            swapAnimation.Commit(this, "Swap", 16, 300, Easing.Linear);
            await Task.Delay(300);
            // Cambiar las esquinas redondeadas 
            BorderLayoutLoginPart1.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(0, 18, 0, 18) };
            BorderLayoutLoginPart2.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(18,0,18,0) };
            await Task.Delay(600);
        }
 
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
