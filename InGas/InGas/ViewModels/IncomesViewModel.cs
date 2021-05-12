using InGas.Services;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace InGas.ViewModels
{
    public class IncomesViewModel : BaseViewModel
    {
        public IncomesViewModel(INavigationService navigationService, IDialogService dialogService, IDatabaseService databaseService) : base(navigationService, dialogService, databaseService)
        {
            ShowAddIncomeCommand = new Xamarin.Forms.Command(OnShowAddIncome);
            AddIncomeTypeCommand = new Xamarin.Forms.Command(OnAddIncomeType);
        }

        public ICommand ShowAddIncomeCommand { get; set; }
        public ICommand AddIncomeTypeCommand { get; set; }

        private async void OnShowAddIncome()
        {
            await NavigationService.NavigateAsync(NavigationConstants.AddIncomePage);
        }
        private async void OnAddIncomeType()
        {
            string name = await DialogService.Prompt("Add income type", "Introduce the name of the new income type", "Ok", "cancel", "Name");
        }
    }
}
