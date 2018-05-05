using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MultimediaMgmt.View.Converters
{
    class StatusBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool? v = (bool?)value;
                if (!v.HasValue)
                    return "";
                switch (int.Parse(parameter.ToString()))
                {
                    case 1:
                        return v.Value ? "开" : "关";
                    case 2:
                        return v.Value ? "降" : "升";
                    case 3:
                        return v.Value ? "连接" : "断开";
                    case 4:
                        return v.Value ? "设防" : "撤防";
                    default:
                        return "";
                }
            }
            catch {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
