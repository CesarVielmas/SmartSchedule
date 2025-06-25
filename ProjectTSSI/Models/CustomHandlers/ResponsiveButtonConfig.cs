using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectTSSI.Models.CustomHandlers;

public class ResponsiveButtonConfig : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private float _borderWidthPercentage;
    private float _cornerRadiusPercentage;
    private string _borderColorView;
    private string _backgroundButtonColorView;
    private string _shadowView;
    public float BorderWidthPercentage
    {
        get => _borderWidthPercentage;
        set => SetField(ref _borderWidthPercentage, value);
    }
    public float CornerRadiusPercentage
    {
        get => _cornerRadiusPercentage;
        set => SetField(ref _cornerRadiusPercentage, value);
    }
    public string BorderColorView
    {
        get => _borderColorView;
        set => SetField(ref _borderColorView, value);
    }
    public string BackgroundButtonColorView
    {
        get => _backgroundButtonColorView;
        set => SetField(ref _backgroundButtonColorView, value);
    }
    public string ShadowView
    {
        get => _shadowView;
        set => SetField(ref _shadowView, value);
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