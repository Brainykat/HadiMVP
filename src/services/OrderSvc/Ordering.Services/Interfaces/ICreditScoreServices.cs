using Microsoft.AspNetCore.Mvc;

namespace Ordering.Services.Interfaces
{
  public interface ICreditScoreServices
  {
    Task<IActionResult> GetCreditScore(Guid businessId);
  }
}
