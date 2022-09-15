﻿using Common.Base.Shared;

namespace Customers.Domain.Entities
{
  public class Department :EntityBase
  {
    public static Department Create(string name) => new Department(name);
    public void AddDepartment(EmployeeDepartment employeeDepartment) =>
      EmployeeDepartments.Add(employeeDepartment);
    private Department(string name)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      EmployeeDepartments = new List<EmployeeDepartment>();
    }
    public string Name { get; private set; }
    public List<EmployeeDepartment> EmployeeDepartments { get; set; }
  }
}
