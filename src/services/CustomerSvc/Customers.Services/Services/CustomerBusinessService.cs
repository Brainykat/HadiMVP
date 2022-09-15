using Common.Base.Shared.ValueObjects;
using CustomerBusinessDtos;
using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Customers.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Services.Services
{
  public class CustomerBusinessService : ControllerBase
  {
    private readonly ICustomerBusinessRepository repo;
    private readonly ILogger<CustomerBusinessService> logger;
    private readonly IRaiseCustomerAccountOpening raiseCustomerAccountOpening;
    public CustomerBusinessService(ICustomerBusinessRepository repo, ILogger<CustomerBusinessService> logger, IRaiseCustomerAccountOpening raiseCustomerAccountOpening)
    {
      this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.raiseCustomerAccountOpening = raiseCustomerAccountOpening ?? throw new ArgumentNullException(nameof(raiseCustomerAccountOpening));
    }
    public async Task<List<CustomerBusiness>> GetCustomerBusinesses() =>
     await repo.GetCustomerBusinesses();
    public async Task<CustomerBusiness> Get(Guid customerId) =>
      await repo.GetCustomerBusinessAsync(customerId);
    public async Task<IActionResult> Add( CustomerBusinessDto dto, Guid user)
    {
      try
      {
        if (await repo.IsPhoneUsed(dto.Phone))
          ModelState.AddModelError(nameof(dto.Phone), $"{dto.Phone} already used");
        if (await repo.IsEmailUsed(dto.Email))
          ModelState.AddModelError(nameof(dto.Email), $"{dto.Email} already used");
        if(!ModelState.IsValid)
          return BadRequest(ModelState);

        var name = Name.Create(dto.Sur.FirstCharToUpper(), dto.First.FirstCharToUpper(), dto.Middle?.FirstCharToUpper());
        var cust = CustomerBusiness.Create(dto.Name, dto.RegistrationNumber, dto.Email, dto.Phone,name);
        await repo.Add(cust);
        //Raise Event for identity server to create initail user
        var result = raiseCustomerAccountOpening.RaiseCustomerAccountOpeningEvent(new NewCustomerEBDto(cust.Id, cust.Name.FullName,
          dto.NationalityId.Value));
        return result ? Ok(cust) : throw new Exception("Customer accounts could not be opened");
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return StatusCode((int)HttpStatusCode.InternalServerError, ex);
      }

    }
    public async Task<IActionResult> Update(Guid customerId, CustomerBusinessDto dto)
    {
      try
      {
        var cur = await repo.GetCustomerBusinessTracked(customerId);
        if (cur == null)
        {
          return NotFound();
        }
        cur.BranchId = dto.BranchId;
        cur.CountyId = dto.CountyId;
        cur.CustomerType = dto.CustomerType;
        cur.DOB = dto.DOB;
        cur.Email = dto.Email;
        cur.GenderId = dto.GenderId;
        cur.IdNumber = dto.IDNumber;
        cur.IdTypeId = dto.IDTypeID;
        cur.KRAPin = dto.KRAPin;
        cur.AccountNumber = dto.AccountNumber;
        cur.MaritalStatusId = dto.MaritalStatusId;
        cur.Name = Name.Create(dto.SurName, dto.FirstName, dto.MiddleName);
        cur.NationalityId = dto.NationalityId;
        cur.Occupation = dto.Occupation;
        cur.Phone = PhoneNumberNomalizer.Nomalize(dto.Phone);
        cur.PysicalLocation = dto.Location;
        cur.SecEmail = dto.SecEmail;
        cur.SecPhone = PhoneNumberNomalizer.Nomalize(dto.SecPhone);
        cur.SuffixId = dto.SuffixId;
        await repo.Update(cur);
        return Ok(cur);
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return StatusCode((int)HttpStatusCode.InternalServerError, ex); ;
      }

    }
    public async Task<IActionResult> Delete(Guid customerId)
    {
      try
      {
        var cat = await repo.GetCustomerBusinessTracked(customerId);
        if (cat == null)
        {
          return NotFound();
        }
        await repo.Delete(cat);
        return NoContent();

      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return StatusCode((int)HttpStatusCode.InternalServerError, ex); ;
      }
    }


  }
}
