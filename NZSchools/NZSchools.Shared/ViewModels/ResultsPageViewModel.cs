using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Helpers;
using NZSchools.Interfaces;
using NZSchools.Models;
using NZSchools.Services.NavigationService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Windows.UI.Xaml.Navigation;

namespace NZSchools.ViewModels
{
    public class ResultsPageViewModel : ViewModel, IResultsPageViewModel
    {
        private ObservableCollection<Directory> _directories;
        private IList<AlphaKeyGroup<Directory>> _grouped;

        public ObservableCollection<Directory> Directories
        {
            get { return _directories; }
            set
            {
                _directories = value;
                OnPropertyChanged("Directories");
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

        public ResultsPageViewModel()
        {
            Directories = new ObservableCollection<Directory>();
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            Directories.Clear();
            var directories = (List<Directory>)NavigationParameters.Instance.GetParameters();
            foreach (var d in directories)
                Directories.Add(d);

            Grouped = AlphaKeyGroup<Directory>.CreateGroups(directories, CultureInfo.CurrentUICulture, s => s.Name, true);
        }
    }
}
