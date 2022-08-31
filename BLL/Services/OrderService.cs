using AutoMapper;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Objects;
using BLL.Services.Interfaces;
using DAL;
using Entities;

namespace BLL.Services;

internal sealed class OrderService : BaseService, IOrderService
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

    public Task<int> Create(string token, string desc, int productId, int userId = 0) =>
        Task<int>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNull(requestUser);
            var product = Mapper.Map<Product>(UnitOfWork.ProductRepository.GetById(productId));
            if (product is null)
                return -1;
            var order = new Order { Description = desc, Owner = Mapper.Map<User>(requestUser), Products = new List<Product>{product} };

            if (userId > 0)
            {
                var newOwner = Mapper.Map<User>(UnitOfWork.UserRepository.GetById(userId));
                if (newOwner is null)
                    return -1;
                ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
                order.Owner = Mapper.Map<User>(newOwner);
                Logger.Log($"Admin {requestUser.Name} created new order to user id {userId}");
            }
            return UnitOfWork.OrderRepository.Add(Mapper.Map<OrderEntity>(order));
        });

    public Task<IEnumerable<Order>> GetAll(string token) =>
        Task<IEnumerable<Order>>.Factory.StartNew(() =>
        {
            var requestUser = TokenHandler.GetUser(token);

            ThrowAuthenticationExceptionIfUserIsNullOrNotAdmin(requestUser);

            Logger.Log($"Admin {requestUser.Name} invoke to get all order");
            return Mapper.Map<IEnumerable<Order>>(UnitOfWork.OrderRepository.GetAll());
        });

    public Task<IEnumerable<Order>> GetUserOrders(string token, int userId) =>
        Task<IEnumerable<Order>>.Factory.StartNew(() =>
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

    public Task<bool> AddProduct(string token, int productId, int orderId) =>
        Task<bool>.Factory.StartNew(() => ChangeProperty(token, orderId, x =>
        {
            var product = UnitOfWork.ProductRepository.GetById(productId);
            if (product is null) return false;
            x.Products.Add(product);
            return true;
        }));

    public Task<bool> DeleteProduct(string token, int productId, int orderId) =>
        Task<bool>.Factory.StartNew(() => ChangeProperty(token, orderId, x =>
        {
            var product = UnitOfWork.ProductRepository.GetById(productId);
            return product is not null && x.Products.Remove(product);
        }));

    public Task<bool> ChangeDescription(string token, string desc, int orderId) =>
        Task<bool>.Factory.StartNew(() =>
            ChangeProperty(token, orderId, x =>
            {
                if (string.IsNullOrWhiteSpace(desc)) return false;
                x.Description = desc;
                return true;
            }));

    public Task<bool> ChangeConfirmed(string token, bool confirmed, int orderId) =>
        Task<bool>.Factory.StartNew(() => ChangeProperty(token, orderId, x =>
        {
            var order = Mapper.Map<Order>(UnitOfWork.OrderRepository.GetById(orderId));
            if (order.OrderStatus != OrderStatus.New) return false;
            x.Confirmed = confirmed;
            return true;
        }));

    public Task<bool> ChangeOrderStatus(string token, string status, int orderId) =>
        Task<bool>.Factory.StartNew(() => ChangeProperty(token, orderId, x =>
        {
            var order = Mapper.Map<Order>(UnitOfWork.OrderRepository.GetById(orderId));
            var orderStatus = Enum.Parse<OrderStatus>(status?.Replace(" ", "").Trim() ?? "New", true);
            if (order?.OrderStatus != null &&
                TokenHandler.GetUser(token) is not { IsAdmin: true } &&
                ((int)order.OrderStatus>2 || orderStatus!=OrderStatus.CanceledByUser)) return false;
            x.OrderStatus = orderStatus.ToString();
            return true;

        }));

    private bool ValidPermissionForModifyOrder(UserEntity requestUser, OrderEntity order)
    {
        ThrowAuthenticationExceptionIfUserIsNull(requestUser);
        if (order is null || order.Confirmed) return false;
        if (order.Owner.Id == requestUser.Id) return true;
        ThrowAuthenticationExceptionIfUserIsNotAdmin(requestUser);
        return true;
    }

    private bool ChangeProperty(string token, int orderId, Func<OrderEntity,bool> func)
    {
        var requestUser = TokenHandler.GetUser(token);
        var order = UnitOfWork.OrderRepository.GetById(orderId);
        if (!ValidPermissionForModifyOrder(requestUser, order) || !(func?.Invoke(order) ?? false)) return false;
        UnitOfWork.OrderRepository.Update(order);
        if(requestUser.IsAdmin)
            Logger.Log($"Admin {requestUser.Name} change property for order with id {order.Id}");
        return true;
    }
}