using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EtCoreApi.Controllers;
using EtCoreApi.Dtos;
using EtCoreApi.Entities;
using EtCoreApi.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EtCoreUT
{
    public class ExpenseControllerTests
    {
        private readonly Mock<IExpensesRepository> repositoryStub = new();
        private readonly Mock<ILogger<ExpenseController>> loggerStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetExpenseAsync_WithUnexistingExpense_ReturnsNotFound()
        {
            //Arrange
            repositoryStub.Setup(repo => repo.GetExpenseAsync(It.IsAny<Guid>())).ReturnsAsync((Expense)null);

            var controller = new ExpenseController(repositoryStub.Object, loggerStub.Object);

            //Act
            var expense = await controller.GetExpenseAsync(Guid.NewGuid());

            //Assert
            //Assert.IsType<NotFoundResult>(expense.Result);
            expense.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetExpenseAsync_WithExistingExpense_ReturnsExpectedExpense()
        {
            //Arrange
            var expectedExpense = CreateRandomExpense();

            repositoryStub.Setup(repo => repo.GetExpenseAsync(It.IsAny<Guid>())).ReturnsAsync(expectedExpense);

            var controller = new ExpenseController(repositoryStub.Object, loggerStub.Object);

            //Act
            var expense = await controller.GetExpenseAsync(Guid.NewGuid());

            //Assert
            ((Microsoft.AspNetCore.Mvc.ObjectResult)expense.Result).Value.Should().BeEquivalentTo(expectedExpense, options => options.ComparingByMembers<Expense>());
        }

        [Fact]
        public async Task GetExpensesAsync_WithMatchingExpenses_ReturnsMatchingExpenses()
        {
            //Arrange
            var allExpenses = new[] { CreateRandomExpense(), CreateRandomExpense(), CreateRandomExpense() };

            var nameToMatch = "random";

            repositoryStub.Setup(repo => repo.GetExpensesAsync()).ReturnsAsync(allExpenses);

            var controller = new ExpenseController(repositoryStub.Object, loggerStub.Object);

            //Act
            IEnumerable<ExpenseDto> foundExpenses = await controller.GetExpensesAsync(nameToMatch);

            //Assert
            foundExpenses.Should().OnlyContain(expense => expense.ExpenseDetails == allExpenses[0].ExpenseDetails);
        }

        [Fact]
        public async Task GetExpensesAsync_WithExistingExpenses_ReturnsAllExpenses()
        {
            //Arrange
            var expectedExpenses = new[] { CreateRandomExpense(), CreateRandomExpense(), CreateRandomExpense() };

            repositoryStub.Setup(repo => repo.GetExpensesAsync()).ReturnsAsync(expectedExpenses);

            var controller = new ExpenseController(repositoryStub.Object, loggerStub.Object);

            //Act

            var actualExpenses = await controller.GetExpensesAsync();

            //Assert
            actualExpenses.Should().BeEquivalentTo(expectedExpenses, options => options.ComparingByMembers<Expense>());
        }

        [Fact]
        public async Task CreateExpensesAsync_WithExpenseToCreate_ReturnsTheCreatedExpenses()
        {
            //Arrange
            var expenseToCreate = new CreateExpenseDto()
            {
                ExpenseDetails = "testing new expense",
                ExpenseAmount = 777,
                ExpenseDate = DateTimeOffset.UtcNow,
            };

            var controller = new ExpenseController(repositoryStub.Object, loggerStub.Object);

            //Act
            var result = await controller.CreateExpenseAsync(expenseToCreate);

            //Assert
            var createdExpense = (((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value as CreatedAtActionResult).Value as ExpenseDto;

            expenseToCreate.Should().BeEquivalentTo(createdExpense, options => options.ComparingByMembers<ExpenseDto>().ExcludingMissingMembers());

            //createdExpense.Id.Should().NotBeEmpty();
            createdExpense.ExpenseDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
        }

        [Fact]
        public async Task UpdateExpensesAsync_WithExistingExpense_ReturnsNoContent()
        {
            //Arrange
            var existingExpense = CreateRandomExpense();

            repositoryStub.Setup(repo => repo.GetExpenseAsync(It.IsAny<Guid>())).ReturnsAsync(existingExpense);

            var expenseId = existingExpense.Id;
            var expenseToUpdate = new UpdateExpenseDto
            {
                ExpenseDetails = "this is an updated expense",
                ExpenseAmount = existingExpense.ExpenseAmount+3
            };

            var controller = new ExpenseController(repositoryStub.Object, loggerStub.Object);

            //Act
            var result = await controller.UpdateExpenseAsync(expenseId, expenseToUpdate);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteExpensesAsync_WithExistingExpense_ReturnsNoContent()
        {
            //Arrange
            var existingExpense = CreateRandomExpense();

            repositoryStub.Setup(repo => repo.GetExpenseAsync(It.IsAny<Guid>())).ReturnsAsync(existingExpense);

           var controller = new ExpenseController(repositoryStub.Object, loggerStub.Object);

            //Act
            var result = await controller.DeleteExpenseAsync(existingExpense.Id);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        private Expense CreateRandomExpense()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                ExpenseDetails = "this is a random expense",
                ExpenseAmount = rand.Next(1000),
                ExpenseDate = DateTimeOffset.UtcNow
            };
        }
    }
}
