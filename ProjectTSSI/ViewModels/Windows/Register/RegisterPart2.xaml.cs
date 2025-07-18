using ProjectTSSI.Handlers.Windows;
using ProjectTSSI.Models.Interfaces;
using ProjectTSSI.Register.ViewModels;

namespace ProjectTSSI;

public partial class RegisterPart2 : ContentView
{
    public RegisterPart2ViewModel _viewModel;
    public RegisterPart2(IAnimationServiceComposer composerAnimation)
    {
        _viewModel = new RegisterPart2ViewModel(composerAnimation);
        _viewModel.SetConfigurationsJson();
        InitializeComponent();
        BindingContext = _viewModel;
        this.Loaded += OnPageLoaded;
    }
    private void OnPageLoaded(object sender, EventArgs e)
    {
        _viewModel._composerAnimation.Animate("TextAnimation", "Visibility", registerStackPrincipal, "OpacityFadeIn", 0, 2000);
        _viewModel.listAbsoluteErrors = new List<VisualElement> { absoluteErrorMail, absoluteErrorPassword, absoluteErrorConfirmPassword };
        _viewModel.SetConfigurationsJson();
    }
    private void OnEntryFocus(object sender, FocusEventArgs e)
    {
        VisualElement objToFocus = sender as VisualElement;
        if (objToFocus == null)
            return;
        CustomImage imageChild = (((objToFocus.Parent as CustomFrame).Parent as CustomGrid).Children[2] as CustomAbsoluteLayout).Children[0] as CustomImage;
        ParameterElementsCommand payload = new ParameterElementsCommand
        {
            MainElement = objToFocus,
            OtherElements = new List<VisualElement> { imageChild }
        };
        if (_viewModel.OnFocusTapped.CanExecute(payload) && imageChild.Source.ToString().Contains("innactive"))
            _viewModel.OnFocusTapped.Execute(payload);
        else if (_viewModel.OnUnFocusTapped.CanExecute(payload) && imageChild.Source.ToString().Contains("active"))
            _viewModel.OnUnFocusTapped.Execute(payload);
    }
}




