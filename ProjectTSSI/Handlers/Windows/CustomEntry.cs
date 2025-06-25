using System.ComponentModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls.Shapes;
using ProjectTSSI.Global;
using ProjectTSSI.Models.CustomHandlers;
using ProjectTSSI.Models.Interfaces;

namespace ProjectTSSI.Handlers.Windows;

public class CustomEntry : Entry, IDisposable, ITapCustomComponent
{
    public CustomEntry()
    {
        this.Focused += (s, e) =>
        {
            GlobalMethods.OnGlobalTapDebug(s, e);
            this.Focus();
        };
    }
    #region  Properties
    public static readonly BindableProperty ResponsiveConfigProperty =
        BindableProperty.Create(
            nameof(ResponsiveConfig),
            typeof(ResponsiveConfig),
            typeof(CustomEntry),
            new ResponsiveConfig { WidthPercentage = 20, HeightPercentage = 20, MarginPercentage = "0,0,0,0", PaddingPercentage = "0,0,0,0", MinimumWidthRequestPercentage = 10, MinimumHeightRequestPercentage = 50, MaximumWidthRequestPercentage = 100, MaximumHeightRequestPercentage = 100, BackgroundColorView = "#FFFFFF" },
            propertyChanged: OnResponsiveConfigChanged);
    public static readonly BindableProperty EntryResponsiveConfigProperty =
        BindableProperty.Create(
            nameof(EntryResponsiveConfig),
            typeof(ResponsiveEntryConfig),
            typeof(CustomEntry),
            new ResponsiveEntryConfig { BackgroundColorHolderView = "#000000", PlaceholderColorView = "#FFFFFF" },
            propertyChanged: OnResponsiveEntryConfigChanged);
    public static readonly BindableProperty LabelResponsiveConfigProperty =
        BindableProperty.Create(
            nameof(LabelResponsiveConfig),
            typeof(ResponsiveLabelConfig),
            typeof(CustomEntry),
            new ResponsiveLabelConfig { CharacterSpacingPercentage = 0, ColorTextView = "#000000", FontSizePercentage = 1, LineHeightPercentage = 0, MaxLinesView = 1 },
            propertyChanged: OnResponsiveLabelConfigChanged);
    public static readonly BindableProperty TapEnabledProperty =
        BindableProperty.Create(
            nameof(IsTapEnabled),
            typeof(bool),
            typeof(CustomEntry),
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
    public ResponsiveEntryConfig EntryResponsiveConfig
    {
        get => (ResponsiveEntryConfig)GetValue(EntryResponsiveConfigProperty);
        set => SetValue(EntryResponsiveConfigProperty, value);
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
        var control = (CustomEntry)bindable;
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
        var control = (CustomEntry)bindable;
        if (oldValue is ResponsiveConfig oldConfig)
            oldConfig.PropertyChanged -= control.OnConfigPropertyChanged;
        if (newValue is ResponsiveConfig newConfig)
            newConfig.PropertyChanged += control.OnConfigPropertyChanged;
        control.UpdateAll();
    }
    private static void OnResponsiveEntryConfigChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomEntry)bindable;
        if (oldValue is ResponsiveEntryConfig oldEntryConfig)
            oldEntryConfig.PropertyChanged -= control.OnEntryConfigPropertyChanged;
        if (newValue is ResponsiveEntryConfig newEntryConfig)
            newEntryConfig.PropertyChanged += control.OnEntryConfigPropertyChanged;
        control.UpdateAll();
    }
    private static void OnResponsiveLabelConfigChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomEntry)bindable;
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
            (CustomEntry)this,
            ResponsiveConfig,
            EntryResponsiveConfig,
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
    private void OnEntryConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        string[] relevantProperties = {
            nameof(ResponsiveEntryConfig.BackgroundColorHolderView),
            nameof(ResponsiveEntryConfig.PlaceholderColorView)
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
        if (ResponsiveConfig != null && EntryResponsiveConfig != null && LabelResponsiveConfig != null && GlobalConstants.ScreenHeight > 0 && GlobalConstants.ScreenWidth > 0 && GlobalConstants.ScreenDensity > 0)
        {
            this.WidthRequest = (int)((GlobalConstants.ScreenWidth * ResponsiveConfig.WidthPercentage) / GlobalConstants.ScreenDensity);
            this.HeightRequest = (int)((GlobalConstants.ScreenHeight * ResponsiveConfig.HeightPercentage) / GlobalConstants.ScreenDensity);
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

            if (!string.IsNullOrWhiteSpace(EntryResponsiveConfig.PlaceholderColorView) &&
                EntryResponsiveConfig.PlaceholderColorView.StartsWith("#") &&
                !int.TryParse(EntryResponsiveConfig.PlaceholderColorView, out _))
            {
                this.PlaceholderColor = Color.FromArgb(EntryResponsiveConfig.PlaceholderColorView);
            }
            if (!string.IsNullOrWhiteSpace(EntryResponsiveConfig.BackgroundColorHolderView) &&
                EntryResponsiveConfig.BackgroundColorHolderView.StartsWith("#") &&
                !int.TryParse(EntryResponsiveConfig.BackgroundColorHolderView, out _))
            {
                this.Background = Color.FromArgb(EntryResponsiveConfig.BackgroundColorHolderView);
            }

            this.FontSize = (int)((GlobalConstants.ScreenHeight * LabelResponsiveConfig.FontSizePercentage) / GlobalConstants.ScreenDensity);
            // this.LineHeight = (int)((GlobalConstants.ScreenHeight * LabelResponsiveConfig.LineHeightPercentage) / GlobalConstants.ScreenDensity);
            this.CharacterSpacing = (int)((GlobalConstants.ScreenWidth * LabelResponsiveConfig.CharacterSpacingPercentage) / GlobalConstants.ScreenDensity);
            // this.MaxLines = LabelResponsiveConfig.MaxLinesView;
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
        if (EntryResponsiveConfig != null)
            EntryResponsiveConfig.PropertyChanged -= OnEntryConfigPropertyChanged;
        if (LabelResponsiveConfig != null)
            LabelResponsiveConfig.PropertyChanged -= OnLabelConfigPropertyChanged;
    }
    #endregion
}