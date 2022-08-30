namespace Entities;

public class OrderEntity : BaseEntity
{
    public UserEntity Owner { get; set; }
    public virtual IList<ProductEntity> Products { get; set; }
    public string Description { get; set; }
    public string OrderStatus { get; set; }
    public bool Confirmed { get; set; }
}