using RabbitMQ.Dtos;

namespace Ordering.Services.Interfaces
{
  public interface IRaiseNewOrderCreation
  {
    bool RaiseNewCustomerIdentityEvent(NewOrderEBDto dto);
  }
}
