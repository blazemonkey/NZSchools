using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Interfaces;
using NZSchools.Models;
using NZSchools.Services.SqlLiteService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Navigation;

namespace NZSchools.ViewModels
{
    public class MainPageViewModel : ViewModel, IMainPageViewModel
    {
        private ISqlLiteService _db;

        private ObservableCollection<string> _cities;
        private ObservableCollection<Region> _regions;

        private string _selectedRegion;

        public ObservableCollection<string> Cities
        {
            get { return _cities; }
            private set
            {
                _cities = value;
                OnPropertyChanged("Cities");
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

        public string SelectedRegion
        {
            get { return _selectedRegion; }
            set
            {
                _selectedRegion = value;
                OnPropertyChanged("SelectedRegion");
            }
        }

        public MainPageViewModel(ISqlLiteService db)
        {
            _db = db;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var directories = await _db.GetDirectories();
            var regions = await _db.GetRegions();
            Cities = new ObservableCollection<string>(directories.GroupBy(x => x.City).Select(x => x.Key));
            Regions = new ObservableCollection<Region>(regions);
        }
    }
}
