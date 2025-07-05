using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;
using ProjectTSSI.Global;
using ProjectTSSI.Handlers.Windows;
using ProjectTSSI.Models.Interfaces;
namespace ProjectTSSI.Register.ViewModels;

public class RegisterPart1ViewModel : INotifyPropertyChanged, IViewModelNeedComponentCustom
{
    public Dictionary<string, INotifyPropertyChanged> Configurations { get; set; } = new();
    public IAnimationServiceComposer _composerAnimation;
    public List<VisualElement> listAbsoluteErrors;
    private string _registerNameText = "";
    private string _registerFatherNameText = "";
    private string _registerMotherNameText = "";
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
    public string RegisterNameText
    {
        get => _registerNameText;
        set
        {
            if (_registerNameText != value)
            {
                _registerNameText = value;
                Preferences.Set("UserName", value);
                OnPropertyChanged(nameof(RegisterNameText));
                CheckErrorsEntrys(RegisterNameText, 0);
            }
        }
    }
    public string RegisterFatherNameText
    {
        get => _registerFatherNameText;
        set
        {
            if (_registerFatherNameText != value)
            {
                _registerFatherNameText = value;
                Preferences.Set("UserFatherName", value);
                OnPropertyChanged(nameof(RegisterFatherNameText));
                CheckErrorsEntrys(RegisterFatherNameText, 1);
            }
        }
    }
    public string RegisterMotherNameText
    {
        get => _registerMotherNameText;
        set
        {
            if (_registerMotherNameText != value)
            {
                _registerMotherNameText = value;
                Preferences.Set("UserMotherName", value);
                OnPropertyChanged(nameof(RegisterMotherNameText));
                CheckErrorsEntrys(RegisterMotherNameText, 2);
            }
        }
    }
    public RegisterPart1ViewModel(IAnimationServiceComposer composerAnimation)
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
        string[] types = ["name", "father_name", "mother_name"];
        string pattern = "";
        bool isWell = false;
        _enabledPart = false;
        CustomAbsoluteLayout errorLayout = listAbsoluteErrors.FirstOrDefault(e => e.AutomationId.Contains(types[typeText])) as CustomAbsoluteLayout;
        if (typeText == 0)
            pattern = @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+(?: [A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)?$";
        else if (typeText == 1 || typeText == 2)
            pattern = @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+$";
        if (Regex.IsMatch(text, pattern) || text == string.Empty)
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
            if (RegisterNameText != string.Empty && RegisterFatherNameText != string.Empty && RegisterMotherNameText != string.Empty)
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
            Text = (typeText == 0 ? "El Nombre Completo Proporcionado No Es Valido" : typeText == 1 ? "El Apellido Paterno No Es Valido" : "El Apellido Materno No Es Valido"),
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
        _registerNameText = Preferences.Get("UserName", string.Empty);
        _registerFatherNameText = Preferences.Get("UserFatherName", string.Empty);
        _registerMotherNameText = Preferences.Get("UserMotherName", string.Empty);
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}