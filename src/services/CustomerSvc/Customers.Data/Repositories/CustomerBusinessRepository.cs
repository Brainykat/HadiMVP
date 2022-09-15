using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Customers.Data.Repositories
{
  public class CustomerBusinessRepository : ICustomerBusinessRepository
  {
    private readonly CustomerContext context;
    public CustomerBusinessRepository(CustomerContext context)
    {
      this.context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<List<CustomerBusiness>> GetCustomerBusinesses() =>
      await context.CustomerBusinesses.AsNoTracking().ToListAsync();
    public async Task<CustomerBusiness> GetCustomerBusinessAsync(Guid id) =>
      await context.CustomerBusinesses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    public async Task<CustomerBusiness> GetCustomerBusinessTracked(Guid id) =>
      await context.CustomerBusinesses.FirstOrDefaultAsync(a => a.Id == id);
    public async Task<bool> IsPhoneUsed(string phone) =>
      await context.CustomerBusinesses.AsNoTracking().AnyAsync(cb => cb.Phone == phone);
    public async Task<bool> IsEmailUsed(string email) =>
      await context.CustomerBusinesses.AsNoTracking().AnyAsync(cb => cb.Email == email);
    public async Task Add(CustomerBusiness customerBusiness)
    {
      context.CustomerBusinesses.Add(customerBusiness);
      await context.SaveChangesAsync();
    }
    public async Task Update(CustomerBusiness customerBusiness)
    {
      context.CustomerBusinesses.Update(customerBusiness);
      await context.SaveChangesAsync();
    }
    public async Task Delete(CustomerBusiness customerBusiness)
    {
      context.CustomerBusinesses.Remove(customerBusiness);
      await context.SaveChangesAsync();
    }
  }
}
