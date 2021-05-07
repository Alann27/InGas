using InGas.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InGas.Services
{
    public interface IDatabaseService
    {
        Task<Income> InsertIncome(int idType, string concept, double value, DateTime date);
        Task<Expense> InsertExpense(int idType, string concept, double value, DateTime date);
        Task<IncomeType> InsertIncomeType(string name);
        Task<ExpenseType> InsertExpenseType(string name);

        Task<Income> GetIncomeById(int id);
        Task<Expense> GetExpenseById(int id);
        Task<IncomeType> GetIncomeTypeById(int id);
        Task<ExpenseType> GetExpenseTypeById(int id);

        Task<List<Income>> GetAllIncomes();
        Task<List<Expense>> GetAllExpenses();
        Task<List<IncomeType>> GetAllIncomeTypes();
        Task<List<ExpenseType>> GetAllExpenseTypes();

        Task<Income> UpdateIncomeById(Income oldIncome, int idType, string concept, double value, DateTime date);
        Task<Expense> UpdateExpenseById(Expense oldExpense,int idType, string concept, double value, DateTime date);


        Task<List<Income>> GetIncomesBetweenDates(DateTime startDate, DateTime finalDate);
        Task<List<Expense>> GetExpensesBetweenDates(DateTime startDate, DateTime finalDate);


    }
}
