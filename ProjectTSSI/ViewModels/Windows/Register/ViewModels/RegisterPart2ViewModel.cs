using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;
using ProjectTSSI.Global;
using ProjectTSSI.Handlers.Windows;
using ProjectTSSI.Models.Interfaces;
namespace ProjectTSSI.Register.ViewModels;

public class RegisterPart2ViewModel : INotifyPropertyChanged, IViewModelNeedComponentCustom
{
    public Dictionary<string, INotifyPropertyChanged> Configurations { get; set; } = new();
    public IAnimationServiceComposer _composerAnimation;
    public List<VisualElement> listAbsoluteErrors;
    private string _registerMailText;
    private string _registerPasswordText;
    private string _registerConfirmPasswordText;
    private bool _enabledPart = false;
    public event PropertyChangedEventHandler PropertyChanged;
    public bool EnabledPart
    {
        get => _enabledPart;
        set
        {
            if (_enabledPart != value)
            {
                _enabledPart = value;
                OnPropertyChanged(nameof(EnabledPart));
            }
        }
    }
    public string RegisterMailText
    {
        get => _registerMailText;
        set
        {
            if (_registerMailText != value)
            {
                _registerMailText = value;
                Preferences.Set("UserMail", value);
                OnPropertyChanged(nameof(RegisterMailText));
                CheckErrorsEntrys(RegisterMailText, 0);
            }
        }
    }
    public string RegisterPasswordText
    {
        get => _registerPasswordText;
        set
        {
            if (_registerPasswordText != value)
            {
                _registerPasswordText = value;
                Preferences.Set("UserPassword", value);
                OnPropertyChanged(nameof(RegisterPasswordText));
                CheckErrorsEntrys(RegisterPasswordText, 1);
            }
        }
    }
    public string RegisterConfirmPasswordText
    {
        get => _registerConfirmPasswordText;
        set
        {
            if (_registerConfirmPasswordText != value)
            {
                _registerConfirmPasswordText = value;
                Preferences.Set("UserConfirmPassword", value);
                OnPropertyChanged(nameof(RegisterConfirmPasswordText));
                CheckErrorsEntrys(RegisterConfirmPasswordText, 2);
            }
        }
    }
    public RegisterPart2ViewModel(IAnimationServiceComposer composerAnimation)
    {
        _composerAnimation = composerAnimation;
        OnLoadPersistData();
    }
    public ICommand OnFocusTapped => new Command<ParameterElementsCommand>(async payload =>
    {
        CustomImage imageIcon = payload.OtherElements[0] as CustomImage;
        imageIcon.Source = imageIcon.Source.ToString().Split("File: ")[1].Replace("innactive", "active");
        await _composerAnimation.Animate("TextAnimation", "Scale", imageIcon, "ScaleFocusPercentage", 15, 400);
    });
    public ICommand OnUnFocusTapped => new Command<ParameterElementsCommand>(async payload =>
    {
        CustomImage imageIcon = payload.OtherElements[0] as CustomImage;
        imageIcon.Source = imageIcon.Source.ToString().Split("File: ")[1].Replace("active", "innactive");
        await _composerAnimation.Animate("TextAnimation", "Scale", imageIcon, "ScaleFocusPercentage", 15, 400);
    });
    public void SetConfigurationsJson()
    {
        GlobalMethods.LoadJsonConfigurations(this.Configurations, "RegisterWSPartsConfigs.json");
        OnPropertyChanged(nameof(this.Configurations));
    }
    private async void CheckErrorsEntrys(string text, byte typeText)
    {
        string[] types = ["mail", "password", "confirm_password"];
        string pattern = "";
        bool isWell = false;
        _enabledPart = false;
        CustomAbsoluteLayout errorLayout = listAbsoluteErrors.FirstOrDefault(e => e.AutomationId.Contains(types[typeText])) as CustomAbsoluteLayout;
        if (typeText == 0)
            pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        else if (typeText == 1)
            pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
        if (typeText < 2 && Regex.IsMatch(text, pattern) || text == string.Empty)
            isWell = true;
        if (typeText == 2 && text == RegisterPasswordText)
            isWell = true;
        if (isWell)
        {
            errorLayout.Children.Clear();
            listAbsoluteErrors.ForEach((e) =>
            {
                if ((e as CustomAbsoluteLayout).Children.Count > 0)
                {
                    _enabledPart = false;
                    return;
                }
            });
            if (RegisterMailText != string.Empty && RegisterPasswordText != string.Empty && RegisterConfirmPasswordText != string.Empty)
                _enabledPart = true;
            return;
        }
        if (errorLayout.Children.Count > 0)
            return;
        var innerAbsoluteLayout = new AbsoluteLayout
        {
            VerticalOptions = LayoutOptions.Fill,
            HeightRequest = (int)((GlobalConstants.ScreenHeight * 0.034) / GlobalConstants.ScreenDensity)
        };
        AbsoluteLayout.SetLayoutBounds(innerAbsoluteLayout, new Rect(0, 23, 1, 0.1));
        AbsoluteLayout.SetLayoutFlags(innerAbsoluteLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.WidthProportional);
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
        border.StrokeShape = new RoundRectangle
        {
            CornerRadius = new Microsoft.Maui.CornerRadius(0, 0, 5, 5)
        };
        var label = new Label
        {
            Text = (typeText == 0 ? "El Mail Proporcionado No Es Valido" : typeText == 1 ? "La Contraseña Proporcionada No Es Valida" : "Las Contraseñas No Coinciden"),
            FontFamily = "PoppinsRegular",
            FontSize = (int)((GlobalConstants.ScreenHeight * 0.0125) / GlobalConstants.ScreenDensity),
            TextColor = Colors.White,
            HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.End
        };
        AbsoluteLayout.SetLayoutBounds(label, new Rect(0.5, 0.5, 1, 1));
        AbsoluteLayout.SetLayoutFlags(label, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
        border.Content = label;
        innerAbsoluteLayout.Children.Add(border);
        errorLayout.Children.Add(innerAbsoluteLayout);
        await _composerAnimation.Animate("ContentAnimation", "Move", innerAbsoluteLayout, "MoveElementY", 3.5f, 200);
        await _composerAnimation.Animate("TextAnimation", "Scale", innerAbsoluteLayout, "ScaleFocusPercentage", 10, 400);
    }
    private void OnLoadPersistData()
    {
        _registerMailText = Preferences.Get("UserMail", string.Empty);
        _registerPasswordText = Preferences.Get("UserPassword", string.Empty);
        _registerConfirmPasswordText = Preferences.Get("UserConfirmPassword", string.Empty);
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}