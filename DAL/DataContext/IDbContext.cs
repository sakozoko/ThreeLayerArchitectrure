using Entities;

namespace DAL.DataContext;

public interface IDbContext
{
    public IList<OrderEntity> Orders { get; }
    public IList<ProductEntity> Products { get; }
    public IList<UserEntity> Users { get; }
    public IList<CategoryEntity> Categories { get; }
    public IList<T> Set<T>();
    public Task Save();
}