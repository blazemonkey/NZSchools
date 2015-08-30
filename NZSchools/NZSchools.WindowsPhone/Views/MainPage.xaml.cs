using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Models;
using NZSchools.Services.MessengerService;
using NZSchools.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
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
    public sealed partial class MainPage : Page, IView
    {
        private Popup _popup;
        private GpsUserControl _gpsUserControl;

        private MessengerService _msg;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            _msg = App.Container.GetInstance<MessengerService>();
            _msg.Register<Geopoint>(this, "PositionChanged", x => DrawPositionChanged(x));
            _msg.Register<IEnumerable<Directory>>(this, "NearbySchoolsChanged", x => DrawNearbySchoolsChanged(x));
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

            if (SchoolsListView.Items.Any())
                SchoolsListView.ScrollIntoView(SchoolsListView.Items.First());
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingsAppBar.Visibility = Visibility.Collapsed;
            NearbySchoolsAppBar.Visibility = Visibility.Collapsed;
            LockMapAppBar.Visibility = Visibility.Collapsed;
            CenterMapAppBar.Visibility = Visibility.Collapsed;

            switch (MainPivot.SelectedIndex)
            {
                case 0:
                    {
                        SettingsAppBar.Visibility = Visibility.Visible;
                    }
                    break;
                case 1:
                    {
                        SettingsAppBar.Visibility = Visibility.Visible;
                    }
                    break;
                case 2:
                    {
                        NearbySchoolsAppBar.Visibility = Visibility.Visible;
                        LockMapAppBar.Visibility = Visibility.Visible;
                        CenterMapAppBar.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void DrawPositionChanged(Geopoint geopoint)
        {
            if (geopoint == null)
                return;

            var currentGps = MapControl.Children.FirstOrDefault(x => x.GetType() == typeof(GpsUserControl));
            if (currentGps != null)
                MapControl.Children.Remove(currentGps);

            var gps = new GpsUserControl();
            MapControl.Children.Add(gps);
            MapControl.SetLocation(gps, geopoint);
        }

        private void DrawNearbySchoolsChanged(IEnumerable<Directory> directories)
        {
            var existing = MapControl.Children.Where(x => x.GetType() == typeof(MapIconUserControl));
            for (var i = existing.Count(); i > 0; i--)
            {
                var directory = (MapIconUserControl)existing.ElementAt(i - 1);
                directory.Tapped -= icon_Tapped;
                MapControl.Children.Remove(directory);
            }

            foreach (var directory in directories)
            {
                var icon = new MapIconUserControl(directory);
                icon.Tapped -= icon_Tapped;
                icon.Tapped += icon_Tapped;

                var position = new BasicGeoposition();
                position.Latitude = directory.Latitude;
                position.Longitude = directory.Longitude;
                var geopoint = new Geopoint(position);

                MapControl.Children.Add(icon);

                MapControl.SetLocation(icon, geopoint);
            }
        }

        private void icon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!(sender is MapIconUserControl))
                return;

            var icon = (MapIconUserControl)sender;
            var quickSchoolView = new QuickSchoolViewUserControl(icon.Directory);
            quickSchoolView.BackToMapButtonTapped += new EventHandler(BackToMapButton_Tapped);
            _popup = new Popup();
            _popup.Child = quickSchoolView;
            _popup.IsOpen = true;
        }

        private void BackToMapButton_Tapped(object sender, EventArgs e)
        {
            _popup.IsOpen = false;
        }
    }
}
