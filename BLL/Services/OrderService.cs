using BLL.Helpers;
using BLL.Logger;
using BLL.Services;
using DAL.Repositories;
using Entities;
using Entities.Goods;

namespace Bll.Services;

public class OrderService : IOrderService
{
    private readonly ILogger _logger;
    private readonly IRepository<Order> _orderRepository;
    private readonly CustomTokenHandler _tokenHandler;
    private const string Msg = "Token is bad";
    public OrderService(IRepository<Order> repository, CustomTokenHandler tokenHandler, ILogger logger)
    {
        _orderRepository = repository;
        _tokenHandler = tokenHandler;
        _logger = logger;
    }

    public Task<Order> GetOrderById(int id, string token)
    {
        return Task<Order>.Factory.StartNew(() =>
        {
            var order = _orderRepository.GetById(id);
            var requestUser = _tokenHandler.GetUser(token);
            if (order is not null && order.Owner == requestUser) return order;

            if (requestUser is { IsAdmin: true })
            {
                return order;
            }

            var msg = $"{requestUser?.Name} do not have permission";
            _logger.Log($"{nameof(OrderService)}.{nameof(GetOrderById)} throw exception." + msg);
            throw new ServiceException(nameof(OrderService), msg);
        });
    }

    public Task<int> CreateOrder(string token, string desc, Product product, User user = null)
    {
        return Task<int>.Factory.StartNew(() =>
        {
            var requestUser = _tokenHandler.GetUser(token);
            if (user is not null)
            {
                if (requestUser is { IsAdmin: true })
                {
                    var order = new Order { Description = desc, Owner = user, Products = new List<Product>() };
                    if (product != null)
                        order.Products.Add(product);
                    var id = _orderRepository.Add(order);
                    _logger.Log($"Admin {requestUser.Name} created new order with id {id} to user id {user.Id}");
                    return id;
                }
            }
            else if (requestUser is not null)
            {
                var order = new Order { Description = desc, Owner = requestUser, Products = new List<Product>() };
                if (product != null)
                    order.Products.Add(product);
                return _orderRepository.Add(order);
            }
            
            _logger.LogException($"{nameof(OrderService)}.{nameof(CreateOrder)} throw exception. " + Msg);
            throw new ServiceException(nameof(OrderService), Msg);
        });
    }

    public Task<IEnumerable<Order>> GetAllOrders(string token)
    {
        return Task<IEnumerable<Order>>.Factory.StartNew(() =>
        {
            var user = _tokenHandler.GetUser(token);
            if (user is { IsAdmin: true })
            {
                _logger.Log($"Admin {user.Name} invoked to get all order");
                return _orderRepository.GetAll();
            }

            var msg = $"{user?.Name} do not have permission";
            _logger.LogException($"{nameof(OrderService)}.{nameof(GetAllOrders)} throw exception." + msg);
            throw new ServiceException(nameof(OrderService), msg);
        });
    }

    public Task<IEnumerable<Order>> GetUserOrders(string token, User user = null)
    {
        return Task<IEnumerable<Order>>.Factory.StartNew(() =>
        {
            var requestUser = _tokenHandler.GetUser(token);
            if (user is null)
            {
                if (requestUser is not null)
                    return _orderRepository.GetAll().Where(x => x.Owner == requestUser);
                _logger.LogException($"{nameof(OrderService)}.{nameof(GetUserOrders)} throw exception." + Msg);
                throw new ServiceException(nameof(OrderService), Msg);
            }

            if (requestUser is { IsAdmin: true })
            {
                _logger.Log($"Admin {requestUser.Name} viewed orders of user with id {user.Id}");
                return _orderRepository.GetAll().Where(x => x.Owner == user);
            }
            _logger.LogException($"{nameof(OrderService)}.{nameof(GetUserOrders)} throw exception." + Msg);
            throw new ServiceException(nameof(OrderService), Msg);
        });
    }

    public Task<bool> AddProduct(Order order, Product product, string token)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            if (order is null || product is null)
                return false;
            var requestUser = _tokenHandler.GetUser(token);

            if (order.Owner.Equals(requestUser))
            {
                order.Products.Add(product);
                return true;
            }

            _logger.Log(
                $"Order owner don't equal token user, Owner.Id: {order.Owner.Id} Token.Id: {requestUser?.Id}");
            return false;
        });
    }

    public Task<bool> DeleteProduct(Order order, Product product, string token)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            if (order is null || product is null)
                return false;
            var requestUser = _tokenHandler.GetUser(token);
            if (order.Owner.Equals(requestUser) || requestUser is { IsAdmin: true })
            {
                if (requestUser is { IsAdmin: true })
                    _logger.Log($"Admin {requestUser.Name} removed product with id {product.Id} from order " +
                                $"with id {order.Id}");
                return order.Products.Remove(product);
            }

            _logger.Log(
                $"Order owner don't equal token user, Owner.Id: {order.Owner.Id} Token.Id: {requestUser?.Id}");
            return false;
        });
    }
}