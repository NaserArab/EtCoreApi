using System;
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
            ((Microsoft.AspNetCore.Mvc.ObjectResult)expense.Result).Value.Should().BeEquivalentTo(expectedExpense);
        }

        private Expense CreateRandomExpense()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                ExpenseDetails = Guid.NewGuid().ToString(),
                ExpenseAmount = rand.Next(1000),
                ExpenseDate = DateTimeOffset.UtcNow
            };
        }
    }
}
