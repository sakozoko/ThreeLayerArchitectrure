using Entities;
using Entities.Goods;

namespace BLL.Services;

public interface IOrderService
{
    public Task<Order> GetOrderById(int id, string token);
    public Task<IEnumerable<Order>> GetAllOrders(string token);
    public Task<IEnumerable<Order>> GetUserOrders(string token, User user = null);
    public Task<int> CreateOrder(string token, string desc, Product product, User user = null);

    public Task<bool> AddProduct(Order order, Product product, string token);
    public Task<bool> DeleteProduct(Order order, Product product, string token);
}