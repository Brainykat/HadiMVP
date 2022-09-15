using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ordering.Data.Mongo.Interfaces;
using Ordering.Services.Interfaces;
using System.Net;

namespace Ordering.Services.Services
{
  

  public class CreditScoreServices : ControllerBase, ICreditScoreServices
  {
    private readonly ILogger<CreditScoreServices> logger;
    private readonly IMongoOrderRepository mongoOrderRepository;

    public CreditScoreServices(ILogger<CreditScoreServices> logger, IMongoOrderRepository mongoOrderRepository)
    {
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.mongoOrderRepository = mongoOrderRepository ?? throw new ArgumentNullException(nameof(mongoOrderRepository));
    }
    public async Task<IActionResult> GetCreditScore(Guid businessId)
    {
      try
      {
        var orderLogs = await mongoOrderRepository.GetPerBusinessAsync(businessId);
        if (orderLogs != null)
        {
          var score = orderLogs.Sum(x => x.Amount) / orderLogs.Count * 100;
          return Ok(score);
        }
        return Ok(0);
      }
      catch (Exception ex)
      {
        logger.LogError(ex, ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, ex);
      }
    }
  }
}
