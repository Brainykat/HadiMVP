using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Services.Interfaces;

namespace Ordering.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CreditScoresController : ControllerBase
  {
    private readonly ICreditScoreServices service;
    public CreditScoresController(ICreditScoreServices service)
    {
      this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// UNDONE: We should retrieve this business ID from claims in Token but lets pass it for now
    /// </summary>
    /// <param name="businessId"></param>
    /// <returns></returns>
    [HttpGet("{businessId}")]
    public async Task<IActionResult> Get(Guid businessId)
      => Ok(await service.GetCreditScore(businessId));
  }
}
