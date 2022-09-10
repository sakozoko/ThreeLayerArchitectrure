using System.Collections.Generic;
using System.Linq;

namespace MarketUI.Models
{
    public class OrderModel : BaseModel
    {
        public UserModel Owner { get; set; }
        public IList<ProductModel> Products { get; set; } = new List<ProductModel>();
        public string Description { get; set; }
        public string OrderStatus { get; set; }
        public bool Confirmed { get; set; }
        public decimal Total => Products.Sum(p => p.Cost);
    }
}