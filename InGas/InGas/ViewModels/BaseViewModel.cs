using InGas.Services;
using Prism.Navigation;
using Prism.Services;
using System.ComponentModel;


namespace InGas.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected IDatabaseService DatabaseService { get; }
        protected INavigationService NavigationService { get; }
        protected IDialogService DialogService { get; }
        protected BaseViewModel(INavigationService navigationService, IDialogService dialogService, IDatabaseService databaseService)
        {
            NavigationService = navigationService;
            DialogService = dialogService;
            DatabaseService = databaseService;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
