using System.Collections.Generic;
using EtCoreApi.Entities;
using MongoDB.Driver;

namespace EtCoreApi.Repositories
{
    public class MongoDbItemsRepository : IExpensesRepository
    {
        private const string databaseName = "ExpensesTrackerDb";
        private const string collectionName = "Expenses";
        private readonly IMongoCollection<Expense> expensesCollection;

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            expensesCollection = database.GetCollection<Expense>(collectionName);
        } 

        public void CreateExpense(Expense expense)
        {
            expensesCollection.InsertOne(expense);
        }

        public void DeleteExpense(int expenseId)
        {
            throw new System.NotImplementedException();
        }

        public Expense GetExpense(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Expense> GetExpenses()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateExpense(Expense expense)
        {
            throw new System.NotImplementedException();
        }
    }
}
