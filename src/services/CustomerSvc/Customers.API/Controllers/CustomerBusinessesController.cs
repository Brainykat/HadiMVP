using CustomerBusinessDtos;
using Customers.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CustomerBusinessesController : ControllerBase
  {
    private readonly ICustomerBusinessService service;
    public CustomerBusinessesController(ICustomerBusinessService service)
    {
      this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
      => Ok(await service.GetCustomerBusinesses());

    [HttpGet("{customerBusinessId}")]
    public async Task<IActionResult> Get(Guid customerBusinessId) => Ok(await service.Get(customerBusinessId));

    //TODO get user from claims
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CustomerBusinessDto dto)
    => await service.Add(dto, Guid.Empty);


    [HttpPut("{customerBusinessId}")]
    public async Task<IActionResult> Update(Guid customerBusinessId, [FromBody] CustomerBusinessDto dto)
    => await service.Update(customerBusinessId, dto);


    [HttpDelete("{customerBusinessId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid customerBusinessId)
    => await service.Delete(customerBusinessId);
  }
}
