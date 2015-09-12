using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
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
using Bing.Maps;
using NZSchools.Helpers;
using NZSchools.Models;
using NZSchools.UserControls;
using System.Threading;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NZSchools.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResultsPage : Page, IView
    {
        private Map _mapControl;
        private ListView _listView;

        public ResultsPage()
        {
            this.InitializeComponent();            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            //await MapControl.TrySetViewAsync(new Geopoint(defaultCenter));    
        }

        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            var map = VisualHelper.FindChildControl<Map>(MapHubSection, "MapControl") as Map;
            _mapControl = map;

            _mapControl.ZoomLevel = 5;
            _mapControl.Center = new Location(-42.897857, 170.816761);

            var geopoints = new List<Geopoint>();
            _listView.Items.Cast<Directory>().Where(x => x.Latitude != 0 && x.Longitude != 0).ToObservable()
                .ObserveOnDispatcher().Do(x =>
                {
                    var icon = new MapIconUserControl(x);
                    icon.Tapped -= icon_Tapped;
                    icon.Tapped += icon_Tapped;

                    var position = new BasicGeoposition();
                    position.Latitude = x.Latitude;
                    position.Longitude = x.Longitude;
                    var geopoint = new Geopoint(position);
                    geopoints.Add(geopoint);
                    MapLayer.SetPosition(icon, new Location(x.Latitude, x.Longitude));
                    _mapControl.Children.Add(icon);
                }).Finally(() => SetCenterOfPoints(geopoints)).Subscribe();
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = VisualHelper.FindChildControl<ListView>(ListHubSection, "SchoolsListView") as ListView;
            _listView = listView;
        }

        private void icon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var icon = sender as MapIconUserControl;
            _listView.ScrollIntoView(_listView.Items.Last());
            _listView.ScrollIntoView(icon.Directory);
        }

        private void SetCenterOfPoints(IEnumerable<Geopoint> positions)
        {
            var mapWidth = _mapControl.ActualWidth;
            var mapHeight = _mapControl.ActualHeight;
            if (mapWidth == 0 || mapHeight == 0)
                return;

            if (positions.Count() == 0)
                return;

            if (positions.Count() == 1)
            {
                _mapControl.Center = new Location(positions.First().Position.Latitude, positions.First().Position.Longitude);
                _mapControl.ZoomLevel = 16;
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
            _mapControl.ZoomLevel = zoom - 0.7;

            //var box = new GeoboundingBox(nw, se);
            _mapControl.Center = new Location(((maxLatitude - minLatitude) / 2) + minLatitude, ((maxLongitude - minLongitude) / 2) + minLongitude);
        }

    }
}
