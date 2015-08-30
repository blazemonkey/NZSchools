using Microsoft.Practices.Prism.Commands;
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
        private INavigationService _nav;

        private bool _isLoading;
        private bool _hasDirectories;

        private ObservableCollection<Directory> _directories;
        private IList<AlphaKeyGroup<Directory>> _grouped;

        private bool _isMapLocked;

        public bool IsLoading
        {
            get { return _isLoading; }
            private set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        public bool HasDirectories
        {
            get { return _hasDirectories; }
            set
            {
                _hasDirectories = value;
                OnPropertyChanged("HasDirectories");
            }
        }

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

        public DelegateCommand TapLockMapCommand { get; set; }
        public DelegateCommand<Directory> TapSchoolCommand { get; set; }

        public ResultsPageViewModel(INavigationService nav)
        {
            _nav = nav;

            Directories = new ObservableCollection<Directory>();
            IsLoading = true;
            IsMapLocked = true;

            TapLockMapCommand = new DelegateCommand(ExecuteTapLockMapCommand);
            TapSchoolCommand = new DelegateCommand<Directory>(ExecuteTapSchoolCommand);
        }

        private void ExecuteTapLockMapCommand()
        {
            IsMapLocked = !IsMapLocked;
        }

        public void ExecuteTapSchoolCommand(Directory directory)
        {
            NavigationParameters.Instance.SetParameters(directory);
            _nav.Navigate(Experiences.School);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (navigationMode == NavigationMode.Back)
                return;

            Directories.Clear();
            var directories = (List<Directory>)NavigationParameters.Instance.GetParameters();
            foreach (var d in directories)
                Directories.Add(d);
            HasDirectories = directories.Count > 0;

            Grouped = AlphaKeyGroup<Directory>.CreateGroups(directories, CultureInfo.CurrentUICulture, s => s.Name, true);
        }
    }
}
