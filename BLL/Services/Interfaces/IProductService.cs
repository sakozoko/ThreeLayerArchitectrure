using BLL.Objects;

namespace BLL.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAll(string token);
    Task<Product> GetById(string token, int id);
    Task<IEnumerable<Product>> GetFromOrder(string token, Order order);
    Task<IEnumerable<Product>> GetByName(string name);
    Task<bool> ChangeName(string token, string value, Product product);
    Task<bool> ChangeDescription(string token, string value, Product product);
    Task<bool> ChangeCost(string token, decimal value, Product product);
    Task<bool> ChangeCategory(string token, Category category, Product product);
    Task<bool> Remove(string token, int id);
    Task<bool> Remove(string token, Product product);
}