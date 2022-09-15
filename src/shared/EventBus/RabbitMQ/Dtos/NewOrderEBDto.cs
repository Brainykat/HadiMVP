namespace RabbitMQ.Dtos
{
  public class NewOrderEBDto
  {
    public Guid order_id { get; set; } //": "dac3549d-aea2-4957-91dc-618f2e2c77f7",
    public int platform_code {get;set;} //": "022",
    public decimal order_amount {get;set;} //": 40000

  }
}
