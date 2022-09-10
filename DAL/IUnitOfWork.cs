using DAL.Repositories;
using Entities;

namespace DAL
{
    public interface IUnitOfWork
    {
        public IRepository<UserEntity> UserRepository { get; }
        public IRepository<ProductEntity> ProductRepository { get; }
        public IRepository<OrderEntity> OrderRepository { get; }
        public IRepository<CategoryEntity> CategoryRepository { get; }
    }
}