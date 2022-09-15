using Common.Base.Shared;
using Common.Base.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Domain.Entities
{
  [Table("Items")]
  public class Item : EntityBase
  {
    public static Item Create(Guid orderId, string description, string name, Money amount) =>
      new Item(orderId, description, name, amount);
    private Item(Guid orderId, string description, string name, Money amount)
    {
      OrderId = orderId;
      Description = description ?? throw new ArgumentNullException(nameof(description));
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Amount = amount ?? throw new ArgumentNullException(nameof(amount));
    }

    public Guid OrderId { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public Money Amount { get; set; }
    public Order? Order { get; set; }
  }
}
