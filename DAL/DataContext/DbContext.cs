using Entities;

namespace DAL.DataContext;

public class DbContext : IDbContext
{
    public DbContext()
    {
        Products = new List<ProductEntity>
        {
            new() { Id = 1, Name = "First", Description = "First description", Cost = 12, Category = Categories[0] },
            new() { Id = 2, Name = "Second", Description = "Second description", Cost = 354, Category = Categories[2] },
            new() { Id = 3, Name = "Third", Description = "Third description", Cost = 2541, Category = Categories[3] },
            new()
            {
                Id = 4, Name = "Fourth", Description = "Fourth description", Cost = 1231, Category = Categories[0]
            },
            new() { Id = 5, Name = "Fifth", Description = "Fifth description", Cost = 511, Category = Categories[4] }
        };
        Orders = new List<OrderEntity>
        {
            new()
            {
                Id = 1, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                OrderStatus = "New"
            },
            new()
            {
                Id = 2, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                OrderStatus = "CanceledByTheAdministrator"
            },
            new()
            {
                Id = 3, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                OrderStatus = "CanceledByTheAdministrator"
            },
            new()
            {
                Id = 4, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                OrderStatus = "CanceledByTheAdministrator"
            },
            new()
            {
                Id = 5, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                OrderStatus = "Sent"
            },
            new()
            {
                Id = 6, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                OrderStatus = "Received"
            }
        };
    }

    public IList<CategoryEntity> Categories { get; } = new List<CategoryEntity>
    {
        new() { Id = 1, Name = "First" },
        new() { Id = 2, Name = "Second" },
        new() { Id = 3, Name = "Third" },
        new() { Id = 4, Name = "Fourth" },
        new() { Id = 5, Name = "Fifth" }
    };

    public IList<OrderEntity> Orders { get; }

    public IList<ProductEntity> Products { get; }

    public IList<UserEntity> Users { get; } = new List<UserEntity>
    {
        new() { Id = 1, Name = "Admin", IsAdmin = true, Password = "123123" },
        new() { Id = 2, Name = "Alex", Surname = "John", IsAdmin = false, Password = "332211" }
    };

    public IList<T> Set<T>()
    {
        var properties = GetType().GetProperties();
        foreach (var propertyInfo in properties)
            if (propertyInfo.PropertyType == typeof(IList<T>))
                return propertyInfo.GetValue(this) as IList<T>;
        throw new ArgumentException($"Not found field with type IList<{nameof(T)}>");
    }

    public Task Save()
    {
        return Task.CompletedTask;
    }
}