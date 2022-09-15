namespace RabbitMQ.Dtos
{
  public class NewOrderEBDto
  {
    public NewOrderEBDto(Guid order_id, string platform_code, decimal order_amount)
    {
      this.order_id = order_id;
      this.platform_code = platform_code;
      this.order_amount = order_amount;
    }

    public Guid order_id { get; set; } //": "dac3549d-aea2-4957-91dc-618f2e2c77f7",
    public string platform_code {get;set;} //": "022",
    public decimal order_amount {get;set;} //": 40000

  }
}
