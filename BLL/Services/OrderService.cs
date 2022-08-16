using AutoMapper;
using BLL.Extension;
using BLL.Helpers.Token;
using BLL.Objects;
using BLL.Services.Interfaces;
using BLL.Util.Logger;
using DAL.Repositories;
using Entities;

namespace BLL.Services;

public class OrderService : BaseService<OrderEntity>, IOrderService
{
    public OrderService(IRepository<OrderEntity> repository, ITokenHandler tokenHandler, ILogger logger, IMapper mapper)
        : base(repository, tokenHandler, logger, mapper)
    {
    }


    public Task<Order> GetById(string token, int id)
    {
        return Task<Order>.Factory.StartNew(() =>
        {
            var order = Mapper.Map<Order>(Repository.GetById(id));
            if (order is null) return null;
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            if (order.Owner.Id != requestUser.Id) ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);

            return order;
        });
    }

    public Task<int> Create(string token, string desc, Product product, User user = null)
    {
        return Task<int>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            var order = new Order { Description = desc, Owner = requestUser, Products = new List<Product>() };

            if (user is not null)
            {
                ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
                order.Owner = user;
                Logger.Log($"Admin {requestUser.Name} created new order to user id {user.Id}");
            }

            if (product is not null) order.Products.Add(product);
            return Repository.Add(Mapper.Map<OrderEntity>(order));
        });
    }

    public Task<IEnumerable<Order>> GetAll(string token)
    {
        return Task<IEnumerable<Order>>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

            Logger.Log($"Admin {requestUser.Name} invoke to get all order");
            return Mapper.Map<IEnumerable<Order>>(Repository.GetAll());
        });
    }

    public Task<IEnumerable<Order>> GetUserOrders(string token, User user = null)
    {
        return Task<IEnumerable<Order>>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNull(requestUser);
            Func<Order, bool> func = x => x.Owner.Id == requestUser.Id;

            if (user is not null)
            {
                ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
                func = x => x.Owner.Id == user.Id;
                Logger.Log($"Admin {requestUser.Name} review orders of user with id {user.Id}");
            }

            return Mapper.Map<IEnumerable<Order>>(Repository.GetAll()).Where(func);
        });
    }

    public Task<bool> AddProduct(string token, Product product, Order order)
    {
        return Task<bool>.Factory.StartNew(() => 
            product is not null && ChangeProperty(token,order,x=>x.Products.Add(product)));

    }

    public Task<bool> DeleteProduct(string token, Product product, Order order)
    {
        return Task<bool>.Factory.StartNew(() => 
            product is not null && ChangeProperty(token,order,x=>x.Products.Remove(product)));
    }

    public Task<bool> ChangeDescription(string token, string desc, Order order)
    {
        return Task<bool>.Factory.StartNew(() =>
            !string.IsNullOrWhiteSpace(desc) && ChangeProperty(token, order, x => x.Description = desc));
    }

    public Task<bool> ChangeOrderStatus(string token, OrderStatus status, Order order)
    {
        return Task<bool>.Factory.StartNew(() => (TokenHandler.GetUser(token) is { IsAdmin: true }
                                                  || status == OrderStatus.CanceledByUser && order.OrderStatus != OrderStatus.Received) &&
                                                 ChangeProperty(token, order, x => { x.OrderStatus = status; }));
    }

    private bool ValidPermissionForModifyOrder(string token, Order order)
    {
        if (order is null) return false;
        
        var requestUser = TokenHandler.GetUser(token);
        
        ThrowAuthenticationExceptionIfUserIsNull(requestUser);
        
        if (order.Owner.Id == requestUser.Id) return true;
        
        ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
        
        Logger.Log($"Admin {requestUser.Name} change property for order with id {order.Id}");

        return true;
    }
    public Task<bool> SaveOrder(string token, Order order)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var result = true;
            if (!ValidPermissionForModifyOrder(token, order)) return false;
            var repositoryOrder = Mapper.Map<Order>(Repository.GetById(order.Id));
            if (repositoryOrder is not null)
            {
                if(repositoryOrder.Description!=order.Description)
                    result = ChangeDescription(token, order.Description, repositoryOrder).Result;
                if(repositoryOrder.OrderStatus!=order.OrderStatus)
                    result = ChangeOrderStatus(token, order.OrderStatus, repositoryOrder).Result;
                repositoryOrder.Products=order.Products;
                Repository.Update(Mapper.Map<OrderEntity>(repositoryOrder));
            }
            if (result)
            {
                Repository.Add(Mapper.Map<OrderEntity>(order));
            }

            return result;
        });
    }

    private bool ChangeProperty(string token, Order order, Action<Order> act)
    {
        if (!ValidPermissionForModifyOrder(token, order)) return false;
        act?.Invoke(order);
        return true;
    }
}