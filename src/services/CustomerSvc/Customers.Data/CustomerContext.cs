using Customers.Data.DBConfigurations;
using Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Data
{
  public class CustomerContext : DbContext
  {
    public CustomerContext(DbContextOptions<CustomerContext> options)
           : base(options) { }
    public DbSet<BusinessLocation> BusinessLocations { get; set; }
    public DbSet<CustomerBusiness> CustomerBusinesses { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfiguration(new CustomerBusinessConfig());
    }
  }
}
