using Common.Base.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Domain.Entities
{
  [Table("Orders")]
  public  class Order : EntityBase
  {
    public static Order Create(Guid businessId, string orderDescription, Guid user)
      => new Order(businessId, orderDescription, user);
    private Order(Guid businessId, string orderDescription, Guid user)
    {
      GenerateNewIdentity();
      BusinessId = businessId;
      OrderDescription = orderDescription ?? throw new ArgumentNullException(nameof(orderDescription));
      User = user;
      GovAPIStatus = 0;
      Items = new List<Item>();
    }
    public void AddItem(Item item) => Items.Add(item);
    public Guid BusinessId { get; set; }
    public string OrderDescription { get; set; }
    public Guid User { get; set; }
    /// <summary>
    /// 200/201 was logged and acknowledged
    /// Others should be retried or so
    /// </summary>
    public  int  GovAPIStatus { get; set; }
    public List<Item> Items { get; set; }
  }
}
