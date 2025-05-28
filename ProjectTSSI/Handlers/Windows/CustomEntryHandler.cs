using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace ProjectTSSI;

public class CustomEntryHandler : EntryHandler
{
    protected override void ConnectHandler(TextBox nativeView)
    {
        base.ConnectHandler(nativeView);
        nativeView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
        nativeView.Background = null;
        nativeView.BorderBrush = null;
        nativeView.Padding = new Microsoft.UI.Xaml.Thickness(0);
        nativeView.Style = GetCustomStyle();

    }
    private static Microsoft.UI.Xaml.Style GetCustomStyle()
    {
        var style = new Microsoft.UI.Xaml.Style { TargetType = typeof(TextBox) };

        // Override visual states (Focused, PointerOver, etc.)
        style.Setters.Add(new Microsoft.UI.Xaml.Setter { Property = TextBox.BorderBrushProperty, Value = null });
        style.Setters.Add(new Microsoft.UI.Xaml.Setter { Property = TextBox.BackgroundProperty, Value = null });
        style.Setters.Add(new Microsoft.UI.Xaml.Setter { Property = TextBox.BorderThicknessProperty, Value = new Microsoft.UI.Xaml.Thickness(0) });
        style.Setters.Add(new Microsoft.UI.Xaml.Setter { Property = TextBox.PaddingProperty, Value = new Microsoft.UI.Xaml.Thickness(0) });

        return style;
    }
}
