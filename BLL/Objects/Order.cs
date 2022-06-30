namespace BLL.Objects;

public class Order
{
    public int Id { get; set; }
    public User Owner { get; set; }
    public virtual List<Product> Products { get; set; }
    public string Description { get; set; }
    public OrderStatus OrderStatus { get; set; }
}