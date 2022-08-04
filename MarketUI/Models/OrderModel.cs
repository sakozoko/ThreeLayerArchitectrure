using System.Collections.Generic;

namespace MarketUI.Models;

public class OrderModel:BaseModel
{
    public UserModel Owner { get; set; }
    public virtual IList<ProductModel> Products { get; set; }
    public string Description { get; set; }
    public string OrderStatus { get; set; }
}