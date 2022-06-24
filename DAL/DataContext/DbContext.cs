using Entities;
using Entities.Goods;

namespace DAL.DataContext;

public class DbContext
{
    public DbContext()
    {
        Products = new List<Product>
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
        Orders = new List<Order>
        {
            new() { Id = 1, Description = "Vishneva st. 34", Owner = Users[1] },
            new() { Id = 2, Description = "Vishneva st. 34", Owner = Users[1] },
            new() { Id = 3, Description = "Vishneva st. 34", Owner = Users[1] },
            new() { Id = 4, Description = "Vishneva st. 34", Owner = Users[1] },
            new() { Id = 5, Description = "Vishneva st. 34", Owner = Users[1] },
            new() { Id = 6, Description = "Vishneva st. 34", Owner = Users[1] }
        };
    }

    public List<Category> Categories => new()
    {
        new Category { Id = 1, Name = "First" },
        new Category { Id = 2, Name = "Second" },
        new Category { Id = 3, Name = "Third" },
        new Category { Id = 4, Name = "Fourth" },
        new Category { Id = 5, Name = "Fifth" }
    };

    public List<Order> Orders { get; }

    public List<Product> Products { get; }

    public List<User> Users => new()
    {
        new User { Id = 1, Name = "Admin", IsAdmin = true, Password = "123123" },
        new User { Id = 2, Name = "Alex", Surname = "John", IsAdmin = false, Password = "332211" }
    };

    public IEnumerable<T> Set<T>()
    {
        var properties = GetType().GetProperties();
        foreach (var propertyInfo in properties)
            if (propertyInfo.PropertyType == typeof(List<T>))
                return (IEnumerable<T>)propertyInfo.GetValue(this);
        throw new ArgumentException($"Not found field with type List<{nameof(T)}>");
    }

    public Task Save()
    {
        return Task.CompletedTask;
    }
}