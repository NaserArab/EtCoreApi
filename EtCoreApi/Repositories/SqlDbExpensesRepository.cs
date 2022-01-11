using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtCoreApi.Entities;
using EtCoreApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EtCore.Api.Repositories
{
    public class SqlDbExpensesRepository : IExpensesRepository
    {
        private readonly AppSqlDbContext _appSqlDbContext;

        public SqlDbExpensesRepository(AppSqlDbContext appSqlDbContext)
        {
            _appSqlDbContext = appSqlDbContext;
        }

        public async Task CreateExpenseAsync(Expense expense)
        {
            await _appSqlDbContext.AddAsync(expense);
        }

        public Task DeleteExpenseAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Expense> GetExpenseAsync(Guid id)
        {
            return await _appSqlDbContext.Expenses.FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public async Task<IEnumerable<Expense>> GetExpensesAsync()
        {
            return await _appSqlDbContext.Expenses.ToListAsync();
        }

        public Task UpdateExpenseAsync(Expense expense)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChanges()
        { 
            await _appSqlDbContext.SaveChangesAsync();
        }
    }
}
