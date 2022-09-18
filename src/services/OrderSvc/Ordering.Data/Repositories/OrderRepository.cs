using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Domain.Interfaces;

namespace Ordering.Data.Repositories
{
  public class OrderRepository : IOrderRepository
  {
    private readonly OrderingContext context;
    public OrderRepository(OrderingContext context)
    {
      this.context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<List<Order>> GetOrders() =>
      await context.Orders.AsNoTracking().ToListAsync();
    public async Task<List<Order>> GetOrdersPerBusiness(Guid businessId) =>
      await context.Orders.AsNoTracking().Where(o=>o.BusinessId == businessId).ToListAsync();
    //Generic Query will work better here to pass the filtering to front end with less overhead
    public async Task<List<Order>> GetOrdersPerBusiness(Guid businessId,DateTime startDate, DateTime endDate) =>
      await context.Orders.AsNoTracking()
      .Where(o => o.BusinessId == businessId && o.DateCreated >= startDate && o.DateCreated <= endDate).ToListAsync();
    public async Task<Order?> Getorder(Guid id) =>
      await context.Orders.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    public async Task<Order?> GetOrderTracked(Guid id) =>
      await context.Orders.FirstOrDefaultAsync(a => a.Id == id);

    public async Task Add(Order order)
    {
      context.Orders.Add(order);
      await context.SaveChangesAsync();
    }
    public async Task Update(Order order)
    {
      context.Orders.Update(order);
      await context.SaveChangesAsync();
    }
    public async Task Delete(Order order)
    {
      context.Orders.Remove(order);
      await context.SaveChangesAsync();
    }
  }
}
