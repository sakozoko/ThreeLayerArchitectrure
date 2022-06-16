using DAL.DataContext;
using Entities;

namespace DAL.Repositories;

public class OrderRepository : IRepository<Order>
{
    private readonly DbContext _dbContext;
    private readonly List<Order> _orders;
    private int _lastId;

    public OrderRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _orders = _dbContext.Orders;
        _lastId = _orders.Count;
    }

    public int Add(Order entity)
    {
        entity.Id = ++_lastId;
        _orders.Add(entity);
        return _lastId;
    }

    public IEnumerable<Order> GetAll()
    {
        return _orders;
    }

    public Order GetById(int id)
    {
        return _orders.Find(x => x.Id == id);
    }

    public void Delete(Order entity)
    {
        _orders.Remove(entity);
    }
}