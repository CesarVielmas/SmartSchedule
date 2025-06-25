using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using ProjectTSSI.Global;
using ProjectTSSI.Models;
using ProjectTSSI.Models.Interfaces;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ProjectTSSI.Handlers.Windows;

public class DebugInspector : AbsoluteLayout
{
    private Dictionary<Label, Entry> _propertysCustomComponent;
    private Grid _gridObject;
    private Grid _gridProperties;
    private object _actualObjectComponent;
    private int _actualObjectIndex;
    private int _cuantityObjects;
    public DebugInspector()
    {
        _propertysCustomComponent = new Dictionary<Label, Entry>();
        IsVisible = false;
        BackgroundColor = Colors.Black.WithAlpha(0.45f);
        HorizontalOptions = LayoutOptions.Fill;
        Opacity = 0;
    }
    public static readonly BindableProperty IsVisibleDebugProperty =
        BindableProperty.Create(
            nameof(IsVisibleDebugConfig),
            typeof(bool),
            typeof(DebugInspector),
            false,
            propertyChanged: OnVisibleConfigChanged);
    public bool IsVisibleDebugConfig
    {
        get => (bool)GetValue(IsVisibleDebugProperty);
        set => SetValue(IsVisibleDebugProperty, value);
    }
    private static async void OnVisibleConfigChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (DebugInspector)bindable;
        if (newValue is bool isVisible)
        {
            control.IsVisible = isVisible;
            if (isVisible)
            {
                GlobalConstants.DebugInspector = control;
                control.Children.Clear();
                double height = (GlobalConstants.ScreenHeight * 0.03) / GlobalConstants.ScreenDensity;
                control.HeightRequest = height;
                AbsoluteLayout.SetLayoutBounds(control, new Rect(0, -height, 1, height));
                AbsoluteLayout.SetLayoutFlags(control, AbsoluteLayoutFlags.WidthProportional);
                control.Opacity = 0;
                control.IsVisible = true;
                control.ZIndex = 5;
                string debugTextInit = AppResources.DebugInspectorInit;
                Label label = new Label
                {
                    TextColor = Color.FromArgb("#ececec"),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = (int)(GlobalConstants.ScreenHeight * 0.015) / GlobalConstants.ScreenDensity,
                    FontFamily = "PoppinsMedium",
                };
                AbsoluteLayout.SetLayoutBounds(label, new Rect(0.5, 0.5, -1, -1));
                AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
                control.Children.Add(label);
                await Task.WhenAll(
                    control.FadeTo(1, 500, Easing.CubicIn),
                    control.TranslateTo(0, 0, 500, Easing.CubicOut)
                );
                await GlobalMethods.AnimateTextByWord(label, debugTextInit, (int)(2000 / debugTextInit.Length));
                var animation = new Animation();
                animation.Add(0, 0.5, new Animation(v => label.Scale = v, 1, 1.05, Easing.CubicOut));
                animation.Add(0.5, 1, new Animation(v => label.Scale = v, 1.05, 1, Easing.CubicIn));
                animation.Commit(label, "ScaleBounce", 16, 350, Easing.Linear, (v, c) => label.Scale = 1);
            }
            else
                GlobalMethods.OnGlobalUnTapDebug();
        }
    }
    public async Task OnSelectedElement(List<object> completeCustomComponent)
    {
        if (completeCustomComponent == null || completeCustomComponent.Count == 0)
            return;
        this.Children.Clear();
        this.HeightRequest = (GlobalConstants.ScreenHeight * 0.03) / GlobalConstants.ScreenDensity;
        Label tittleCustomComponent = new Label
        {
            TextColor = Color.FromArgb("#ececec"),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = (int)(GlobalConstants.ScreenHeight * 0.015) / GlobalConstants.ScreenDensity,
            FontFamily = "PoppinsMedium",
        };
        string tittleText = completeCustomComponent.FirstOrDefault().GetType().GetProperty("AutomationId").GetValue(completeCustomComponent.FirstOrDefault()) as string;
        AbsoluteLayout.SetLayoutBounds(tittleCustomComponent, new Rect(0.5, 0.5, -1, -1));
        AbsoluteLayout.SetLayoutFlags(tittleCustomComponent, AbsoluteLayoutFlags.PositionProportional);
        this.Children.Add(tittleCustomComponent);
        await GlobalMethods.AnimateTextByWord(tittleCustomComponent, tittleText, (int)(2000 / tittleText.Length));
        this.AbortAnimation("HeightAnimation");
        var animation = new Animation(v => this.HeightRequest = v, this.HeightRequest, (GlobalConstants.ScreenHeight * 0.06) / GlobalConstants.ScreenDensity, Easing.CubicIn);
        animation.Commit(this, "HeightAnimation", 16, 500, Easing.Linear, (v, c) => this.HeightRequest = (GlobalConstants.ScreenHeight * 0.06) / GlobalConstants.ScreenDensity);
        GlobalMethods.AnimateMoveAbsoluteElementY(tittleCustomComponent, "top", 0, 1500);
        _cuantityObjects = completeCustomComponent.Count;
        _actualObjectIndex = 1;
        _actualObjectComponent = completeCustomComponent[_actualObjectIndex];
        await GenerateBindableCustomComponent(1);
    }
    private async Task GenerateBindableCustomComponent(byte typeArrows = 0)
    {
        _gridObject = new Grid
        {
            WidthRequest = (int)(GlobalConstants.ScreenWidth * (0.011 * _actualObjectComponent.GetType().Name.Length)) / GlobalConstants.ScreenDensity,
            HeightRequest = (int)(GlobalConstants.ScreenHeight * 0.0165) / GlobalConstants.ScreenDensity,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            },
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        if (typeArrows != 1)
        {
            Image leftArrow = new Image
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "back_icon_white.png",
                WidthRequest = (int)(GlobalConstants.ScreenHeight * 0.0145) / GlobalConstants.ScreenDensity,
                HeightRequest = (int)(GlobalConstants.ScreenHeight * 0.0165) / GlobalConstants.ScreenDensity,
            };
            var leftTap = new TapGestureRecognizer();
            leftTap.Tapped += ChangeLeftObject;
            leftArrow.GestureRecognizers.Add(leftTap);
            _gridObject.SetColumn(leftArrow, 0);
            _gridObject.SetRow(leftArrow, 0);
            _gridObject.Children.Add(leftArrow);
        }
        Label objectTittle = new Label
        {
            TextColor = Color.FromArgb("#ececec"),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = (int)(GlobalConstants.ScreenHeight * 0.012) / GlobalConstants.ScreenDensity,
            FontFamily = "PoppinsMedium",
            Text = _actualObjectComponent?.GetType().Name ?? "???"
        };
        _gridObject.SetColumn(objectTittle, 1);
        _gridObject.SetRow(objectTittle, 0);
        _gridObject.Children.Add(objectTittle);
        if (typeArrows != 2)
        {
            Image rightArrow = new Image
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "next_icon_white.png",
                WidthRequest = (int)(GlobalConstants.ScreenHeight * 0.0145) / GlobalConstants.ScreenDensity,
                HeightRequest = (int)(GlobalConstants.ScreenHeight * 0.0165) / GlobalConstants.ScreenDensity,
            };
            var rightTap = new TapGestureRecognizer();
            rightTap.Tapped += ChangeRightObject;
            rightArrow.GestureRecognizers.Add(rightTap);
            _gridObject.SetColumn(rightArrow, 2);
            _gridObject.SetRow(rightArrow, 0);
            _gridObject.Children.Add(rightArrow);
        }

        AbsoluteLayout.SetLayoutBounds(_gridObject, new Rect(0.5, 0.6, -1, -1));
        AbsoluteLayout.SetLayoutFlags(_gridObject, AbsoluteLayoutFlags.PositionProportional);
        this.Children.Add(_gridObject);
        await GlobalMethods.AnimateTextByWord(objectTittle, _actualObjectComponent.GetType().Name, (int)(2000 / _actualObjectComponent.GetType().Name.Length));
        GlobalMethods.AnimateMoveAbsoluteElementY(_gridObject, "", 0.35, 1500);
        int indexGrid = 0;
        int totalProperties = _actualObjectComponent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Count(p => p.CanRead && p.CanWrite);
        int rowsNeeded = (int)Math.Ceiling(totalProperties / 4.0);
        this.AbortAnimation("HeightAnimation");
        var animation = new Animation(v => this.HeightRequest = v, this.HeightRequest, (GlobalConstants.ScreenHeight * (0.06 + (0.03 * rowsNeeded))) / GlobalConstants.ScreenDensity, Easing.CubicIn);
        animation.Commit(this, "HeightAnimation", 16, 500, Easing.Linear, (v, c) => this.HeightRequest = (GlobalConstants.ScreenHeight * (0.06 + (0.03 * rowsNeeded))) / GlobalConstants.ScreenDensity);
        _gridProperties = new Grid
        {
            WidthRequest = this.Width,
            HeightRequest = (int)(GlobalConstants.ScreenHeight * (0.0285 * rowsNeeded)) / GlobalConstants.ScreenDensity,
            BackgroundColor = Colors.Transparent,
        };
        Dictionary<Type, Keyboard> keyboards = new Dictionary<Type, Keyboard>
        {
            { typeof(int), Keyboard.Numeric },
            { typeof(float), Keyboard.Numeric },
            { typeof(double), Keyboard.Numeric },
            { typeof(string), Keyboard.Default }
        };
        var converters = new Dictionary<Type, IValueConverter>
        {
            { typeof(int), new GenericConverter<int>() },
            { typeof(float), new GenericConverter<float>() },
            { typeof(double), new GenericConverter<double>() },
            { typeof(string), new GenericConverter<string>() }
        };
        for (int i = 0; i < rowsNeeded; i++)
            _gridProperties.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        for (int i = 0; i < 4; i++)
            _gridProperties.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        foreach (var property in _actualObjectComponent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanRead || !property.CanWrite)
                continue;
            int col = indexGrid % 4;
            int row = indexGrid / 4;
            var container = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Margin = GlobalMethods.ConvertThicknessFromString("0.01,0,0,0")
            };
            Label label = new Label
            {
                TextColor = Color.FromArgb("#ececec"),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                FontSize = (int)(GlobalConstants.ScreenHeight * 0.012) / GlobalConstants.ScreenDensity,
                FontFamily = "PoppinsMedium",
                Text = property.Name ?? "???"
            };
            label.Text += ":";
            Entry entry = new Entry
            {
                Placeholder = property.Name ?? "???",
                HeightRequest = (int)(GlobalConstants.ScreenHeight * 0.015) / GlobalConstants.ScreenDensity,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = (int)(GlobalConstants.ScreenHeight * 0.012) / GlobalConstants.ScreenDensity,
                FontFamily = "PoppinsMedium",
                Margin = GlobalMethods.ConvertThicknessFromString("0.007,0,0,0"),
                BackgroundColor = Colors.White,
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Center
            };
            if (keyboards.TryGetValue(property.PropertyType, out var keyboard))
                entry.Keyboard = keyboard;
            var binding = new Binding(property.Name, BindingMode.TwoWay) { Source = _actualObjectComponent };
            if (converters.TryGetValue(property.PropertyType, out var converter))
                binding.Converter = converter;
            entry.SetBinding(Entry.TextProperty, binding);
            container.Children.Add(label);
            container.Children.Add(entry);
            _gridObject.SetColumn(container, col);
            _gridObject.SetRow(container, row);
            _gridProperties.Children.Add(container);
            _propertysCustomComponent.Add(label, entry);
            indexGrid++;
        }
        AbsoluteLayout.SetLayoutBounds(_gridProperties, new Rect(0.5, 1, -1, -1));
        AbsoluteLayout.SetLayoutFlags(_gridProperties, AbsoluteLayoutFlags.PositionProportional);
        this.Children.Add(_gridProperties);
        List<Task> labelAnimations = new();
        List<Animation> entryAnimations = new();
        foreach (var (label, entry) in _propertysCustomComponent)
        {
            string fullText = label.Text;
            label.Text = "";
            labelAnimations.Add(GlobalMethods.AnimateTextByWord(label, fullText, (int)(2000 / fullText.Length)));
            entry.Opacity = 0;
            var fadeIn = new Animation(v => entry.Opacity = v, 0, 1);
            entryAnimations.Add(fadeIn);
        }
        var parentAnimation = new Animation();
        foreach (var anim in entryAnimations)
            parentAnimation.Add(0, 1, anim);
        parentAnimation.Commit(this, "FadeInEntries", 16, 500, Easing.CubicInOut);
        await Task.WhenAll(labelAnimations);
    }
    private async void ChangeLeftObject(object sender, EventArgs e)
    {
        if (_actualObjectIndex - 1 > 0)
        {
            _actualObjectIndex -= 1;
            _actualObjectComponent = GlobalConstants.CustomComponent[_actualObjectIndex];
            _propertysCustomComponent = new Dictionary<Label, Entry>();
            this.Children.Remove(_gridObject);
            this.Children.Remove(_gridProperties);
            await GenerateBindableCustomComponent((byte)(_actualObjectIndex == 1 ? 1 : 0));
        }
    }
    private async void ChangeRightObject(object sender, EventArgs e)
    {
        if (_actualObjectIndex + 1 <= _cuantityObjects - 1)
        {
            _actualObjectIndex += 1;
            _actualObjectComponent = GlobalConstants.CustomComponent[_actualObjectIndex];
            _propertysCustomComponent = new Dictionary<Label, Entry>();
            this.Children.Remove(_gridObject);
            this.Children.Remove(_gridProperties);
            await GenerateBindableCustomComponent((byte)(_actualObjectIndex == _cuantityObjects - 1 ? 2 : 0));
        }
    }
}