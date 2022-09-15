using Customers.Domain.Entities;

namespace Customers.Domain.Interfaces
{
  public interface ICustomerBusinessRepository
  {
    Task Add(CustomerBusiness customerBusiness);
    Task Delete(CustomerBusiness customerBusiness);
    Task<CustomerBusiness> GetCustomerBusinessAsync(Guid id);
    Task<List<CustomerBusiness>> GetCustomerBusinesses();
    Task<CustomerBusiness> GetCustomerBusinessTracked(Guid id);
    Task<bool> IsEmailUsed(string email);
    Task<bool> IsPhoneUsed(string phone);
    Task Update(CustomerBusiness customerBusiness);
  }
}
