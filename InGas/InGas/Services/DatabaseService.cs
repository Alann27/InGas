using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InGas.Models;
using SQLite;
using Xamarin.Essentials;
using System.Linq;

namespace InGas.Services
{
    public class DatabaseService : IDatabaseService
    {
        private static SQLiteAsyncConnection _connection;

        private bool _initialized = false;
        public DatabaseService()
        {

        }

        public async Task Initialize()
        {
            if (!_initialized)
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, /*"InGasDB"*/"testDB");
                _connection = new SQLiteAsyncConnection(dbPath);

                await _connection.CreateTableAsync<Income>();
                await _connection.CreateTableAsync<Expense>();
                await _connection.CreateTableAsync<IncomeType>();
                await _connection.CreateTableAsync<ExpenseType>();

                IncomeType defaultIncomeType = new IncomeType { Name = "Others" };
                ExpenseType defaultExpenseType = new ExpenseType { Name = "Others" };

                if ( await _connection.Table<IncomeType>().FirstOrDefaultAsync(incomeType => incomeType.Name == defaultIncomeType.Name) == null)
                {
                    await _connection.InsertAsync(defaultIncomeType);
                }

                if (await _connection.Table<ExpenseType>().FirstOrDefaultAsync(expenseType => expenseType.Name == defaultExpenseType.Name) == null)
                {
                    await _connection.InsertAsync(defaultExpenseType);
                }

                _initialized = true;
            }
        }

        public async Task<List<Income>> GetIncomesOnDate(DateTime date)
        {
            await Initialize();

            return await _connection.QueryAsync<Income>(
                "SELECT * FROM Income AS I" +
                "INNER JOIN IncomeType AS IT ON I.IdType = IT.Id" +
                "WHERE Convert(DATE,I.Date) = ?", date.Date);
        }

        public async Task<Income> InsertIncome(int idType, string concept, double value, DateTime date)
        {
            if (idType > 0 && !string.IsNullOrEmpty(concept) && value != 0 && date != null )
            {
                Income newIncome = new Income
                {
                    IdType = idType,
                    Concept = concept,
                    Value = value,
                    Date = date
                };

                int result = await _connection.InsertAsync(newIncome);

                if (result != 1)
                {
                    throw new Exception("Insert failed");
                }
                else
                {
                    return newIncome;
                }
            }
            else
            {
                if (idType <= 0)
                    throw new ArgumentException("You have not select a Type for the income", "idType");

                if (string.IsNullOrEmpty(concept))
                    throw new ArgumentException("The concept cannot be empty", "concept");

                if (value == 0)
                    throw new ArgumentException("The value cannot be zero", "value");

                if (date == null)
                    throw new ArgumentException("You need to select the date","date");

                return null;
            }
        }

        public async Task<Expense> InsertExpense(int idType, string concept, double value, DateTime date)
        {
            if (idType > 0 && !string.IsNullOrEmpty(concept) && value != 0 && date != null)
            {
                Expense newExpense = new Expense
                {
                    IdType = idType,
                    Concept = concept,
                    Value = value,
                    Date = date
                };

                int result = await _connection.InsertAsync(newExpense);

                if (result < 1)
                {
                    throw new Exception("Insert failed");
                }
                else
                {
                    return newExpense;
                }
            }
            else
            {
                if (idType <= 0)
                    throw new ArgumentException("You have not select a Type for the income", "idType");

                if (string.IsNullOrEmpty(concept))
                    throw new ArgumentException("The concept cannot be empty", "concept");

                if (value == 0)
                    throw new ArgumentException("The value cannot be zero", "value");

                if (date == null)
                    throw new ArgumentException("You need to select the date", "date");

                return null;
            }
        }

        public async Task<IncomeType> InsertIncomeType(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IncomeType newIncomeType = new IncomeType { Name = name };
                int result = await _connection.InsertAsync(newIncomeType);

                if (result < 1)
                {
                    throw new Exception();
                }
                else
                {
                    return newIncomeType;
                }
            }
            else
            {
                throw new ArgumentException("The name of the type cannot be empty","name");
            }
        }

        public async Task<ExpenseType> InsertExpenseType(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                ExpenseType newExpenseType = new ExpenseType{ Name = name };
                int result = await _connection.InsertAsync(newExpenseType);

                if (result < 1)
                {
                    throw new Exception();
                }
                else
                {
                    return newExpenseType;
                }
            }
            else
            {
                throw new ArgumentException("The name of the type cannot be empty", "name");
            }
        }

        public async Task<Income> GetIncomeById(int id)
        {
            if (id > 0)
            {
                return await _connection.Table<Income>().FirstOrDefaultAsync(income => income.Id == id);
            }
            else
            {
                throw new ArgumentException("The income must have an id higher than 0","id");
            }
        }

        public async Task<Expense> GetExpenseById(int id)
        {
            if (id > 0)
            {
                return await _connection.Table<Expense>().FirstOrDefaultAsync(expense => expense.Id == id);
            }
            else
            {
                throw new ArgumentException("The expense must have an id higher than 0", "id");
            }
        }

        public async Task<IncomeType> GetIncomeTypeById(int id)
        {
            if (id > 0)
            {
                return await _connection.Table<IncomeType>().FirstOrDefaultAsync(incomeType => incomeType.Id == id);
            }
            else
            {
                throw new ArgumentException("The income type must have an id higher than 0", "id");
            }
        }

        public async Task<ExpenseType> GetExpenseTypeById(int id)
        {
            if (id > 0)
            {
                return await _connection.Table<ExpenseType>().FirstOrDefaultAsync(expenseType => expenseType.Id == id);
            }
            else
            {
                throw new ArgumentException("The expense type must have an id higher than 0", "id");
            }
        }

        public async Task<List<Income>> GetAllIncomes()
        {
            return await _connection.Table<Income>().ToListAsync();
        }

        public async Task<List<Expense>> GetAllExpenses()
        {
            return await _connection.Table<Expense>().ToListAsync();
        }

        public async Task<List<IncomeType>> GetAllIncomeTypes()
        {
            return await _connection.Table<IncomeType>().ToListAsync();
        }

        public async Task<List<ExpenseType>> GetAllExpenseTypes()
        {
            return await _connection.Table<ExpenseType>().ToListAsync();
        }

        public async Task<Income> UpdateIncomeById(Income oldIncome, int idType, string concept, double value, DateTime date)
        {
            if (oldIncome != null && idType > 0 && !string.IsNullOrEmpty(concept) && value != 0 && date != null)
            {
                Income newIncome = new Income
                {
                    Id = oldIncome.Id,
                    IdType = idType,
                    Concept = concept,
                    Value = value,
                    Date = date
                };

                int result = await _connection.UpdateAsync(newIncome);

                if (result != 1)
                {
                    throw new Exception("Insert failed");
                }
                else
                {
                    return newIncome;
                }
            }
            else
            {
                if (oldIncome == null)
                    throw new ArgumentException("The income cannot be null", "oldIncome");

                if (idType <= 0)
                    throw new ArgumentException("You have not select a Type for the income", "idType");

                if (string.IsNullOrEmpty(concept))
                    throw new ArgumentException("The concept cannot be empty", "concept");

                if (value == 0)
                    throw new ArgumentException("The value cannot be zero", "value");

                if (date == null)
                    throw new ArgumentException("You need to select the date", "date");

                return null;
            }
        }

        public async Task<Expense> UpdateExpenseById(Expense oldExpense, int idType, string concept, double value, DateTime date)
        {
            if (oldExpense != null && idType > 0 && !string.IsNullOrEmpty(concept) && value != 0 && date != null)
            {
                Expense newExpense = new Expense
                {
                    Id = oldExpense.Id,
                    IdType = idType,
                    Concept = concept,
                    Value = value,
                    Date = date
                };

                int result = await _connection.UpdateAsync(newExpense);

                if (result != 1)
                {
                    throw new Exception("Insert failed");
                }
                else
                {
                    return newExpense;
                }
            }
            else
            {
                if (oldExpense == null)
                    throw new ArgumentException("The expense cannot be null", "oldIncome");

                if (idType <= 0)
                    throw new ArgumentException("You have not select a Type for the income", "idType");

                if (string.IsNullOrEmpty(concept))
                    throw new ArgumentException("The concept cannot be empty", "concept");

                if (value == 0)
                    throw new ArgumentException("The value cannot be zero", "value");

                if (date == null)
                    throw new ArgumentException("You need to select the date", "date");

                return null;
            }
        }

        public async Task<List<Income>> GetIncomesBetweenDates(DateTime startDate, DateTime finalDate)
        {
            var incomes = await _connection.Table<Income>().ToListAsync();

            return new List<Income>(incomes.Where(income => income.Date >= startDate && income.Date <= finalDate));
        }

        public async Task<List<Expense>> GetExpensesBetweenDates(DateTime startDate, DateTime finalDate)
        {
            var expenses = await _connection.Table<Expense>().ToListAsync();

            return new List<Expense>(expenses.Where(expense => expense.Date >= startDate && expense.Date <= finalDate));
        }
    }
}
