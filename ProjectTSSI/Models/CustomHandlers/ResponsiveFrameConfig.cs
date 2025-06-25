using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectTSSI.Models.CustomHandlers;

public class ResponsiveFrameConfig : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private float _cornerRadiusPercentage;
    private string _shadowView;
    private string _borderColorView;

    public float CornerRadiusPercentage
    {
        get => _cornerRadiusPercentage;
        set => SetField(ref _cornerRadiusPercentage, value);
    }
    public string ShadowView
    {
        get => _shadowView;
        set => SetField(ref _shadowView, value);
    }
    public string BorderColorView
    {
        get => _borderColorView;
        set => SetField(ref _borderColorView, value);
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