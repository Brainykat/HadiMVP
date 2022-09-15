using Customers.Domain.Entities;

namespace Customers.Domain.Interfaces
{
  public interface ICustomerBusinessRepository
  {
    Task Add(CustomerBusiness customerBusiness);
    Task Delete(CustomerBusiness customerBusiness);
    Task<CustomerBusiness> GetCustomerBusinessAsync(Guid id);
    Task<List<CustomerBusiness>> GetCustomerBusinesses(Guid customerBusinessId);
    Task<CustomerBusiness> GetCustomerBusinessTracked(Guid id);
    Task Update(CustomerBusiness customerBusiness);
  }
}
