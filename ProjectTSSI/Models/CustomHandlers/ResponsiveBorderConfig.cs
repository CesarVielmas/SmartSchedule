using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectTSSI.Models.CustomHandlers;

public class ResponsiveBorderConfig : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private string _borderRadiusPercentages;
    private string _shadowView;
    private float _strokeThicknessPercentage;
    private string _strokeColor;

    public string BorderRadiusPercentages
    {
        get => _borderRadiusPercentages;
        set => SetField(ref _borderRadiusPercentages, value);
    }
    public string ShadowView
    {
        get => _shadowView;
        set => SetField(ref _shadowView, value);
    }
    public float StrokeThicknessPercentage
    {
        get => _strokeThicknessPercentage;
        set => SetField(ref _strokeThicknessPercentage, value);
    }
    public string StrokeColor
    {
        get => _strokeColor;
        set => SetField(ref _strokeColor, value);
    }
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}