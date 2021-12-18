using System;
using EtCoreApi.Entities;
using System.Collections.Generic;

namespace EtCoreApi.Repositories
{
    public interface IExpensesRepository
    {
        void CreateExpense(Expense expense);

        void DeleteExpense(Guid id);

        Expense GetExpense(Guid id);

        IEnumerable<Expense> GetExpenses();

        void UpdateExpense(Expense expense);
    }
}