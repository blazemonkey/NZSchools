using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Interfaces;
using NZSchools.Models;
using NZSchools.Services.SqlLiteService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace NZSchools.ViewModels
{
    public class MainPageViewModel : ViewModel, IMainPageViewModel
    {
        private ISqlLiteService _db;

        private List<Directory> _directories;
        private ObservableCollection<Region> _regions;
        private ObservableCollection<string> _cities;
        private ObservableCollection<string> _suburbs;
        private ObservableCollection<string> _genders;
        private ObservableCollection<string> _schoolTypes;
        private ObservableCollection<string> _deciles;

        private Region _selectedRegion;
        private string _selectedCity;
        private string _selectedSuburb;        
        private string _selectedGender;
        private string _selectedSchoolType;
        private string _selectedDecile;

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

        public DelegateCommand SelectedRegionChangedCommand { get; set; }
        public DelegateCommand SelectedCityChangedCommand { get; set; }

        public MainPageViewModel(ISqlLiteService db)
        {
            _db = db;

            Cities = new ObservableCollection<string>();
            Suburbs = new ObservableCollection<string>();
            Genders = new ObservableCollection<string>();
            SchoolTypes = new ObservableCollection<string>();
            Deciles = new ObservableCollection<string>();

            SelectedRegionChangedCommand = new DelegateCommand(ExecuteSelectedRegionChangedCommand);
            SelectedCityChangedCommand = new DelegateCommand(ExecuteSelectedCityChangedCommand);
        }

        private async void ExecuteSelectedRegionChangedCommand()
        {
            if (Cities.Any())
            {
                Cities.Clear();
                // no joke, without this task delay it doesn't work
                // bloody cost me till 2am! 
                // http://stackoverflow.com/questions/28199771/universal-app-loading-combobox-itemssource-async-gives-weird-behaviour
                await Task.Delay(100);                                                   
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
            if (Suburbs.Any())
            {
                Suburbs.Clear();
                await Task.Delay(100);
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

        private void Populate<T>(ObservableCollection<T> obv, List<T> collection)
        {
            foreach (var c in collection)
                obv.Add(c);
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var directories = await _db.GetDirectories();
            Directories = new List<Directory>(directories);

            var regions = await _db.GetRegions();
            Regions = new ObservableCollection<Region>(regions);
            SelectedRegion = Regions.First();
            ExecuteSelectedRegionChangedCommand();

            Populate<string>(Genders, directories.GroupBy(x => x.GenderOfStudents.ToLower()).Select(x => x.Key.ToString()).OrderBy(x => x).ToList());
            SelectedGender = Genders.FirstOrDefault();

            Populate<string>(SchoolTypes, directories.GroupBy(x => x.SchoolType.ToLower()).Select(x => x.Key.ToString()).OrderBy(x => x).ToList());
            SelectedSchoolType = SchoolTypes.FirstOrDefault();

            Populate<string>(Deciles, directories.GroupBy(x => x.Decile).Select(x => x.Key.ToString()).OrderBy(x => Int32.Parse(x)).ToList());
            SelectedDecile = Deciles.FirstOrDefault();
        }
    }
}
