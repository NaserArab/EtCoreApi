using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using EtCoreApi.Entities;

namespace EtCore.Api.Repositories
{
    public static class PrepSqlDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppSqlDbContext>());
            }
        }

        private static void SeedData(AppSqlDbContext appSqlDbContext)
        {
            if (appSqlDbContext.Expenses.Any() == false)
            {
                Console.WriteLine("--> Seeding data...");

                appSqlDbContext.Expenses.AddRange(
                    new Expense { ExpenseDetails = "First SQL Expense", ExpenseAmount = 700, ExpenseDate = DateTime.Now },
                    new Expense { ExpenseDetails = "Second SQL Expense", ExpenseAmount = 800, ExpenseDate = DateTime.Now },
                    new Expense { ExpenseDetails = "Third SQL Expense", ExpenseAmount = 900, ExpenseDate = DateTime.Now }
                    );

                appSqlDbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
