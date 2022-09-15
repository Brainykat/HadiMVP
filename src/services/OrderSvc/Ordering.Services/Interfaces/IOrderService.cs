using Microsoft.AspNetCore.Mvc;
using Ordering.Domain.Entities;
using OrderingDtos;

namespace Ordering.Services.Interfaces
{
  public interface IOrderService
  {
    Task<IActionResult> Add(OrderDto dto, Guid user);
    Task<IActionResult> Delete(Guid orderId);
    Task<Order> Get(Guid orderId);
    Task<List<Order>> GetAllOrders();
    Task<List<Order>> GetOrdersPerBusines(Guid businessId);
    Task<List<Order>> GetOrdersPerBusines(Guid businessId, DateTime startDate, DateTime endDate);
    Task<IActionResult> Update(Guid orderId, OrderDto dto);
  }
}
