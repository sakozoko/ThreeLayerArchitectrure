using DAL.DataContext;
using Entities.Goods;

namespace DAL.Repositories;

public class ProductRepository : IRepository<Product>
{
    private readonly DbContext _dbContext;
    private readonly List<Product> _products;

    public ProductRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _products = _dbContext.Products;
    }


    public void Add(Product entity)
    {
        _products.Add(entity);
    }

    public IEnumerable<Product> GetAll()
    {
        return _products;
    }

    public Product GetById(int id)
    {
        return _products.Find(x => x.Id == id);
    }

    public void Delete(Product entity)
    {
        _products.Remove(entity);
    }
}