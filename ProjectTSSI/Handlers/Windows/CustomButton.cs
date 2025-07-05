using System.ComponentModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls.Shapes;
using ProjectTSSI.Global;
using ProjectTSSI.Models.CustomHandlers;
using ProjectTSSI.Models.Interfaces;

namespace ProjectTSSI.Handlers.Windows;

public class CustomButton : Button, IDisposable, ITapCustomComponent
{
    private TapGestureRecognizer _debugTap;

    public CustomButton()
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
            typeof(CustomButton),
            new ResponsiveConfig { WidthPercentage = 20, HeightPercentage = 20, MarginPercentage = "0,0,0,0", PaddingPercentage = "0,0,0,0", MinimumWidthRequestPercentage = 10, MinimumHeightRequestPercentage = 50, MaximumWidthRequestPercentage = 100, MaximumHeightRequestPercentage = 100, BackgroundColorView = "#FFFFFF" },
            propertyChanged: OnResponsiveConfigChanged);
    public static readonly BindableProperty ButtonResponsiveConfigProperty =
        BindableProperty.Create(
            nameof(ButtonResponsiveConfig),
            typeof(ResponsiveButtonConfig),
            typeof(CustomButton),
            new ResponsiveButtonConfig { CornerRadiusPercentage = 0, BorderWidthPercentage = 0, BorderColorView = "#000000", BackgroundButtonColorView = "#FFFFFF", ShadowView = "0,1,#000000,(0,0)" },
            propertyChanged: OnResponsiveButtonConfigChanged);
    public static readonly BindableProperty LabelResponsiveConfigProperty =
        BindableProperty.Create(
            nameof(LabelResponsiveConfig),
            typeof(ResponsiveLabelConfig),
            typeof(CustomButton),
            new ResponsiveLabelConfig { CharacterSpacingPercentage = 0, ColorTextView = "#000000", FontSizePercentage = 1, LineHeightPercentage = 0, MaxLinesView = 1 },
            propertyChanged: OnResponsiveLabelConfigChanged);
    public static readonly BindableProperty TapEnabledProperty =
        BindableProperty.Create(
            nameof(IsTapEnabled),
            typeof(bool),
            typeof(CustomButton),
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
    public ResponsiveButtonConfig ButtonResponsiveConfig
    {
        get => (ResponsiveButtonConfig)GetValue(ButtonResponsiveConfigProperty);
        set => SetValue(ButtonResponsiveConfigProperty, value);
    }
    public ResponsiveLabelConfig LabelResponsiveConfig
    {
        get => (ResponsiveLabelConfig)GetValue(LabelResponsiveConfigProperty);
        set => SetValue(LabelResponsiveConfigProperty, value);
    }
    #endregion

    #region MethodsChange
    private static void OnTapEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomButton)bindable;
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
        var control = (CustomButton)bindable;
        if (oldValue is ResponsiveConfig oldConfig)
            oldConfig.PropertyChanged -= control.OnConfigPropertyChanged;
        if (newValue is ResponsiveConfig newConfig)
            newConfig.PropertyChanged += control.OnConfigPropertyChanged;
        control.UpdateAll();
    }
    private static void OnResponsiveButtonConfigChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomButton)bindable;
        if (oldValue is ResponsiveButtonConfig oldButtonConfig)
            oldButtonConfig.PropertyChanged -= control.OnButtonConfigPropertyChanged;
        if (newValue is ResponsiveButtonConfig newButtonConfig)
            newButtonConfig.PropertyChanged += control.OnButtonConfigPropertyChanged;
        control.UpdateAll();
    }
    private static void OnResponsiveLabelConfigChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomButton)bindable;
        if (oldValue is ResponsiveLabelConfig oldLabelConfig)
            oldLabelConfig.PropertyChanged -= control.OnLabelConfigPropertyChanged;
        if (newValue is ResponsiveLabelConfig newLabelConfig)
            newLabelConfig.PropertyChanged += control.OnLabelConfigPropertyChanged;
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
            (CustomButton)this,
            ResponsiveConfig,
            ButtonResponsiveConfig,
            LabelResponsiveConfig
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
    private void OnButtonConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        string[] relevantProperties = {
            nameof(ResponsiveButtonConfig.CornerRadiusPercentage),
            nameof(ResponsiveButtonConfig.BorderWidthPercentage),
            nameof(ResponsiveButtonConfig.BorderColorView),
            nameof(ResponsiveButtonConfig.BackgroundButtonColorView),
            nameof(ResponsiveButtonConfig.ShadowView)

        };

        if (relevantProperties.Contains(e.PropertyName))
        {
            UpdateAll();
        }
    }
    private void OnLabelConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        string[] relevantProperties = {
            nameof(ResponsiveLabelConfig.FontSizePercentage),
            nameof(ResponsiveLabelConfig.LineHeightPercentage),
            nameof(ResponsiveLabelConfig.CharacterSpacingPercentage),
            nameof(ResponsiveLabelConfig.MaxLinesView),
            nameof(ResponsiveLabelConfig.ColorTextView)
        };

        if (relevantProperties.Contains(e.PropertyName))
        {
            UpdateAll();
        }
    }
    private void UpdateAll()
    {
        if (ResponsiveConfig != null && ButtonResponsiveConfig != null && LabelResponsiveConfig != null && GlobalConstants.ScreenHeight > 0 && GlobalConstants.ScreenWidth > 0 && GlobalConstants.ScreenDensity > 0)
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

            this.CornerRadius = (int)((GlobalConstants.ScreenHeight * ButtonResponsiveConfig.CornerRadiusPercentage) / GlobalConstants.ScreenDensity);
            this.BorderWidth = (int)((GlobalConstants.ScreenWidth * ButtonResponsiveConfig.BorderWidthPercentage) / GlobalConstants.ScreenDensity);
            this.Shadow = GlobalMethods.ConvertShadowFromString(ButtonResponsiveConfig.ShadowView);
            if (!string.IsNullOrWhiteSpace(ButtonResponsiveConfig.BorderColorView) &&
                ButtonResponsiveConfig.BorderColorView.StartsWith("#") &&
                !int.TryParse(ButtonResponsiveConfig.BorderColorView, out _))
            {
                this.BorderColor = Color.FromArgb(ButtonResponsiveConfig.BorderColorView);
            }
            if (!string.IsNullOrWhiteSpace(ButtonResponsiveConfig.BackgroundButtonColorView) &&
                ButtonResponsiveConfig.BackgroundButtonColorView.StartsWith("#") &&
                !int.TryParse(ButtonResponsiveConfig.BackgroundButtonColorView, out _))
            {
                this.Background = Color.FromArgb(ButtonResponsiveConfig.BackgroundButtonColorView);
            }
            this.FontSize = (int)((GlobalConstants.ScreenHeight * LabelResponsiveConfig.FontSizePercentage) / GlobalConstants.ScreenDensity);
            this.CharacterSpacing = (int)((GlobalConstants.ScreenWidth * LabelResponsiveConfig.CharacterSpacingPercentage) / GlobalConstants.ScreenDensity);
            if (!string.IsNullOrWhiteSpace(LabelResponsiveConfig.ColorTextView) &&
                LabelResponsiveConfig.ColorTextView.StartsWith("#") &&
                !int.TryParse(LabelResponsiveConfig.ColorTextView, out _))
            {
                this.TextColor = Color.FromArgb(LabelResponsiveConfig.ColorTextView);
            }
        }
    }
    #endregion
    #region IDisposable
    public void Dispose()
    {
        if (ResponsiveConfig != null)
            ResponsiveConfig.PropertyChanged -= OnConfigPropertyChanged;
        if (ButtonResponsiveConfig != null)
            ButtonResponsiveConfig.PropertyChanged -= OnButtonConfigPropertyChanged;
        if (LabelResponsiveConfig != null)
            LabelResponsiveConfig.PropertyChanged -= OnLabelConfigPropertyChanged;
        if (_debugTap != null)
        {
            _debugTap.Tapped -= GlobalMethods.OnGlobalTapDebug;
            this.GestureRecognizers.Remove(_debugTap);
            _debugTap = null;
        }
    }
    #endregion
}