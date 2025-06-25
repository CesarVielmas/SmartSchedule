using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Maui.Layouts;
using Microsoft.UI.Windowing;
using Microsoft.Maui.Platform;


namespace ProjectTSSI
{
    public partial class RegisterWS : ContentPage, INotifyPropertyChanged
    {
        private readonly List<string> _listImagesRegister = new List<string> { "login_image_one.png", "login_image_two.png", "login_image_three.png" };
        private int _stepNow = 0;
        private string registerNameText = "";
        private string registerFatherNameText = "";
        private string registerMotherNameText = "";
        private string registerMailText = "";
        private string registerPasswordText = "";
        private string registerConfirmPasswordText = "";
        private int _indexImagesRegister = 0;
        private bool _correctName = false;
        private int _elementOfName = 0;
        private bool _correctFatherName = false;
        private int _elementOfFatherName = 0;
        private bool _correctMotherName = false;
        private int _elementOfMotherName = 0;
        private bool _correctMail = false;
        private int _elementOfMail = 0;
        private bool _correctPassword = false;
        private int _elementOfPassword = 0;
        private bool _correctConfirmPassword = false;
        private int _elementOfConfirmPassword = 0;
        private bool _continueNextStep = false;
        private StackLayout _currentPartRegisterContent;
        public string RegisterNameText
        {
            get => registerNameText;
            set
            {
                if (registerNameText != value)
                {
                    registerNameText = value;
                    OnPropertyChanged();
                    CheckErrors();
                }
            }
        }
        public string RegisterFatherNameText
        {
            get => registerFatherNameText;
            set
            {
                if (registerFatherNameText != value)
                {
                    registerFatherNameText = value;
                    OnPropertyChanged();
                    CheckErrors();
                }
            }
        }
        public string RegisterMotherNameText
        {
            get => registerMotherNameText;
            set
            {
                if (registerMotherNameText != value)
                {
                    registerMotherNameText = value;
                    OnPropertyChanged();
                    CheckErrors();
                }
            }
        }
        public string RegisterMailText
        {
            get => registerMailText;
            set
            {
                if (registerMailText != value)
                {
                    registerMailText = value;
                    OnPropertyChanged();
                    CheckErrors();
                }
            }
        }
        public string RegisterPasswordText
        {
            get => registerPasswordText;
            set
            {
                if (registerPasswordText != value)
                {
                    registerPasswordText = value;
                    OnPropertyChanged();
                    CheckErrors();
                }
            }
        }
        public string RegisterConfirmPasswordText
        {
            get => registerConfirmPasswordText;
            set
            {
                if (registerConfirmPasswordText != value)
                {
                    registerConfirmPasswordText = value;
                    OnPropertyChanged();
                    CheckErrors();
                }
            }
        }
        public RegisterWS()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                ImagesAnimations();
                await AssignAllEntrysFirst();
                await FadeInAll();
                SetWindowRestrictions();
            }
            catch (System.Exception e)
            {
                DisplayAlert("ERROR", e.Message, "OK");

            }

        }
        private async Task AssignAllEntrysFirst()
        {
            // Obtener el ensamblado actual
            var assembly = Assembly.GetExecutingAssembly();
            // Buscar el recurso embebido
            using (var stream = assembly.GetManifestResourceStream("ProjectTSSI.Views.Windows.RegisterPart1.xaml"))
            {
                using (var reader = new StreamReader(stream))
                {
                    // Leer el contenido XAML
                    var xamlContent = reader.ReadToEnd();
                    var stackLayout = new StackLayout();
                    stackLayout.LoadFromXaml(xamlContent);

                    // Asegurarse de que el contenido tiene el tamaño correcto
                    stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                    stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;

                    // Agregar el XAML al layout
                    _currentPartRegisterContent = stackLayout;
                    RegisterForm.Children.Add(_currentPartRegisterContent);
                }
            }
            // Asegurarse de que el XAML se haya cargado y la UI esté actualizada
            await Task.Yield(); // Esto libera el hilo principal y asegura que la UI se actualice
            var entryName = _currentPartRegisterContent.FindByName<Entry>("entryName");
            var entryPathernal = _currentPartRegisterContent.FindByName<Entry>("entryPathernal");
            var entryMaternal = _currentPartRegisterContent.FindByName<Entry>("entryMaternal");
            entryName.Focused += OnFocusedName;
            entryName.Unfocused += OnUnfocusedName;
            entryPathernal.Focused += OnFocusedFatherName;
            entryPathernal.Unfocused += OnUnfocusedFatherName;
            entryMaternal.Focused += OnFocusedMotherName;
            entryMaternal.Unfocused += OnUnfocusedMotherName;
        }
        private void SetWindowRestrictions()
        {
            // Obtén la ventana actual de la aplicación
            var mauiWindow = Application.Current.Windows[0].Handler.PlatformView as MauiWinUIWindow;
            var appWindow = mauiWindow?.AppWindow;

            if (appWindow != null)
            {
                // Configura el tamaño de la ventana
                appWindow.Resize(new Windows.Graphics.SizeInt32(1900, 1050));

                /// Configura el tamaño mínimo de la ventana
                var presenter = appWindow.Presenter as OverlappedPresenter;
                if (presenter != null)
                {
                    presenter.IsResizable = false; // Bloquea el redimensionamiento de la ventana
                    presenter.IsMaximizable = false; //Bloquea la maximizacion
                }
            }
        }
        private async Task FadeInAll()
        {
            var fadeInAnimation1 = new Animation(v => StackRegisterPart1.Opacity = v, 0, 1);
            fadeInAnimation1.Commit(this, "FadeIn1", 16, 2000, Easing.Linear);

            var fadeInAnimation2 = new Animation(v => StackRegisterPart2.Opacity = v, 0, 1);
            fadeInAnimation2.Commit(this, "FadeIn2", 16, 2000, Easing.Linear);

            var fadeInAnimation3 = new Animation(v => RegisterFadeOutPart1.Opacity = v, 0, 1);
            fadeInAnimation3.Commit(this, "FadeIn3", 16, 2000, Easing.Linear);

            var fadeInAnimation4 = new Animation(v => RegisterFadeOutPart2.Opacity = v, 0, 1);
            fadeInAnimation3.Commit(this, "FadeIn4", 16, 2000, Easing.Linear);
            await Task.Delay(2000);

        }
        private async void ImagesAnimations()
        {
            while (true)
            {
                animationImagesRegister.Source = _listImagesRegister[_indexImagesRegister];
                switch (_indexImagesRegister)
                {
                    case 0:
                        animationImagesRegister.HeightRequest = 350;
                        animationImagesRegister.WidthRequest = 280;
                        AbsoluteLayout.SetLayoutBounds(animationImagesRegister, new Rect(70, -3, 280, 350));
                        _indexImagesRegister += 1;
                        break;

                    case 1:
                        animationImagesRegister.HeightRequest = 350;
                        animationImagesRegister.WidthRequest = 300;
                        AbsoluteLayout.SetLayoutBounds(animationImagesRegister, new Rect(0, 8, 280, 350));
                        _indexImagesRegister += 1;
                        break;

                    case 2:
                        animationImagesRegister.HeightRequest = 355;
                        animationImagesRegister.WidthRequest = 340;
                        AbsoluteLayout.SetLayoutBounds(animationImagesRegister, new Rect(0, -6, 280, 350));
                        _indexImagesRegister = 0;
                        break;
                }
                await animationImagesRegister.FadeTo(1, 2000);
                await Task.Delay(10000);
                await animationImagesRegister.FadeTo(0, 2000);
            }
        }
        private async void OnFocusedName(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconName").Source = "icon_name_active.png";
            await _currentPartRegisterContent.FindByName<Image>("iconName").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconName").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void OnUnfocusedName(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconName").Source = "icon_name_innactive.png";
            await _currentPartRegisterContent.FindByName<Image>("iconName").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconName").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void OnFocusedMotherName(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconMaternal").Source = "icon_mother_name_active.png";
            await _currentPartRegisterContent.FindByName<Image>("iconMaternal").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconMaternal").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void OnUnfocusedMotherName(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconMaternal").Source = "icon_mother_name_innactive.png";
            await _currentPartRegisterContent.FindByName<Image>("iconMaternal").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconMaternal").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void OnFocusedFatherName(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconPathernal").Source = "icon_father_name_active.png";
            await _currentPartRegisterContent.FindByName<Image>("iconPathernal").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconPathernal").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void OnUnfocusedFatherName(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconPathernal").Source = "icon_father_name_innactive.png";
            await _currentPartRegisterContent.FindByName<Image>("iconPathernal").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconPathernal").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void OnFocusedMail(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconMail").Source = "user_icon_active.png";
            await _currentPartRegisterContent.FindByName<Image>("iconMail").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconMail").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void OnUnfocusedMail(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconMail").Source = "user_icon_inactive.png";
            await _currentPartRegisterContent.FindByName<Image>("iconMail").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconMail").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void OnFocusedPassword(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconPassword").Source = "password_icon_active.png";
            await _currentPartRegisterContent.FindByName<Image>("iconPassword").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconPassword").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void OnUnfocusedPassword(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconPassword").Source = "password_icon_inactive.png";
            await _currentPartRegisterContent.FindByName<Image>("iconPassword").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconPassword").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void OnFocusedConfirmPassword(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconConfirmPassword").Source = "confirm_password_icon_active.png";
            await _currentPartRegisterContent.FindByName<Image>("iconConfirmPassword").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconConfirmPassword").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void OnUnfocusedConfirmPassword(object sender, FocusEventArgs e)
        {
            _currentPartRegisterContent.FindByName<Image>("iconConfirmPassword").Source = "confirm_password_icon_innactive.png";
            await _currentPartRegisterContent.FindByName<Image>("iconConfirmPassword").ScaleTo(1.4, 300, Easing.CubicIn);
            await _currentPartRegisterContent.FindByName<Image>("iconConfirmPassword").ScaleTo(1.0, 250, Easing.CubicOut);
        }
        private async void CheckErrors()
        {
            Regex regexValidateName = new Regex(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            Regex regexValidateMail = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            Regex regexValidatePassword = new Regex(@"^(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            if (_stepNow == 0)
            {
                if (!string.IsNullOrWhiteSpace(RegisterNameText))
                {
                    if (regexValidateName.IsMatch(RegisterNameText))
                    {
                        _correctName = true;
                        _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorName").Children.Clear();
                        _elementOfName = 0;
                    }
                    else
                    {
                        _correctName = false;
                        //Code Creator
                        if (_elementOfName == 0)
                        {
                            // Crear el AbsoluteLayout principal
                            var innerAbsoluteLayout = new AbsoluteLayout
                            {
                                HeightRequest = 35,
                            };
                            AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, 23, 1, 0.1));
                            AbsoluteLayout.SetLayoutFlags(innerAbsoluteLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.WidthProportional);
                            // Crear el Border
                            var border = new Border
                            {
                                BackgroundColor = Color.FromArgb("#CD5C5C"),
                                HeightRequest = 35,
                                StrokeThickness = 0,
                                Padding = new Thickness(0)
                            };
                            AbsoluteLayout.SetLayoutBounds(border, new Rect(0, 17, 1, 1));
                            AbsoluteLayout.SetLayoutFlags(border, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                            // Definir la forma del borde con esquinas redondeadas en la parte inferior
                            border.StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(0, 0, 5, 5)
                            };

                            // Crear el Label
                            var label = new Label
                            {
                                Text = "El Nombre Proporcionado No Es Valido",
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
                            _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorName").Children.Add(innerAbsoluteLayout);
                            _elementOfName = 1;
                            // Ejecutar la animación para mover el AbsoluteLayout 
                            var taskCompletionSource = new TaskCompletionSource<bool>();
                            innerAbsoluteLayout.Animate("moveAnimation1", new Animation(v => AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, v, 1, 0.1)), 23, 50), length: 200, easing: Easing.Linear, finished: (v, c) => taskCompletionSource.SetResult(true));
                            await taskCompletionSource.Task;
                            await innerAbsoluteLayout.ScaleTo(1.01, 200, Easing.CubicIn);
                            await innerAbsoluteLayout.ScaleTo(1.0, 200, Easing.CubicOut);
                        }
                    }
                }
                else
                {
                    _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorName").Children.Clear();
                    _elementOfName = 0;
                }
                if (!string.IsNullOrWhiteSpace(RegisterFatherNameText))
                {
                    if (regexValidateName.IsMatch(RegisterFatherNameText))
                    {
                        _correctFatherName = true;
                        _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorPathernal").Children.Clear();
                        _elementOfFatherName = 0;
                    }
                    else
                    {
                        _correctFatherName = false;
                        //Code Creator
                        if (_elementOfFatherName == 0)
                        {
                            // Crear el AbsoluteLayout principal
                            var innerAbsoluteLayout = new AbsoluteLayout
                            {
                                HeightRequest = 35,
                            };
                            AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, 23, 1, 0.1));
                            AbsoluteLayout.SetLayoutFlags(innerAbsoluteLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.WidthProportional);
                            // Crear el Border
                            var border = new Border
                            {
                                BackgroundColor = Color.FromArgb("#CD5C5C"),
                                HeightRequest = 35,
                                StrokeThickness = 0,
                                Padding = new Thickness(0)
                            };
                            AbsoluteLayout.SetLayoutBounds(border, new Rect(0, 17, 1, 1));
                            AbsoluteLayout.SetLayoutFlags(border, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                            // Definir la forma del borde con esquinas redondeadas en la parte inferior
                            border.StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(0, 0, 5, 5)
                            };

                            // Crear el Label
                            var label = new Label
                            {
                                Text = "El Apellido Paterno Proporcionado No Es Valido",
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
                            _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorPathernal").Children.Add(innerAbsoluteLayout);
                            _elementOfFatherName = 1;
                            // Ejecutar la animación para mover el AbsoluteLayout 
                            var taskCompletionSource = new TaskCompletionSource<bool>();
                            innerAbsoluteLayout.Animate("moveAnimation2", new Animation(v => AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, v, 1, 0.1)), 23, 50), length: 200, easing: Easing.Linear, finished: (v, c) => taskCompletionSource.SetResult(true));
                            await taskCompletionSource.Task;
                            await innerAbsoluteLayout.ScaleTo(1.01, 200, Easing.CubicIn);
                            await innerAbsoluteLayout.ScaleTo(1.0, 200, Easing.CubicOut);
                        }
                    }
                }
                else
                {
                    _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorPathernal").Children.Clear();
                    _elementOfFatherName = 0;
                }
                if (!string.IsNullOrWhiteSpace(RegisterMotherNameText))
                {
                    if (regexValidateName.IsMatch(RegisterMotherNameText))
                    {
                        _correctMotherName = true;
                        _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorMaternal").Children.Clear();
                        _elementOfMotherName = 0;
                    }
                    else
                    {
                        _correctMotherName = false;
                        //Code Creator
                        if (_elementOfMotherName == 0)
                        {
                            // Crear el AbsoluteLayout principal
                            var innerAbsoluteLayout = new AbsoluteLayout
                            {
                                HeightRequest = 35,
                            };
                            AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, 23, 1, 0.1));
                            AbsoluteLayout.SetLayoutFlags(innerAbsoluteLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.WidthProportional);
                            // Crear el Border
                            var border = new Border
                            {
                                BackgroundColor = Color.FromArgb("#CD5C5C"),
                                HeightRequest = 35,
                                StrokeThickness = 0,
                                Padding = new Thickness(0)
                            };
                            AbsoluteLayout.SetLayoutBounds(border, new Rect(0, 17, 1, 1));
                            AbsoluteLayout.SetLayoutFlags(border, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                            // Definir la forma del borde con esquinas redondeadas en la parte inferior
                            border.StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(0, 0, 5, 5)
                            };

                            // Crear el Label
                            var label = new Label
                            {
                                Text = "El Apellido Materno Proporcionado No Es Valido",
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
                            _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorMaternal").Children.Add(innerAbsoluteLayout);
                            _elementOfMotherName = 1;
                            // Ejecutar la animación para mover el AbsoluteLayout 
                            var taskCompletionSource = new TaskCompletionSource<bool>();
                            innerAbsoluteLayout.Animate("moveAnimation3", new Animation(v => AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, v, 1, 0.1)), 23, 50), length: 200, easing: Easing.Linear, finished: (v, c) => taskCompletionSource.SetResult(true));
                            await taskCompletionSource.Task;
                            await innerAbsoluteLayout.ScaleTo(1.01, 200, Easing.CubicIn);
                            await innerAbsoluteLayout.ScaleTo(1.0, 200, Easing.CubicOut);
                        }
                    }
                }
                else
                {
                    _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorMaternal").Children.Clear();
                    _elementOfMotherName = 0;
                }

            }
            else if (_stepNow == 1)
            {
                if (!string.IsNullOrWhiteSpace(RegisterMailText))
                {
                    if (regexValidateMail.IsMatch(RegisterMailText))
                    {
                        _correctMail = true;
                        _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorMail").Children.Clear();
                        _elementOfMail = 0;
                    }
                    else
                    {
                        _correctMail = false;
                        //Code Creator
                        if (_elementOfMail == 0)
                        {
                            // Crear el AbsoluteLayout principal
                            var innerAbsoluteLayout = new AbsoluteLayout
                            {
                                HeightRequest = 35,
                            };
                            AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, 23, 1, 0.1));
                            AbsoluteLayout.SetLayoutFlags(innerAbsoluteLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.WidthProportional);
                            // Crear el Border
                            var border = new Border
                            {
                                BackgroundColor = Color.FromArgb("#CD5C5C"),
                                HeightRequest = 35,
                                StrokeThickness = 0,
                                Padding = new Thickness(0)
                            };
                            AbsoluteLayout.SetLayoutBounds(border, new Rect(0, 17, 1, 1));
                            AbsoluteLayout.SetLayoutFlags(border, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                            // Definir la forma del borde con esquinas redondeadas en la parte inferior
                            border.StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(0, 0, 5, 5)
                            };

                            // Crear el Label
                            var label = new Label
                            {
                                Text = "El Correo Electronico Proporcionado No Es Valido",
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
                            _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorMail").Children.Add(innerAbsoluteLayout);
                            _elementOfMail = 1;
                            // Ejecutar la animación para mover el AbsoluteLayout 
                            var taskCompletionSource = new TaskCompletionSource<bool>();
                            innerAbsoluteLayout.Animate("moveAnimation4", new Animation(v => AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, v, 1, 0.1)), 23, 50), length: 200, easing: Easing.Linear, finished: (v, c) => taskCompletionSource.SetResult(true));
                            await taskCompletionSource.Task;
                            await innerAbsoluteLayout.ScaleTo(1.01, 200, Easing.CubicIn);
                            await innerAbsoluteLayout.ScaleTo(1.0, 200, Easing.CubicOut);
                        }
                    }
                }
                else
                {
                    _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorMail").Children.Clear();
                    _elementOfMail = 0;
                }
                if (!string.IsNullOrWhiteSpace(RegisterPasswordText))
                {
                    if (regexValidatePassword.IsMatch(RegisterPasswordText))
                    {
                        _correctPassword = true;
                        _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorPassword").Children.Clear();
                        _elementOfPassword = 0;
                    }
                    else
                    {
                        _correctPassword = false;
                        //Code Creator
                        if (_elementOfPassword == 0)
                        {
                            // Crear el AbsoluteLayout principal
                            var innerAbsoluteLayout = new AbsoluteLayout
                            {
                                HeightRequest = 35,
                            };
                            AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, 23, 1, 0.1));
                            AbsoluteLayout.SetLayoutFlags(innerAbsoluteLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.WidthProportional);
                            // Crear el Border
                            var border = new Border
                            {
                                BackgroundColor = Color.FromArgb("#CD5C5C"),
                                HeightRequest = 35,
                                StrokeThickness = 0,
                                Padding = new Thickness(0)
                            };
                            AbsoluteLayout.SetLayoutBounds(border, new Rect(0, 17, 1, 1));
                            AbsoluteLayout.SetLayoutFlags(border, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                            // Definir la forma del borde con esquinas redondeadas en la parte inferior
                            border.StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(0, 0, 5, 5)
                            };

                            // Crear el Label
                            var label = new Label
                            {
                                Text = "La Contraseña Proporcionada No Es Segura",
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
                            _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorPassword").Children.Add(innerAbsoluteLayout);
                            _elementOfPassword = 1;
                            // Ejecutar la animación para mover el AbsoluteLayout 
                            var taskCompletionSource = new TaskCompletionSource<bool>();
                            innerAbsoluteLayout.Animate("moveAnimation5", new Animation(v => AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, v, 1, 0.1)), 23, 50), length: 200, easing: Easing.Linear, finished: (v, c) => taskCompletionSource.SetResult(true));
                            await taskCompletionSource.Task;
                            await innerAbsoluteLayout.ScaleTo(1.01, 200, Easing.CubicIn);
                            await innerAbsoluteLayout.ScaleTo(1.0, 200, Easing.CubicOut);
                        }
                    }
                }
                else
                {
                    _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorPassword").Children.Clear();
                    _elementOfPassword = 0;
                }
                if (!string.IsNullOrWhiteSpace(RegisterConfirmPasswordText))
                {
                    if (RegisterConfirmPasswordText == RegisterPasswordText)
                    {
                        _correctConfirmPassword = true;
                        _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorConfirmPassword").Children.Clear();
                        _elementOfConfirmPassword = 0;
                    }
                    else
                    {
                        _correctConfirmPassword = false;
                        //Code Creator
                        if (_elementOfConfirmPassword == 0)
                        {
                            // Crear el AbsoluteLayout principal
                            var innerAbsoluteLayout = new AbsoluteLayout
                            {
                                HeightRequest = 35,
                            };
                            AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, 23, 1, 0.1));
                            AbsoluteLayout.SetLayoutFlags(innerAbsoluteLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.WidthProportional);
                            // Crear el Border
                            var border = new Border
                            {
                                BackgroundColor = Color.FromArgb("#CD5C5C"),
                                HeightRequest = 35,
                                StrokeThickness = 0,
                                Padding = new Thickness(0)
                            };
                            AbsoluteLayout.SetLayoutBounds(border, new Rect(0, 17, 1, 1));
                            AbsoluteLayout.SetLayoutFlags(border, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                            // Definir la forma del borde con esquinas redondeadas en la parte inferior
                            border.StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(0, 0, 5, 5)
                            };

                            // Crear el Label
                            var label = new Label
                            {
                                Text = "Las Contraseñas No Coinciden",
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
                            _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorConfirmPassword").Children.Add(innerAbsoluteLayout);
                            _elementOfConfirmPassword = 1;
                            // Ejecutar la animación para mover el AbsoluteLayout 
                            var taskCompletionSource = new TaskCompletionSource<bool>();
                            innerAbsoluteLayout.Animate("moveAnimation5", new Animation(v => AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, v, 1, 0.1)), 23, 50), length: 200, easing: Easing.Linear, finished: (v, c) => taskCompletionSource.SetResult(true));
                            await taskCompletionSource.Task;
                            await innerAbsoluteLayout.ScaleTo(1.01, 200, Easing.CubicIn);
                            await innerAbsoluteLayout.ScaleTo(1.0, 200, Easing.CubicOut);
                        }
                    }
                }
                else
                {
                    _currentPartRegisterContent.FindByName<AbsoluteLayout>("ErrorConfirmPassword").Children.Clear();
                    _elementOfConfirmPassword = 0;
                }

            }
        }
        private async void OnNextStep(object sender, EventArgs e)
        {
            if (_stepNow == 0)
            {
                if (!string.IsNullOrWhiteSpace(RegisterNameText) && !string.IsNullOrWhiteSpace(RegisterFatherNameText) && !string.IsNullOrWhiteSpace(RegisterMotherNameText) && _correctName && _correctFatherName && _correctMotherName)
                {
                    await StepNextChanges();
                }
            }
            else if (_stepNow == 1)
            {
                if (!string.IsNullOrWhiteSpace(RegisterMailText) && !string.IsNullOrWhiteSpace(RegisterPasswordText) && !string.IsNullOrWhiteSpace(RegisterConfirmPasswordText) && _correctMail && _correctPassword && _correctConfirmPassword)
                {
                    await StepNextChanges();
                }
            }
            else if (_stepNow == 2)
            {
                if (!string.IsNullOrWhiteSpace(RegisterNameText) && !string.IsNullOrWhiteSpace(RegisterFatherNameText) && !string.IsNullOrWhiteSpace(RegisterMotherNameText) && !string.IsNullOrWhiteSpace(RegisterMailText) && !string.IsNullOrWhiteSpace(RegisterPasswordText) && !string.IsNullOrWhiteSpace(RegisterConfirmPasswordText) && _correctMail && _correctPassword && _correctConfirmPassword && _correctName && _correctFatherName && _correctMotherName)
                {
                    //Terminar y mandar a la pagina principal
                    //Finalizacion Pagina Principal
                    try
                    {
                        await StepNextChanges();
                    }
                    catch (System.Exception ex)
                    {
                        DisplayAlert("ERROR", ex.Message, "OK");
                    }

                }
            }
        }
        private async void OnBackStep(object sender, EventArgs e)
        {
            await StepBackChanges();
        }
        private async Task StepNextChanges()
        {
            _stepNow += 1;
            if (_stepNow == 1)
            {
                //Paso 1 al Paso 2
                BorderButtonBack.BackgroundColor = Color.FromArgb("#8589ea");
                ImageButtonBack.Source = "back_icon_white.png";
                LabelButtonBack.Text = "Regresar  Paso";
                LabelButtonBack.Style = (Style)Application.Current.Resources["BackToStepTextButton"];
                RegisterStepTop1.BackgroundColor = Color.FromArgb("#a1a6ff");
                var gradientBrush = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1),
                    GradientStops = new GradientStopCollection{
                            new GradientStop{
                                Color = Color.FromArgb("#a1a6ff"),
                                Offset = 0.0f // Inicio del gradiente
                            },
                            new GradientStop{
                                Color = Color.FromArgb("#8589ea"),
                                Offset = 0.0f // Superpuesto al inicio
                            }
                    }
                };
                RegisterStepTopBorder2.Background = gradientBrush;
                var animation = new Animation();
                animation.WithConcurrent(
                    (progress) =>
                    {
                        gradientBrush.GradientStops[1].Offset = (float)progress; // Progresivamente se expande
                    },
                    start: 0.0,
                    end: 1.0
                );
                animation.Commit(
                    owner: this,
                    name: "GradientAnimation",
                    length: 400,
                    easing: Easing.Linear
                );
                RegisterStepTopImage2.Source = "icon_register_two_first.png";
                RegisterStepTopLabel2.TextColor = Colors.White;
                //Change Part
                double distance = 600;
                var animationSlide = new Animation(v =>
                {
                    RegisterForm.TranslationX = v;
                }, 0, -distance);
                animationSlide.Commit(
                    owner: this,
                    name: "SlideAnimation",
                    length: 250,
                    easing: Easing.CubicInOut
                );
                await Task.Delay(250);
                RegisterForm.Children.Clear();
                RegisterForm.TranslationX += distance;
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream("ProjectTSSI.Views.Windows.RegisterPart2.xaml"))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var xamlContent = reader.ReadToEnd();
                        var stackLayout = new StackLayout();
                        stackLayout.LoadFromXaml(xamlContent);
                        stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                        stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
                        _currentPartRegisterContent = stackLayout;
                        RegisterForm.Children.Add(_currentPartRegisterContent);
                    }
                }
                _currentPartRegisterContent.FindByName<Entry>("entryMail").Focused += OnFocusedMail;
                _currentPartRegisterContent.FindByName<Entry>("entryMail").Unfocused += OnUnfocusedMail;
                _currentPartRegisterContent.FindByName<Entry>("entryPassword").Focused += OnFocusedPassword;
                _currentPartRegisterContent.FindByName<Entry>("entryPassword").Unfocused += OnUnfocusedPassword;
                _currentPartRegisterContent.FindByName<Entry>("entryConfirmPassword").Focused += OnFocusedConfirmPassword;
                _currentPartRegisterContent.FindByName<Entry>("entryConfirmPassword").Unfocused += OnUnfocusedConfirmPassword;
                //Animation
                await BorderButtonBack.ScaleTo(1.2, 300, Easing.CubicIn);
                await BorderButtonBack.ScaleTo(1.0, 250, Easing.CubicOut);
            }
            else if (_stepNow == 2)
            {
                //Paso 2 a la Finalizacion
                BorderButtonNext.BackgroundColor = Colors.Transparent;
                ImageButtonNext.Source = "next_icon_color.png";
                LabelButtonNext.Text = "Finalizar Registro";
                LabelButtonNext.Style = (Style)Application.Current.Resources["BackToLoginTextButton"];
                RegisterStepTop2.BackgroundColor = Color.FromArgb("#a1a6ff");
                var gradientBrush = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1),
                    GradientStops = new GradientStopCollection{
                            new GradientStop{
                                Color = Color.FromArgb("#a1a6ff"),
                                Offset = 0.0f // Inicio del gradiente
                            },
                            new GradientStop{
                                Color = Color.FromArgb("#8589ea"),
                                Offset = 0.0f // Superpuesto al inicio
                            }
                    }
                };
                RegisterStepTopBorder3.Background = gradientBrush;
                var animation = new Animation();
                animation.WithConcurrent(
                    (progress) =>
                    {
                        gradientBrush.GradientStops[1].Offset = (float)progress; // Progresivamente se expande
                    },
                    start: 0.0,
                    end: 1.0
                );
                animation.Commit(
                    owner: this,
                    name: "GradientAnimation",
                    length: 400,
                    easing: Easing.Linear
                );
                RegisterStepTopImage3.Source = "icon_register_three_first.png";
                RegisterStepTopLabel3.TextColor = Colors.White;
                await Task.Delay(250);
                double distance = 600;
                var animationSlide = new Animation(v =>
                {
                    RegisterForm.TranslationX = v;
                }, 0, -distance);
                animationSlide.Commit(
                    owner: this,
                    name: "SlideAnimation",
                    length: 250,
                    easing: Easing.CubicInOut
                );
                await Task.Delay(250);
                RegisterForm.Children.Clear();
                RegisterForm.TranslationX += distance;
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream("ProjectTSSI.Views.Windows.RegisterPart3.xaml"))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var xamlContent = reader.ReadToEnd();
                        var stackLayout = new StackLayout();
                        stackLayout.LoadFromXaml(xamlContent);
                        stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                        stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
                        _currentPartRegisterContent = stackLayout;
                        RegisterForm.Children.Add(_currentPartRegisterContent);
                    }
                }
                _currentPartRegisterContent.FindByName<Label>("FinalLabelName").Text = $"BIENVENIDO {RegisterNameText.Split(" ")[0].ToUpper()}";
                await BorderButtonNext.ScaleTo(1.2, 300, Easing.CubicIn);
                await BorderButtonNext.ScaleTo(1.0, 250, Easing.CubicOut);
            }
            else if (_stepNow == 3)
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
                RegisterForm.Children.Clear();
                LoaderPrincipalPage.Children.Clear();
                PrincipalGrid.Children.Clear();
                _currentPartRegisterContent = null;
                Application.Current.MainPage = new PrincipalPage();

            }
        }
        private async Task StepBackChanges()
        {
            _stepNow -= 1;
            if (_stepNow == -1)
            {
                //Salir hacia el login
                await AnimationStackFadeReg();
                await SwapPositionsBorders();
                await Task.Delay(200);
                Application.Current.MainPage = new LoginWS();
            }
            else if (_stepNow == 0)
            {
                //Paso 2 al Paso 1
                BorderButtonBack.BackgroundColor = Colors.Transparent;
                ImageButtonBack.Source = "back_icon_color.png";
                LabelButtonBack.Text = "Salir Del Registro";
                LabelButtonBack.Style = (Style)Application.Current.Resources["BackToLoginTextButton"];
                RegisterStepTop1.BackgroundColor = Color.FromArgb("#edeffe");
                RegisterStepTopBorder2.Background = Color.FromArgb("#edeffe");
                RegisterStepTopImage2.Source = "icon_register_two_second.png";
                RegisterStepTopLabel2.TextColor = Color.FromArgb("#9ea8c1");
                double distance = 600;
                var animationSlide = new Animation(v =>
                {
                    RegisterForm.TranslationX = v;
                }, 0, distance);
                animationSlide.Commit(
                    owner: this,
                    name: "SlideAnimation",
                    length: 250,
                    easing: Easing.CubicInOut
                );
                await Task.Delay(250);
                RegisterForm.Children.Clear();
                RegisterForm.TranslationX -= distance;
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream("ProjectTSSI.Views.Windows.RegisterPart1.xaml"))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var xamlContent = reader.ReadToEnd();
                        var stackLayout = new StackLayout();
                        stackLayout.LoadFromXaml(xamlContent);
                        stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                        stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
                        _currentPartRegisterContent = stackLayout;
                        RegisterForm.Children.Add(_currentPartRegisterContent);
                    }
                }
                _currentPartRegisterContent.FindByName<Entry>("entryName").Focused += OnFocusedName;
                _currentPartRegisterContent.FindByName<Entry>("entryName").Unfocused += OnUnfocusedName;
                _currentPartRegisterContent.FindByName<Entry>("entryPathernal").Focused += OnFocusedFatherName;
                _currentPartRegisterContent.FindByName<Entry>("entryPathernal").Unfocused += OnUnfocusedFatherName;
                _currentPartRegisterContent.FindByName<Entry>("entryMaternal").Focused += OnFocusedMotherName;
                _currentPartRegisterContent.FindByName<Entry>("entryMaternal").Unfocused += OnUnfocusedMotherName;
                await BorderButtonBack.ScaleTo(1.2, 300, Easing.CubicIn);
                await BorderButtonBack.ScaleTo(1.0, 250, Easing.CubicOut);
            }
            else if (_stepNow == 1)
            {
                //Finalizacion al Paso 2
                BorderButtonNext.BackgroundColor = Color.FromArgb("#8589ea");
                ImageButtonNext.Source = "next_icon_white.png";
                LabelButtonNext.Text = "Siguiente  Paso";
                LabelButtonNext.Style = (Style)Application.Current.Resources["BackToStepTextButton"];
                RegisterStepTop2.BackgroundColor = Color.FromArgb("#edeffe");
                RegisterStepTopBorder3.Background = Color.FromArgb("#edeffe");
                RegisterStepTopImage3.Source = "icon_register_three_second.png";
                RegisterStepTopLabel3.TextColor = Color.FromArgb("#9ea8c1");
                double distance = 600;
                var animationSlide = new Animation(v =>
                {
                    RegisterForm.TranslationX = v;
                }, 0, distance);
                animationSlide.Commit(
                    owner: this,
                    name: "SlideAnimation",
                    length: 250,
                    easing: Easing.CubicInOut
                );
                await Task.Delay(250);
                RegisterForm.Children.Clear();
                RegisterForm.TranslationX -= distance;
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream("ProjectTSSI.Views.Windows.RegisterPart2.xaml"))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var xamlContent = reader.ReadToEnd();
                        var stackLayout = new StackLayout();
                        stackLayout.LoadFromXaml(xamlContent);
                        stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                        stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
                        _currentPartRegisterContent = stackLayout;
                        RegisterForm.Children.Add(_currentPartRegisterContent);
                    }
                }
                _currentPartRegisterContent.FindByName<Entry>("entryMail").Focused += OnFocusedMail;
                _currentPartRegisterContent.FindByName<Entry>("entryMail").Unfocused += OnUnfocusedMail;
                _currentPartRegisterContent.FindByName<Entry>("entryPassword").Focused += OnFocusedPassword;
                _currentPartRegisterContent.FindByName<Entry>("entryPassword").Unfocused += OnUnfocusedPassword;
                _currentPartRegisterContent.FindByName<Entry>("entryConfirmPassword").Focused += OnFocusedConfirmPassword;
                _currentPartRegisterContent.FindByName<Entry>("entryConfirmPassword").Unfocused += OnUnfocusedConfirmPassword;
                await BorderButtonNext.ScaleTo(1.2, 300, Easing.CubicIn);
                await BorderButtonNext.ScaleTo(1.0, 250, Easing.CubicOut);
            }
        }
        private async Task AnimationStackFadeReg()
        {
            var fadeOutAnimation1 = new Animation(v => RegisterFadeOutPart1.Opacity = v, 1, 0);
            fadeOutAnimation1.Commit(this, "FadeOut1", 16, 300, Easing.Linear);

            var fadeOutAnimation2 = new Animation(v => RegisterFadeOutPart2.Opacity = v, 1, 0);
            fadeOutAnimation2.Commit(this, "FadeOut2", 16, 300, Easing.Linear);
            await Task.Delay(300);
            if (BorderLayoutRegisterPart2.Content is Layout layout1)
            {
                layout1.Children.Clear();
            }
        }

        private async Task SwapPositionsBorders()
        {
            var layoutBounds1 = AbsoluteLayout.GetLayoutBounds(BorderLayoutRegisterPart1);
            var layoutBounds2 = AbsoluteLayout.GetLayoutBounds(BorderLayoutRegisterPart2);
            var swapAnimation = new Animation();
            swapAnimation.Add(0, 1, new Animation(v => AbsoluteLayout.SetLayoutBounds(BorderLayoutRegisterPart1, new Rect(v, layoutBounds1.Y, layoutBounds1.Width, layoutBounds1.Height)), layoutBounds1.X, layoutBounds2.X));
            swapAnimation.Add(0, 1, new Animation(v => AbsoluteLayout.SetLayoutBounds(BorderLayoutRegisterPart2, new Rect(v, layoutBounds2.Y, layoutBounds2.Width, layoutBounds2.Height)), layoutBounds2.X, layoutBounds1.X));
            swapAnimation.Commit(this, "Swap", 16, 300, Easing.Linear);
            await Task.Delay(300);
            // Cambiar las esquinas redondeadas 
            BorderLayoutRegisterPart1.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(0, 18, 0, 18) };
            BorderLayoutRegisterPart2.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(18, 0, 18, 0) };
            await Task.Delay(600);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
