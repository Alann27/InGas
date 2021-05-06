using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace InGas.Models
{
    public class Income
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(IncomeType))]
        public int IdType { get; set; }
        [NotNull, MaxLength(100)]
        public string Concept { get; set; }
        [NotNull]
        public double Value { get; set; }
        [NotNull]
        public DateTime Date { get; set; }

        [ManyToOne]
        public IncomeType Type { get; set; }

    }
}
