using EtCoreApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtCoreApi.Repositories
{
    public class InMemExpensesRepository : IExpensesRepository
    {
        private readonly List<Expense> expenses = new()
        {
            new Expense { Id = Guid.NewGuid(), ExpenseDetails = "First expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 700 },
            new Expense { Id = Guid.NewGuid(), ExpenseDetails = "Second expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 800 },
            new Expense { Id = Guid.NewGuid(), ExpenseDetails = "Third expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 900 },
        };

        public async Task CreateExpenseAsync(Expense expense)
        {
            expenses.Add(expense);
            await Task.CompletedTask;
        }

        public async Task DeleteExpenseAsync(Guid Id)
        {
            var index = expenses.FindIndex(p => p.Id == Id);
            expenses.RemoveAt(index);

            await Task.CompletedTask;
        }

        public async Task<Expense> GetExpenseAsync(Guid Id)
        {
            var expense = expenses.FirstOrDefault(p => p.Id == Id);
            return await Task.FromResult(expense);
        }

        public async Task<IEnumerable<Expense>> GetExpensesAsync()
        {
            return await Task.FromResult(expenses);
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            var index = expenses.FindIndex(p => p.Id == expense.Id);
            expenses[index] = expense;

            await Task.CompletedTask;
        }

        public Task SaveChanges()
        {
            return Task.CompletedTask;
        }
    }
}