using Microsoft.UI.Xaml.Input;
using System.ComponentModel;

namespace ProjectTSSI.Models.Interfaces;

public interface IViewModelNeedComponentCustom
{
    Dictionary<string, INotifyPropertyChanged> Configurations { get; set; }
    public void SetConfigurationsJson();
}