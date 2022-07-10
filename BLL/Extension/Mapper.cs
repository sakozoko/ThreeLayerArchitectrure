using System.Reflection;
using BLL.Objects;
using Entities;

namespace BLL.Extension;

public static class Mapper
{
    public static TTo Map<TTo, TFrom>(TFrom from)
        where TTo : BaseEntity
        where TFrom : BaseDto
    {
        var to = (TTo)Activator.CreateInstance(typeof(TTo));
        
        to.SetProperty(from);

        return to;
    }
    public static void SetProperty<TTo, TFrom>(this TTo to, TFrom from)
        where TTo : BaseEntity
        where TFrom : BaseDto
    {
        var toProperty = typeof(TTo).GetProperties();
        var fromProperty = typeof(TFrom).GetProperties();
        foreach (var propertyInfo in toProperty)
        {
            propertyInfo.SetValue(to, fromProperty.FirstOrDefault(x =>
                x.PropertyType == propertyInfo.PropertyType && x.Name == propertyInfo.Name)?.GetValue(from));
        }
    }
    #region ToEntity

    public static CategoryEntity ToEntity(this Category category)
    {
        if (category is null)
            return null;
        return new CategoryEntity
        {
            Id = category.Id,
            Name = category.Name
        };
    }


    public static ProductEntity ToEntity(this Product product)
    {
        if (product is null)
            return null;
        return new ProductEntity
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category.ToEntity(),
            Cost = product.Cost,
            Description = product.Description
        };
    }

    public static OrderEntity ToEntity(this Order order)
    {
        if (order is null)
            return null;
        return new OrderEntity
        {
            Id = order.Id,
            Description = order.Description,
            OrderStatus = nameof(order.OrderStatus),
            Owner = order.Owner.ToEntity(),
            Products = order.Products.ToEntity().ToList()
        };
    }


    public static UserEntity ToEntity(this User user)
    {
        if (user is null)
            return null;
        return new UserEntity
        {
            Id = user.Id,
            Name = user.Name,
            Password = user.Password,
            IsAdmin = user.IsAdmin,
            Surname = user.Surname
        };
    }

    public static IEnumerable<ProductEntity> ToEntity(this IEnumerable<Product> products)
    {
        return products.Select(product => product.ToEntity());
    }

    #endregion

    #region ToDomain

    public static User ToDomain(this UserEntity userEntity)
    {
        if (userEntity is null)
            return null;
        return new User
        {
            Id = userEntity.Id,
            Name = userEntity.Name,
            Password = userEntity.Password,
            IsAdmin = userEntity.IsAdmin,
            Surname = userEntity.Surname,
            Orders = new List<Order>()
        };
    }


    public static Category ToDomain(this CategoryEntity category)
    {
        if (category is null)
            return null;
        return new Category
        {
            Id = category.Id,
            Name = category.Name
        };
    }


    public static Product ToDomain(this ProductEntity product)
    {
        if (product is null)
            return null;
        return new Product
        {
            Id = product.Id,
            Category = product.Category.ToDomain(),
            Cost = product.Cost,
            Description = product.Description,
            Name = product.Name
        };
    }

    public static Order ToDomain(this OrderEntity order)
    {
        if (order is null)
            return null;
        return new Order
        {
            Id = order.Id,
            Description = order.Description,
            OrderStatus = order.OrderStatus switch
            {
                nameof(OrderStatus.Completed) => OrderStatus.Completed,
                nameof(OrderStatus.Received) => OrderStatus.Received,
                nameof(OrderStatus.Sent) => OrderStatus.Sent,
                nameof(OrderStatus.PaymentReceived) => OrderStatus.PaymentReceived,
                nameof(OrderStatus.CanceledByUser) => OrderStatus.CanceledByUser,
                nameof(OrderStatus.CanceledByTheAdministrator) => OrderStatus.CanceledByTheAdministrator,
                _ => OrderStatus.New
            },
            Owner = order.Owner.ToDomain(),
            Products = order.Products.ToDomain().ToList()
        };
    }

    public static IEnumerable<Product> ToDomain(this IEnumerable<ProductEntity> products)
    {
        return products.Select(x => x.ToDomain());
    }

    public static IEnumerable<Category> ToDomain(this IEnumerable<CategoryEntity> categories)
    {
        return categories.Select(x => x.ToDomain());
    }

    public static IEnumerable<Order> ToDomain(this IEnumerable<OrderEntity> orders)
    {
        return orders.Select(x => x.ToDomain());
    }

    public static IEnumerable<User> ToDomain(this IEnumerable<UserEntity> users)
    {
        return users.Select(x => x.ToDomain());
    }

    #endregion
}