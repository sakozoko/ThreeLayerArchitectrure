namespace ConsolesShop.Models.Goods;

public class Category
{
    public Category(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public override string ToString()
    {
        return $"{Name} category";
    }
}