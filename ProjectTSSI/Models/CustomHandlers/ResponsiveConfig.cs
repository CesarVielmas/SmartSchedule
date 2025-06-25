using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectTSSI.Models.CustomHandlers;

public class ResponsiveConfig : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private float _widthPercentage;
    private float _heightPercentage;
    private float _minimumWidthRequestPercentage;
    private float _minimunHeightRequestPercentage;
    private float _maximumWidthRequestPercentage;
    private float _maximumHeightRequestPercentage;
    private string _marginPercentage;
    private string _paddingPercentage;
    private string _backgroundColorView;
    public float WidthPercentage
    {
        get => _widthPercentage;
        set => SetField(ref _widthPercentage, value);
    }

    public float HeightPercentage
    {
        get => _heightPercentage;
        set => SetField(ref _heightPercentage, value);
    }
    public float MinimumWidthRequestPercentage
    {
        get => _minimumWidthRequestPercentage;
        set => SetField(ref _minimumWidthRequestPercentage, value);
    }

    public float MinimumHeightRequestPercentage
    {
        get => _minimunHeightRequestPercentage;
        set => SetField(ref _minimunHeightRequestPercentage, value);
    }
    public float MaximumWidthRequestPercentage
    {
        get => _maximumWidthRequestPercentage;
        set => SetField(ref _maximumWidthRequestPercentage, value);
    }
    public float MaximumHeightRequestPercentage
    {
        get => _maximumHeightRequestPercentage;
        set => SetField(ref _maximumHeightRequestPercentage, value);
    }
    public string MarginPercentage
    {
        get => _marginPercentage;
        set => SetField(ref _marginPercentage, value);
    }
    public string PaddingPercentage
    {
        get => _paddingPercentage;
        set => SetField(ref _paddingPercentage, value);
    }
    public string BackgroundColorView
    {
        get => _backgroundColorView;
        set => SetField(ref _backgroundColorView, value);
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