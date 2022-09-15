using Common.Base.Shared;
using Common.Base.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customers.Domain.Entities
{
  [Table("Employees")]
  public class Employee : EntityBase
  {
    
    public static Employee Create(Name name, string jobNumber, string idNumber, string email, string phone)
      => new Employee(name, jobNumber, idNumber, email, phone);
    public void AddDepartment(EmployeeDepartment employeeDepartment) =>
      EmployeeDepartments.Add(employeeDepartment);
    private Employee(Name name, string jobNumber, string idNumber, string email, string phone)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      JobNumber = jobNumber ?? throw new ArgumentNullException(nameof(jobNumber));
      IdNumber = idNumber ?? throw new ArgumentNullException(nameof(idNumber));
      Email = email ?? throw new ArgumentNullException(nameof(email));
      Phone = phone ?? throw new ArgumentNullException(nameof(phone));
      EmployeeDepartments = new List<EmployeeDepartment>();
    }
    public Name Name { get; private set; }
    public string JobNumber { get; private set; }
    public string IdNumber { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public List<EmployeeDepartment> EmployeeDepartments { get; set; }
  }
}
