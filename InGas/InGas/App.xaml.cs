using InGas.Services;
using InGas.ViewModels;
using InGas.Views;
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

            await NavigationService.NavigateAsync($"{NavigationConstants.MasterDetailPage}/{NavigationConstants.NavigationPage}/{NavigationConstants.DetailPage}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainPage>(NavigationConstants.MainPage);

            containerRegistry.RegisterForNavigation<NavigationPage>(NavigationConstants.NavigationPage);

            containerRegistry.RegisterForNavigation<MasterPage>(NavigationConstants.MasterPage);
            containerRegistry.RegisterForNavigation<MyMasterDetailPage, MasterViewModel>(NavigationConstants.MasterDetailPage);
            containerRegistry.RegisterForNavigation<DetailPage>(NavigationConstants.DetailPage);

            containerRegistry.RegisterForNavigation<AddIncomePage, AddIncomeViewModel>(NavigationConstants.AddIncomePage);
            containerRegistry.RegisterForNavigation<IncomesPage, IncomesViewModel>(NavigationConstants.IncomesPage);

            containerRegistry.RegisterForNavigation<ExpensesPage, ExpensesViewModel>(NavigationConstants.ExpensesPage);
            containerRegistry.RegisterForNavigation<AddExpensePage, AddExpenseViewModel>(NavigationConstants.AddExpensePage);

            containerRegistry.RegisterInstance<IDatabaseService>(new DatabaseService());
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
        }


    }
}
