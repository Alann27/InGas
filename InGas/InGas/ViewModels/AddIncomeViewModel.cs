using InGas.Models;
using InGas.Services;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InGas.ViewModels
{
    public class AddIncomeViewModel : BaseViewModel, IInitialize
    {
        public AddIncomeViewModel(INavigationService navigationService, IDialogService dialogService, IDatabaseService databaseService) : base(navigationService, dialogService, databaseService)
        {
            GetIncomesTypes();

            AddIncomeCommand = new Xamarin.Forms.Command(OnInsertIncome);
            ClearCommand = new Xamarin.Forms.Command(OnClear);
        }

        public string Concept { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public List<IncomeType> Types { get; set; }
        public IncomeType IncomeTypeSelected { get; set; }

        public ICommand AddIncomeCommand { get; set; }
        public ICommand ClearCommand { get; set; }

        private ObservableCollection<Income> _incomes = new ObservableCollection<Income>();

        public async Task GetIncomesTypes()
        {
            try
            {
                Types = new List<IncomeType>(await DatabaseService.GetAllIncomeTypes());
                IncomeTypeSelected = Types.FirstOrDefault();
            }
            catch (ArgumentException ex)
            {
                await DialogService.DisplayAlert(StringConstants.Error, $"Cannot get all the income types. {StringConstants.Error}: {ex.Message}", StringConstants.Ok);
            }
            catch (Exception ex)
            {
                await DialogService.DisplayAlert(StringConstants.Error, ex.Message, StringConstants.Ok);
            }
        }

        public async void OnInsertIncome()
        {
            try
            {
                double value = Math.Round(Convert.ToDouble(Value),2);

                Income newIncome = await DatabaseService.InsertIncome(IncomeTypeSelected.TypeId, Concept, value, Date);

                _incomes.Add(newIncome);

                await DialogService.DisplayAlert("Add income", "Income added succesfully", StringConstants.Ok);

                OnClear();
            }
            catch (ArgumentException ex)
            {
                await DialogService.DisplayAlert(StringConstants.Error, $"{StringConstants.AddIncomeErrorMsg} {StringConstants.Error}: {ex.Message}", StringConstants.Ok);
            }

        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.IncomeList, out ObservableCollection<Income> incomes))
            {
                _incomes = incomes;
            }
        }

        public void OnClear()
        {
            Concept = string.Empty;
            Value = string.Empty;
            Date = DateTime.Now;
            IncomeTypeSelected = Types.FirstOrDefault();
        }
    }
}
