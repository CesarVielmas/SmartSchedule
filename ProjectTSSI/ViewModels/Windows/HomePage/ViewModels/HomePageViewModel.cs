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

public class HomePageViewModel : INotifyPropertyChanged, IViewModelNeedCustom
{
    public event PropertyChangedEventHandler PropertyChanged;
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

    public Dictionary<string, INotifyPropertyChanged> Configurations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Accelerator_Invoked(object sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        throw new NotImplementedException();
    }

    public void OnPageLoaded(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void OnPageUnloaded(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}