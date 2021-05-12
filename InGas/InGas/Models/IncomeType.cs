using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace InGas.Models
{
    [Table("IncomeType")]
    public class IncomeType
    {
        [PrimaryKey, AutoIncrement]
        public int TypeId { get; set; }
        [NotNull, MaxLength(30), Unique]
        public string Name { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Income> Incomes { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
