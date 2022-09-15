using Common.Base.Shared;
using Common.Base.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customers.Domain.Entities
{
  [Table("CustomerBusinesses")]
  public class CustomerBusiness : EntityBase
  {
    public static CustomerBusiness Create(string name, string registrationNumber,
      string email, string phone, Name owner) =>
      new CustomerBusiness(name, registrationNumber, email, phone, owner);
    private CustomerBusiness(string name, string registrationNumber, string email, string phone, Name owner)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      RegistrationNumber = registrationNumber ?? throw new ArgumentNullException(nameof(registrationNumber));
      Email = email ?? throw new ArgumentNullException(nameof(email));
      Phone = phone ?? throw new ArgumentNullException(nameof(phone));
      Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }
    public string Name { get; private set; }
    public string RegistrationNumber { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public Name Owner { get; private set; }
  }
}
