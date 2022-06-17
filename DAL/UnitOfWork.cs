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

    public IRepository<User> UserRepository =>
        _userRepository ??= new GenericRepository<User>(_context);

    public IRepository<Product> ProductRepository =>
        _productRepository ??= new GenericRepository<Product>(_context);

    public IRepository<Order> OrderRepository =>
        _orderRepository ??= new GenericRepository<Order>(_context);

    public IRepository<Category> CategoryRepository => 
        _categoryRepository ??= new GenericRepository<Category>(_context);
    


}