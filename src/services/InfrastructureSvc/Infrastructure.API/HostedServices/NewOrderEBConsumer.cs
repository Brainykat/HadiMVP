using Infrastructure.API.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Dtos;
using System.Text;

namespace Infrastructure.API.HostedServices
{
  
  public class NewOrderEBConsumer : IHostedService, IDisposable
  {
    public IServiceProvider Services { get; }
    private readonly ILogger<NewOrderEBConsumer> logger;
    private readonly RabbitMQConfiguration rabbitMQConfiguration;
    private IModel channel;
    private IConnection connection;

    public NewOrderEBConsumer(IServiceProvider services, ILogger<NewOrderEBConsumer> logger,
      IOptions<RabbitMQConfiguration> optionsMonitor)
    {
      Services = services ?? throw new ArgumentNullException(nameof(services));
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.rabbitMQConfiguration = optionsMonitor.Value;
      InitializeRabbitMqListener();
    }
    private void InitializeRabbitMqListener()
    {
      try
      {
        //var con = configuration.GetSection(nameof(RabbitMQConfiguration)).<RabbitMQConfiguration>();
        var factory = new ConnectionFactory
        {
          HostName = rabbitMQConfiguration.HostName,
          UserName = rabbitMQConfiguration.UserName,
          Password = rabbitMQConfiguration.Password,
          Port = rabbitMQConfiguration.Port,
          ClientProvidedName = rabbitMQConfiguration.SubscriptionClientName

        };

        connection = factory.CreateConnection();
        connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        channel = connection.CreateModel();
        //_channel.QueueDeclare(queue: rabbitMQConfiguration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
      }
      catch (Exception ex)
      {
        logger.LogError($"{GetType().FullName} ==> Error {ex.Message} inner {ex.InnerException?.InnerException}");
      }

    }
    public async Task Register()
    {
      try
      {
        channel.ExchangeDeclare(exchange: rabbitMQConfiguration.OrderingExchange, type: rabbitMQConfiguration.OrderingExchangeType,
            durable: true, autoDelete: true);
        channel.QueueDeclare(queue: rabbitMQConfiguration.OrderingQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: rabbitMQConfiguration.OrderingQueueName,
                          exchange: rabbitMQConfiguration.OrderingExchange,
                          routingKey: rabbitMQConfiguration.OrderingRouteKey);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (ch, ea) =>
        {
          var content = Encoding.UTF8.GetString(ea.Body.ToArray());
          logger.LogInformation("New Event on Order Queue Queued message {j}", content);
          var queDto = JsonConvert.DeserializeObject<NewOrderEBDto>(content);

          var consume = await HandleMessage(queDto);
          if (consume)
          {
            channel.BasicAck(ea.DeliveryTag, false);
          }
        };
        consumer.Shutdown += OnConsumerShutdown;
        consumer.Registered += OnConsumerRegistered;
        consumer.Unregistered += OnConsumerUnregistered;
        consumer.ConsumerCancelled += OnConsumerCancelled;
        channel.BasicQos(0, 1, false);
        channel.BasicConsume(queue: rabbitMQConfiguration.OrderingQueueName, autoAck: false, consumer);
      }
      catch (Exception ex)
      {
        logger.LogError($"{GetType().FullName} ==> Error {ex.Message} inner {ex.InnerException?.InnerException}");
      }
      return;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
      await Register();
      return;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      connection.Close();
      return Task.CompletedTask;
    }
    private async Task<bool> HandleMessage(NewOrderEBDto dto)
    {
      try
      {
        logger.LogInformation("Handling data from OrderQue");
        using (var scope = Services.CreateScope())
        {
          var taxService = scope.ServiceProvider.GetRequiredService<IGovTaxLoggingService>();
          var response = await taxService.LogTaxData(dto);
          return response;
        }
      }
      catch (Exception ex)
      {
        logger.LogError($"{GetType().FullName}  ==> Error {ex.Message} inner {ex.InnerException?.InnerException}");
        return false;
      }
    }

    private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
    {
    }

    private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
    {
    }

    private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
    {
    }

    private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
    {
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
    }

    public void Dispose()
    {
      channel.Close();
      connection.Close();

    }
  }
}
