using Common.Base.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customers.Domain.Entities
{
  [Table("BusinessLocations")]
  public class BusinessLocation : EntityBase
  {
    public static BusinessLocation Create(Guid customerBusinessId, string name, decimal latitude, decimal longitude)
      => new BusinessLocation(customerBusinessId, name, latitude, longitude);
    private BusinessLocation(Guid customerBusinessId, string name, decimal latitude, decimal longitude)
    {
      CustomerBusinessId = customerBusinessId;
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Latitude = latitude;
      Longitude = longitude;
    }
    public Guid CustomerBusinessId { get; set; }
    public string Name { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public CustomerBusiness? CustomerBusiness { get; set; }
  }
}
