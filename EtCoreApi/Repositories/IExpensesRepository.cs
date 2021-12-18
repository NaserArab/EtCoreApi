using System;
using EtCoreApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EtCoreApi.Repositories
{
    public interface IExpensesRepository
    {
        Task CreateExpenseAsync(Expense expense);

        Task DeleteExpenseAsync(Guid id);

        Task<Expense> GetExpenseAsync(Guid  id);

        Task<IEnumerable<Expense>> GetExpensesAsync();

        Task UpdateExpenseAsync(Expense expense);
    }
}