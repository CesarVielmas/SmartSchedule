using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using ProjectTSSI.Global;

namespace ProjectTSSI.Handlers.Windows;

[ContentProperty(nameof(Key))]
public class CustomConfigBinding : IMarkupExtension
{
    public string Key { get; set; }
    public BindingMode Mode { get; set; } = BindingMode.TwoWay;

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        var safeKey = Key.Replace('.', '_');
        var path = $"Configurations[{safeKey}]";
        return new Binding(path, mode: Mode);
    }
}