using DAL.DataContext;
using DAL.Repositories;
using Entities;

namespace DAL;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContext _context;
    private IRepository<CategoryEntity> _categoryRepository;
    private IRepository<OrderEntity> _orderRepository;
    private IRepository<ProductEntity> _productRepository;
    private IRepository<UserEntity> _userRepository;

    public UnitOfWork(IDbContext context)
    {
        _context = context;
    }

    public IRepository<UserEntity> UserRepository =>
        _userRepository ??= new GenericRepository<UserEntity>(_context);

    public IRepository<ProductEntity> ProductRepository =>
        _productRepository ??= new GenericRepository<ProductEntity>(_context);

    public IRepository<OrderEntity> OrderRepository =>
        _orderRepository ??= new GenericRepository<OrderEntity>(_context);

    public IRepository<CategoryEntity> CategoryRepository =>
        _categoryRepository ??= new GenericRepository<CategoryEntity>(_context);
}