using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Interfaces;
using NZSchools.Models;
using NZSchools.Services.NavigationService;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Navigation;

namespace NZSchools.ViewModels
{
    public class SchoolPageViewModel : ViewModel, ISchoolPageViewModel
    {
        private Directory _directory;

        public Directory Directory
        {
            get { return _directory; }
            set
            {
                _directory = value;
                OnPropertyChanged("Directory");
            }
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            Directory = (Directory)NavigationParameters.Instance.GetParameters();
        }
    }
}
