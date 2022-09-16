using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Services.Interfaces;
using OrderingDtos;

namespace Ordering.API.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class OrdersController : ControllerBase
  {
    private readonly IOrderService service;
    public OrdersController(IOrderService service)
    {
      this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
      => Ok(await service.GetAllOrders());

    [HttpGet("{businessId}")]
    public async Task<IActionResult> GetOrdersPerBusiness(Guid businessId) 
      => Ok(await service.GetOrdersPerBusines(businessId));
    /// <summary>
    /// There is a more elegant way of doing this with a time range value object
    /// </summary>
    /// <param name="businessId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    [HttpGet("{businessId}/{startDate}/{endDate}")]
    public async Task<IActionResult> GetOrdersPerBusinessRange(Guid businessId, DateTime startDate, DateTime endDate)
      => Ok(await service.GetOrdersPerBusines(businessId,startDate,endDate));

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(Guid orderId)
      => Ok(await service.Get(orderId));

    //TODO get user from claims
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] OrderDto dto)
    => await service.Add(dto, Guid.Empty);


    [HttpPut("{orderId}")]
    public async Task<IActionResult> Update(Guid orderId, [FromBody] OrderDto dto)
    => await service.Update(orderId, dto);


    [HttpDelete("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid orderId)
    => await service.Delete(orderId);
  }
}
