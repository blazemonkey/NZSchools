using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Interfaces;
using NZSchools.Models;
using NZSchools.Services.NavigationService;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Calls;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml.Navigation;

namespace NZSchools.ViewModels
{
    public class SchoolPageViewModel : ViewModel, ISchoolPageViewModel
    {
        private Directory _directory;
        private List<Graph> _graphSeries;
        private double _zoomLevel;
        private Geopoint _center;
        private DataTransferManager _dataTransferManager;

        public Directory Directory
        {
            get { return _directory; }
            set
            {
                _directory = value;
                OnPropertyChanged("Directory");
            }
        }

        public List<Graph> GraphSeries
        {
            get { return _graphSeries; }
            set
            {
                _graphSeries = value;
                OnPropertyChanged("GraphSeries");
            }
        }

        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set
            {
                _zoomLevel = value;
                OnPropertyChanged("ZoomLevel");
            }
        }

        public Geopoint Center
        {
            get { return _center; }
            set
            {
                _center = value;
                OnPropertyChanged("Center");
            }
        }

        public DelegateCommand CallCommand { get; set; }
        public DelegateCommand OpenWebsiteCommand { get; set; }
        public DelegateCommand ShareCommand { get; set; }
        public DelegateCommand TapCenterMapCommand { get; set; }

        public SchoolPageViewModel()
        {
            GraphSeries = new List<Graph>();
            ZoomLevel = 16;            

            CallCommand = new DelegateCommand(ExecuteCallCommand);
            OpenWebsiteCommand = new DelegateCommand(ExecuteOpenWebsiteCommand);
            ShareCommand = new DelegateCommand(ExecuteShareCommand);
            TapCenterMapCommand = new DelegateCommand(ExecuteTapCenterMapCommand);

            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(ShareTextHandler);
        }

        public void ExecuteCallCommand()
        {
            if (string.IsNullOrEmpty(Directory.Telephone))
                return;

            var telephone = Directory.Telephone.Replace(" ", "");
            PhoneCallManager.ShowPhoneCallUI(telephone, Directory.Name);
        }

        public async void ExecuteOpenWebsiteCommand()
        {
            if (string.IsNullOrEmpty(Directory.SchoolWebsite))
                return;

            await Launcher.LaunchUriAsync(new Uri(Directory.SchoolWebsite));
        }

        public void ExecuteShareCommand()
        {
            try
            {
                DataTransferManager.ShowShareUI();
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Show Error");
            }
        }

        public void ExecuteTapCenterMapCommand()
        {
            ZoomLevel = 16;
            var position = new BasicGeoposition();
            position.Latitude = Directory.Latitude;
            position.Longitude = Directory.Longitude;
            Center = new Geopoint(position);
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            var request = e.Request;
            var sb = new StringBuilder();
            sb.AppendLine(Directory.Name);
            sb.AppendLine("");
            sb.AppendLine(Directory.AddressCombined);
            if (!string.IsNullOrEmpty(Directory.Suburb)) sb.AppendLine(Directory.Suburb);
            if (!string.IsNullOrEmpty(Directory.City)) sb.AppendLine(Directory.City);            
            sb.AppendLine(Directory.RegionalCouncil);
            sb.AppendLine(Directory.Telephone);
            sb.AppendLine(Directory.SchoolWebsite);

            // The Title is mandatory
            request.Data.Properties.Title = "New Zealand Schools";
            request.Data.Properties.Description = "An New Zelanad School's details";

            request.Data.SetText(sb.ToString());
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            Directory = (Directory)NavigationParameters.Instance.GetParameters();
            var position = new BasicGeoposition();
            position.Latitude = Directory.Latitude;
            position.Longitude = Directory.Longitude;
            Center = new Geopoint(position);

            GraphSeries.Clear();
            GraphSeries.Add(new Graph() { Name = "european / pākehā", Number = Directory.EuropeanPākehā });
            GraphSeries.Add(new Graph() { Name = "māori", Number = Directory.Māori });
            GraphSeries.Add(new Graph() { Name = "pasifika", Number = Directory.Pasifika });
            GraphSeries.Add(new Graph() { Name = "asian", Number = Directory.Asian });
            GraphSeries.Add(new Graph() { Name = "melaa", Number = Directory.Melaa });
            GraphSeries.Add(new Graph() { Name = "other", Number = Directory.Other });
            GraphSeries.Add(new Graph() { Name = "international", Number = Directory.InternationalStudents });
        }
    }
}
