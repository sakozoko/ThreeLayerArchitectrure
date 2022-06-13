using DAL.DataContext;
using DAL.Repositories;
using Entities;
using Entities.Goods;
using Entities.User;

namespace DAL;

public class UnitOfWork : IUnitOfWork
{
    private DbContext _context = new ();
    private  IRepository<IUser> _userRepository;
    private  IRepository<Product> _productRepository;
    private  IRepository<Order> _orderRepository;
    private  IRepository<Category> _categoryRepository;


    public IRepository<IUser> UserRepository
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