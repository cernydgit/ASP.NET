using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Repositories
{
    public class MongoRepository : IRepository
    {
        IMongoCollection<Item> itemsCollection;

        FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        public MongoRepository(IMongoClient mongoClient)
        {
            itemsCollection = mongoClient.GetDatabase("catalog").GetCollection<Item>("items");
        }

        public void CreateItem(Item item)
        {
            itemsCollection.InsertOne(item);
        }

        public void DeleteItem(string id)
        {
            itemsCollection.DeleteOne(i => i.Id == id);
        }

        public Item GetItemByIndex(int index)
        {
            //var filter = filterBuilder.Eq(item => item.Price, index);
            //return itemsCollection.Find(Builders<Item>.Filter.Eq(nameof(Item.Price), index)).FirstOrDefault();

            return itemsCollection.AsQueryable().FirstOrDefault(x => x.Price == index);
        }

        public IEnumerable<Item> GetItems()
        {
            return itemsCollection.Find(Builders<Item>.Filter.Empty).ToList(); //.Tol new BsonDocument()).ToList();
        }

        public void UpdateItem(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
        }
    }
}
