using System.ComponentModel;
using ProjectTSSI.Global;
using ProjectTSSI.Handlers.Windows;
using ProjectTSSI.Models.Interfaces;
namespace ProjectTSSI.Register.ViewModels;

public class RegisterPart3ViewModel : INotifyPropertyChanged, IViewModelNeedComponentCustom
{
    public Dictionary<string, INotifyPropertyChanged> Configurations { get; set; } = new();
    public IAnimationServiceComposer _composerAnimation;
    public event PropertyChangedEventHandler PropertyChanged;
    public RegisterPart3ViewModel(IAnimationServiceComposer composerAnimation)
    {
        _composerAnimation = composerAnimation;
    }
    public void SetConfigurationsJson()
    {
        GlobalMethods.LoadJsonConfigurations(this.Configurations, "RegisterWSPartsConfigs.json");
        OnPropertyChanged(nameof(this.Configurations));
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}