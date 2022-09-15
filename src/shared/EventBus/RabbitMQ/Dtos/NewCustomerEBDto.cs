namespace RabbitMQ.Dtos
{
  public class NewCustomerEBDto
  {
    public NewCustomerEBDto(Guid customerBusinessId,string name, string email, string phone, string role, Dictionary<string, string> claims)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Email = email ?? throw new ArgumentNullException(nameof(email));
      Phone = phone ?? throw new ArgumentNullException(nameof(phone));
      Role = role ?? throw new ArgumentNullException(nameof(role));
      Claims = claims ?? throw new ArgumentNullException(nameof(claims));
      CustomerBusinessId = customerBusinessId;
    }
    public Guid CustomerBusinessId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Role { get; set; }
    public Dictionary<string,string> Claims { get; set; } = new Dictionary<string,string>();
  }
}
