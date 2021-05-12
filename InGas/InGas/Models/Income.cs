using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace InGas.Models
{
    [Table("Income")]
    public class Income
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(IncomeType))]
        public int TypeId { get; set; }
        [NotNull, MaxLength(100)]
        public string Concept { get; set; }
        [NotNull]
        public double Value { get; set; }
        [NotNull]
        public DateTime Date { get; set; }

        [ManyToOne]
        public IncomeType IncomeType { get; set; }
    }
}
