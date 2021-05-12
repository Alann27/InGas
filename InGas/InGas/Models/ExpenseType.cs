using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace InGas.Models
{
    [Table("ExpenseType")]
    public class ExpenseType
    {
        [PrimaryKey, AutoIncrement]
        public int TypeId { get; set; }
        [NotNull, MaxLength(30), Unique]
        public string Name { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Expense> Expenses { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
