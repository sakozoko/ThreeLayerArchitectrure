namespace BLL.Objects;

public class Order : BaseDto
{
    public User Owner { get; set; }
    public List<Product> Products { get; set; }
    public string Description { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public bool Confirmed { get; set; }
}