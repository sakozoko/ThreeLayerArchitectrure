namespace Entities.Goods;

public class Product:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public Category Category { get; set; }
}