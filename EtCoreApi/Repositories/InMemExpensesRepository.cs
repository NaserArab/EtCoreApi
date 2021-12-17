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
            new Expense { ExpenseId = 1, ExpenseDetails = "First expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 700 },
            new Expense { ExpenseId = 2, ExpenseDetails = "Second expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 800 },
            new Expense { ExpenseId = 3, ExpenseDetails = "Third expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 900 },
        };

        public void CreateExpense(Expense expense)
        {
            expenses.Add(expense);
        }

        public void DeleteExpense(int expenseId)
        {
            var index = expenses.FindIndex(p => p.ExpenseId == expenseId);
            expenses.RemoveAt(index);
        }

        public Expense GetExpense(int expenseId)
        {
            return expenses.FirstOrDefault(p => p.ExpenseId == expenseId);
        }

        public IEnumerable<Expense> GetExpenses()
        {
            return expenses;
        }

        public void UpdateExpense(Expense expense)
        {
            var index = expenses.FindIndex(p => p.ExpenseId == expense.ExpenseId);
            expenses[index] = expense;
        }
    }
}