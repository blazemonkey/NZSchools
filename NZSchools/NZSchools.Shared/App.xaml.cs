using Microsoft.Practices.Prism.Mvvm;
using NZSchools.Interfaces;
using NZSchools.Services.AppDataService;
using NZSchools.Services.FileReaderService;
using NZSchools.Services.JsonService;
using NZSchools.Services.NavigationService;
using NZSchools.Services.SqlLiteService;
using NZSchools.ViewModels;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace NZSchools
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : MvvmAppBase
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>

        public static readonly Container Container = new Container();

        public App()
        {
            this.InitializeComponent();
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterSingle(NavigationService);
            Container.Register<IAppDataService, AppDataService>();
            Container.Register<IFileReaderService, FileReaderService>();
            Container.Register<IJsonService, JsonService>();
            Container.Register<INavigationService, NavigationService>();
            Container.Register<ISqlLiteService, SqlLiteService>();
            Container.RegisterSingle<ISettingsPageViewModel, SettingsPageViewModel>();

            await Container.GetInstance<SqlLiteService>().ClearLocalDb();
            Container.GetInstance<AppDataService>().InitializeAppDataContainer();
        }

        protected override object Resolve(Type type)
        {
            return Container.GetInstance(type);
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs e)
        {
            NavigationService.Navigate(Experiences.Main.ToString(), null);
            return Task.FromResult<object>(null);
        }
    }
}