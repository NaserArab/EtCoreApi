using System;
using System.Collections.Generic;
using System.Linq;
using EtCoreApi.Entities;

namespace EtCoreApi.Repositories
{
    public class InMemExpensesRepository : IExpensesRepository
    {
        private readonly List<Expense> expenses = new()
        {
            new Expense { ExpenseId = 1, ExpenseDetails = "First expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 700 },
            new Expense { ExpenseId = 2, ExpenseDetails = "Second expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 800 },
            new Expense { ExpenseId = 3, ExpenseDetails = "Third expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 900 },
        };

        public IEnumerable<Expense> GetExpenses()
        {
            return expenses;
        }

        public void CreateExpense(Expense expense)
        {
            expenses.Add(expense);
        }

        public Expense GetExpense(int expenseId)
        {
            return expenses.FirstOrDefault(p => p.ExpenseId == expenseId);
        }
    }
}
