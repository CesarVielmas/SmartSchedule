using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectTSSI.Models.CustomHandlers;

public class ResponsiveLabelConfig : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private float _fontSizePercentage;
    private float _lineHeightPercentage;
    private float _characterSpacingPercentage;
    private int _maxLinesView;
    private string _colorTextView;
    public float FontSizePercentage
    {
        get => _fontSizePercentage;
        set => SetField(ref _fontSizePercentage, value);
    }
    public float LineHeightPercentage
    {
        get => _lineHeightPercentage;
        set => SetField(ref _lineHeightPercentage, value);
    }
    public float CharacterSpacingPercentage
    {
        get => _characterSpacingPercentage;
        set => SetField(ref _characterSpacingPercentage, value);
    }
    public int MaxLinesView
    {
        get => _maxLinesView;
        set => SetField(ref _maxLinesView, value);
    }
    public string ColorTextView
    {
        get => _colorTextView;
        set => SetField(ref _colorTextView, value);
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