using System;
using System.Collections.Generic;
using EtCoreApi.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EtCoreApi.Repositories
{
    public class MongoDbExpensesRepository : IExpensesRepository
    {
        private const string databaseName = "ExpensesTrackerDb";
        private const string collectionName = "Expenses";
        private readonly IMongoCollection<Expense> expensesCollection;
        private readonly FilterDefinitionBuilder<Expense> filterBuilder = Builders<Expense>.Filter;

        public MongoDbExpensesRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            expensesCollection = database.GetCollection<Expense>(collectionName);
        } 

        public void CreateExpense(Expense expense)
        {
            expensesCollection.InsertOne(expense);
        }

        public void DeleteExpense(Guid Id)
        {
            var filter = filterBuilder.Eq(expense => expense.Id, Id);
            expensesCollection.DeleteOne(filter);
        }

        public Expense GetExpense(Guid id)
        {
            var filter = filterBuilder.Eq(expense => expense.Id,id);
            return expensesCollection.Find(filter).SingleOrDefault();
        }

        public IEnumerable<Expense> GetExpenses()
        {
            return expensesCollection.Find(new BsonDocument()).ToList();
        }
            
        public void UpdateExpense(Expense expense)
        {
            var filter = filterBuilder.Eq(existingExpense => existingExpense.Id, expense.Id);
            expensesCollection.ReplaceOne(filter, expense);

        }
    }
}
