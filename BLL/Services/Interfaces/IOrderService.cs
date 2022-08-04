using BLL.Objects;

namespace BLL.Services.Interfaces;

public interface IOrderService
{
    public Task<Order> GetById(string token, int id);
    public Task<IEnumerable<Order>> GetAll(string token);
    public Task<IEnumerable<Order>> GetUserOrders(string token, User user = null);
    public Task<int> Create(string token, string desc, Product product, User user = null);
    public Task<bool> AddProduct(string token, Product product, Order order);
    public Task<bool> DeleteProduct(string token, Product product, Order order);
    public Task<bool> ChangeOrderStatus(string token, OrderStatus status, Order order);
    public bool SaveOrder(string token, Order order);
    public Task<bool> ChangeDescription(string token, string desc, Order order);
}