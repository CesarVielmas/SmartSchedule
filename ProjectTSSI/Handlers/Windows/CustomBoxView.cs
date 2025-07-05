using System.ComponentModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls.Shapes;
using ProjectTSSI.Global;
using ProjectTSSI.Models.CustomHandlers;
using ProjectTSSI.Models.Interfaces;

namespace ProjectTSSI.Handlers.Windows;

public class CustomBoxView : BoxView, IDisposable, ITapCustomComponent
{
    private TapGestureRecognizer _debugTap;

    public CustomBoxView()
    {
        _debugTap = new TapGestureRecognizer();
        _debugTap.Tapped += GlobalMethods.OnGlobalTapDebug;
        this.GestureRecognizers.Add(_debugTap);
    }
    #region  Properties
    public static readonly BindableProperty ResponsiveConfigProperty =
        BindableProperty.Create(
            nameof(ResponsiveConfig),
            typeof(ResponsiveConfig),
            typeof(CustomBoxView),
            new ResponsiveConfig { WidthPercentage = 20, HeightPercentage = 20, MarginPercentage = "0,0,0,0", PaddingPercentage = "0,0,0,0", MinimumWidthRequestPercentage = 10, MinimumHeightRequestPercentage = 50, MaximumWidthRequestPercentage = 100, MaximumHeightRequestPercentage = 100, BackgroundColorView = "#FFFFFF" },
            propertyChanged: OnResponsiveConfigChanged);
    public static readonly BindableProperty BoxViewResponsiveConfigProperty =
        BindableProperty.Create(
            nameof(BoxViewResponsiveConfig),
            typeof(ResponsiveBoxViewConfig),
            typeof(CustomBoxView),
            new ResponsiveBoxViewConfig { CornerRadiusPercentage = 0, ColorView = "#000000", ShadowView = "0,1,#000000,(0,0)" },
            propertyChanged: OnResponsiveBoxViewConfigChanged);
    public static readonly BindableProperty TapEnabledProperty =
        BindableProperty.Create(
            nameof(IsTapEnabled),
            typeof(bool),
            typeof(CustomBoxView),
            false,
            propertyChanged: OnTapEnabledChanged);
    public bool IsTapEnabled
    {
        get => (bool)GetValue(TapEnabledProperty);
        set => SetValue(TapEnabledProperty, value);
    }
    public ResponsiveConfig ResponsiveConfig
    {
        get => (ResponsiveConfig)GetValue(ResponsiveConfigProperty);
        set => SetValue(ResponsiveConfigProperty, value);
    }
    public ResponsiveBoxViewConfig BoxViewResponsiveConfig
    {
        get => (ResponsiveBoxViewConfig)GetValue(BoxViewResponsiveConfigProperty);
        set => SetValue(BoxViewResponsiveConfigProperty, value);
    }
    #endregion

    #region MethodsChange
    private static void OnTapEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomBoxView)bindable;
        if (newValue is bool isTapEnabled)
        {
            if (isTapEnabled)
            {
                control.OnTapStyle();
            }
        }
    }
    private static void OnResponsiveConfigChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomBoxView)bindable;
        if (oldValue is ResponsiveConfig oldConfig)
            oldConfig.PropertyChanged -= control.OnConfigPropertyChanged;
        if (newValue is ResponsiveConfig newConfig)
            newConfig.PropertyChanged += control.OnConfigPropertyChanged;
        control.UpdateAll();
    }
    private static void OnResponsiveBoxViewConfigChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomBoxView)bindable;
        if (oldValue is ResponsiveBoxViewConfig oldBoxViewConfig)
            oldBoxViewConfig.PropertyChanged -= control.OnBoxViewConfigPropertyChanged;
        if (newValue is ResponsiveBoxViewConfig newBoxViewConfig)
            newBoxViewConfig.PropertyChanged += control.OnBoxViewConfigPropertyChanged;
        control.UpdateAll();
    }
    public void OnTapStyle()
    {
        this.AbortAnimation("ScaleBounce");
        var animation = new Animation();
        animation.Add(0, 0.5, new Animation(v => this.Scale = v, 1, 1.1, Easing.CubicOut));
        animation.Add(0.5, 1, new Animation(v => this.Scale = v, 1.1, 1, Easing.CubicIn));
        animation.Add(0, 0.5, new Animation(v => this.Opacity = v, 1, 0.2, Easing.CubicInOut));
        animation.Add(0.5, 1, new Animation(v => this.Opacity = v, 0.2, 1, Easing.CubicInOut));
        animation.Commit(this, "ScaleBounce", 16, 2500, Easing.Linear, (v, c) =>
        {
            this.Scale = 1;
            this.Opacity = 1;
        }, repeat: () => true);
    }
    public void OnUnTapStyle()
    {
        this.Shadow = null;
        this.AbortAnimation("ScaleBounce");
        this.Scale = 1;
        this.Opacity = 1;
    }
    public List<object> GetCustomerComponentData()
    {
        return new List<object>
        {
            (CustomBoxView)this,
            ResponsiveConfig,
            BoxViewResponsiveConfig
        };
    }
    private void OnConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        string[] relevantProperties = {
            nameof(ResponsiveConfig.WidthPercentage),
            nameof(ResponsiveConfig.HeightPercentage),
            nameof(ResponsiveConfig.MarginPercentage),
            nameof(ResponsiveConfig.PaddingPercentage),
            nameof(ResponsiveConfig.MinimumWidthRequestPercentage),
            nameof(ResponsiveConfig.MinimumHeightRequestPercentage),
            nameof(ResponsiveConfig.MaximumWidthRequestPercentage),
            nameof(ResponsiveConfig.MaximumHeightRequestPercentage),
            nameof(ResponsiveConfig.BackgroundColorView)
        };
        if (relevantProperties.Contains(e.PropertyName))
        {
            UpdateAll();
        }
    }
    private void OnBoxViewConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        string[] relevantProperties = {
            nameof(ResponsiveBoxViewConfig.CornerRadiusPercentage),
            nameof(ResponsiveBoxViewConfig.ColorView),
            nameof(ResponsiveBoxViewConfig.ShadowView)
        };

        if (relevantProperties.Contains(e.PropertyName))
        {
            UpdateAll();
        }
    }
    private void UpdateAll()
    {
        if (ResponsiveConfig != null && BoxViewResponsiveConfig != null && GlobalConstants.ScreenHeight > 0 && GlobalConstants.ScreenWidth > 0 && GlobalConstants.ScreenDensity > 0)
        {
            this.WidthRequest = (int)((GlobalConstants.ScreenWidth * ResponsiveConfig.WidthPercentage) / GlobalConstants.ScreenDensity);
            this.HeightRequest = (int)((GlobalConstants.ScreenHeight * ResponsiveConfig.HeightPercentage) / GlobalConstants.ScreenDensity);
            if (this.WidthRequest == 0)
                this.WidthRequest = -1;
            if (this.HeightRequest == 0)
                this.HeightRequest = -1;
            this.Margin = GlobalMethods.ConvertThicknessFromString(ResponsiveConfig.MarginPercentage);
            // this.Padding = GlobalMethods.ConvertThicknessFromString(ResponsiveConfig.PaddingPercentage);
            this.MinimumWidthRequest = (int)((GlobalConstants.ScreenWidth * ResponsiveConfig.MinimumWidthRequestPercentage) / GlobalConstants.ScreenDensity);
            this.MinimumHeightRequest = (int)((GlobalConstants.ScreenHeight * ResponsiveConfig.MinimumHeightRequestPercentage) / GlobalConstants.ScreenDensity);
            this.MaximumWidthRequest = (int)((GlobalConstants.ScreenWidth * ResponsiveConfig.MaximumWidthRequestPercentage) / GlobalConstants.ScreenDensity);
            this.MaximumHeightRequest = (int)((GlobalConstants.ScreenHeight * ResponsiveConfig.MaximumHeightRequestPercentage) / GlobalConstants.ScreenDensity);
            if (!string.IsNullOrWhiteSpace(ResponsiveConfig.BackgroundColorView) &&
                ResponsiveConfig.BackgroundColorView.StartsWith("#") &&
                !int.TryParse(ResponsiveConfig.BackgroundColorView, out _))
            {
                this.BackgroundColor = Color.FromArgb(ResponsiveConfig.BackgroundColorView);
            }
            this.CornerRadius = (int)((GlobalConstants.ScreenHeight * BoxViewResponsiveConfig.CornerRadiusPercentage) / GlobalConstants.ScreenDensity);
            this.Shadow = GlobalMethods.ConvertShadowFromString(BoxViewResponsiveConfig.ShadowView);
            if (!string.IsNullOrWhiteSpace(BoxViewResponsiveConfig.ColorView) &&
               BoxViewResponsiveConfig.ColorView.StartsWith("#") &&
               !int.TryParse(BoxViewResponsiveConfig.ColorView, out _))
            {
                this.Color = Color.FromArgb(BoxViewResponsiveConfig.ColorView);
            }
        }
    }
    #endregion
    #region IDisposable
    public void Dispose()
    {
        if (ResponsiveConfig != null)
            ResponsiveConfig.PropertyChanged -= OnConfigPropertyChanged;
        if (BoxViewResponsiveConfig != null)
            BoxViewResponsiveConfig.PropertyChanged -= OnBoxViewConfigPropertyChanged;
        if (_debugTap != null)
        {
            _debugTap.Tapped -= GlobalMethods.OnGlobalTapDebug;
            this.GestureRecognizers.Remove(_debugTap);
            _debugTap = null;
        }
    }
    #endregion
}