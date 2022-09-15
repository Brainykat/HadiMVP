using Common.Base.Shared.ValueObjects;
using CustomerBusinessDtos;
using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Customers.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Dtos;
using System.Net;

namespace Customers.Services.Services
{
  

  public class CustomerBusinessService : ControllerBase, ICustomerBusinessService
  {
    private readonly ICustomerBusinessRepository repo;
    private readonly ILogger<CustomerBusinessService> logger;
    private readonly IRaiseCustomerAccountOpening raiseCustomerAccountOpening;
    public CustomerBusinessService(ICustomerBusinessRepository repo, ILogger<CustomerBusinessService> logger, IRaiseCustomerAccountOpening raiseCustomerAccountOpening)
    {
      this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.raiseCustomerAccountOpening = raiseCustomerAccountOpening ?? throw new ArgumentNullException(nameof(raiseCustomerAccountOpening));
    }
    public async Task<List<CustomerBusiness>> GetCustomerBusinesses() =>
     await repo.GetCustomerBusinesses();
    public async Task<CustomerBusiness> Get(Guid customerId) =>
      await repo.GetCustomerBusinessAsync(customerId);
    public async Task<IActionResult> Add(CustomerBusinessDto dto, Guid user)
    {
      try
      {
        if (await repo.IsPhoneUsed(dto.Phone))
          ModelState.AddModelError(nameof(dto.Phone), $"{dto.Phone} already used");
        if (await repo.IsEmailUsed(dto.Email))
          ModelState.AddModelError(nameof(dto.Email), $"{dto.Email} already used");
        if (!ModelState.IsValid)
          return BadRequest(ModelState);

        var name = Name.Create(dto.Sur.FirstCharToUpper(), dto.First.FirstCharToUpper(), dto.Middle?.FirstCharToUpper());
        var cust = CustomerBusiness.Create(dto.Name, dto.RegistrationNumber, dto.Email, dto.Phone, name);
        await repo.Add(cust);
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
    public async Task<IActionResult> Update(Guid customerId, CustomerBusinessDto dto)
    {
      try
      {
        throw new NotImplementedException();
        var cur = await repo.GetCustomerBusinessTracked(customerId);
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
    public async Task<IActionResult> Delete(Guid customerId)
    {
      try
      {
        var cat = await repo.GetCustomerBusinessTracked(customerId);
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
