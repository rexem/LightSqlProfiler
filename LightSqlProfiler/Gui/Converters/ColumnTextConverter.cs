using System;
using System.Windows.Data;

namespace LightSqlProfiler.Gui.Converters
{
    public class ColumnTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var val = value as string ?? string.Empty;
            return val.Replace(Environment.NewLine, " ").Replace("\n", " ");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("Method not implemented");
        }
    }
}
