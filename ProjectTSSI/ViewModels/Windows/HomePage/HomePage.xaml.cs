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

public partial class HomePage : ContentPage, INotifyPropertyChanged
{
    private HomePageViewModel _viewModel;
    public HomePage()
    {
        _viewModel = new HomePageViewModel();
        // _viewModel.SetConfigurationsJson();
        InitializeComponent();
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }
}
