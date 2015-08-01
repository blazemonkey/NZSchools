using System;
using System.Collections.Generic;
using System.Text;

namespace NZSchools.Services.NavigationService
{
    public class NavigationService : INavigationService
    {
        public Microsoft.Practices.Prism.Mvvm.Interfaces.INavigationService _navigationService { get; private set; }

        public NavigationService(Microsoft.Practices.Prism.Mvvm.Interfaces.INavigationService navigationService)
        {
            this._navigationService = navigationService;
        }

        public bool Navigate(Experiences experience, object param = null)
        {
            return _navigationService.Navigate(experience.ToString(), param);
        }

        public void ClearHistory()
        {
            _navigationService.ClearHistory();
        }

        public bool CanGoBack { get { return _navigationService.CanGoBack(); } }

        public void GoBack()
        {
            if (_navigationService.CanGoBack())
                _navigationService.GoBack();
        }

    }
}
