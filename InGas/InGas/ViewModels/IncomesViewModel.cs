using InGas.Models;
using InGas.Services;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Prism.Commands;

namespace InGas.ViewModels
{
    public class IncomesViewModel : BaseViewModel, INavigatedAware
    {
        public IncomesViewModel(INavigationService navigationService, IDialogService dialogService, IDatabaseService databaseService) : base(navigationService, dialogService, databaseService)
        {
            ShowAddIncomeCommand = new DelegateCommand(OnShowAddIncome);
            AddIncomeTypeCommand = new DelegateCommand(OnAddIncomeType);

            GetIncomes();
        }

        public ObservableCollection<Income> Incomes { get; set; }
        private int _incomesCount;
        public DelegateCommand ShowAddIncomeCommand { get; set; }
        public DelegateCommand AddIncomeTypeCommand { get; set; }

        private async void OnShowAddIncome()
        {
            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.IncomeList, Incomes);

            await NavigationService.NavigateAsync(NavigationConstants.AddIncomePage, parameters);
        }
        private async void OnAddIncomeType()
        {
            try
            {
                string name = await DialogService.Prompt("Add income type", "Introduce the name of the new income type", StringConstants.Ok, "cancel", "Name");

                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (await DatabaseService.GetIncomeTypeByName(name) == null)
                    {
                        IncomeType newIncomeType = await DatabaseService.InsertIncomeType(name);

                        if (newIncomeType != null)
                        {
                            await DialogService.DisplayAlert("Add income type", $"{newIncomeType.Name} add as an income type", StringConstants.Ok);
                        }
                        else
                        {

                        }

                    }
                    else
                    {
                        await DialogService.DisplayAlert(StringConstants.Error, $"There's already an income type with the name: {name}", StringConstants.Ok);
                    }
                }

            }
            catch (Exception ex)
            {
                await DialogService.DisplayAlert(StringConstants.Error, $"There was an error trying to insert the income type. {StringConstants.Error}: {ex.Message}", StringConstants.Ok);
            }

        }
        private async Task GetIncomes()
        {
            try
            {
                var incomeList = await DatabaseService.GetAllIncomes();
                Incomes = new ObservableCollection<Income>(incomeList.OrderByDescending(income => income.Date));
                _incomesCount = Incomes.Count;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.Back)
            {
                if (Incomes != null && _incomesCount != Incomes.Count)
                {
                    Incomes = new ObservableCollection<Income>(Incomes.OrderByDescending(income => income.Date));
                }
            }
        }
    }
}
