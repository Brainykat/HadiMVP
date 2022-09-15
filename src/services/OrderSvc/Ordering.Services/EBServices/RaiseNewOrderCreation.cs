using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ordering.Services.Interfaces;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Services.EBServices
{
  public class RaiseNewOrderCreation : IRaiseNewOrderCreation
  {
    private readonly ILogger<RaiseNewOrderCreation> logger;
    private readonly RabbitMQConfiguration rabbitMQConfiguration;
    public RaiseNewOrderCreation(ILogger<RaiseNewOrderCreation> logger, IOptionsMonitor<RabbitMQConfiguration> optionsMonitor)
    {
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.rabbitMQConfiguration = optionsMonitor.CurrentValue;
    }
    public bool RaiseNewCustomerIdentityEvent(NewOrderEBDto dto)
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
          channel.ExchangeDeclare(exchange: rabbitMQConfiguration.OrderingExchange, type: rabbitMQConfiguration.OrderingExchangeType,
            durable: true, autoDelete: true);

          channel.QueueDeclare(queue: rabbitMQConfiguration.OrderingQueueName,
                               durable: true,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);
          channel.QueueBind(queue: rabbitMQConfiguration.OrderingQueueName,
                            exchange: rabbitMQConfiguration.OrderingExchange,
                            routingKey: rabbitMQConfiguration.OrderingRouteKey);
          var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto));
          //var body = dto.Serialize();
          if (body == null) return false;
          channel.BasicPublish(exchange: rabbitMQConfiguration.OrderingExchange,
                               routingKey: rabbitMQConfiguration.OrderingRouteKey,
                               basicProperties: null,
                               body: body);
          logger.LogInformation("in Ordering Queue Queued message {j}", JsonConvert.SerializeObject(dto));
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
