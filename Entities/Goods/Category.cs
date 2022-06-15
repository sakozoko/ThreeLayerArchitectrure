namespace Entities.Goods;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    public override string ToString()
    {
        return $"{Name} category";
    }
}