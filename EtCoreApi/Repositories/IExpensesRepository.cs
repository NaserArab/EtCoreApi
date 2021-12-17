using EtCoreApi.Entities;
using System.Collections.Generic;

namespace EtCoreApi.Repositories
{
    public interface IExpensesRepository
    {
        void CreateExpense(Expense expense);

        void DeleteExpense(int expenseId);

        Expense GetExpense(int id);

        IEnumerable<Expense> GetExpenses();

        void UpdateExpense(Expense expense);
    }
}