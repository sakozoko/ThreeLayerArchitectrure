using BLL.Objects;

namespace BLL.Services.Interfaces;

public interface IOrderService
{
    Task<Order> GetById(string token, int id);
    Task<IEnumerable<Order>> GetAll(string token);
    Task<IEnumerable<Order>> GetUserOrders(string token, User user = null);
    Task<int> Create(string token, string desc, Product product, User user = null);
    Task<bool> AddProduct(string token, Product product, Order order);
    Task<bool> DeleteProduct(string token, Product product, Order order);
    Task<bool> ChangeOrderStatus(string token, OrderStatus status, Order order);
    bool SaveOrder(string token, Order order);
    Task<bool> ChangeDescription(string token, string desc, Order order);
}