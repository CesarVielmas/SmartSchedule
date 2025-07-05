using System.ComponentModel;
using ProjectTSSI.Global;
using Windows.System;
using Microsoft.UI.Xaml.Input;
using ProjectTSSI.Models.Interfaces;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.UI.Xaml;
using System.Windows.Input;
using Microsoft.UI.Composition;
using ProjectTSSI.Handlers.Windows;
namespace ProjectTSSI.Register.ViewModels;

public class RegisterWSViewModel : INotifyPropertyChanged, IViewModelNeedCustom
{
    public event PropertyChangedEventHandler PropertyChanged;
    private RegisterPart1 _registerPart1View;
    private RegisterPart2 _registerPart2View;
    public List<VisualElement> _listRegisterBorders;
    public ContentView _contentToStep;
    public readonly IAnimationServiceComposer _composerAnimations;
    private List<Border> listBordersAnimation { get; set; } = new();
    private DispatcherTimer _checkCoreRegisterTimer = new();
    public readonly List<string> _listImagesRegister = new List<string> { "login_image_one.png", "login_image_two.png", "login_image_three.png" };
    public Dictionary<string, INotifyPropertyChanged> Configurations { get; set; } = new();
    private int _indexImagesRegister;
    private int _stepNow = 0;
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
    public int StepNow
    {
        get => _stepNow;
        set
        {
            if (_stepNow != value)
            {
                _stepNow = value;
                OnPropertyChanged(nameof(StepNow));
                GenerateContentStep();
            }
        }
    }
    public int IndexImagesRegister
    {
        get => _indexImagesRegister;
        set
        {
            if (_indexImagesRegister != value)
            {
                _indexImagesRegister = value;
                OnPropertyChanged(nameof(IndexImagesRegister));
            }
        }
    }
    public RegisterWSViewModel(IAnimationServiceComposer composerAnimations)
    {
        _composerAnimations = composerAnimations;
        _registerPart1View = new RegisterPart1(_composerAnimations);
    }
    public ICommand NextStepTapped => new Command<ParameterElementsCommand>(async payload =>
    {
        if (StepNow + 1 > 3)
            return;
        if (_registerPart1View != null && StepNow == 1)
            if (!_registerPart1View._viewModel.EnabledPart)
                return;
            else
                _registerPart2View = new RegisterPart2(_composerAnimations);
        if (_registerPart2View != null && StepNow == 2)
            if (!_registerPart2View._viewModel.EnabledPart)
                return;
        //Header
        AbsoluteLayout layoutTopPrevius = payload.OtherElements.FirstOrDefault(e => e.AutomationId == $"absolute_layout_register_step{StepNow}") as AbsoluteLayout;
        Border borderStepChange = payload.OtherElements.FirstOrDefault(e => e.AutomationId == $"border_register_step{StepNow + 1}") as Border;
        Image imageStepChange = payload.OtherElements.FirstOrDefault(e => e.AutomationId == $"image_register_step{StepNow + 1}") as Image;
        Label labelStepChange = payload.OtherElements.FirstOrDefault(e => e.AutomationId == $"label_register_step{StepNow + 1}") as Label;
        //Footer Buttons
        Border nextButtonBorder = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "border_button_next") as Border;
        Image nextButtonImage = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "image_button_next") as Image;
        Label nextButtonLabel = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "label_button_next") as Label;

        Border backButtonBorder = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "border_button_back") as Border;
        Image backButtonImage = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "image_button_back") as Image;
        Label backButtonLabel = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "label_button_back") as Label;
        //Animations and styles
        layoutTopPrevius.BackgroundColor = Color.FromArgb("#a1a6ff");
        imageStepChange.Source = $"icon_register_{(StepNow + 1 == 2 ? "two" : "three")}_first.png";
        labelStepChange.TextColor = Color.FromArgb("#FFFFFF");
        if (StepNow + 1 == 3)
        {
            nextButtonBorder.BackgroundColor = Colors.Transparent;
            nextButtonImage.Source = "next_icon_color.png";
            nextButtonLabel.Text = "Finalizar Registro";
            nextButtonLabel.TextColor = Color.FromArgb("#8589ea");
            layoutTopPrevius = payload.OtherElements.FirstOrDefault(e => e.AutomationId == $"absolute_layout_register_step{StepNow + 1}") as AbsoluteLayout;
            layoutTopPrevius.BackgroundColor = Color.FromArgb("#878beb");
        }
        backButtonBorder.BackgroundColor = Color.FromArgb("#8589ea");
        backButtonBorder.StrokeShape = new RoundRectangle { CornerRadius = GlobalMethods.ConvertCornerRadiusFromString("0.003,0.003,0.003,0.003") };
        backButtonImage.Source = "back_icon_white.png";
        backButtonLabel.Text = "Regresar Paso";
        backButtonLabel.TextColor = Color.FromArgb("#FFFFFF");
        await Task.WhenAll(
             _composerAnimations.Animate("ContentAnimation", "Scale", borderStepChange, "ScaleColor", 0, 400, ["#a1a6ff", "#8589ea"]),
             _composerAnimations.Animate("TextAnimation", "Scale", nextButtonBorder, "ScaleFocusPercentage", 15, 400),
             _composerAnimations.Animate("TextAnimation", "Scale", backButtonBorder, "ScaleFocusPercentage", 15, 400)

        );
        StepNow += 1;
    });
    public ICommand BackStepTapped => new Command<ParameterElementsCommand>(async payload =>
    {
        if (StepNow - 1 == 0)
            await SwapPositionsBorders();
        if (StepNow - 1 < 1)
            return;
        //Header
        AbsoluteLayout nextTopPrevius = payload.OtherElements.FirstOrDefault(e => e.AutomationId == $"absolute_layout_register_step{StepNow - 1}") as AbsoluteLayout;
        AbsoluteLayout actualTopPrevius = payload.OtherElements.FirstOrDefault(e => e.AutomationId == $"absolute_layout_register_step{StepNow}") as AbsoluteLayout;
        Border borderStepChange = payload.OtherElements.FirstOrDefault(e => e.AutomationId == $"border_register_step{StepNow}") as Border;
        Image imageStepChange = payload.OtherElements.FirstOrDefault(e => e.AutomationId == $"image_register_step{StepNow}") as Image;
        Label labelStepChange = payload.OtherElements.FirstOrDefault(e => e.AutomationId == $"label_register_step{StepNow}") as Label;
        //Footer Buttons
        Border nextButtonBorder = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "border_button_next") as Border;
        Image nextButtonImage = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "image_button_next") as Image;
        Label nextButtonLabel = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "label_button_next") as Label;

        Border backButtonBorder = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "border_button_back") as Border;
        Image backButtonImage = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "image_button_back") as Image;
        Label backButtonLabel = payload.OtherElements.FirstOrDefault(e => e.AutomationId == "label_button_back") as Label;
        //Animations and styles
        nextTopPrevius.BackgroundColor = Color.FromArgb("#edeffe");
        actualTopPrevius.BackgroundColor = Color.FromArgb("#edeffe");
        borderStepChange.Background = Color.FromArgb("#edeffe");
        imageStepChange.Source = $"icon_register_{(StepNow == 2 ? "two" : "three")}_second.png";
        labelStepChange.TextColor = Color.FromArgb("#9ea8c1");
        if (StepNow - 1 == 1)
        {
            backButtonBorder.BackgroundColor = Colors.Transparent;
            backButtonImage.Source = "back_icon_color.png";
            backButtonLabel.Text = "Salir Del Registro";
            backButtonLabel.TextColor = Color.FromArgb("#8589ea");
        }
        nextButtonBorder.BackgroundColor = Color.FromArgb("#8589ea");
        nextButtonBorder.StrokeShape = new RoundRectangle { CornerRadius = GlobalMethods.ConvertCornerRadiusFromString("0.003,0.003,0.003,0.003") };
        nextButtonImage.Source = "next_icon_white.png";
        nextButtonLabel.Text = "Regresar Paso";
        nextButtonLabel.TextColor = Color.FromArgb("#FFFFFF");
        await Task.WhenAll(
             _composerAnimations.Animate("TextAnimation", "Scale", nextButtonBorder, "ScaleFocusPercentage", 15, 400),
             _composerAnimations.Animate("TextAnimation", "Scale", backButtonBorder, "ScaleFocusPercentage", 15, 400)

        );
        StepNow -= 1;
    });
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
    }
    public void Accelerator_Invoked(object sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        args.Handled = true;
        this.IsDebugMode = !this.IsDebugMode;
        GlobalConstants.IsDebugMode = this.IsDebugMode;
    }
    public void SetConfigurationsJson()
    {
        GlobalMethods.LoadJsonConfigurations(this.Configurations, "RegisterWSConfigs.json");
        OnPropertyChanged(nameof(this.Configurations));
    }
    public void DisposeContentModel(object sender, EventArgs e, AbsoluteLayout absoluteLayoutCircles)
    {
        OnPageUnloaded(sender, e);
        _checkCoreRegisterTimer.Stop();
        listBordersAnimation.ForEach(b => { b.AbortAnimation("ColorAnimation"); absoluteLayoutCircles.Children.Remove(b); });
        listBordersAnimation.Clear();
    }
    public void GenerateCirclesAnimation(AbsoluteLayout absoluteLayoutCircles)
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
        _checkCoreRegisterTimer.Interval = TimeSpan.FromSeconds(1);
        _checkCoreRegisterTimer.Tick += async (s, e) =>
        {
            if (listBordersAnimation.Count <= cuantityRectsCircle)
            {
                int randomNumber = random.Next(0, listRectsCircles.Count);
                int randomRed = random.Next(0, 256);
                int randomBlue = random.Next(0, 256);
                int randomGreen = random.Next(0, 256);
                Border borderAdd = GenerateCircles(listRectsCircles[randomNumber], randomRed, randomBlue, randomGreen, absoluteLayoutCircles);
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
        _checkCoreRegisterTimer.Start();
    }
    private Border GenerateCircles(Rect rect, int red, int blue, int green, AbsoluteLayout absoluteLayoutCircles)
    {
        var border = new Border
        {
            Stroke = Colors.White,
            StrokeThickness = 2,
            Background = Colors.White,
            WidthRequest = (int)GlobalConstants.ScreenWidth * 0.01,
            HeightRequest = (int)GlobalConstants.ScreenHeight * 0.0177,
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
    private void GenerateContentStep()
    {
        switch (StepNow)
        {
            case 1:
                _contentToStep.Content = _registerPart1View;
                break;
            case 2:
                _contentToStep.Content = _registerPart2View;
                break;
            case 3:
                _contentToStep.Content = new RegisterPart3(_composerAnimations);
                break;
            default:
                break;

        }
    }
    private async Task SwapPositionsBorders()
    {
        CustomBorder BorderLayoutRegisterPart1 = _listRegisterBorders[0] as CustomBorder;
        CustomBorder BorderLayoutRegisterPart2 = _listRegisterBorders[1] as CustomBorder;
        var layoutBounds1 = AbsoluteLayout.GetLayoutBounds(BorderLayoutRegisterPart1);
        var layoutBounds2 = AbsoluteLayout.GetLayoutBounds(BorderLayoutRegisterPart2);
        var swapAnimation = new Animation();
        swapAnimation.Add(0, 1, new Animation(v => AbsoluteLayout.SetLayoutBounds(BorderLayoutRegisterPart1, new Rect(v, layoutBounds1.Y, layoutBounds1.Width, layoutBounds1.Height)), layoutBounds1.X, layoutBounds2.X));
        swapAnimation.Add(0, 1, new Animation(v => AbsoluteLayout.SetLayoutBounds(BorderLayoutRegisterPart2, new Rect(v, layoutBounds2.Y, layoutBounds2.Width, layoutBounds2.Height)), layoutBounds2.X, layoutBounds1.X));
        (BorderLayoutRegisterPart1.Content as CustomStackLayout).Children.Clear();
        (BorderLayoutRegisterPart2.Content as CustomStackLayout).Children.Clear();
        swapAnimation.Commit(_listRegisterBorders[2], "Swap", 16, 300, Easing.Linear);
        await Task.Delay(300);
        BorderLayoutRegisterPart1.StrokeShape = new RoundRectangle { CornerRadius = new Microsoft.Maui.CornerRadius(0, 18, 0, 18) };
        BorderLayoutRegisterPart2.StrokeShape = new RoundRectangle { CornerRadius = new Microsoft.Maui.CornerRadius(18, 0, 18, 0) };
        await Task.Delay(600);
        Microsoft.Maui.Controls.Application.Current.MainPage = new LoginWS();
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
public class ParameterElementsCommand
{
    public VisualElement MainElement { get; set; }
    public List<VisualElement> OtherElements { get; set; }
}