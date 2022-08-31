using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Objects;
using BLL.Services.Exception;
using BLL.Test.Helpers;
using DAL;
using DAL.Repositories;
using Entities;
using Moq;
using Xunit;

namespace BLL.Test;

public class CategoryServiceTest
{


    #region CreateMethodTest

    [Theory]
    [InlineData("First name", true)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("Alcohol", true)]
    public void CreateCategoryAndLoggingCorrectlyWork(string categoryName, bool expected)
    {
        const int resultCategoryId = 1;

        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == new UserEntity()
        {
            IsAdmin = true
        });
        var moqRepository = Mock.Of<IRepository<CategoryEntity>>(x =>
            x.Add(It.Is<CategoryEntity>(c => c.Name == categoryName)) == resultCategoryId);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == moqRepository);
        var categoryService = new ServiceContainer(unitOfWork, moqLogger.Object, tokenHandler,new AutoMapperHandlerTest()).CategoryService;

        var resultId = categoryService.Create("", categoryName).Result;
        var actual = resultId == resultCategoryId;
        var expectedTimes = actual ? Times.Once() : Times.Never();

        moqLogger.Verify(x => x.Log(It.IsAny<string>()), expectedTimes);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CreateCategoryAndLoggingTestUserIsNotAdminReturnedException()
    {
        const string categoryName = "name";

        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();
        moqLogger.Setup(x => x.Log(It.IsAny<Exception>())).Verifiable();

        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == new UserEntity
        {
            IsAdmin = false
        });
        var unitOfWork = Mock.Of<IUnitOfWork>();
        var categoryService =
            new ServiceContainer(unitOfWork, moqLogger.Object, tokenHandler, new AutoMapperHandlerTest())
                .CategoryService;

        var actual = categoryService.Create("", categoryName);

        Assert.ThrowsAsync<AuthenticationException>(() => actual).Wait();
        moqLogger.Verify(x => x.Log(It.IsAny<string>()), Times.Never);
        moqLogger.Verify(x => x.Log(It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public void CreateCategoryAndLoggingTestUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceContainer>(null, service => service.CategoryService.Create("", ""));
    }

    [Fact]
    public void CreateCategoryAndLoggingTestWhenNameIsNotUnique()
    {
        const string categoryName = "name";

        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();

        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == new UserEntity
        {
            IsAdmin = true
        });

        var moqRepository = Mock.Of<IRepository<CategoryEntity>>(x =>
            x.GetAll() == new List<CategoryEntity>
            {
                new()
                {
                    Name = categoryName
                }
            });
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == moqRepository);
        var categoryService = new ServiceContainer(unitOfWork, moqLogger.Object, tokenHandler, new AutoMapperHandlerTest())
            .CategoryService;

        var actual = categoryService.Create("", categoryName).Result;

        Assert.Equal(-1, actual);
        moqLogger.Verify(x => x.Log(It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region ChangeNameMethodTest

    [Theory]
    [InlineData("Some name", "New Name", true)]
    [InlineData("Some name", "NewName", true)]
    [InlineData("Some name", "Some name", false)]
    [InlineData("Some name", "", false)]
    [InlineData("Some name", " ", false)]
    public void ChangeNameAndLoggingTestWhenUserIsAdmin(string oldName, string newName, bool expected)
    {
        const string token = "token";
        var storedCategoryEntity = new CategoryEntity { Id = 1, Name = oldName };

        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(token) == new UserEntity { IsAdmin = true });
        var repository = Mock.Of<IRepository<CategoryEntity>>(x =>
            x.GetById(storedCategoryEntity.Id) == storedCategoryEntity &&
            x.GetAll() == new List<CategoryEntity> { storedCategoryEntity });
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();
        
        var categoryService = new ServiceContainer(unitOfWork, moqLogger.Object, tokenHandler, new AutoMapperHandlerTest())
            .CategoryService;

        var actual = categoryService.ChangeName(token, newName, 1).Result;
        var expectedInvokedTimes = expected ? Times.Once() : Times.Never();

        Assert.Equal(expected, actual);
        moqLogger.Verify(x => x.Log(It.IsAny<string>()), expectedInvokedTimes);
    }

    [Fact]
    public void ChangeNameAndLoggingTestWhenUserIsNotAdminReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceContainer>(new UserEntity { IsAdmin = false },
            service => service.CategoryService.ChangeName("", "New Name", 1));
    }

    [Fact]
    public void ChangeNameAndLoggingTestWhenUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceContainer>(null, service => service.CategoryService.ChangeName("", "New Name", 1));
    }

    #endregion

    #region GetAllMethodTest

    [Fact]
    public void GetAllTestReturnedCorrectValue()
    {
        var categoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "First"
            },
            new()
            {
                Id = 2,
                Name = "Second"
            },
            new()
            {
                Id = 3,
                Name = "Third"
            }
        };

        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == new UserEntity());
        var repository = Mock.Of<IRepository<CategoryEntity>>(x => x.GetAll() == categoryEntities);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var logger = Mock.Of<ILogger>();

        var service = new ServiceContainer(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest())
            .CategoryService;

        var actual = service.GetAll("").Result.ToList();

        Assert.True(actual.All(x=>categoryEntities.Any(c=>c.Id==x.Id)));
        Assert.Equal(categoryEntities.Count,actual.Count);
    }

    [Fact]
    public void GetAllTestUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceContainer>(null, service => service.CategoryService.GetAll(""));
    }

    #endregion

    #region GetByNameMethodTest

    [Fact]
    public void GetByNameTestReturnedCorrectValue()
    {
        var categoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "First"
            },
            new()
            {
                Id = 2,
                Name = "Second"
            },
            new()
            {
                Id = 3,
                Name = "Third"
            }
        };
        var expected = new Category
        {
            Id = 2,
            Name = "Second"
        };

        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == new UserEntity());
        var repository = Mock.Of<IRepository<CategoryEntity>>(x => x.GetAll() == categoryEntities);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var logger = Mock.Of<ILogger>();
        var service = new ServiceContainer(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest())
            .CategoryService;

        var actual = service.GetByName("", "Second").Result;

        Assert.Equal(expected.Id, actual.Id);
    }

    [Fact]
    public void GetByNameTestUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceContainer>(null, service => service.CategoryService.GetByName("", ""));
    }

    #endregion

    #region GetByIdMethodTest

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void GetByIdTestReturnedCorrectValue(int expectedId)
    {
        var categoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "First"
            },
            new()
            {
                Id = 2,
                Name = "Second"
            },
            new()
            {
                Id = 3,
                Name = "Third"
            },
            new()
            {
                Id = 4,
                Name = "Fourth"
            },
            new()
            {
                Id = 5,
                Name = "Fifth"
            },
            new()
            {
                Id = 6,
                Name = "Sixth"
            }
        };

        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == new UserEntity());
        var repository =
            Mock.Of<IRepository<CategoryEntity>>(x => x.GetById(expectedId) == categoryEntities[expectedId - 1]);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var logger = Mock.Of<ILogger>();
        var service = new ServiceContainer(unitOfWork, logger, tokenHandler, new AutoMapperHandlerTest())
            .CategoryService;

        var actual = service.GetById("", expectedId).Result;
        var expected = categoryEntities[expectedId - 1];

        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Name, actual.Name);
    }

    [Fact]
    public void GetByIdTestUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceContainer>(null, service => service.CategoryService.GetById("", 0));
    }

    #endregion

    #region RemoveMethodTest

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(1, 0, false)]
    [InlineData(0, 1, false)]
    [InlineData(0, -1, false)]
    [InlineData(1, 1, true)]
    public void RemoveTestCorrectlyWork(int targetId, int removedId, bool expected)
    {
        var categoryEntity = new CategoryEntity { Id = targetId };
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == new UserEntity { IsAdmin = true });
        var repository = Mock.Of<IRepository<CategoryEntity>>(x =>
            x.Delete(It.Is<CategoryEntity>(c => c.Id == targetId)) == true &&
            x.GetById(targetId) == categoryEntity);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();
        var service = new ServiceContainer(unitOfWork, moqLogger.Object, tokenHandler, new AutoMapperHandlerTest())
            .CategoryService;

        var actual = service.Remove("", removedId).Result;
        var times = expected ? Times.Once() : Times.Never();

        Assert.Equal(expected, actual);
        moqLogger.Verify(x => x.Log(It.IsAny<string>()), times);
    }

    [Fact]
    public void RemoveTestUserIsNotAdminReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceContainer>(new UserEntity { IsAdmin = false }, service => service.CategoryService.Remove("", 0));
    }

    [Fact]
    public void RemoveTestUserIsNullReturnedException()
    {
        ServiceTest.MethodsTestReturnedException<ServiceContainer>(null, service => service.CategoryService.Remove("", 0));
    }

    #endregion
}