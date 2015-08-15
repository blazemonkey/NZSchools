using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Interfaces;
using NZSchools.Models;
using NZSchools.Services.SqlLiteService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.UI.Xaml.Navigation;

namespace NZSchools.ViewModels
{
    public class SettingsPageViewModel : ViewModel, ISettingsPageViewModel
    {
        private ISqlLiteService _db;

        private ObservableCollection<Region> _regions;
        private Region _selectedRegion;

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
            }
        }

        public SettingsPageViewModel(ISqlLiteService db)
        {
            _db = db;

            Regions = new ObservableCollection<Region>();
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var regions = await _db.GetRegions();
            foreach (var r in regions)
            {
                Regions.Add(r);
            }
        }
    }
}
