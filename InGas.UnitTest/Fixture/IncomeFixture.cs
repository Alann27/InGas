using InGas.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InGas.UnitTest.Fixture
{
    public class IncomeFixture : IBuilder
    {
        public static implicit operator Income( IncomeFixture fixture ) => fixture.Build();

        private Income Build() => new Income
        {
            Concept = concept,
            Value = value,
            Date = date,
            TypeId = 1
        };

        private string concept = "Buy a PS5";
        private double value = 750.556;
        private DateTime date = DateTime.Now;
    }
}
