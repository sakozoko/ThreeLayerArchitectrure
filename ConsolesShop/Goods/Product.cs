namespace ConsolesShop.Goods;

public class Product
{
    public Product(int id, string name, string description, decimal cost, Category category = null)
    {
        Id = id;
        Name = name;
        Description = description;
        Cost = cost;
        Category = category;
    }

    public int Id { get; set; }
    public string Name { get; }
    public string Description { get; set; }
    public decimal Cost { get; }
    public Category Category { get; }
}