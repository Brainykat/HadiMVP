using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ordering.Data.Mongo.Entities
{
  public class MongoOrder
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("CustomerBusinessId")]
    public string BusinessId { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow!;

    public string Status { get; set; } = null!;
  }
}
