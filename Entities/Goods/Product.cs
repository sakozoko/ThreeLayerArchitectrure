namespace Entities.Goods;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public Category Category { get; set; }

    public override string ToString()
    {
        return $"{Id} " +
               $"{Name} \t " +
               $"{Category} \t " +
               $"{Cost} \t " +
               $"{Description}";
    }
}