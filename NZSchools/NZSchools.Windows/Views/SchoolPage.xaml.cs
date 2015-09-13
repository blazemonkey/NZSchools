using Bing.Maps;
using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Helpers;
using NZSchools.Models;
using NZSchools.Services.NavigationService;
using NZSchools.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NZSchools.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SchoolPage : Page, IView
    {
        private Map _mapControl;

        public SchoolPage()
        {
            this.InitializeComponent();
        }

        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            var map = VisualHelper.FindChildControl<Map>(MapHubSection, "MapControl") as Map;
            _mapControl = map;

            var directory = (Directory)NavigationParameters.Instance.GetParameters();

            var icon = new MapIconUserControl(directory);

            var position = new BasicGeoposition();
            position.Latitude = directory.Latitude;
            position.Longitude = directory.Longitude;
            MapLayer.SetPosition(icon, new Location(position.Latitude, position.Longitude));

            _mapControl.Children.Add(icon);
            _mapControl.Center = new Location(directory.Latitude, directory.Longitude);
            _mapControl.ZoomLevel = 16;
        }
    }
}
