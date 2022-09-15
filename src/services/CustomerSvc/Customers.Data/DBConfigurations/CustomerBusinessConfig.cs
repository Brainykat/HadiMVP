using Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customers.Data.DBConfigurations
{
  public class CustomerBusinessConfig : IEntityTypeConfiguration<CustomerBusiness>
  {
    public void Configure(EntityTypeBuilder<CustomerBusiness> builder)
    {
      builder.Property(e => e.Name)
          .IsRequired()
          .HasMaxLength(80);
      builder.Property(e => e.Phone)
          .IsRequired()
          .HasMaxLength(15);
      builder.Property(e => e.Email)
          .IsRequired()
          .HasMaxLength(50);
      builder.Property(e => e.RegistrationNumber)
          .IsRequired()
          .HasMaxLength(80);
      builder.OwnsOne(i => i.Owner, f =>
      {
        f.Property(n => n.First).IsRequired().HasMaxLength(50);
        f.Property(n => n.Sur).IsRequired().HasMaxLength(50);
        f.Property(n => n.Middle).HasMaxLength(50);
      });
    }
  }
}
