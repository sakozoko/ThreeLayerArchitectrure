using DAL.Repositories;
using Entities;
using Entities.Goods;

namespace DAL;

public interface IUnitOfWork
{
    public IRepository<User> UserRepository { get; }
    public IRepository<Product> ProductRepository { get; }
    public IRepository<Order> OrderRepository { get; }
    public IRepository<Category> CategoryRepository { get; }
}