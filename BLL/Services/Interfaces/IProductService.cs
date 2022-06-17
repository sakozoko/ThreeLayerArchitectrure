using Entities;
using Entities.Goods;

namespace BLL.Services.Interfaces;

public interface IProductService
{
    public Task<IEnumerable<Product>> GetAll(string token);
    public Task<Product> GetById(string token, int id);
    public Task<IEnumerable<Product>> GetFromOrder(string token,Order order);
    public Task<IEnumerable<Product>> GetByName(string name);
    public Task<bool> ChangeName(string token, string value, Product product);
    public Task<bool> ChangeDescription(string token, string value, Product product);
    public Task<bool> ChangeCost(string token, decimal value, Product product);
    public Task<bool> ChangeCategory(string token, Category category, Product product);
    public Task<bool> Remove(string token, int id);
    public Task<bool> Remove(string token, Product entity);
}