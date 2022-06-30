namespace Entities;

public class ProductEntity : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public CategoryEntity Category { get; set; }
}