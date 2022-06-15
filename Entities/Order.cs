using Entities.Goods;

namespace Entities;

public class Order
{
    public User Owner { get; set; }
    public virtual List<Product> Products { get; set; }
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsDelivered { get; set; }
    public bool IsCancelled { get; set; }

    public override string ToString()
    {
        return $"{Id} \t {Description} \t {IsCancelled} \t {IsDelivered} ";
    }
}