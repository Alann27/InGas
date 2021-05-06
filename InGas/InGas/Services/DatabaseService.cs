using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InGas.Models;
using SQLite;
using Xamarin.Essentials;

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
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "InGasDB");
                _connection = new SQLiteAsyncConnection(dbPath);

                await _connection.CreateTableAsync<Income>();
                await _connection.CreateTableAsync<Expense>();
                await _connection.CreateTableAsync<IncomeType>();
                await _connection.CreateTableAsync<ExpenseType>();

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
    }
}
