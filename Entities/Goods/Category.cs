namespace Entities.Goods;

public class Category
{
    public Category(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }

    public string Name { get; }

    public override string ToString()
    {
        return $"{Name} category";
    }
}