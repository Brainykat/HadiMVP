using Customers.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Dtos;
using System.Text;

namespace Customers.Services.EventBusServices
{
  public class RaiseCustomerAccountOpening : IRaiseCustomerAccountOpening
  {
    private readonly ILogger<RaiseCustomerAccountOpening> logger;
    private readonly RabbitMQConfiguration rabbitMQConfiguration;
    public RaiseCustomerAccountOpening(ILogger<RaiseCustomerAccountOpening> logger, IOptionsMonitor<RabbitMQConfiguration> optionsMonitor)
    {
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.rabbitMQConfiguration = optionsMonitor.CurrentValue;
    }
    public bool RaiseNewCustomerIdentityEvent(NewCustomerEBDto dto)
    {
      try
      {
        var factory = new ConnectionFactory()
        {
          HostName = rabbitMQConfiguration.HostName,
          Password = rabbitMQConfiguration.Password,
          UserName = rabbitMQConfiguration.UserName,
          Port = rabbitMQConfiguration.Port,
          ClientProvidedName = rabbitMQConfiguration.SubscriptionClientName
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
          channel.ExchangeDeclare(exchange: rabbitMQConfiguration.CustomerExchange, type: rabbitMQConfiguration.CustomerExchangeType,
            durable: true, autoDelete: true);

          channel.QueueDeclare(queue: rabbitMQConfiguration.CustomerQueueName,
                               durable: true,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);
          channel.QueueBind(queue: rabbitMQConfiguration.CustomerQueueName,
                            exchange: rabbitMQConfiguration.CustomerExchange,
                            routingKey: rabbitMQConfiguration.CustomerRouteKey);
          var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto));
          //var body = dto.Serialize();
          if (body == null) return false;
          channel.BasicPublish(exchange: rabbitMQConfiguration.CustomerExchange,
                               routingKey: rabbitMQConfiguration.CustomerRouteKey,
                               basicProperties: null,
                               body: body);
          logger.LogInformation("in CustomerQueueName Queued message {j}", JsonConvert.SerializeObject(dto));
          return true;
        }
      }
      catch (System.Exception ex)
      {
        logger.LogCritical($"{GetType().FullName} ==> Error {ex.Message} inner {ex.InnerException?.InnerException}");
        return false;
      }
    }
  }
}
