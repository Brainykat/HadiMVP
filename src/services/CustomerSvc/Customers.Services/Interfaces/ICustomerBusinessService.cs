using CustomerBusinessDtos;
using Customers.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Services.Interfaces
{
  public interface ICustomerBusinessService
  {
    Task<IActionResult> Add(CustomerBusinessDto dto, Guid user);
    Task<IActionResult> Delete(Guid customerId);
    Task<CustomerBusiness> Get(Guid customerId);
    Task<List<CustomerBusiness>> GetCustomerBusinesses();
    Task<IActionResult> Update(Guid customerId, CustomerBusinessDto dto);
  }
}
