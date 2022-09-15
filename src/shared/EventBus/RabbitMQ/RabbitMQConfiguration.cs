namespace RabbitMQ
{
  public class RabbitMQConfiguration
  {
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public int EventBusRetryCount { get; set; }
    public string SubscriptionClientName { get; set; }
    public string CustomerQueueName { get; set; } 
    public string CustomerRouteKey { get; set; } 
    public string CustomerExchange { get; set; } 
    public string CustomerExchangeType { get; set; }

    public string OrderingQueueName { get; set; }
    public string OrderingRouteKey { get; set; }
    public string OrderingExchange { get; set; }
    public string OrderingExchangeType { get; set; }
  }
}
