using System.Globalization;

namespace ProjectTSSI.Models;

public class GenericConverter<T> : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            if (typeof(T) == typeof(int))
                return int.Parse(value?.ToString() ?? "0", CultureInfo.InvariantCulture);
            if (typeof(T) == typeof(float))
                return float.Parse(value?.ToString() ?? "0", CultureInfo.InvariantCulture);
            if (typeof(T) == typeof(double))
                return double.Parse(value?.ToString() ?? "0", CultureInfo.InvariantCulture);
            if (typeof(T) == typeof(string))
                return value.ToString();
            return value;
        }
        catch
        {
            return default(T);
        }
    }
}

