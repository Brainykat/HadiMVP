using System.ComponentModel.DataAnnotations;

namespace OrderingDtos
{
  public class OrderDto
  {
    /// <summary>
    /// TODO: This should be retrieved from user claims
    /// </summary>
    public Guid BusinessId { get; set; }

    [Required]
    public string OrderDescription { get; set; }
    /// <summary>
    /// TODO: This should be retrieved from user claims
    /// </summary>
    public Guid User { get; set; }

    [Required]
    public List<ItemDto> Items { get; set; }
  }
}