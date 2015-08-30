using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace NZSchools.Converters
{
    public class PercentFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var text = value.ToString();
            return string.Format("{0}{1}", text, "%");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
