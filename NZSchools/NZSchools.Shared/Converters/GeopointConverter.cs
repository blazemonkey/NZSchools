using NZSchools.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;

namespace NZSchools.Converters
{
    public class GeopointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || value.GetType() != typeof(Directory))
                return null;

            var directory = (Directory)value;
            if (directory.Latitude == 0 || directory.Longitude == 0)
                return null;

            var position = new BasicGeoposition();
            position.Latitude = directory.Latitude;
            position.Longitude = directory.Longitude;

            return new Geopoint(position);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
