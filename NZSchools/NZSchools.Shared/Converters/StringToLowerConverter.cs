using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace NZSchools.Converters
{
    public class StringToLowerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var text = value.ToString();
            return text.ToLower();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
