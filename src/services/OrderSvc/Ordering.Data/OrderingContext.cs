using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Data
{
  public class OrderingContext :DbContext
  {
    public OrderingContext(DbContextOptions<OrderingContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.UseSerialColumns();
    }
    public DbSet<Order> Orders {get;set;}
    public DbSet<Item> Items {get;set;}
  }
}
