﻿using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace InGas.Models
{
    public class ExpenseType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(30)]
        public string Name { get; set; }

        [OneToMany]
        public List<Expense> Expenses { get; set; }
    }
}
