using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Entities;

namespace DAL.DataContext
{
    public class DbContext : IDbContext
    {
        public DbContext()
        {
            Products = new List<ProductEntity>
            {
                new ProductEntity()
                {
                    Id = 1, Name = "First", Description = "First description", Cost = 12, Category = Categories[0]
                },
                new ProductEntity()
                {
                    Id = 2, Name = "Second", Description = "Second description", Cost = 354, Category = Categories[2]
                },
                new ProductEntity()
                {
                    Id = 3, Name = "Third", Description = "Third description", Cost = 2541, Category = Categories[3]
                },
                new ProductEntity()
                {
                    Id = 4, Name = "Fourth", Description = "Fourth description", Cost = 1231, Category = Categories[0]
                },
                new ProductEntity()
                {
                    Id = 5, Name = "Fifth", Description = "Fifth description", Cost = 511, Category = Categories[4]
                }
            };
            Orders = new List<OrderEntity>
            {
                new OrderEntity()
                {
                    Id = 1, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                    OrderStatus = "New"
                },
                new OrderEntity()
                {
                    Id = 2, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                    OrderStatus = "CanceledByTheAdministrator"
                },
                new OrderEntity()
                {
                    Id = 3, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                    OrderStatus = "CanceledByTheAdministrator"
                },
                new OrderEntity()
                {
                    Id = 4, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                    OrderStatus = "CanceledByTheAdministrator"
                },
                new OrderEntity()
                {
                    Id = 5, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                    OrderStatus = "Sent"
                },
                new OrderEntity()
                {
                    Id = 6, Description = "Vishneva st. 34", Owner = Users[1], Products = new List<ProductEntity>(),
                    OrderStatus = "Received"
                }
            };
        }

        private IList<CategoryEntity> Categories { get; } = new List<CategoryEntity>
        {
            new CategoryEntity() { Id = 1, Name = "First" },
            new CategoryEntity() { Id = 2, Name = "Second" },
            new CategoryEntity() { Id = 3, Name = "Third" },
            new CategoryEntity() { Id = 4, Name = "Fourth" },
            new CategoryEntity() { Id = 5, Name = "Fifth" }
        };

        private IList<OrderEntity> Orders { get; }

        private IList<ProductEntity> Products { get; }

        private IList<UserEntity> Users { get; } = new List<UserEntity>
        {
            new UserEntity { Id = 1, Name = "Admin", IsAdmin = true, Password = "123123" },
            new UserEntity { Id = 2, Name = "Alex", Surname = "John", IsAdmin = false, Password = "332211" }
        };

        public IList<T> Set<T>()
        {
            var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
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
}