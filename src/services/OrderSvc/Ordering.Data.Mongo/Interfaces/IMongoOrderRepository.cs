using Ordering.Data.Mongo.Entities;

namespace Ordering.Data.Mongo.Interfaces
{
  public interface IMongoOrderRepository
  {
    Task CreateAsync(MongoOrder newOrder);
    Task<List<MongoOrder>> GetAsync();
    Task<MongoOrder?> GetAsync(string id);
    Task<List<MongoOrder>> GetPerBusinessAsync(Guid businessId);
    Task RemoveAsync(string id);
    Task UpdateAsync(string id, MongoOrder updatedOrder);
  }
}
