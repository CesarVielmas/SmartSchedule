using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectTSSI.Models.CustomHandlers;

public class ResponsiveGridConfig : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private float _rowSpacingPercentage;
    private float _columnSpacingPercentage;
    public float RowSpacingPercentage
    {
        get => _rowSpacingPercentage;
        set => SetField(ref _rowSpacingPercentage, value);
    }

    public float ColumnSpacingPercentage
    {
        get => _columnSpacingPercentage;
        set => SetField(ref _columnSpacingPercentage, value);
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