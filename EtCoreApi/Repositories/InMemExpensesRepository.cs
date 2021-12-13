using System;
using System.Collections.Generic;
using EtCoreApi.Entities;

namespace EtCoreApi.Repositories
{
    public class InMemExpensesRepository
    {
        private readonly List<Expense> expenses = new()
        {
            new Expense { ExpenseId = 1, ExpenseDetails = "First expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 700 },
            new Expense { ExpenseId = 2, ExpenseDetails = "Second expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 800 },
            new Expense { ExpenseId = 2, ExpenseDetails = "Third expense", ExpenseDate = DateTimeOffset.UtcNow, ExpenseAmount = 900 },
        };
    }
}
