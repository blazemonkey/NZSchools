using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Interfaces;
using NZSchools.Models;
using NZSchools.Services.AppDataService;
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

        private ObservableCollection<Region> _regions;
        private Region _selectedRegion;
        private bool _gps;
        private ObservableCollection<string> _distances;
        private string _selectedDistance;
        private string _version;

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

        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                OnPropertyChanged("Version");
            }
        }

        public SettingsPageViewModel(IAppDataService appData, ISqlLiteService db)
        {
            _appData = appData;
            _db = db;

            Regions = new ObservableCollection<Region>();
            Distances = new ObservableCollection<string>();
            Distances.Add("500 m");
            Distances.Add("1 km");
            Distances.Add("2 km");
            Distances.Add("5 km");
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
