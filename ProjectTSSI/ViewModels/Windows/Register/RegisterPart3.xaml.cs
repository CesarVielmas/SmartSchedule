using ProjectTSSI.Handlers.Windows;
using ProjectTSSI.Models.Interfaces;
using ProjectTSSI.Register.ViewModels;

namespace ProjectTSSI;

public partial class RegisterPart3 : ContentView
{
    public RegisterPart3ViewModel _viewModel;
    public RegisterPart3(IAnimationServiceComposer composerAnimation)
    {
        _viewModel = new RegisterPart3ViewModel(composerAnimation);
        _viewModel.SetConfigurationsJson();
        InitializeComponent();
        BindingContext = _viewModel;
        this.Loaded += OnPageLoaded;
    }
    private void OnPageLoaded(object sender, EventArgs e)
    {
        _viewModel._composerAnimation.Animate("TextAnimation", "Visibility", registerStackPrincipal, "OpacityFadeIn", 0, 2000);
        _viewModel.SetConfigurationsJson();
    }
}