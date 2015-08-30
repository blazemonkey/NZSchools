using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Helpers;
using NZSchools.Interfaces;
using NZSchools.Models;
using NZSchools.Services.AppDataService;
using NZSchools.Services.MessengerService;
using NZSchools.Services.NavigationService;
using NZSchools.Services.SqlLiteService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Navigation;

namespace NZSchools.ViewModels
{
    public class MainPageViewModel : ViewModel, IMainPageViewModel
    {
        private ISqlLiteService _db;
        private INavigationService _nav;
        private IAppDataService _appData;
        private IMessengerService _msg;
        private ISettingsPageViewModel _settings;

        private bool _isLoading;
        private List<Directory> _directories;
        private ObservableCollection<Region> _regions;
        private ObservableCollection<string> _cities;
        private ObservableCollection<string> _suburbs;
        private ObservableCollection<string> _genders;
        private ObservableCollection<string> _schoolTypes;
        private ObservableCollection<string> _deciles;

        private string _searchText;
        private Region _selectedRegion;
        private string _selectedCity;
        private string _selectedSuburb;
        private string _selectedGender;
        private string _selectedSchoolType;
        private string _selectedDecile;

        private IList<AlphaKeyGroup<Directory>> _grouped;
        private bool _hasFavourites;

        private bool _isMapLocked;
        private IDisposable _statusChanged;
        private IDisposable _positionChanged;
        private double _lastPositionLatitude;
        private double _lastPositionLongitude;
        private bool _gpsState;
        private Geolocator _geolocator;
        private Geopoint _center;
        private double _zoomLevel;
        private List<Directory> _nearbySchools;

        public bool IsLoading
        {
            get { return _isLoading; }
            private set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        public List<Directory> Directories
        {
            get { return _directories; }
            private set
            {
                _directories = value;
                OnPropertyChanged("Directories");
            }
        }

        public ObservableCollection<Region> Regions
        {
            get { return _regions; }
            private set
            {
                _regions = value;
                OnPropertyChanged("Regions");
            }
        }

        public ObservableCollection<string> Cities
        {
            get { return _cities; }
            private set
            {
                _cities = value;
                OnPropertyChanged("Cities");
            }
        }

        public ObservableCollection<string> Suburbs
        {
            get { return _suburbs; }
            private set
            {
                _suburbs = value;
                OnPropertyChanged("Suburbs");
            }
        }

        public ObservableCollection<string> Genders
        {
            get { return _genders; }
            private set
            {
                _genders = value;
                OnPropertyChanged("Genders");

                if (Genders.Any())
                    return;

                Genders.Add("all genders");
            }
        }

        public ObservableCollection<string> SchoolTypes
        {
            get { return _schoolTypes; }
            private set
            {
                _schoolTypes = value;
                OnPropertyChanged("SchoolTypes");

                if (SchoolTypes == null)
                    return;

                SchoolTypes.Add("all school types");
            }
        }

        public ObservableCollection<string> Deciles
        {
            get { return _deciles; }
            private set
            {
                _deciles = value;
                OnPropertyChanged("Deciles");

                if (Deciles == null)
                    return;

                Deciles.Add("all deciles");
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
            }
        }

        public Region SelectedRegion
        {
            get { return _selectedRegion; }
            set
            {
                _selectedRegion = value;
                OnPropertyChanged("SelectedRegion");
            }
        }

        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                OnPropertyChanged("SelectedCity");
            }
        }

        public string SelectedSuburb
        {
            get { return _selectedSuburb; }
            set
            {
                _selectedSuburb = value;
                OnPropertyChanged("SelectedSuburb");
            }
        }

        public string SelectedGender
        {
            get { return _selectedGender; }
            set
            {
                _selectedGender = value;
                OnPropertyChanged("SelectedGender");
            }
        }

        public string SelectedSchoolType
        {
            get { return _selectedSchoolType; }
            set
            {
                _selectedSchoolType = value;
                OnPropertyChanged("SelectedSchoolType");
            }
        }

        public string SelectedDecile
        {
            get { return _selectedDecile; }
            set
            {
                _selectedDecile = value;
                OnPropertyChanged("SelectedDecile");
            }
        }

        public IList<AlphaKeyGroup<Directory>> Grouped
        {
            get { return _grouped; }
            set
            {
                _grouped = value;
                OnPropertyChanged("Grouped");
            }
        }

        public bool HasFavourites
        {
            get { return _hasFavourites; }
            set
            {
                _hasFavourites = value;
                OnPropertyChanged("HasFavourites");
            }
        }

        public bool IsMapLocked
        {
            get { return _isMapLocked; }
            set
            {
                _isMapLocked = value;
                OnPropertyChanged("IsMapLocked");
                OnPropertyChanged("IsMapLockedLabel");
            }
        }

        public string IsMapLockedLabel
        {
            get { return IsMapLocked ? "unlock" : "lock"; }
        }

        public IDisposable StatusChanged
        {
            get { return _statusChanged; }
            private set
            {
                _statusChanged = value;
                OnPropertyChanged("StatusChanged");
            }
        }

        public IDisposable PositionChanged
        {
            get { return _positionChanged; }
            private set
            {
                _positionChanged = value;
                OnPropertyChanged("PositionChanged");
            }
        }

        public bool GpsState
        {
            get { return _gpsState; }
            set
            {
                _gpsState = value;
                OnPropertyChanged("GpsState");
            }
        }

        public double LastPositionLatitude
        {
            get { return _lastPositionLatitude; }
            set
            {
                _lastPositionLatitude = value;
                OnPropertyChanged("LastPositionLatitude");
            }
        }

        public double LastPositionLongitude
        {
            get { return _lastPositionLongitude; }
            set
            {
                _lastPositionLongitude = value;
                OnPropertyChanged("LastPositionLongitude");
            }
        }

        public Geolocator Geolocator
        {
            get { return _geolocator; }
            private set
            {
                _geolocator = value;
                OnPropertyChanged("Geolocator");
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

        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set
            {
                _zoomLevel = value;
                OnPropertyChanged("ZoomLevel");
            }
        }

        public List<Directory> NearbySchools
        {
            get { return _nearbySchools; }
            set
            {
                _nearbySchools = value;
                OnPropertyChanged("NearbySchools");
            }
        }

        public DelegateCommand SelectedRegionChangedCommand { get; set; }
        public DelegateCommand SelectedCityChangedCommand { get; set; }

        public DelegateCommand TapSearchSchoolsCommand { get; set; }
        public DelegateCommand<Directory> TapSchoolCommand { get; set; }
        public DelegateCommand TapSettingsCommand { get; set; }
        public DelegateCommand TapLockMapCommand { get; set; }
        public DelegateCommand TapNearbyListCommand { get; set; }
        public DelegateCommand TapCenterMapCommand { get; set; }

        public MainPageViewModel(ISqlLiteService db, INavigationService nav, IAppDataService appData, IMessengerService msg,
            ISettingsPageViewModel settings)
        {
            _db = db;
            _nav = nav;
            _appData = appData;
            _msg = msg;
            _settings = settings;

            Directories = new List<Directory>();
            Cities = new ObservableCollection<string>();
            Suburbs = new ObservableCollection<string>();
            Genders = new ObservableCollection<string>();
            SchoolTypes = new ObservableCollection<string>();
            Deciles = new ObservableCollection<string>();

            IsMapLocked = true;

            SelectedRegionChangedCommand = new DelegateCommand(ExecuteSelectedRegionChangedCommand);
            SelectedCityChangedCommand = new DelegateCommand(ExecuteSelectedCityChangedCommand);

            TapSearchSchoolsCommand = new DelegateCommand(ExecuteTapSearchSchoolsCommand);
            TapSchoolCommand = new DelegateCommand<Directory>(ExecuteTapSchoolCommand);
            TapSettingsCommand = new DelegateCommand(ExecuteTapSettingsCommand);
            TapLockMapCommand = new DelegateCommand(ExecuteTapLockMapCommand);
            TapNearbyListCommand = new DelegateCommand(ExecuteTapNearbyListCommand);
            TapCenterMapCommand = new DelegateCommand(ExecuteTapCenterMapCommand);           
        }

        private void UpdateStatusChanged(EventPattern<StatusChangedEventArgs> e)
        {
            if (e.EventArgs.Status == PositionStatus.Ready)
                GpsState = true;
            else
                GpsState = false;

            _msg.Send<Geopoint>(Center, "PositionChanged");
        }

        private void UpdatePositionChanged(EventPattern<PositionChangedEventArgs> e)
        {
            LastPositionLatitude = e.EventArgs.Position.Coordinate.Point.Position.Latitude;
            LastPositionLongitude = e.EventArgs.Position.Coordinate.Point.Position.Longitude;
            System.Diagnostics.Debug.WriteLine(string.Format("Latitude: {0}, Longitude: {1}",
                LastPositionLatitude, LastPositionLongitude));

            var position = new BasicGeoposition();
            position.Latitude = LastPositionLatitude;
            position.Longitude = LastPositionLongitude;
            Center = new Geopoint(position);
            ZoomLevel = 16;

            _appData.UpdateSettingsKeyValue<double>("LastLatitude", position.Latitude);
            _appData.UpdateSettingsKeyValue<double>("LastLongitude", position.Longitude);

            NearbySchools = new List<Directory>(SchoolsWithinsSquare(Center));
            _msg.Send<Geopoint>(Center, "PositionChanged");
            _msg.Send<IEnumerable<Directory>>(NearbySchools, "NearbySchoolsChanged");
        }

        private IEnumerable<Directory> SchoolsWithinsSquare(Geopoint center)
        {
            var defaultDistance = _settings.GetDefaultDistance();
            var distance = 0.0;

            switch (defaultDistance)
            {
                case "500 m":
                    distance = 0.0025;
                    break;
                case "1 km":
                    distance = 0.0050;
                    break;
                case "2 km":
                    distance = 0.0100;
                    break;
                case "5 km":
                    distance = 0.0250;
                    break;
            }

            var directories = Directories.Where(x => x.Latitude != 0.0 && x.Longitude != 0.0)
                        .Where(x => (x.Longitude <= center.Position.Longitude + distance) &&
                        (x.Longitude >= center.Position.Longitude - distance) &&
                        (x.Latitude <= center.Position.Latitude + distance) &&
                        (x.Latitude >= center.Position.Latitude - distance));

            return directories;
        }

        private async void ExecuteSelectedRegionChangedCommand()
        {
            if (Cities.Any())
            {
                Cities.Clear();
                // no joke, without this task delay it doesn't work
                // bloody cost me till 2am! 
                // http://stackoverflow.com/questions/28199771/universal-app-loading-combobox-itemssource-async-gives-weird-behaviour
                await Task.Delay(50);
            }

            Cities.Add("all cities");
            Populate<string>(Cities, Directories.Where(x => x.RegionalCouncil.ToLower() == SelectedRegion.Name + " region")
                                                                 .GroupBy(x => x.City)
                                                                 .Select(x => x.Key.ToLower().ToString())
                                                                 .OrderBy(x => x).ToList());
            SelectedCity = Cities.FirstOrDefault();
        }

        private async void ExecuteSelectedCityChangedCommand()
        {
            if (!Cities.Any())
                return;

            if (Suburbs.Any())
            {
                Suburbs.Clear();
                await Task.Delay(50);
            }

            Suburbs.Add("all suburbs");

            if (SelectedCity == "all cities")
            {
                Populate<string>(Suburbs, Directories.Where(x => x.RegionalCouncil.ToLower() == SelectedRegion.Name + " region")
                                                                      .GroupBy(x => x.Suburb)
                                                                      .Where(x => !string.IsNullOrEmpty(x.Key))
                                                                      .Select(x => x.Key.ToLower().ToString())
                                                                      .OrderBy(x => x).ToList());
            }
            else
            {
                Populate<string>(Suburbs, Directories.Where(x => x.RegionalCouncil.ToLower() == SelectedRegion.Name + " region")
                                                                      .Where(x => x.City.ToLower() == SelectedCity)
                                                                      .GroupBy(x => x.Suburb)
                                                                      .Where(x => !string.IsNullOrEmpty(x.Key))
                                                                      .Select(x => x.Key.ToLower().ToString())
                                                                      .OrderBy(x => x).ToList());
            }

            SelectedSuburb = Suburbs.FirstOrDefault();
        }

        private void ExecuteTapSearchSchoolsCommand()
        {
            var schools = Directories.Where(x => x.RegionalCouncil.ToLower() == SelectedRegion.Name + " region");

            if (!string.IsNullOrEmpty(SearchText))
                schools = schools.Where(x => x.Name.ToLower().Contains(SearchText.ToLower()));

            if (SelectedCity != "all cities")
                schools = schools.Where(x => x.City.ToLower() == SelectedCity);

            if (SelectedSuburb != "all suburbs")
                schools = schools.Where(x => x.Suburb.ToLower() == SelectedSuburb);

            if (SelectedGender != "all genders")
                schools = schools.Where(x => x.GenderOfStudents.ToLower() == SelectedGender);

            if (SelectedSchoolType != "all school types")
                schools = schools.Where(x => x.SchoolType.ToLower() == SelectedSchoolType);

            if (SelectedDecile != "all deciles")
                schools = schools.Where(x => x.Decile == Int32.Parse(SelectedDecile));

            NavigationParameters.Instance.SetParameters(schools.ToList());
            _nav.Navigate(Experiences.Results);
        }

        public void ExecuteTapSchoolCommand(Directory directory)
        {
            NavigationParameters.Instance.SetParameters(directory);
            _nav.Navigate(Experiences.School);
        }

        private void ExecuteTapSettingsCommand()
        {
            _nav.Navigate(Experiences.Settings, null);
        }

        private void ExecuteTapLockMapCommand()
        {
            IsMapLocked = !IsMapLocked;
        }

        public void ExecuteTapNearbyListCommand()
        {
            NavigationParameters.Instance.SetParameters(NearbySchools);
            _nav.Navigate(Experiences.Results);
        }

        public void ExecuteTapCenterMapCommand()
        {
            ZoomLevel = 16;
            var position = new BasicGeoposition();
            position.Latitude = LastPositionLatitude;
            position.Longitude = LastPositionLongitude;
            Center = new Geopoint(position);
        }

        private void Populate<T>(IList<T> obv, List<T> collection)
        {
            foreach (var c in collection)
                obv.Add(c);
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var lastLatitude = _appData.GetSettingsKeyValue<double>("LastLatitude");
            var lastLongitude = _appData.GetSettingsKeyValue<double>("LastLongitude");

            if (_settings.GetGps())
            {
                Geolocator = new Geolocator();
                Geolocator.DesiredAccuracy = PositionAccuracy.High;
                Geolocator.MovementThreshold = 10;

                StatusChanged = Observable.FromEventPattern<StatusChangedEventArgs>(Geolocator, "StatusChanged")
                    .ObserveOnDispatcher()
                    .Do(x => UpdateStatusChanged(x)).Subscribe();
                PositionChanged = Observable.FromEventPattern<PositionChangedEventArgs>(Geolocator, "PositionChanged")
                    .ObserveOnDispatcher()
                    .Do(x => UpdatePositionChanged(x)).Subscribe();

                if (lastLatitude == 0.0 && lastLongitude == 0.0)
                    ZoomLevel = 2;
                else
                {
                    ZoomLevel = 16;
                    LastPositionLatitude = lastLatitude;
                    LastPositionLongitude = lastLongitude;
                    var position = new BasicGeoposition();
                    position.Latitude = LastPositionLatitude;
                    position.Longitude = LastPositionLongitude;
                    Center = new Geopoint(position);

                    _msg.Send<Geopoint>(Center, "PositionChanged");
                }
            }
            else
            {
                Geolocator = null;
                GpsState = false;
                _msg.Send<Geopoint>(Center, "PositionChanged");
            }

            if (Directories.Any())
            {
                Grouped = AlphaKeyGroup<Directory>.CreateGroups(Directories.Where(x => x.IsFavourites), CultureInfo.CurrentUICulture, s => s.Name, true);
                HasFavourites = Grouped.Select(x => x.Count()).Sum() > 0;
                return;
            }

            IsLoading = true;

            var directories = await _db.GetDirectories();
            Populate<Directory>(Directories, directories.ToList());

            var regions = await _db.GetRegions();
            Regions = new ObservableCollection<Region>(regions);
            SelectedRegion = Regions.FirstOrDefault(x => x.Id == _settings.GetDefaultRegion());
            ExecuteSelectedRegionChangedCommand();

            Populate<string>(Genders, directories.GroupBy(x => x.GenderOfStudents.ToLower()).Select(x => x.Key.ToString()).OrderBy(x => x).ToList());
            SelectedGender = Genders.FirstOrDefault();

            Populate<string>(SchoolTypes, directories.GroupBy(x => x.SchoolType.ToLower()).Select(x => x.Key.ToString()).OrderBy(x => x).ToList());
            SelectedSchoolType = SchoolTypes.FirstOrDefault();

            Populate<string>(Deciles, directories.GroupBy(x => x.Decile).Select(x => x.Key.ToString()).OrderBy(x => Int32.Parse(x)).ToList());
            SelectedDecile = Deciles.FirstOrDefault();

            Grouped = AlphaKeyGroup<Directory>.CreateGroups(directories.Where(x => x.IsFavourites), CultureInfo.CurrentUICulture, s => s.Name, true);
            HasFavourites = Grouped.Select(x => x.Count()).Sum() > 0;
            IsLoading = false;

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            if (StatusChanged != null)
                StatusChanged.Dispose();

            if (PositionChanged != null)
                PositionChanged.Dispose();

            base.OnNavigatedFrom(viewModelState, suspending);
        }
    }
}
