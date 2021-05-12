using FakeItEasy;
using FluentAssertions;
using InGas.Models;
using InGas.Services;
using InGas.UnitTest.Fixture;
using System;
using Xunit;

namespace InGas.UnitTest.DatabaseTest
{
    public sealed class DatabaseTest
    {
        private IDatabaseService _databaseService;

        public DatabaseTest()
        {
            _databaseService = A.Fake<IDatabaseService>();
        }

        [Fact]
        public async void WhenGetAllIncomes_GivenIsExecuted_ThenShouldGetAllIncomes()
        {
            // Arrange

           IDatabaseService databaseService = A.Fake<IDatabaseService>();

            // Act

            A.CallTo(() => databaseService.GetAllIncomes());

            await databaseService.GetAllIncomes();

            // Assert

            A.CallTo(() => databaseService.GetAllIncomes()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void WhenInsertIncome_GivenIsExecuted_ThenShouldRetrieveTheIncome()
        {
            // Arrange



            Income incomeExpected = (Income)new IncomeFixture();

            // Act

            var actualIncome = await _databaseService.InsertIncome(1, incomeExpected.Concept, incomeExpected.Value, incomeExpected.Date);

            // Assert

            A.CallTo(() => _databaseService.InsertIncome(1, incomeExpected.Concept, incomeExpected.Value, incomeExpected.Date)).MustHaveHappenedOnceExactly();

            incomeExpected.Concept.Should().Be(actualIncome.Concept);
            incomeExpected.Value.Should().Be(actualIncome.Value);
            incomeExpected.Date.Should().Be(actualIncome.Date);
        }
    }
}
