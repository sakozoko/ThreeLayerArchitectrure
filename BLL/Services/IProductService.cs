using Entities;
using Entities.Goods;

namespace BLL.Services;

public interface IProductService
{
    public Task<IEnumerable<Product>> GetAllProducts(string token);
    public Task<Product> GetProductById(string token, int id);
    public Task<IEnumerable<Product>> GetProductsFromOrder(string token,Order order);
    public Task<IEnumerable<Product>> GetProductsByName(string name);
}