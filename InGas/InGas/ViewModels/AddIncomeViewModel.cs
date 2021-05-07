using InGas.Models;
using InGas.Services;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InGas.ViewModels
{
    public class AddIncomeViewModel : BaseViewModel, IInitialize
    {
        public AddIncomeViewModel(INavigationService navigationService, IPageDialogService dialogService, IDatabaseService databaseService) : base(navigationService, dialogService, databaseService)
        {
        }

        public string Concept { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }

        public List<IncomeType> Types { get; set; }
        public IncomeType IncomeTypeSelected { get; set; }

        private List<Income> _incomes;

        public async Task GetIncomesTypes()
        {
            try
            {
                Types = new List<IncomeType>(await DatabaseService.GetAllIncomeTypes());
            }
            catch (ArgumentException ex)
            {
                await DialogService.DisplayAlertAsync("Error", $"Cannot get all the income types. Error: {ex.Message}", "Ok");
            }
            catch (Exception ex)
            {
                await DialogService.DisplayAlertAsync("Error", ex.Message, "Ok");
            }
        }

        public async Task OnInsertIncome()
        {
            try
            {
                double value = Math.Round(Convert.ToDouble(Value));

                Income newIncome = await DatabaseService.InsertIncome(IncomeTypeSelected.Id, Concept, value, Date);

                _incomes.Add(newIncome);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParametersConstants.IncomeList, out List<Income> incomes))
            {
                _incomes = incomes;
            }
        }
    }
}
