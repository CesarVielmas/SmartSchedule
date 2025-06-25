using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectTSSI.Models.CustomHandlers;

public class ResponsiveEntryConfig : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private string _placeholderColorView;
    private string _backgroundColorHolderView;

    public string PlaceholderColorView
    {
        get => _placeholderColorView;
        set => SetField(ref _placeholderColorView, value);
    }
    public string BackgroundColorHolderView
    {
        get => _backgroundColorHolderView;
        set => SetField(ref _backgroundColorHolderView, value);
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