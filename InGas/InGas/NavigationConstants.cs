using System;
using System.Collections.Generic;
using System.Text;

namespace InGas
{
    public static class NavigationConstants
    {
        public const string MainPage = "Main";
        public const string MasterDetailPage = "MasterDetail";
        public const string MasterPage = "MasterPage";
        public const string NavigationPage = "Nav";
        public const string DetailPage = "Detail";
        public const string AddIncomePage = "AddIncome";
        public const string IncomesPage = "Incomes";
        public const string ExpensesPage = "Expenses";
        public const string AddExpensePage = "AddExpense";
        public const string PreferencesPage = "Preferences";

        public static List<Page> Pages = new List<Page>
        {
            new Page
            {
                Name = "Home",
                NavigationUri = DetailPage
            },
            new Page
            {
                Name = "Incomes",
                NavigationUri = IncomesPage
            },
            new Page
            {
                Name = "Expenses",
                NavigationUri = ExpensesPage
            },
            new Page
            {
                Name = "Preferences",
                NavigationUri = PreferencesPage
            }
        };
    }

    public static class ParametersConstants
    {
        public const string IncomeList = "incomes";
        public const string ExpenseList = "expenses";
    }

    public class Page
    {
        public string Name { get; set; }
        public string NavigationUri { get; set; }
    }
}
