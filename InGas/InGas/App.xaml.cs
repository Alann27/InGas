using InGas.Services;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InGas
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer) { }

        protected async override void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync($"{NavigationConstants.NavigationPage}/{NavigationConstants.MainPage}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>(NavigationConstants.NavigationPage);
            containerRegistry.RegisterForNavigation<MainPage>(NavigationConstants.MainPage);

            containerRegistry.RegisterInstance<IDatabaseService>(new DatabaseService());
        }


    }
}
