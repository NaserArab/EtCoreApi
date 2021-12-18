using EtCoreApi.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EtCoreApi.Repositories
{
    public class MongoDbExpensesRepository : IExpensesRepository
    {
        private const string collectionName = "Expenses";
        private const string databaseName = "ExpensesTrackerDb";
        private readonly IMongoCollection<Expense> expensesCollection;
        private readonly FilterDefinitionBuilder<Expense> filterBuilder = Builders<Expense>.Filter;

        public MongoDbExpensesRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            expensesCollection = database.GetCollection<Expense>(collectionName);
        }

        public async Task CreateExpenseAsync(Expense expense)
        {
            await expensesCollection.InsertOneAsync(expense);
        }

        public async Task DeleteExpenseAsync(Guid Id)
        {
            var filter = filterBuilder.Eq(expense => expense.Id, Id);
            await expensesCollection.DeleteOneAsync(filter);
        }

        public async Task<Expense> GetExpenseAsync(Guid id)
        {
            var filter = filterBuilder.Eq(expense => expense.Id, id);
            return await expensesCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesAsync()
        {
            return await expensesCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            var filter = filterBuilder.Eq(existingExpense => existingExpense.Id, expense.Id);
            await expensesCollection.ReplaceOneAsync(filter, expense);
        }
    }
}