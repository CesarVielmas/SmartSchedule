using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Maui.Layouts;
using Microsoft.UI.Windowing;
using Microsoft.Maui.Platform;
using ProjectTSSI.Models.Interfaces;
using ProjectTSSI.Global;
using ProjectTSSI.Register.ViewModels;


namespace ProjectTSSI;

public partial class RegisterWS : ContentPage, INotifyPropertyChanged
{
    private RegisterWSViewModel _viewModel;
    public RegisterWS(IAnimationServiceComposer composerAnimations)
    {
        _viewModel = new RegisterWSViewModel(composerAnimations);
        _viewModel.SetConfigurationsJson();
        InitializeComponent();
        BindingContext = _viewModel;
        this.Loaded += _viewModel.OnPageLoaded;
        this.Unloaded += (s, e) => _viewModel.DisposeContentModel(s, e, absoluteLayoutCircles);
    }
    protected override async void OnAppearing()
    {
        _viewModel._listRegisterBorders = new List<VisualElement> { BorderLayoutRegisterPart1, BorderLayoutRegisterPart2, this };
        _viewModel.SetConfigurationsJson();
        _viewModel._contentToStep = StepRegisterNow;
        _viewModel.StepNow += 1;
        _viewModel.GenerateCirclesAnimation(absoluteLayoutCircles);
        await FadeInAll();
        await ImagesAnimations();
        base.OnAppearing();
    }
    private async Task FadeInAll()
    {
        await Task.WhenAll(
            _viewModel._composerAnimations.Animate("TextAnimation", "Visibility", RegisterFadeOutPart1, "OpacityFadeIn", 0, 2000),
            _viewModel._composerAnimations.Animate("TextAnimation", "Visibility", RegisterFadeOutPart2, "OpacityFadeIn", 0, 2000)
        );
    }
    private async Task ImagesAnimations()
    {
        while (true)
        {
            animationImagesRegister.Source = _viewModel._listImagesRegister[_viewModel.IndexImagesRegister];
            switch (_viewModel.IndexImagesRegister)
            {
                case 0:
                    _viewModel.IndexImagesRegister += 1;
                    break;

                case 1:
                    _viewModel.IndexImagesRegister += 1;
                    break;

                case 2:
                    _viewModel.IndexImagesRegister = 0;
                    break;
            }
            await animationImagesRegister.FadeTo(1, 2000);
            await Task.Delay(10000);
            await animationImagesRegister.FadeTo(0, 2000);
        }
    }
    private async void OnClickStep(object sender, EventArgs e)
    {
        var mainElement = sender as VisualElement;
        if (mainElement == null)
            return;
        var payloadElements = new ParameterElementsCommand
        {
            MainElement = mainElement,
            OtherElements = new List<VisualElement> { RegisterStepTop1, RegisterStepTopBorder1, RegisterStepTopImage1, RegisterStepTopLabel1, RegisterStepTop2, RegisterStepTopBorder2, RegisterStepTopImage2, RegisterStepTopLabel2, RegisterStepTop3, RegisterStepTopBorder3, RegisterStepTopImage3, RegisterStepTopLabel3, BorderButtonBack, ImageButtonBack, LabelButtonBack, BorderButtonNext, ImageButtonNext, LabelButtonNext }
        };
        if (mainElement.AutomationId.Contains("next") && _viewModel.NextStepTapped.CanExecute(payloadElements))
            _viewModel.NextStepTapped.Execute(payloadElements);
        else if (mainElement.AutomationId.Contains("back") && _viewModel.BackStepTapped.CanExecute(payloadElements))
            _viewModel.BackStepTapped.Execute(payloadElements);

    }
}
