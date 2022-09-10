using System.Linq;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Test.Helpers;
using DAL;
using DAL.Repositories;
using Entities;
using Moq;
using Xunit;

namespace BLL.Test;

public class ProductServiceTest
{
    #region GetAllMethodTest

    [Fact]
    public void GetAllTestUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceManager>(null, manager => manager.ProductService.GetAll(""));
    }

    #endregion

    #region GetByNameMethodTest

    [Fact]
    public void GetByNameCorrectlyWork()
    {
        const string testName = "nameeee";
        var logger = Mock.Of<ILogger>();
        var repository = Mock.Of<IRepository<ProductEntity>>(x =>
            x.GetAll() == new[]
            {
                new ProductEntity
                {
                    Name = testName
                }
            });
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.ProductRepository == repository);
        var tokenHandler = Mock.Of<ITokenHandler>();

        var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).ProductService;

        var actual = service.GetByName(testName).Result.First().Name;

        Assert.Equal(testName, actual);
    }

    #endregion

    #region CreateMethodTest

    [Fact]
    public void CreateCorrectlyWork()
    {
        const int categoryId = 1;
        const int expectedProductId = 1;
        var logger = Mock.Of<ILogger>();
        var productRepository = new Mock<IRepository<ProductEntity>>();
        productRepository.Setup(x =>
            x.Add(It.IsAny<ProductEntity>())).Returns(expectedProductId).Verifiable();
        var categoryRepository =
            Mock.Of<IRepository<CategoryEntity>>(x => x.GetById(categoryId) == new CategoryEntity());
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.ProductRepository == productRepository.Object &&
                                                   x.CategoryRepository == categoryRepository);
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser("") == new UserEntity { IsAdmin = true });

        var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).ProductService;

        var actual = service.Create("", "Namee", "Some desc", 0.01M, categoryId).Result;

        Assert.Equal(expectedProductId, actual);
        productRepository.Verify(x => x.Add(It.IsAny<ProductEntity>()), Times.Once);
    }

    [Fact]
    public void CreateTestUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceManager>(null,
            manager => manager.ProductService.Create("", "", "", 1, 1));
    }

    [Fact]
    public void CreateTestUserIsNotAdminReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceManager>(new UserEntity(),
            manager => manager.ProductService.Create("", "", "", 1, 1));
    }

    #endregion

    #region ChangeNameMethodTest

    [Fact]
    public void ChangeNameCorrectlyWork()
    {
        const string newName = "SomeName";
        const int productId = 1;
        var logger = Mock.Of<ILogger>();
        var productRepository = new Mock<IRepository<ProductEntity>>();
        productRepository.Setup(x => x.GetById(productId))
            .Returns(new ProductEntity()).Verifiable();
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.ProductRepository == productRepository.Object);
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser("") == new UserEntity
        {
            IsAdmin = true
        });

        var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).ProductService;

        var actual = service.ChangeName("", newName, productId).Result;
        Assert.True(actual);
        productRepository.Verify(x => x.GetById(productId), Times.Once);
    }

    [Fact]
    public void ChangeNameTestUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceManager>(null,
            manager => manager.ProductService.ChangeName("", "", 1));
    }

    [Fact]
    public void ChangeNameTestUserIsNotAdminReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceManager>(new UserEntity(),
            manager => manager.ProductService.ChangeName("", "", 1));
    }

    #endregion

    #region ChangeDescriptionMethodTest

    [Fact]
    public void ChangeDescriptionCorrectlyWork()
    {
        const string newDesc = "SomeName";
        const int productId = 1;
        var logger = Mock.Of<ILogger>();
        var productRepository = new Mock<IRepository<ProductEntity>>();
        productRepository.Setup(x => x.GetById(productId))
            .Returns(new ProductEntity()).Verifiable();
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.ProductRepository == productRepository.Object);
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser("") == new UserEntity
        {
            IsAdmin = true
        });

        var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).ProductService;

        var actual = service.ChangeDescription("", newDesc, productId).Result;
        Assert.True(actual);
        productRepository.Verify(x => x.GetById(productId), Times.Once);
    }

    [Fact]
    public void ChangeDescriptionTestUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceManager>(null,
            manager => manager.ProductService.ChangeDescription("", "", 1));
    }

    [Fact]
    public void ChangeDescriptionTestUserIsNotAdminReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceManager>(new UserEntity(),
            manager => manager.ProductService.ChangeDescription("", "", 1));
    }

    #endregion

    #region ChangeCostMethodTest

    [Fact]
    public void ChangeCostCorrectlyWork()
    {
        const decimal newCost = 0.01m;
        const int productId = 1;
        var logger = Mock.Of<ILogger>();
        var productRepository = new Mock<IRepository<ProductEntity>>();
        productRepository.Setup(x => x.GetById(productId))
            .Returns(new ProductEntity()).Verifiable();
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.ProductRepository == productRepository.Object);
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser("") == new UserEntity
        {
            IsAdmin = true
        });

        var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).ProductService;

        var actual = service.ChangeCost("", newCost, productId).Result;
        Assert.True(actual);
        productRepository.Verify(x => x.GetById(productId), Times.Once);
    }

    [Fact]
    public void ChangeCostTestUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceManager>(null,
            manager => manager.ProductService.ChangeCost("", 1, 1));
    }

    [Fact]
    public void ChangeCostTestUserIsNotAdminReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceManager>(new UserEntity(),
            manager => manager.ProductService.ChangeCost("", 1, 1));
    }

    #endregion

    #region ChangeCategoryMethodTest

    [Fact]
    public void ChangeCategoryCorrectlyWork()
    {
        const int productId = 1;
        const int categoryId = productId;
        var logger = Mock.Of<ILogger>();
        var productRepository = new Mock<IRepository<ProductEntity>>();
        productRepository.Setup(x => x.GetById(productId))
            .Returns(new ProductEntity()).Verifiable();
        var categoryRepository = new Mock<IRepository<CategoryEntity>>();
        categoryRepository.Setup(x => x.GetById(categoryId))
            .Returns(new CategoryEntity()).Verifiable();
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.ProductRepository == productRepository.Object &&
                                                   x.CategoryRepository == categoryRepository.Object);
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser("") == new UserEntity
        {
            IsAdmin = true
        });

        var service = new ServiceManager(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest()).ProductService;

        var actual = service.ChangeCategory("", categoryId, productId).Result;
        Assert.True(actual);
        productRepository.Verify(x => x.GetById(productId), Times.Once);
    }

    [Fact]
    public void ChangeCategoryTestUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceManager>(null,
            manager => manager.ProductService.ChangeCategory("", 1, 1));
    }

    [Fact]
    public void ChangeCategoryTestUserIsNotAdminReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceManager>(new UserEntity(),
            manager => manager.ProductService.ChangeCategory("", 1, 1));
    }

    #endregion
}