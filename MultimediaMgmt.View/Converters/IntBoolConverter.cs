using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MultimediaMgmt.View.Converters
{
    class DoubleBoolConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
                return false;
            return ((double)value) == 0 ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return 0;
            return (bool)value ? 1 : 0;
        }
    }
}
