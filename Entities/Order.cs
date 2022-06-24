using Entities.Goods;

namespace Entities;

public class Order : BaseEntity
{
    public User Owner { get; set; }
    public virtual List<Product> Products { get; set; }
    public string Description { get; set; }
    public OrderStatus OrderStatus { get; set; }
}