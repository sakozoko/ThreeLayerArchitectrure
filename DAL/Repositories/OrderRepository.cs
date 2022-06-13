using DAL.DataContext;
using Entities;

namespace DAL.Repositories;

public class OrderRepository:IRepository<Order>
{
    private DbContext _dbContext;
    private List<Order> _orders;

    public OrderRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _orders = _dbContext.Orders;
    }
    public void Add(Order entity)
    {
        _orders.Add(entity);
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