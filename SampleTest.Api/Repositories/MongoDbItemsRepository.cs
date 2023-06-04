using MongoDB.Bson;
using MongoDB.Driver;
using SampleTest.Api.Entiites;

namespace SampleTest.Api.Repositories
{
    public class MongoDbItemsRepository : IInMemItemsRepository
    {
        private const string databaseName = "SampleTest";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;
        // we need a filterdefinitionbuilder, which helps to filter the items you would be returning as found in the db. Since we woould be using it commingly, we can declare its class definition here
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        
        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollection =  database.GetCollection<Item>(collectionName);
            
        }
        // public void CreateItem(Item item)
        // {
        //     itemsCollection.InsertOne(item);
        // }

        // public void DeleteItem(Guid Id)
        // {
        //     var filter = filterBuilder.Eq(x=>x.Id, Id);
        //     itemsCollection.DeleteOne(filter);
        // }

        // public Item GetItem(Guid Id)
        // {
        //     // var filter = filterBuilder.Eq(item => item.Id, Id);
        //     // return itemsCollection.Find(filter).SingleOrDefault();
        //     return itemsCollection.Find(x=>x.Id == Id).SingleOrDefault();
        // }

        // public IEnumerable<Item> GetItems()
        // {
        //     return itemsCollection.Find(new BsonDocument()).ToList();
        // }

        // public void UpdateItem(Item item)
        // {
        //     var filter = filterBuilder.Eq(item => item.Id, item.Id);
        //     itemsCollection.ReplaceOne(filter, item);
        // }

        public async Task CreateItemAsync(Item item)
        {
           await itemsCollection.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Guid Id)
        {
            var filter = filterBuilder.Eq(x=>x.Id, Id);
           await itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item> GetItemAsync(Guid Id)
        {
            // var filter = filterBuilder.Eq(item => item.Id, Id);
            // return itemsCollection.Find(filter).SingleOrDefault();
            return await itemsCollection.Find(x=>x.Id == Id).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await itemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(item => item.Id, item.Id);
           await itemsCollection.ReplaceOneAsync(filter, item);
        }
    }
}