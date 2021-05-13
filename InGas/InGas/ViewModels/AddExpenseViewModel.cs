using InGas.Models;
using InGas.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace InGas.ViewModels
{
    public class AddExpenseViewModel : BaseViewModel, IInitialize
    {
        public AddExpenseViewModel(INavigationService navigationService, IDialogService dialogService, IDatabaseService databaseService) : base(navigationService, dialogService, databaseService)
        {
            AddExpenseCommand = new DelegateCommand(OnInsertExpense);
            ClearCommand = new DelegateCommand(OnClear);

            GetExpensesTypes();
        }

        public string Concept { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public List<ExpenseType> Types { get; set; }
        public ExpenseType ExpenseTypeSelected { get; set; }

        public DelegateCommand AddExpenseCommand { get; set; }
        public DelegateCommand ClearCommand { get; set; }

        private ObservableCollection<Expense> _expenses = new ObservableCollection<Expense>();

        public async Task GetExpensesTypes()
        {
            try
            {
                Types = new List<ExpenseType>(await DatabaseService.GetAllExpenseTypes());
                ExpenseTypeSelected = Types.FirstOrDefault();
            }
            catch (ArgumentException ex)
            {
                await DialogService.DisplayAlert(StringConstants.Error, $"Cannot get all the expense types. {StringConstants.Error}: {ex.Message}", StringConstants.Ok);
            }
            catch (Exception ex)
            {
                await DialogService.DisplayAlert(StringConstants.Error, ex.Message, StringConstants.Ok);
            }
        }

        public async void OnInsertExpense()
        {
            try
            {
                double value = Math.Round(Convert.ToDouble(Value), 2);

                Expense newExpense = await DatabaseService.InsertExpense(ExpenseTypeSelected.TypeId, Concept, value, Date);
                newExpense.ExpenseType = ExpenseTypeSelected;

                _expenses.Add(newExpense);

                await DialogService.DisplayAlert("Add expense", "Expense added succesfully", StringConstants.Ok);

                OnClear();
            }
            catch (ArgumentException ex)
            {
                await DialogService.DisplayAlert(StringConstants.Error, $"{StringConstants.AddExpenseErrorMsg} {StringConstants.Error}: {ex.Message}", StringConstants.Ok);
            }

        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.ExpenseList, out ObservableCollection<Expense> Expenses))
            {
                _expenses = Expenses;
            }
        }

        public void OnClear()
        {
            Concept = string.Empty;
            Value = string.Empty;
            Date = DateTime.Now;
            ExpenseTypeSelected = Types.FirstOrDefault();
        }
    }
}
