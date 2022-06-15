using DAL.DataContext;
using DAL.Repositories;
using Entities;
using Entities.Goods;

namespace DAL;

public class UnitOfWork : IUnitOfWork
{
    private IRepository<Category> _categoryRepository;
    private readonly DbContext _context = new();
    private IRepository<Order> _orderRepository;
    private IRepository<Product> _productRepository;
    private IRepository<User> _userRepository;


    public IRepository<User> UserRepository
    {
        get
        {
            _userRepository ??= new UserRepository(_context);
            return _userRepository;
        }
    }

    public IRepository<Product> ProductRepository
    {
        get
        {
            _productRepository ??= new ProductRepository(_context);
            return _productRepository;
        }
    }

    public IRepository<Order> OrderRepository
    {
        get
        {
            _orderRepository ??= new OrderRepository(_context);
            return _orderRepository;
        }
    }

    public IRepository<Category> CategoryRepository
    {
        get
        {
            _categoryRepository ??= new CategoryRepository(_context);
            return _categoryRepository;
        }
    }
}