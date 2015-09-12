using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Interfaces;
using NZSchools.Models;
using NZSchools.Services.AppDataService;
using NZSchools.Services.NavigationService;
using NZSchools.Services.SqlLiteService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Navigation;

namespace NZSchools.ViewModels
{
    public class SettingsPageViewModel : ViewModel, ISettingsPageViewModel
    {
        private IAppDataService _appData;
        private ISqlLiteService _db;
        private INavigationService _nav;

        private ObservableCollection<Region> _regions;
        private Region _selectedRegion;
        private bool _gps;
        private ObservableCollection<string> _distances;
        private string _selectedDistance;
        private string _infoVersion;
        private string _version;
        private string _policyText;
        private string _policy2Text;

        public ObservableCollection<Region> Regions
        {
            get { return _regions; }
            private set
            {
                _regions = value;
                OnPropertyChanged("Regions");
            }
        }

        public Region SelectedRegion
        {
            get { return _selectedRegion; }
            set
            {
                _selectedRegion = value;
                OnPropertyChanged("SelectedRegion");

                _appData.UpdateSettingsKeyValue<int>("DefaultRegion", SelectedRegion.Id);
            }
        }

        public bool Gps
        {
            get { return _gps; }
            set
            {
                _gps = value;
                OnPropertyChanged("Gps");

                _appData.UpdateSettingsKeyValue<bool>("GPS", Gps);
            }
        }

        public ObservableCollection<string> Distances
        {
            get { return _distances; }
            private set
            {
                _distances = value;
                OnPropertyChanged("Distances");
            }
        }

        public string SelectedDistance
        {
            get { return _selectedDistance; }
            set
            {
                _selectedDistance = value;
                OnPropertyChanged("SelectedDistance");

                _appData.UpdateSettingsKeyValue<string>("DefaultDistance", SelectedDistance);
            }
        }

        public string InfoVersion
        {
            get { return _infoVersion; }
            set
            {
                _infoVersion = value;
                OnPropertyChanged("InfoVersion");
            }
        }

        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                OnPropertyChanged("Version");
            }
        }

        public string PolicyText
        {
            get { return _policyText; }
            set
            {
                _policyText = value;
                OnPropertyChanged("PolicyText");
            }
        }

        public string Policy2Text
        {
            get { return _policy2Text; }
            set
            {
                _policy2Text = value;
                OnPropertyChanged("Policy2Text");
            }
        }

        public DelegateCommand BackCommand { get; set; }

        public SettingsPageViewModel(IAppDataService appData, ISqlLiteService db, INavigationService nav)
        {
            _appData = appData;
            _db = db;
            _nav = nav;

            Regions = new ObservableCollection<Region>();
            Distances = new ObservableCollection<string>();
            Distances.Add("500 m");
            Distances.Add("1 km");
            Distances.Add("2 km");
            Distances.Add("5 km");

            PolicyText = "While using our New Zealand Schools app, we may collect GPS data from your location to determine any nearby schools in your area. This GPS data will not be stored or transmitted, and will be automatically erased from memory every time the app is closed.";
            Policy2Text = "All information displayed has been gathered from the New Zealand Ministry of Education, where it is available under the Creative Commons Attribution 3.0 New Zealand licence. Therefore Mosu Apps will not be responsible for the accuracy, availability, completeneess of the information and shall not have any legal liability for any loss resulting in the use of such information.";

            InfoVersion = "aug 2015";

            BackCommand = new DelegateCommand(ExecuteBackCommand);
        }

        private void ExecuteBackCommand()
        {
            _nav.GoBack();
        }

        public int GetDefaultRegion()
        {
            return _appData.GetSettingsKeyValue<int>("DefaultRegion");
        }

        public bool GetGps()
        {
            return _appData.GetSettingsKeyValue<bool>("GPS");
        }

        public string GetDefaultDistance()
        {
            return _appData.GetSettingsKeyValue<string>("DefaultDistance");
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var regions = await _db.GetRegions();
            foreach (var r in regions)
            {
                Regions.Add(r);
            }

            var defaultRegion = _appData.GetSettingsKeyValue<int>("DefaultRegion");
            SelectedRegion = Regions.FirstOrDefault(x => x.Id == defaultRegion);
            SelectedDistance = _appData.GetSettingsKeyValue<string>("DefaultDistance");

            Gps = _appData.GetSettingsKeyValue<bool>("GPS");            

            var file = await Package.Current.InstalledLocation.GetFileAsync("AppxManifest.xml");
            var appVersion = XDocument.Load("AppxManifest.xml").Root.Elements().Where(x => x.Name.LocalName == "Identity")
                                .First().Attributes().Where(x => x.Name.LocalName == "Version").First().Value;
            Version = appVersion;

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }
    }
}
