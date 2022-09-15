using Common.Base.Shared.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ordering.Data.Mongo.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Interfaces;
using Ordering.Services.Interfaces;
using OrderingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Services.Services
{
  public class OrderService: ControllerBase
  {
    private readonly IOrderRepository repo;
    private readonly ILogger<OrderService> logger;
    private readonly IRaiseNewOrderCreation raiseNewOrderCreation;
    private readonly IMongoOrderRepository mongoOrderRepository;
    public OrderService(IOrderRepository repo, ILogger<OrderService> logger, 
      IRaiseNewOrderCreation raiseNewOrderCreation, IMongoOrderRepository mongoOrderRepository)
    {
      this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.raiseNewOrderCreation = raiseNewOrderCreation ?? throw new ArgumentNullException(nameof(raiseNewOrderCreation));
      this.mongoOrderRepository = mongoOrderRepository ?? throw new ArgumentNullException(nameof(mongoOrderRepository));
    }
    public async Task<List<Order>> GetCustomerBusinesses() =>
     await repo.GetOrders();
    public async Task<Order> Get(Guid orderId) =>
      await repo.Getorder(orderId);
    public async Task<IActionResult> Add(OrderDto dto, Guid user)
    {
      try
      {
        if (!dto.Items.Any())
          ModelState.AddModelError(nameof(dto.Items), $"Please add at least one item to the order");        
        if (!ModelState.IsValid)
          return BadRequest(ModelState);
        var order = Order.Create(dto.BusinessId, dto.OrderDescription, user);
        //This can be done better for buying two of the same item
        foreach (var item in dto.Items)
        {
          if (item.Amount > 0)
            order.AddItem(Item.Create(order.Id, item.Description, item.Name, Money.Create(item.Currency, item.Amount)));
        }
        await repo.Add(order);
        var claims = new Dictionary<string, string>();
        claims.Add("BusinessId", cust.Id.ToString());
        //Raise Event for identity server to create initail user
        var result = raiseCustomerAccountOpening.RaiseNewCustomerIdentityEvent(
          new NewCustomerEBDto(cust.Id, cust.Owner.FullName, cust.Email, cust.Phone, "BusinessOwner",
          claims));
        return result ? Ok(cust) : throw new Exception("Customer initial system User could not be created");
      }
      catch (Exception ex)
      {
        logger.LogError(ex, ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, ex);
      }

    }
    public async Task<IActionResult> Update(Guid orderId, OrderDto dto)
    {
      try
      {
        throw new NotImplementedException();
        var cur = await repo.GetOrderTracked(orderId);
        if (cur == null)
        {
          return NotFound();
        }

        await repo.Update(cur);
        return Ok(cur);
      }
      catch (Exception ex)
      {
        logger.LogError(ex, ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, ex); ;
      }

    }
    public async Task<IActionResult> Delete(Guid orderId)
    {
      try
      {
        var cat = await repo.GetOrderTracked(orderId);
        if (cat == null)
        {
          return NotFound();
        }
        await repo.Delete(cat);
        return NoContent();

      }
      catch (Exception ex)
      {
        logger.LogError(ex, ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, ex); ;
      }
    }
  }
}
