using Microsoft.Practices.Prism.Mvvm;
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
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace NZSchools.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SchoolPage : Page, IView
    {
        public SchoolPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
                return;

            var directory = (Directory)NavigationParameters.Instance.GetParameters();

            var icon = new MapIconUserControl(directory);
            var position = new BasicGeoposition();
            position.Latitude = directory.Latitude;
            position.Longitude = directory.Longitude;
            var geopoint = new Geopoint(position);

            MapControl.Children.Add(icon);            

            MapControl.SetLocation(icon, geopoint);
        }
    }
}
