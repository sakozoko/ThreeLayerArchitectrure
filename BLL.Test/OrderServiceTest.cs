using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Test.Helpers;
using DAL;
using DAL.Repositories;
using Entities;
using Moq;
using Xunit;

namespace BLL.Test;

public class OrderServiceTest
{
    
    #region GetByIdMethodTest
    
    
    
    #endregion

    #region GetAllMethodTest

    

    #endregion

    #region GetUserOrdersMethodTest

    

    #endregion

    #region CreateMethodTest

    [Fact]
    public void CreateTestCorrectlyWork()
    {
        const int productId = 1;
        var product = new ProductEntity() { Id = productId }; 
        var orders = new List<OrderEntity>();
        var logger = Mock.Of<ILogger>();
        var tokenHandler = Mock.Of<ITokenHandler>(x=>x.GetUser(It.IsAny<string>())==new UserEntity());
        var productRepository = Mock.Of<IRepository<ProductEntity>>(x => x.GetById(productId) == product);
        var orderRepository = new Mock<IRepository<OrderEntity>>();
        orderRepository.Setup(x => x.Add(It.IsAny<OrderEntity>()))
            .Callback<OrderEntity>(oe => orders.Add(oe));
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.ProductRepository == productRepository && 
                                                   x.OrderRepository==orderRepository.Object);
        var service = new ServiceContainer(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).OrderService;

        service.Create("", "Desc", productId).Wait(); 
        service.Create("", "Desc", productId).Wait();
        Assert.Equal(2, orders.Count);
    }

    [Fact]
    public void CreateTestUserIsNullReturnedException()
    {
        MethodsTestReturnedExceptionIfUserIsNull(service => service.OrderService.Create("", "desc", 1));
    }
    
    [Fact]
    public void CreateTestUserIsNotAdminReturnedException()
    {
        const int productId = 1;
        const int userId = 1;
        var product = new ProductEntity() { Id = productId };
        var user = new UserEntity { Id = userId };
        var productRepository = Mock.Of<IRepository<ProductEntity>>(x => x.GetById(productId) == product);
        var userRepository = Mock.Of<IRepository<UserEntity>>(x => x.GetById(userId) == user);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.ProductRepository == productRepository && 
                                                   x.UserRepository==userRepository);
        
        ServiceTest.MethodsTestReturnedException<ServiceContainer>(new UserEntity(),unitOfWork,
            container =>container.OrderService.Create("","desc",productId,userId));
    }

    #endregion

    #region AddProductMethodTest
    
    [Fact]
    public void AddProductTestCorrectlyWork()
    {
        const int ownerId = 1;
        const int orderId = 1;
        const int productId = 1;
        var owner = new UserEntity() { Id = ownerId };
        var product = new ProductEntity() { Id = productId };
        var actualOrder = new OrderEntity()
        {
            Id = orderId,
            Products = new List<ProductEntity>(),
            Owner = owner
        };
        var logger = Mock.Of<ILogger>();
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == owner);
        var orderRepository = new Mock<IRepository<OrderEntity>>();
        orderRepository.Setup(x => x.GetById(orderId)).Returns(actualOrder);
        orderRepository.Setup(x => x.Update(It.Is<OrderEntity>(oe => oe.Id == orderId)))
            .Callback<OrderEntity>(e => actualOrder = e);
        var productRepository = Mock.Of<IRepository<ProductEntity>>(x=>x.GetById(productId)==product);
        var unitOfWork = Mock.Of<IUnitOfWork>(x =>
            x.OrderRepository == orderRepository.Object && x.ProductRepository == productRepository);
        var service = new ServiceContainer(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).OrderService;

        service.AddProduct("", productId, orderId).Wait();
        var actual = service.AddProduct("", productId, orderId).Result;
        
        Assert.True(actual);
        Assert.Equal(2,actualOrder.Products.Count);
    }

    [Fact]
    public void AddProductTestUserIsNullReturnedException()
    {
        MethodsTestReturnedExceptionIfUserIsNull(service => service.OrderService.AddProduct("", 0, 1));
    }
    [Fact]
    public void AddProductTestUserIsNotAdminReturnedException()
    {
        MethodsTestReturnedExceptionIfUserIsNotAdmin(service => service.OrderService.AddProduct("", 0, 1));
    }

    #endregion

    #region DeleteProductMethodTest

    [Fact]
    public void DeleteProductTestCorrectlyWork()
    {
        const int ownerId = 1;
        const int orderId = 1;
        const int productId = 1;
        var owner = new UserEntity() { Id = ownerId };
        var product = new ProductEntity() { Id = productId };
        var actualOrder = new OrderEntity()
        {
            Id = orderId,
            Products = new List<ProductEntity>(){product},
            Owner = owner
        };
        var logger = Mock.Of<ILogger>();
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == owner);
        var orderRepository = new Mock<IRepository<OrderEntity>>();
        orderRepository.Setup(x => x.GetById(orderId)).Returns(actualOrder);
        orderRepository.Setup(x => x.Update(It.Is<OrderEntity>(oe => oe.Id == orderId)))
            .Callback<OrderEntity>(e => actualOrder = e);
        var productRepository = Mock.Of<IRepository<ProductEntity>>(x=>x.GetById(productId)==product);
        var unitOfWork = Mock.Of<IUnitOfWork>(x =>
            x.OrderRepository == orderRepository.Object && x.ProductRepository == productRepository);
        var service = new ServiceContainer(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).OrderService;

        var actual = service.DeleteProduct("", productId, orderId).Result;
        
        Assert.True(actual);
        Assert.Equal(0,actualOrder.Products.Count);
    }

    [Fact]
    public void DeleteProductTestUserIsNullReturnedException()
    {
        MethodsTestReturnedExceptionIfUserIsNull(service => service.OrderService.DeleteProduct("", 0, 1));
    }
    [Fact]
    public void DeleteProductTestUserIsNotAdminReturnedException()
    {
        MethodsTestReturnedExceptionIfUserIsNotAdmin(service => service.OrderService.DeleteProduct("", 0, 1));
    }

    #endregion

    #region ChangeOrderStatusMethodTest

    [Theory]
    [InlineData("New")]
    [InlineData("CanceledByTheAdministrator")]
    [InlineData("Payment Received")]
    [InlineData("Sent")]
    [InlineData("Completed")]
    [InlineData("Received")]
    [InlineData("CanceledByUser")]
    public void ChangeOrderStatusCorrectlyWorkWhenUserIsAdmin(string status)
    {
        const int orderId = 1;
        var user = new UserEntity { Id = orderId + 1, IsAdmin = true};
        var actualOrder = new OrderEntity()
        {
            Id = orderId,
            Owner = new UserEntity(){Id=orderId},
            Description = "...."
        };
        var logger = Mock.Of<ILogger>();
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == user);
        var repository = new Mock<IRepository<OrderEntity>>();
        repository.Setup(x => x.GetById(orderId)).Returns(actualOrder);
        repository.Setup(x => x.Update(It.Is<OrderEntity>(c => c.Id == actualOrder.Id)))
            .Callback<OrderEntity>(entity => actualOrder = entity);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.OrderRepository == repository.Object);
        var service = new ServiceContainer(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).OrderService;

        var actual = service.ChangeOrderStatus(It.IsAny<string>(), status, orderId).Result;
        
        Assert.True(actual);
    }
    
    [Theory]
    [InlineData("CanceledByUser","Completed",false)]
    [InlineData("CanceledByUser","Sent",true)]
    [InlineData("CanceledByUser","PaymentReceived",true)]
    [InlineData("CanceledByUser","New",true)]
    [InlineData("CanceledByUser","Received",false)]
    [InlineData("CanceledByUser","CanceledByTheAdministrator",false)]
    [InlineData("PaymentReceived","New",false)]
    [InlineData("Sent","New",false)]
    [InlineData("Received","PaymentReceived",false)]
    [InlineData("Completed","Sent",false)]
    [InlineData("CanceledByTheAdministrator","Received",false)]
    public void ChangeOrderStatusCorrectlyWorkWhenUserIsNotAdmin(string status, string startedStatus, bool expected)
    {
        const int orderId = 1;
        var user = new UserEntity { Id = orderId + 1};
        var actualOrder = new OrderEntity()
        {
            Id = orderId,
            Owner = user,
            Description = "....",
            OrderStatus = startedStatus
        };
        var logger = Mock.Of<ILogger>();
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == user);
        var repository = new Mock<IRepository<OrderEntity>>();
        repository.Setup(x => x.GetById(orderId)).Returns(actualOrder);
        repository.Setup(x => x.Update(It.Is<OrderEntity>(c => c.Id == actualOrder.Id)))
            .Callback<OrderEntity>(entity => actualOrder = entity);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.OrderRepository == repository.Object);
        var service = new ServiceContainer(unitOfWork, logger,tokenHandler, new AutoMapperHandlerTest()).OrderService;

        var actual = service.ChangeOrderStatus(It.IsAny<string>(), status, orderId).Result;
        
        Assert.Equal(expected,actual);
        if (expected)
        {
            Assert.Equal(actualOrder.OrderStatus, status);
        }
        else
        {
            Assert.NotEqual(actualOrder.OrderStatus, status);
        }
    }
    
    
    [Theory]
    [InlineData("New")]
    [InlineData("Canceled By TheAdministrator")]
    [InlineData("Payment Received")]
    [InlineData("Sent")]
    [InlineData("Completed")]
    [InlineData("Received")]
    [InlineData("CanceledByUser")]
    public void ChangeOrderStatusTestUserIsNullReturnedException(string status)
    {
        MethodsTestReturnedExceptionIfUserIsNull(service => service.OrderService.ChangeOrderStatus("",status,1));
    }

    [Theory]
    [InlineData("New")]
    [InlineData("Canceled By TheAdministrator")]
    [InlineData("Payment Received")]
    [InlineData("Sent")]
    [InlineData("Completed")]
    [InlineData("Received")]
    [InlineData("CanceledByUser")]
    public void ChangeOrderStatusTestUserIsNotAdminReturnedException(string status)
    {
        MethodsTestReturnedExceptionIfUserIsNotAdmin(service => service.OrderService.ChangeOrderStatus("",status,1));
    }

    #endregion

    #region ChangeDescriptionMethodTest

    [Theory]
    [InlineData("Any desc",true)]
    [InlineData("Anydesc",true)]
    [InlineData("",false)]
    [InlineData(" ",false)]
    public void ChangeDescriptionTestCorrectlyWork(string expectedDesc, bool expected)
    {
        const int orderId = 1;
        var user = new UserEntity { Id = orderId + 1 };
        var actualOrder = new OrderEntity()
        {
            Id = orderId,
            Owner = user,
            Description = "...."
        };
        var logger = Mock.Of<ILogger>();
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == user);
        var repository = new Mock<IRepository<OrderEntity>>();
        repository.Setup(x => x.GetById(orderId)).Returns(actualOrder);
        repository.Setup(x => x.Update(It.Is<OrderEntity>(c => c.Id == actualOrder.Id)))
            .Callback<OrderEntity>(entity => actualOrder = entity);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.OrderRepository == repository.Object);

        var service = new ServiceContainer(unitOfWork,logger, tokenHandler, new AutoMapperHandlerTest()).OrderService;

        var actual = service.ChangeDescription(It.IsAny<string>(), expectedDesc, 1).Result;
        
        Assert.Equal(expected,actual);
        if (expected)
        {
            Assert.Equal(expectedDesc,actualOrder.Description);
        }
        else
        {
            Assert.NotEqual(expectedDesc,actualOrder.Description);
        }

    }
    [Theory]
    [InlineData("Any desc")]
    [InlineData("Anydesc")]
    [InlineData("")]
    [InlineData(" ")]
    public void ChangeDescriptionTestCorrectlyWorkWhenConfirmedIsTrue(string expectedDesc)
    {
        const int orderId = 1;
        var user = new UserEntity { Id = orderId + 1 };
        var actualOrder = new OrderEntity()
        {
            Id = orderId,
            Owner = user,
            Description = "....",
            Confirmed = true
        };
        var logger = Mock.Of<ILogger>();
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == user);
        var repository = new Mock<IRepository<OrderEntity>>();
        repository.Setup(x => x.GetById(orderId)).Returns(actualOrder);
        repository.Setup(x => x.Update(It.Is<OrderEntity>(c => c.Id == actualOrder.Id)))
            .Callback<OrderEntity>(entity => actualOrder = entity);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.OrderRepository == repository.Object);

        var service = new ServiceContainer(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).OrderService;

        var actual = service.ChangeDescription(It.IsAny<string>(), expectedDesc, 1).Result;
        
        Assert.False(actual);
        Assert.NotEqual(expectedDesc,actualOrder.Description);

    }

    [Fact]
    public void ChangeDescriptionTestUserIsNullReturnedException()
    {
        MethodsTestReturnedExceptionIfUserIsNull(service => service.OrderService.ChangeDescription("","New value",1));
    }

    [Fact]
    public void ChangeDescriptionTestUserIsNotAdminReturnedException()
    {
        MethodsTestReturnedExceptionIfUserIsNotAdmin(service => service.OrderService.ChangeDescription("","Value",1));
    }

    #endregion

    #region ChangeConfirmedMethodTest

    [Fact]
    public void ChangeConfirmedTestUserIsNullReturnedException()
    {
        MethodsTestReturnedExceptionIfUserIsNull(service => service.OrderService.ChangeConfirmed("",true,1));
    }
    [Fact]
    public void ChangeConfirmedTestUserIsNotAdminReturnedException()
    {
        MethodsTestReturnedExceptionIfUserIsNotAdmin(service => service.OrderService.ChangeConfirmed("",true,1));
    }
    [Theory]
    [InlineData("New",true)]
    [InlineData("CanceledByTheAdministrator",false)]
    [InlineData("PaymentReceived",false)]
    [InlineData("Sent",false)]
    [InlineData("Completed",false)]
    [InlineData("Received",false)]
    [InlineData("CanceledByUser",false)]
    public void ChangeConfirmedTestCorrectlyWorkWithDifferentStatus(string status, bool expected)
    {
        const int orderId = 1;
        const int ownerId = 1;
        var logger = Mock.Of<ILogger>();
        var tokenHandler = Mock.Of<ITokenHandler>(x=>x.GetUser(It.IsAny<string>())==new UserEntity
        {
            Id=ownerId
        });
        var repository = Mock.Of<IRepository<OrderEntity>>(x=>x.GetById(orderId)==new OrderEntity
        {
            Owner = new UserEntity
            {
                Id=ownerId
            },
            Confirmed = false,
            OrderStatus = status
        });
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.OrderRepository == repository);


        var service = new ServiceContainer(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).OrderService;

        var actual = service.ChangeConfirmed(It.IsAny<string>(), true, orderId).Result;
        
        Assert.Equal(expected,actual);
        
    }
    
    [Theory]
    [InlineData("New")]
    [InlineData("CanceledByTheAdministrator")]
    [InlineData("PaymentReceived")]
    [InlineData("Sent")]
    [InlineData("Completed")]
    [InlineData("Received")]
    [InlineData("CanceledByUser")]
    public void ChangeConfirmedTestCorrectlyWorkWithDifferentStatusWhenUserIdsDoNotMatch(string status)
    {
        const int orderId = 1;
        const int ownerId = 1;
        var repository = Mock.Of<IRepository<OrderEntity>>(x=>x.GetById(orderId)==new OrderEntity
        {
            Owner = new UserEntity
            {
                Id=ownerId+1
            },
            Confirmed = false,
            OrderStatus = status
        });
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.OrderRepository == repository);

        ServiceTest.MethodsTestReturnedException<ServiceContainer>(new UserEntity() { Id = ownerId }, unitOfWork,
            service => service.OrderService.ChangeConfirmed(It.IsAny<string>(), true, orderId));
    }

    #endregion
    
    private void MethodsTestReturnedExceptionIfUserIsNull(Func<ServiceContainer, Task> testMethod)
    {
        var repository = Mock.Of<IRepository<OrderEntity>>();
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.OrderRepository == repository);

        ServiceTest.MethodsTestReturnedException(null,unitOfWork,testMethod);

    }
    private void MethodsTestReturnedExceptionIfUserIsNotAdmin(Func<ServiceContainer, Task> testMethod)
    {
        const int orderId = 1;
        const int ownerId = 1;
        var repository = Mock.Of<IRepository<OrderEntity>>(x=>x.GetById(orderId)==new OrderEntity
        {
            Owner = new UserEntity
            {
                Id=ownerId+1
            }
        });
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.OrderRepository == repository);

        ServiceTest.MethodsTestReturnedException(new UserEntity
        {
            Id=ownerId,
            IsAdmin = false
        },unitOfWork,testMethod);

    }
}