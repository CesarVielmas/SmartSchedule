using System.ComponentModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using ProjectTSSI.Global;
using ProjectTSSI.Models.CustomHandlers;
using ProjectTSSI.Models.Interfaces;

namespace ProjectTSSI.Handlers.Windows;

public class CustomGrid : Grid, IDisposable, ITapCustomComponent
{
    private TapGestureRecognizer _debugTap;

    public CustomGrid()
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
            typeof(CustomGrid),
            new ResponsiveConfig { WidthPercentage = 20, HeightPercentage = 20, MarginPercentage = "0,0,0,0", PaddingPercentage = "0,0,0,0", MinimumWidthRequestPercentage = 10, MinimumHeightRequestPercentage = 50, MaximumWidthRequestPercentage = 100, MaximumHeightRequestPercentage = 100, BackgroundColorView = "#FFFFFF" },
            propertyChanged: OnResponsiveConfigChanged);
    public static readonly BindableProperty GridResponsiveConfigProperty =
        BindableProperty.Create(
            nameof(GridResponsiveConfig),
            typeof(ResponsiveGridConfig),
            typeof(CustomGrid),
            new ResponsiveGridConfig { ColumnSpacingPercentage = 20, RowSpacingPercentage = 10 },
            propertyChanged: OnResponsiveGridConfigChanged);
    public static readonly BindableProperty TapEnabledProperty =
        BindableProperty.Create(
            nameof(IsTapEnabled),
            typeof(bool),
            typeof(CustomGrid),
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
    public ResponsiveGridConfig GridResponsiveConfig
    {
        get => (ResponsiveGridConfig)GetValue(GridResponsiveConfigProperty);
        set => SetValue(GridResponsiveConfigProperty, value);
    }
    #endregion

    #region MethodsChange
    private static void OnTapEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomGrid)bindable;
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
        var control = (CustomGrid)bindable;
        if (oldValue is ResponsiveConfig oldConfig)
            oldConfig.PropertyChanged -= control.OnConfigPropertyChanged;
        if (newValue is ResponsiveConfig newConfig)
            newConfig.PropertyChanged += control.OnConfigPropertyChanged;
        control.UpdateAll();
    }
    private static void OnResponsiveGridConfigChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomGrid)bindable;
        if (oldValue is ResponsiveGridConfig oldGridConfig)
            oldGridConfig.PropertyChanged -= control.OnGridConfigPropertyChanged;
        if (newValue is ResponsiveGridConfig newGridConfig)
            newGridConfig.PropertyChanged += control.OnGridConfigPropertyChanged;
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
            (CustomGrid)this,
            ResponsiveConfig,
            GridResponsiveConfig
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
    private void OnGridConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        string[] relevantProperties = {
            nameof(ResponsiveGridConfig.ColumnSpacingPercentage),
            nameof(ResponsiveGridConfig.RowSpacingPercentage)
        };

        if (relevantProperties.Contains(e.PropertyName))
        {
            UpdateAll();
        }
    }
    private void UpdateAll()
    {
        if (ResponsiveConfig != null && GridResponsiveConfig != null && GlobalConstants.ScreenHeight > 0 && GlobalConstants.ScreenWidth > 0 && GlobalConstants.ScreenDensity > 0)
        {
            this.WidthRequest = (int)((GlobalConstants.ScreenWidth * ResponsiveConfig.WidthPercentage) / GlobalConstants.ScreenDensity);
            this.HeightRequest = (int)((GlobalConstants.ScreenHeight * ResponsiveConfig.HeightPercentage) / GlobalConstants.ScreenDensity);
            if (this.WidthRequest == 0)
                this.WidthRequest = -1;
            if (this.HeightRequest == 0)
                this.HeightRequest = -1;
            this.Margin = GlobalMethods.ConvertThicknessFromString(ResponsiveConfig.MarginPercentage);
            this.Padding = GlobalMethods.ConvertThicknessFromString(ResponsiveConfig.PaddingPercentage);
            if (!string.IsNullOrWhiteSpace(ResponsiveConfig.BackgroundColorView) &&
                ResponsiveConfig.BackgroundColorView.StartsWith("#") &&
                !int.TryParse(ResponsiveConfig.BackgroundColorView, out _))
            {
                this.BackgroundColor = Color.FromArgb(ResponsiveConfig.BackgroundColorView);
            }

            this.MinimumWidthRequest = (int)((GlobalConstants.ScreenWidth * ResponsiveConfig.MinimumWidthRequestPercentage) / GlobalConstants.ScreenDensity);
            this.MinimumHeightRequest = (int)((GlobalConstants.ScreenHeight * ResponsiveConfig.MinimumHeightRequestPercentage) / GlobalConstants.ScreenDensity);
            this.MaximumWidthRequest = (int)((GlobalConstants.ScreenWidth * ResponsiveConfig.MaximumWidthRequestPercentage) / GlobalConstants.ScreenDensity);
            this.MaximumHeightRequest = (int)((GlobalConstants.ScreenHeight * ResponsiveConfig.MaximumHeightRequestPercentage) / GlobalConstants.ScreenDensity);

            this.ColumnSpacing = (int)((GlobalConstants.ScreenWidth * GridResponsiveConfig.ColumnSpacingPercentage) / GlobalConstants.ScreenDensity);
            this.RowSpacing = (int)((GlobalConstants.ScreenHeight * GridResponsiveConfig.RowSpacingPercentage) / GlobalConstants.ScreenDensity);
        }
    }
    #endregion
    #region IDisposable
    public void Dispose()
    {
        if (ResponsiveConfig != null)
            ResponsiveConfig.PropertyChanged -= OnConfigPropertyChanged;
        if (GridResponsiveConfig != null)
            GridResponsiveConfig.PropertyChanged -= OnGridConfigPropertyChanged;
        if (_debugTap != null)
        {
            _debugTap.Tapped -= GlobalMethods.OnGlobalTapDebug;
            this.GestureRecognizers.Remove(_debugTap);
            _debugTap = null;
        }
    }
    #endregion
}