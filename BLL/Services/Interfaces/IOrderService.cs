using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Objects;

namespace BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetById(string token, int id);
        Task<IEnumerable<Order>> GetAll(string token);
        Task<IEnumerable<Order>> GetUserOrders(string token, int userId = 0);
        Task<int> Create(string token, string desc, int productId, int userId = 0);
        Task<bool> AddProduct(string token, int productId, int orderId);
        Task<bool> DeleteProduct(string token, int productId, int orderId);
        Task<bool> ChangeOrderStatus(string token, string status, int orderId);
        Task<bool> ChangeDescription(string token, string desc, int orderId);
        Task<bool> ChangeConfirmed(string token, bool confirmed, int orderId);
    }
}