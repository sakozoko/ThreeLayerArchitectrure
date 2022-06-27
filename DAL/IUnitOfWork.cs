using DAL.Util.Repositories;
using Entities;
using Entities.Goods;

namespace DAL.Util;

public interface IUnitOfWork
{
    public IRepository<User> UserRepository { get; }
    public IRepository<Product> ProductRepository { get; }
    public IRepository<Order> OrderRepository { get; }
    public IRepository<Category> CategoryRepository { get; }
}