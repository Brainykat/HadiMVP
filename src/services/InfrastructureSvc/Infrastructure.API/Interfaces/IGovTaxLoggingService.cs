using RabbitMQ.Dtos;

namespace Infrastructure.API.Interfaces
{
  public interface IGovTaxLoggingService
  {
    Task<bool> LogTaxData(NewOrderEBDto dto);
  }
}
