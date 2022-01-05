using EtCoreApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace EtCore.Api.Repositories
{
    public class AppSqlDbContext : DbContext
    {
        public AppSqlDbContext(DbContextOptions<AppSqlDbContext> options) : base(options)
        {
                
        } 

        public DbSet<Expense> Expenses { get; set; }
    }
}
