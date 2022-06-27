using Entities;
using Entities.Goods;

namespace DAL.Util.DataContext;

public interface IDbContext
{
    public IList<Order> Orders { get; }
    public IList<Product> Products { get; }
    public IList<User> Users { get; }
    public IList<Category> Categories { get; }
    public IList<T> Set<T>();
    public Task Save();
}