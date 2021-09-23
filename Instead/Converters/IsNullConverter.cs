using System;
using System.Globalization;
using Xamarin.Forms;

namespace Instead.Converters
{
    public class IsNullConverter : IValueConverter
    {
        public bool Not { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Not ? value != null : value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
