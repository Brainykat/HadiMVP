using Common.Base.Shared;

namespace Customers.Domain.Entities
{
  public class EmployeeDepartment : EntityBase
  {
    public static EmployeeDepartment Create(Guid employeeId, Guid departmentId, bool isDepartmentHead = false)
      => new EmployeeDepartment(employeeId, departmentId, isDepartmentHead);
    private EmployeeDepartment(Guid employeeId, Guid departmentId, bool isDepartmentHead = false)
    {
      EmployeeId = employeeId;
      DepartmentId = departmentId;
      IsDepartmentHead = isDepartmentHead;
    }
    public Guid EmployeeId { get; set; }
    public Guid DepartmentId { get; set; }
    public bool IsDepartmentHead { get; set; }
    public Employee? Employee { get; set; }
    public Department? Department { get; set; }
  }
}
