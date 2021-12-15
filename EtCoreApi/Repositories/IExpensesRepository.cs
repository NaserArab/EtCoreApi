using System.Collections.Generic;
using EtCoreApi.Entities;

namespace EtCoreApi.Repositories
{
    public interface IExpensesRepository
    {
        Expense GetExpense(int id);
        IEnumerable<Expense> GetExpenses();
        void CreateExpense(Expense expense);
    }
}
