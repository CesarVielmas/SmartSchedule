using Microsoft.UI.Xaml.Input;
using System.ComponentModel;

namespace ProjectTSSI.Models.Interfaces;

public interface IViewModelNeedCustom
{
    Dictionary<string, INotifyPropertyChanged> Configurations { get; set; }
    bool IsDebugMode { get; set; }
    void Accelerator_Invoked(object sender, KeyboardAcceleratorInvokedEventArgs args);
    void OnPageLoaded(object sender, EventArgs e);
    void OnPageUnloaded(object sender, EventArgs e);
    // public void SetConfigurationsJson();
}