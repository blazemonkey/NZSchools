using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Models;
using NZSchools.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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
    public sealed partial class ResultsPage : Page, IView
    {
        private Popup _popup;
        public ResultsPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += OnBackPressed;
            
            if (e.NavigationMode == NavigationMode.Back)
                return;

            if (SchoolsListView.Items.Any())
                SchoolsListView.ScrollIntoView(SchoolsListView.Items.First());

            ResultsPivot.SelectedIndex = 0;
            MapControl.Children.Clear();

            // Default
            var defaultCenter = new BasicGeoposition()
            {
                Longitude = 170.816761,
                Latitude = -42.897857
            };
            MapControl.ZoomLevel = 5;
            await MapControl.TrySetViewAsync(new Geopoint(defaultCenter));            
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //remove the handler before you leave!
            HardwareButtons.BackPressed -= OnBackPressed;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is Pivot))
                return;

            var pivot = (Pivot)sender;
            MainCommandBar.Visibility = Visibility.Visible;
            LockMapAppBar.Visibility = Visibility.Collapsed;
            switch (pivot.SelectedIndex)
            {
                case 0:
                    {
                        MainCommandBar.Visibility = Visibility.Collapsed;
                        break;
                    }
                case 1:
                    {
                        LockMapAppBar.Visibility = Visibility.Visible;
                        break;
                    }
            }

            if (pivot.SelectedIndex == 1)
            {
                if (MapControl.Children.Any())
                    return;

                var semaphore = new SemaphoreSlim(1);
                var geopoints = new List<Geopoint>();
                SchoolsListView.Items.Cast<Directory>().Where(x => x.Latitude != 0 && x.Longitude != 0).ToObservable()
                    .ObserveOnDispatcher().Do(x =>
                        {
                            var icon = new MapIconUserControl(x);
                            icon.Tapped -= icon_Tapped;
                            icon.Tapped += icon_Tapped;

                            var position = new BasicGeoposition();
                            position.Latitude = x.Latitude;
                            position.Longitude = x.Longitude;
                            MapControl.Children.Add(icon);
                            var geopoint = new Geopoint(position);
                            MapControl.SetLocation(icon, geopoint);
                            geopoints.Add(geopoint);
                        }).Finally(async () =>
                        {
                            await semaphore.WaitAsync();
                            try
                            {
                                await SetCenterOfPoints(geopoints);
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        }).Subscribe();
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

        private async Task SetCenterOfPoints(IEnumerable<Geopoint> positions)
        {
            var mapWidth = MapControl.ActualWidth;
            var mapHeight = MapControl.ActualHeight;
            if (mapWidth == 0 || mapHeight == 0)
                return;

            if (positions.Count() == 0)
                return;

            if (positions.Count() == 1)
            {
                await MapControl.TrySetViewAsync(positions.First());
                MapControl.ZoomLevel = 16;
                return;
            }

            var maxLatitude = positions.Max(x => x.Position.Latitude);
            var minLatitude = positions.Min(x => x.Position.Latitude);

            var maxLongitude = positions.Max(x => x.Position.Longitude);
            var minLongitude = positions.Min(x => x.Position.Longitude);

            var centerLatitude = ((maxLatitude - minLatitude) / 2) + minLatitude;
            var centerLongitude = ((maxLongitude - minLongitude) / 2) + minLongitude;

            var nw = new BasicGeoposition()
            {
                Latitude = maxLatitude,
                Longitude = minLongitude
            };

            var se = new BasicGeoposition()
            {
                Latitude = minLatitude,
                Longitude = maxLongitude
            };

            var buffer = 1;
            //best zoom level based on map width
            var zoomWidth = Math.Log(360.0 / 256.0 * (mapWidth - 2 * buffer) / (maxLongitude - minLongitude)) / Math.Log(2);
            //best zoom level based on map height
            var zoomHeight = Math.Log(180.0 / 256.0 * (mapHeight - 2 * buffer) / (maxLatitude - minLatitude)) / Math.Log(2);
            var zoom = (zoomWidth + zoomHeight) / 2;
            MapControl.ZoomLevel = zoom - 0.7;

            var box = new GeoboundingBox(nw, se);
            await MapControl.TrySetViewAsync(new Geopoint(box.Center));
        }

        private void OnBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (_popup == null)
                return;

            e.Handled = true;            

            if (_popup.IsOpen == true)
                _popup.IsOpen = false;
        }
    }
}
