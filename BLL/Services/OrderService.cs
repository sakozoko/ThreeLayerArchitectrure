using AutoMapper;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Objects;
using BLL.Services.Interfaces;
using DAL;
using Entities;

namespace BLL.Services;

public class OrderService : BaseService, IOrderService
{
    public OrderService(IUnitOfWork unitOfWork, ITokenHandler tokenHandler, ILogger logger, IMapper mapper)
        : base(unitOfWork, tokenHandler, logger, mapper)
    {
    }


    public Task<Order> GetById(string token, int id)
    {
        return Task<Order>.Factory.StartNew(() =>
        {
            var order = Mapper.Map<Order>(UnitOfWork.OrderRepository.GetById(id));
            if (order is null) return null;
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            if (order.Owner.Id != requestUser.Id) ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);

            return order;
        });
    }

    public Task<int> Create(string token, string desc, int productId, int userId = 0)
    {
        return Task<int>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);

            var order = new Order { Description = desc, Owner = requestUser, Products = new List<Product>() };

            if (userId > 0)
            {
                var newOwner = Mapper.Map<User>(UnitOfWork.UserRepository.GetById(userId));
                if (newOwner is null)
                    return -1;
                ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
                order.Owner = Mapper.Map<User>(newOwner);
                Logger.Log($"Admin {requestUser.Name} created new order to user id {userId}");
            }

            if (productId > 0)
            {
                var product = Mapper.Map<Product>(UnitOfWork.ProductRepository.GetById(productId));
                if (product is null)
                    return -1;
                order.Products.Add(product);
            }

            return UnitOfWork.OrderRepository.Add(Mapper.Map<OrderEntity>(order));
        });
    }

    public Task<IEnumerable<Order>> GetAll(string token)
    {
        return Task<IEnumerable<Order>>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

            Logger.Log($"Admin {requestUser.Name} invoke to get all order");
            return Mapper.Map<IEnumerable<Order>>(UnitOfWork.OrderRepository.GetAll());
        });
    }

    public Task<IEnumerable<Order>> GetUserOrders(string token, int userId)
    {
        return Task<IEnumerable<Order>>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);
            ThrowAuthenticationExceptionIfUserIsNull(requestUser);
            Func<Order, bool> func = x => x.Owner.Id == requestUser.Id;

            if (userId > 0)
            {
                ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
                func = x => x.Owner.Id == userId;
                Logger.Log($"Admin {requestUser.Name} review orders of user with id {userId}");
            }

            return Mapper.Map<IEnumerable<Order>>(UnitOfWork.OrderRepository.GetAll()).Where(func);
        });
    }

    public Task<bool> AddProduct(string token, int productId, int orderId)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var product = Mapper.Map<Product>(UnitOfWork.ProductRepository.GetById(productId));
            return product is not null && ChangeProperty(token, orderId, x => x.Products.Add(product));
        });
    }

    public Task<bool> DeleteProduct(string token, int productId, int orderId)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var product = Mapper.Map<Product>(UnitOfWork.ProductRepository.GetById(productId));
            return product is not null && ChangeProperty(token, orderId, x => x.Products.Remove(product));
        });
    }

    public Task<bool> ChangeDescription(string token, string desc, int orderId)
    {
        return Task<bool>.Factory.StartNew(() =>
            !string.IsNullOrWhiteSpace(desc) && ChangeProperty(token, orderId, x => x.Description = desc));
    }

    public Task<bool> ChangeConfirmed(string token, bool confirmed, int orderId)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var order = Mapper.Map<Order>(UnitOfWork.OrderRepository.GetById(orderId));
            return order?.OrderStatus == OrderStatus.New && ChangeProperty(token, orderId, x => x.Confirmed = confirmed);
        });
    }

    public Task<bool> ChangeOrderStatus(string token, string status, int orderId)
    {
        return Task<bool>.Factory.StartNew(() =>
        {
            var order = Mapper.Map<Order>(UnitOfWork.OrderRepository.GetById(orderId));
            var orderStatus = Enum.Parse<OrderStatus>(status?.Replace(" ", "").Trim() ?? "New", true);
            return (TokenHandler.GetUser(token) is { IsAdmin: true }
                    || (orderStatus == OrderStatus.CanceledByUser &&
                        order?.OrderStatus != OrderStatus.Received)) &&
                   ChangeProperty(token, orderId, x => { x.OrderStatus = orderStatus; });
        });
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

    private bool ChangeProperty(string token, int orderId, Action<Order> act)
    {
        var order = Mapper.Map<Order>(UnitOfWork.OrderRepository.GetById(orderId));
        if (!ValidPermissionForModifyOrder(token, order)) return false;
        act?.Invoke(order);
        UnitOfWork.OrderRepository.Update(Mapper.Map<OrderEntity>(order));
        return true;
    }
}