using DAL.Repositories;
using Entities;
using Entities.Goods;
using Entities.User;

namespace DAL;

public interface IUnitOfWork
{
    public IRepository<IUser> UserRepository { get; }
    public IRepository<Product> ProductRepository { get; }
    public IRepository<Order> OrderRepository { get; }
    public IRepository<Category> CategoryRepository { get; }
}