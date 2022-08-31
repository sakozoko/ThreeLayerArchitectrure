using BLL.Objects;

namespace BLL.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAll(string token);
    Task<Product> GetById(string token, int id);
    Task<IEnumerable<Product>> GetByName(string name);
    Task<int> Create(string token, string name, string desc, decimal cost, int categoryId);
    Task<bool> ChangeName(string token, string value, int productId);
    Task<bool> ChangeDescription(string token, string value, int productId);
    Task<bool> ChangeCost(string token, decimal value, int productId);
    Task<bool> ChangeCategory(string token, int categoryId, int productId);
    Task<bool> Remove(string token, int id);
}