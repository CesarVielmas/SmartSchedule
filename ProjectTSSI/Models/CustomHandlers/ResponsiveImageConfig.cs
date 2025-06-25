using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectTSSI.Models.CustomHandlers;

public class ResponsiveImageConfig : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private float _anchorXPercentage;
    private float _anchorYPercentage;
    private string _shadowView;

    public float AnchorXPercentage
    {
        get => _anchorXPercentage;
        set => SetField(ref _anchorXPercentage, value);
    }

    public float AnchorYPercentage
    {
        get => _anchorYPercentage;
        set => SetField(ref _anchorYPercentage, value);
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