using RabbitMQ.Dtos;

namespace Customers.Services.Interfaces
{
  public interface IRaiseCustomerAccountOpening
  {
    bool RaiseNewCustomerIdentityEvent(NewCustomerEBDto dto);
  }
}
