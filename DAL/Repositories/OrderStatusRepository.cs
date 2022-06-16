using DAL.DataContext;
using Entities;

namespace DAL.Repositories;

public class OrderStatusRepository : IRepository<OrderStatus>
{
    private readonly DbContext _dbContext;
    private readonly List<OrderStatus> _orderStatusList;
    private int _lastId;

    public OrderStatusRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _orderStatusList = dbContext.OrderStatuses;
        _lastId = _orderStatusList.Count;
    }

    public int Add(OrderStatus entity)
    {
        entity.Id = ++_lastId;
        _orderStatusList.Add(entity);
        return _lastId;
    }

    public IEnumerable<OrderStatus> GetAll()
    {
        return _orderStatusList;
    }

    public OrderStatus GetById(int id)
    {
        return _orderStatusList.FirstOrDefault(x => x.Id == id);
    }

    public void Delete(OrderStatus entity)
    {
        _orderStatusList.Remove(entity);
    }
}