using DAL.DataContext;
using DAL.Repositories;
using Entities;
using Entities.Goods;

namespace DAL;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context = new();
    private IRepository<Category> _categoryRepository;
    private IRepository<Order> _orderRepository;
    private IRepository<Product> _productRepository;
    private IRepository<User> _userRepository;
    private IRepository<OrderStatus> _orderStatusRepository;

    public IRepository<User> UserRepository =>
        _userRepository ??= new UserRepository(_context);

    public IRepository<Product> ProductRepository =>
        _productRepository ??= new ProductRepository(_context);

    public IRepository<Order> OrderRepository =>
        _orderRepository ??= new OrderRepository(_context);

    public IRepository<Category> CategoryRepository => 
        _categoryRepository ??= new CategoryRepository(_context);

    public IRepository<OrderStatus> OrderStatusRepository =>
        _orderStatusRepository ??= new OrderStatusRepository(_context);


}