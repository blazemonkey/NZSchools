using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace NZSchools.Converters
{
    public class LockIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || value.GetType() != typeof(bool))
                return null;

            var boolean = System.Convert.ToBoolean(value.ToString());

            return boolean == true ? "ms-appx:///Images/Icons/unlock.png" : "ms-appx:///Images/Icons/lock.png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
