using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace InGas.Models
{
    [Table("Expense")]
    public class Expense
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(ExpenseType))]
        public int TypeId { get; set; }
        
        [NotNull, MaxLength(100)]
        public string Concept { get; set; }
        [NotNull]
        public double Value { get; set; }
        [NotNull]
        public DateTime Date { get; set; }

        [ManyToOne]
        public ExpenseType ExpenseType { get; set; }
    }
}
