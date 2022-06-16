using Entities;
using Entities.Goods;

namespace BLL.Services;

public interface IProductService
{
    public Task<IEnumerable<Product>> GetAllProducts(string token);
    public Task<Product> GetProductById(string token, int id);
    public Task<IEnumerable<Product>> GetProductsFromOrder(string token,Order order);
    public Task<IEnumerable<Product>> GetProductsByName(string name);
    public Task<bool> ChangeName(string token, string value, Product product);
    public Task<bool> ChangeDescription(string token, string value, Product product);
    public Task<bool> ChangeCost(string token, decimal value, Product product);
    public Task<bool> ChangeCategory(string token, Category category, Product product);
}