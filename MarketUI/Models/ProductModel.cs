namespace MarketUI.Models;

public class ProductModel : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public CategoryModel Category { get; set; }
}