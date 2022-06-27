﻿using BLL.Util.Helpers.Token;
using BLL.Util.Logger;
using BLL.Util.Services.Interfaces;
using DAL.Util.Repositories;
using Entities;
using Entities.Goods;

namespace BLL.Util.Services;

public class OrderService : BaseService<Order>, IOrderService
{
    public OrderService(IRepository<Order> repository, ITokenHandler tokenHandler, ILogger logger) :
        base(repository, tokenHandler, logger)
    {
    }

    public Task<Order> GetById(string token, int id)
    {
        return Task<Order>.Factory.StartNew(() =>
        {
            var order = Repository.GetById(id);
            if (order is null) return null;
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            if (order.Owner != requestUser) ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);

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
            return Repository.Add(order);
        });
    }

    public Task<IEnumerable<Order>> GetAll(string token)
    {
        return Task<IEnumerable<Order>>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

            Logger.Log($"Admin {requestUser.Name} invoke to get all order");
            return Repository.GetAll();
        });
    }

    public Task<IEnumerable<Order>> GetUserOrders(string token, User user = null)
    {
        return Task<IEnumerable<Order>>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNull(requestUser);
            Func<Order, bool> func = x => x.Owner == requestUser;

            if (user is not null)
            {
                ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
                func = x => x.Owner == user;
                Logger.Log($"Admin {requestUser.Name} review orders of user with id {user.Id}");
            }

            return Repository.GetAll().Where(func);
        });
    }

    public Task<bool> AddProduct(string token, Product product, Order order)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            if (order is null || product is null)
                return false;
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            if (order.Owner != requestUser)
            {
                ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
                Logger.Log(
                    $"Admin {requestUser.Name} added product with id {product.Id} from order with id {order.Id}");
            }

            order.Products.Add(product);

            return true;
        });
    }

    public Task<bool> DeleteProduct(string token, Product product, Order order)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            if (order is null || product is null)
                return false;
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            if (order.Owner != requestUser)
            {
                ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
                Logger.Log(
                    $"Admin {requestUser.Name} removed product with id {product.Id} from order with id {order.Id}");
            }

            return order.Products.Remove(product);
        });
    }

    public Task<bool> ChangeDescription(string token, string desc, Order order)
    {
        return Task<bool>.Factory.StartNew(() =>
            !string.IsNullOrWhiteSpace(desc) && ChangeProperty(token, order, x => x.Description = desc));
    }

    public Task<bool> ChangeOrderStatus(string token, OrderStatus status, Order order)
    {
        return Task<bool>.Factory.StartNew(() => ChangeProperty(token, order, x =>
        {
            if (TokenHandler.GetUser(token) is { IsAdmin: false }
                && ((status == OrderStatus.CanceledByUser
                     && order.OrderStatus == OrderStatus.Received)
                    || (status == OrderStatus.Received
                        && order.OrderStatus == OrderStatus.Completed)))
                x.OrderStatus = status;

            if (TokenHandler.GetUser(token) is { IsAdmin: true }) x.OrderStatus = status;
        }));
    }

    private bool ChangeProperty(string token, Order order, Action<Order> act)
    {
        if (order is null) return false;

        var requestUser = TokenHandler.GetUser(token);

        ThrowAuthenticationExceptionIfUserIsNull(requestUser);
        if (order.Owner != requestUser)
        {
            ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
            Logger.Log($"Admin {requestUser.Name} change property for order with id {order.Id}");
        }

        act.Invoke(order);
        return true;
    }
}