using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InGas.Models;
using SQLite;
using Xamarin.Essentials;
using System.Linq;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace InGas.Services
{
    public class DatabaseService : IDatabaseService
    {
        private static SQLiteAsyncConnection _connection;

        private bool _initialized = false;
        public DatabaseService()
        {
            Initialize();
            
        }

        public async void Initialize()
        {
            if (!_initialized)
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, /*"InGasDB"*/"testsDB");
                _connection = new SQLiteAsyncConnection(dbPath);

                await _connection.CreateTableAsync<IncomeType>();
                await _connection.CreateTableAsync<ExpenseType>();
                await _connection.CreateTableAsync<Income>();
                await _connection.CreateTableAsync<Expense>();

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
            //Initialize();

            return await _connection.QueryAsync<Income>(
                "SELECT * FROM Income AS I" +
                "INNER JOIN IncomeType AS IT ON I.IdType = IT.Id" +
                "WHERE Convert(DATE,I.Date) = ?", date.Date);
        }

        public async Task<Income> InsertIncome(int typeId, string concept, double value, DateTime date)
        {
            if (typeId > 0 && !string.IsNullOrEmpty(concept) && value != 0 && date != null )
            {
                Income newIncome = new Income
                {
                    TypeId = typeId,
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
                if (typeId <= 0)
                    throw new ArgumentException("You have not select a Type for the income", "typeId");

                if (string.IsNullOrEmpty(concept))
                    throw new ArgumentException("The concept cannot be empty", "concept");

                if (value == 0)
                    throw new ArgumentException("The value cannot be zero", "value");

                if (date == null)
                    throw new ArgumentException("You need to select the date","date");

                return null;
            }
        }

        public async Task<Expense> InsertExpense(int typeId, string concept, double value, DateTime date)
        {
            if (typeId > 0 && !string.IsNullOrEmpty(concept) && value != 0 && date != null)
            {
                Expense newExpense = new Expense
                {
                    TypeId = typeId,
                    Concept = concept,
                    Value = value,
                    Date = date
                };

                int result = await _connection.InsertAsync(newExpense);

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
                if (typeId <= 0)
                    throw new ArgumentException("You have not select a Type for the income", "typeId");

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

                if (result != 1)
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

                if (result != 1)
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
               return await _connection.GetWithChildrenAsync<Income>(id);
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
                return await _connection.GetWithChildrenAsync<Expense>(id);
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
                return await _connection.GetAsync<IncomeType>(id);
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
                return await _connection.GetAsync<ExpenseType>(id);
            }
            else
            {
                throw new ArgumentException("The expense type must have an id higher than 0", "id");
            }
        }

        public async Task<List<Income>> GetAllIncomes()
        {
            return await _connection.GetAllWithChildrenAsync<Income>();
        }

        public async Task<List<Expense>> GetAllExpenses()
        {
            return await _connection.GetAllWithChildrenAsync<Expense>();
        }

        public async Task<List<IncomeType>> GetAllIncomeTypes()
        {
            return await _connection.Table<IncomeType>().ToListAsync();
        }

        public async Task<List<ExpenseType>> GetAllExpenseTypes()
        {
            return await _connection.Table<ExpenseType>().ToListAsync();
        }

        public async Task<Income> UpdateIncomeById(Income oldIncome, int typeId, string concept, double value, DateTime date)
        {
            if (oldIncome != null && typeId > 0 && !string.IsNullOrEmpty(concept) && value != 0 && date != null)
            {
                Income newIncome = new Income
                {
                    Id = oldIncome.Id,
                    TypeId = typeId,
                    Concept = concept,
                    Value = value,
                    Date = date
                };

                int result = await _connection.UpdateAsync(newIncome);

                if (result != 1)
                {
                    throw new Exception("Update failed");
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

                if (typeId <= 0)
                    throw new ArgumentException("You have not select a Type for the income", "typeId");

                if (string.IsNullOrEmpty(concept))
                    throw new ArgumentException("The concept cannot be empty", "concept");

                if (value == 0)
                    throw new ArgumentException("The value cannot be zero", "value");

                if (date == null)
                    throw new ArgumentException("You need to select the date", "date");

                return null;
            }
        }

        public async Task<Expense> UpdateExpenseById(Expense oldExpense, int typeId, string concept, double value, DateTime date)
        {
            if (oldExpense != null && typeId > 0 && !string.IsNullOrEmpty(concept) && value != 0 && date != null)
            {
                Expense newExpense = new Expense
                {
                    Id = oldExpense.Id,
                    TypeId = typeId,
                    Concept = concept,
                    Value = value,
                    Date = date
                };

                int result = await _connection.UpdateAsync(newExpense);

                if (result != 1)
                {
                    throw new Exception("Update failed");
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

                if (typeId <= 0)
                    throw new ArgumentException("You have not select a Type for the income", "typeId");

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
            return await _connection.GetAllWithChildrenAsync<Income>(income => income.Date >= startDate && income.Date <= finalDate);
        }

        public async Task<List<Expense>> GetExpensesBetweenDates(DateTime startDate, DateTime finalDate)
        {
            return await _connection.GetAllWithChildrenAsync<Expense>(expense => expense.Date >= startDate && expense.Date <= finalDate);
        }
    }
}
