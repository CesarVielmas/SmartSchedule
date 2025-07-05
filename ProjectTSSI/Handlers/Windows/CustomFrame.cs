using System.ComponentModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using ProjectTSSI.Global;
using ProjectTSSI.Models.CustomHandlers;
using ProjectTSSI.Models.Interfaces;

namespace ProjectTSSI.Handlers.Windows;

public class CustomFrame : Frame, IDisposable, ITapCustomComponent
{
    private TapGestureRecognizer _debugTap;

    public CustomFrame()
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
            typeof(CustomFrame),
            new ResponsiveConfig { WidthPercentage = 20, HeightPercentage = 20, MarginPercentage = "0,0,0,0", PaddingPercentage = "0,0,0,0", MinimumWidthRequestPercentage = 10, MinimumHeightRequestPercentage = 50, MaximumWidthRequestPercentage = 100, MaximumHeightRequestPercentage = 100, BackgroundColorView = "#FFFFFF" },
            propertyChanged: OnResponsiveConfigChanged);
    public static readonly BindableProperty FrameResponsiveConfigProperty =
        BindableProperty.Create(
            nameof(FrameResponsiveConfig),
            typeof(ResponsiveFrameConfig),
            typeof(CustomFrame),
            new ResponsiveFrameConfig { BorderColorView = "#000000", CornerRadiusPercentage = 0.01f, ShadowView = "0,1,#000000,(0,0)" },
            propertyChanged: OnResponsiveFrameConfigChanged);
    public static readonly BindableProperty TapEnabledProperty =
        BindableProperty.Create(
            nameof(IsTapEnabled),
            typeof(bool),
            typeof(CustomFrame),
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
    public ResponsiveFrameConfig FrameResponsiveConfig
    {
        get => (ResponsiveFrameConfig)GetValue(FrameResponsiveConfigProperty);
        set => SetValue(FrameResponsiveConfigProperty, value);
    }
    #endregion

    #region MethodsChange
    private static void OnTapEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomFrame)bindable;
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
        var control = (CustomFrame)bindable;
        if (oldValue is ResponsiveConfig oldConfig)
            oldConfig.PropertyChanged -= control.OnConfigPropertyChanged;
        if (newValue is ResponsiveConfig newConfig)
            newConfig.PropertyChanged += control.OnConfigPropertyChanged;
        control.UpdateAll();
    }
    private static void OnResponsiveFrameConfigChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomFrame)bindable;
        if (oldValue is ResponsiveFrameConfig oldFrameConfig)
            oldFrameConfig.PropertyChanged -= control.OnFrameConfigPropertyChanged;
        if (newValue is ResponsiveFrameConfig newFrameConfig)
            newFrameConfig.PropertyChanged += control.OnFrameConfigPropertyChanged;
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
            (CustomFrame)this,
            ResponsiveConfig,
            FrameResponsiveConfig
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
    private void OnFrameConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        string[] relevantProperties = {
            nameof(ResponsiveFrameConfig.BorderColorView),
            nameof(ResponsiveFrameConfig.CornerRadiusPercentage),
            nameof(ResponsiveFrameConfig.ShadowView)
        };

        if (relevantProperties.Contains(e.PropertyName))
        {
            UpdateAll();
        }
    }
    private void UpdateAll()
    {
        if (ResponsiveConfig != null && FrameResponsiveConfig != null && GlobalConstants.ScreenHeight > 0 && GlobalConstants.ScreenWidth > 0 && GlobalConstants.ScreenDensity > 0)
        {
            this.WidthRequest = (int)((GlobalConstants.ScreenWidth * ResponsiveConfig.WidthPercentage) / GlobalConstants.ScreenDensity);
            this.HeightRequest = (int)((GlobalConstants.ScreenHeight * ResponsiveConfig.HeightPercentage) / GlobalConstants.ScreenDensity);
            if (this.WidthRequest == 0)
                this.WidthRequest = -1;
            if (this.HeightRequest == 0)
                this.HeightRequest = -1;
            this.Margin = GlobalMethods.ConvertThicknessFromString(ResponsiveConfig.MarginPercentage);
            this.Padding = GlobalMethods.ConvertThicknessFromString(ResponsiveConfig.PaddingPercentage);
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
            this.CornerRadius = (int)((GlobalConstants.ScreenHeight * FrameResponsiveConfig.CornerRadiusPercentage) / GlobalConstants.ScreenDensity);
            this.Shadow = GlobalMethods.ConvertShadowFromString(FrameResponsiveConfig.ShadowView);
            if (!string.IsNullOrWhiteSpace(FrameResponsiveConfig.BorderColorView) &&
                FrameResponsiveConfig.BorderColorView.StartsWith("#") &&
                !int.TryParse(FrameResponsiveConfig.BorderColorView, out _))
            {
                this.BorderColor = Color.FromArgb(FrameResponsiveConfig.BorderColorView);
            }
        }
    }
    #endregion
    #region IDisposable
    public void Dispose()
    {
        if (ResponsiveConfig != null)
            ResponsiveConfig.PropertyChanged -= OnConfigPropertyChanged;
        if (FrameResponsiveConfig != null)
            FrameResponsiveConfig.PropertyChanged -= OnFrameConfigPropertyChanged;
        if (_debugTap != null)
        {
            _debugTap.Tapped -= GlobalMethods.OnGlobalTapDebug;
            this.GestureRecognizers.Remove(_debugTap);
            _debugTap = null;
        }
    }
    #endregion
}