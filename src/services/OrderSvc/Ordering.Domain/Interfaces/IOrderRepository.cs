using Ordering.Domain.Entities;

namespace Ordering.Domain.Interfaces
{
  public interface IOrderRepository
  {
    Task Add(Order order);
    Task Delete(Order order);
    Task<Order> Getorder(Guid id);
    Task<List<Order>> GetOrders();
    Task<Order> GetOrderTracked(Guid id);
    Task Update(Order order);
  }
}
