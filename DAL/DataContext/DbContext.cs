using Entities;
using Entities.Goods;

namespace DAL.DataContext;

internal class DbContext
{
    private static readonly object Obj = new ();
    private static DbContext _instance;
    public static DbContext Instance
    {
        get
        {
            if(_instance is null)
            {
                lock (Obj)
                {
                    _instance ??= new DbContext();
                }
            }
            return _instance;
        }
    }

    protected DbContext()
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

    public IList<Category> Categories { get; } = new List<Category>
    {
        new() { Id = 1, Name = "First" },
        new() { Id = 2, Name = "Second" },
        new() { Id = 3, Name = "Third" },
        new() { Id = 4, Name = "Fourth" },
        new() { Id = 5, Name = "Fifth" }
    };

    public IList<Order> Orders { get; }

    public IList<Product> Products { get; }

    public IList<User> Users { get; } = new List<User>
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