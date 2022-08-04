namespace BLL.Objects;

public class Product : BaseDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public Category Category { get; set; }
}