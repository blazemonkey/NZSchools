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
    public class ComparePageViewModel : ViewModel, IComparePageViewModel
    {        
        private ISqlLiteService _sql;

        private ObservableCollection<Directory> _favourites;
        private Directory _selectedSchool1;
        private Directory _selectedSchool2;

        public ObservableCollection<Directory> Favourites
        {
            get { return _favourites; }
            set
            {
                _favourites = value;
                OnPropertyChanged("Favourites");

                if (Favourites.Any())
                    return;

                Favourites.Add(new Directory { Id = 0, Name = "no directory to select" });
            }
        }

        public Directory SelectedSchool1
        {
            get { return _selectedSchool1; }
            set
            {
                _selectedSchool1 = value;
                OnPropertyChanged("SelectedSchool1");
            }
        }

        public Directory SelectedSchool2
        {
            get { return _selectedSchool2; }
            set
            {
                _selectedSchool2 = value;
                OnPropertyChanged("SelectedSchool2");
            }
        }

        public ComparePageViewModel(ISqlLiteService sql)
        {
            _sql = sql;        
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var favourites = await _sql.GetFavourites();            
            Favourites = new ObservableCollection<Directory>(favourites.OrderBy(x => x.Name));

            if (!(Favourites.Any()))
                return;

            await Task.Delay(50);
            if (Favourites.Count() == 1)
            {
                SelectedSchool1 = Favourites.First();
                SelectedSchool2 = Favourites.First();
            }
            else
            {
                SelectedSchool1 = Favourites.First();
                SelectedSchool2 = Favourites[1];
            }            
        }
    }
}
