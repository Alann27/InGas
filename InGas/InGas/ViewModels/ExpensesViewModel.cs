using InGas.Models;
using InGas.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace InGas.ViewModels
{
    public class ExpensesViewModel : BaseViewModel, INavigatedAware
    {
        public ExpensesViewModel(INavigationService navigationService, IDialogService dialogService, IDatabaseService databaseService) : base(navigationService, dialogService, databaseService)
        {
            ShowAddExpenseCommand = new DelegateCommand(OnShowAddExpense);
            AddExpenseTypeCommand = new DelegateCommand(OnAddExpenseType);

            GetExpenses();
        }

        public ObservableCollection<Expense> Expenses { get; set; }
        private int _expensesCount;
        public DelegateCommand ShowAddExpenseCommand { get; set; }
        public DelegateCommand AddExpenseTypeCommand { get; set; }

        private async void OnShowAddExpense()
        {
            var parameters = new NavigationParameters();
            parameters.Add(ParametersConstants.ExpenseList, Expenses);

            await NavigationService.NavigateAsync(NavigationConstants.AddExpensePage, parameters);
        }
        private async void OnAddExpenseType()
        {
            try
            {
                string name = await DialogService.Prompt("Add expense type", "Introduce the name of the new expense type", StringConstants.Ok, "cancel", "Name");

                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (await DatabaseService.GetExpenseTypeByName(name) == null)
                    {
                        ExpenseType newExpenseType = await DatabaseService.InsertExpenseType(name);

                        if (newExpenseType != null)
                        {
                            await DialogService.DisplayAlert("Add expense type", $"{newExpenseType.Name} add as an expense type", StringConstants.Ok);
                        }
                        else
                        {

                        }

                    }
                    else
                    {
                        await DialogService.DisplayAlert(StringConstants.Error, $"There's already an expense type called: {name}", StringConstants.Ok);
                    }
                }

            }
            catch (Exception ex)
            {
                await DialogService.DisplayAlert(StringConstants.Error, $"There was an error trying to insert the expense type. {StringConstants.Error}: {ex.Message}", StringConstants.Ok);
            }

        }
        private async Task GetExpenses()
        {
            try
            {
                var ExpenseList = await DatabaseService.GetAllExpenses();
                Expenses = new ObservableCollection<Expense>(ExpenseList.OrderByDescending(expense => expense.Date));
                _expensesCount = Expenses.Count;
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
                if (Expenses != null && _expensesCount != Expenses.Count)
                {
                    Expenses = new ObservableCollection<Expense>(Expenses.OrderByDescending(expense => expense.Date));
                    _expensesCount = Expenses.Count;
                }
            }
        }
    }
}
