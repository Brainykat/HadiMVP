using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ordering.Data.Mongo.Configurations;
using Ordering.Data.Mongo.Entities;
using Ordering.Data.Mongo.Interfaces;

namespace Ordering.Data.Mongo.Repositories
{
  public class MongoOrderRepository : IMongoOrderRepository
  {
    private readonly IMongoCollection<MongoOrder> _ordersCollection;

    public MongoOrderRepository(
        IOptions<OrderDatabaseSettings> ordersDatabaseSettings)
    {
      var mongoClient = new MongoClient(
          ordersDatabaseSettings.Value.ConnectionString);

      var mongoDatabase = mongoClient.GetDatabase(
          ordersDatabaseSettings.Value.DatabaseName);

      _ordersCollection = mongoDatabase.GetCollection<MongoOrder>(
          ordersDatabaseSettings.Value.OrdersCollectionName);
    }

    public async Task<List<MongoOrder>> GetAsync() =>
        await _ordersCollection.Find(_ => true).ToListAsync();
    public async Task<List<MongoOrder>> GetPerBusinessAsync(Guid businessId) =>
        await _ordersCollection.Find(o => o.BusinessId == businessId.ToString()).ToListAsync();

    public async Task<MongoOrder?> GetAsync(string id) =>
        await _ordersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(MongoOrder newOrder) =>
        await _ordersCollection.InsertOneAsync(newOrder);

    public async Task UpdateAsync(string id, MongoOrder updatedOrder) =>
        await _ordersCollection.ReplaceOneAsync(x => x.Id == id, updatedOrder);

    public async Task RemoveAsync(string id) =>
        await _ordersCollection.DeleteOneAsync(x => x.Id == id);
  }
}
