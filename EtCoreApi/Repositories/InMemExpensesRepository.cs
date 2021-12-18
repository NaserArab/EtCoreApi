using EtCoreApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void CreateExpense(Expense expense)
        {
            expenses.Add(expense);
        }

        public void DeleteExpense(Guid Id)
        {
            var index = expenses.FindIndex(p => p.Id == Id);
            expenses.RemoveAt(index);
        }

        public Expense GetExpense(Guid Id)
        {
            return expenses.FirstOrDefault(p => p.Id == Id);
        }

        public IEnumerable<Expense> GetExpenses()
        {
            return expenses;
        }

        public void UpdateExpense(Expense expense)
        {
            var index = expenses.FindIndex(p => p.Id == expense.Id);
            expenses[index] = expense;
        }
    }
}