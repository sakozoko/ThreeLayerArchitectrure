using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Objects;
using BLL.Services;
using BLL.Services.Exception;
using DAL;
using Entities;
using Moq;
using Xunit;
using DAL.Repositories;

namespace BLL.Test;

public class CategoryServiceTest
{
    
    
    #region CreateMethodTest
    
    [Theory]
    [InlineData("First name",true)]
    [InlineData("",false)]
    [InlineData(" ",false)]
    [InlineData("Alcohol",true)]
    public void CreateCategoryAndLoggingWhenUserIsAdminTest(string categoryName, bool expected)
    {
        const int resultCategoryId = 1;
        
        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x=>x.Log(It.IsAny<string>())).Verifiable();
        
        var tokenHandler = Mock.Of<ITokenHandler>(x=>x.GetUser(It.IsAny<string>())==new User
        {
            IsAdmin = true
        });

        var moqRepository = Mock.Of<IRepository<CategoryEntity>>(x =>
            x.Add(It.Is<CategoryEntity>(c => c.Name == categoryName)) == resultCategoryId);
        var unitOfWork = Mock.Of<IUnitOfWork>(x=>x.CategoryRepository==moqRepository);
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CategoryEntity, Category>();
            cfg.CreateMap<Category, CategoryEntity>();
        }).CreateMapper();

        var categoryService = new CategoryService(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        var resultId = categoryService.Create("", categoryName).Result;
        var actual = resultId == resultCategoryId;
        var expectedTimes = actual ? Times.Once() : Times.Never();
        
        moqLogger.Verify(x=>x.Log(It.IsAny<string>()), expectedTimes);
        Assert.Equal(expected,actual);
    }
    [Fact]
    public void CreateCategoryAndLoggingTestUserIsNotAdminReturnedException()
    {
        const string categoryName = "name";
        
        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x=>x.Log(It.IsAny<string>())).Verifiable();
        moqLogger.Setup(x=>x.Log(It.IsAny<Exception>())).Verifiable();
        
        var tokenHandler = Mock.Of<ITokenHandler>(x=>x.GetUser(It.IsAny<string>())==new User
        {
            IsAdmin = false
        });
        var unitOfWork = Mock.Of<IUnitOfWork>();
        var mapper = Mock.Of<IMapper>();
        var categoryService = new CategoryService(unitOfWork, tokenHandler, moqLogger.Object, mapper);
        
        var actual = categoryService.Create("", categoryName);
        
        Assert.ThrowsAsync<AuthenticationException>(() => actual).Wait();
        moqLogger.Verify(x=>x.Log(It.IsAny<string>()), Times.Never);
        moqLogger.Verify(x=>x.Log(It.IsAny<Exception>()), Times.Once);
    }
    [Fact]
    public void CreateCategoryAndLoggingTestUserIsNullReturnedException()
    {
        const string categoryName = "name";
        
        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();
        moqLogger.Setup(x => x.Log(It.IsAny<Exception>())).Verifiable();
        
        var tokenHandler = Mock.Of<ITokenHandler>();
        var unitOfWork = Mock.Of<IUnitOfWork>();
        var mapper = Mock.Of<IMapper>();
        var categoryService = new CategoryService(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        var actual = categoryService.Create("", categoryName);
        
        Assert.ThrowsAsync<AuthenticationException>(() => actual).Wait();
        moqLogger.Verify(x=>x.Log(It.IsAny<string>()),Times.Never);
        moqLogger.Verify(x=>x.Log(It.IsAny<Exception>()),Times.Once);
    }

    [Fact]
    public void CreateCategoryAndLoggingTestWhenNameIsNotUnique()
    {
        const string categoryName = "name";
        
        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x=>x.Log(It.IsAny<string>())).Verifiable();
        
        var tokenHandler = Mock.Of<ITokenHandler>(x=>x.GetUser(It.IsAny<string>())==new User
        {
            IsAdmin = true
        });

        var moqRepository = Mock.Of<IRepository<CategoryEntity>>(x =>
            x.GetAll() == new List<CategoryEntity>
            {
                new ()
                {
                    Name = categoryName
                }
            });
        var unitOfWork = Mock.Of<IUnitOfWork>(x=>x.CategoryRepository==moqRepository);
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CategoryEntity, Category>();
        }).CreateMapper();

        var categoryService = new CategoryService(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        var actual = categoryService.Create("", categoryName).Result;
        
        Assert.Equal(-1,actual);
        moqLogger.Verify(x=>x.Log(It.IsAny<string>()),Times.Never);
    }
    #endregion
    
    #region ChangeNameMethodTest

    [Theory]
    [InlineData("Some name","New Name",true)]
    [InlineData("Some name","NewName",true)]
    [InlineData("Some name","Some name",false)]
    [InlineData("Some name","",false)]
    [InlineData("Some name"," ",false)]
    public void ChangeNameAndLoggingTestWhenUserIsAdmin(string oldName, string newName, bool expected)
    {
        const string token = "token";
        var storedCategoryEntity = new CategoryEntity{Id=1,Name = oldName};
        var mappedCategory = new Category() { Id = 1, Name=newName };
        
        var tokenHandler = Mock.Of<ITokenHandler>(x=>x.GetUser(token)==new User(){IsAdmin = true});
        var repository = Mock.Of<IRepository<CategoryEntity>>(x =>
            x.GetById(storedCategoryEntity.Id) == storedCategoryEntity &&
            x.GetAll()== new List<CategoryEntity>{storedCategoryEntity});
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CategoryEntity, Category>();
            cfg.CreateMap<Category, CategoryEntity>();
        }).CreateMapper();
        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();
        
        var actual = new CategoryService(unitOfWork, tokenHandler, moqLogger.Object, mapper).ChangeName(token, newName, 1).Result;
        var expectedInvokedTimes = expected ? Times.Once() : Times.Never();
        
        Assert.Equal(expected,actual);
        moqLogger.Verify(x=>x.Log(It.IsAny<string >()), expectedInvokedTimes);
    }
    [Fact]
    public void ChangeNameAndLoggingTestWhenUserIsNotAdminReturnedException()
    {
        const string token = "token";
        var tokenHandler = Mock.Of<ITokenHandler>(x=>x.GetUser(token)==new User(){IsAdmin = false});
        var repository = Mock.Of<IRepository<CategoryEntity>>();
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var mapper = Mock.Of<IMapper>();
        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();
        moqLogger.Setup(x => x.Log(It.IsAny<Exception>())).Verifiable();
        
        var actual = new CategoryService(unitOfWork, tokenHandler, moqLogger.Object, mapper).ChangeName(token, "NewName", 1);

        Assert.ThrowsAsync<AuthenticationException>(()=>actual).Wait();
        moqLogger.Verify(x=>x.Log(It.IsAny<string >()), Times.Never());
        moqLogger.Verify(x=>x.Log(It.IsAny<Exception>()), Times.Once());
    }
    
    [Fact]
    public void ChangeNameAndLoggingTestWhenUserIsNullReturnedException()
    {
        const string token = "token";
        var tokenHandler = Mock.Of<ITokenHandler>(x=>x.GetUser(token)==null);
        var repository = Mock.Of<IRepository<CategoryEntity>>();
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var mapper = Mock.Of<IMapper>();
        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();
        moqLogger.Setup(x => x.Log(It.IsAny<Exception>())).Verifiable();
        
        var actual = new CategoryService(unitOfWork, tokenHandler, moqLogger.Object, mapper).ChangeName(token, "NewName", 1);

        Assert.ThrowsAsync<AuthenticationException>(()=>actual).Wait();
        moqLogger.Verify(x=>x.Log(It.IsAny<string >()), Times.Never());
        moqLogger.Verify(x=>x.Log(It.IsAny<Exception>()), Times.Once());
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
        var expected = new List<Category>
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
        
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == new User());
        var repository = Mock.Of<IRepository<CategoryEntity>>(x => x.GetAll() == categoryEntities);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var logger = Mock.Of<ILogger>();
        var mapper = Mock.Of<IMapper>(x =>x.Map<IEnumerable<Category>>(
                It.Is<IEnumerable<CategoryEntity>>(c => c.All(v => expected.Any(b => b.Id == v.Id)))) ==
            expected);

        var service = new CategoryService(unitOfWork, tokenHandler, logger, mapper);

        var actual = service.GetAll("").Result;
        
        Assert.Same(expected,actual);

    }
    [Fact]
    public void GetAllTestUserIsNullReturnedException()
    {

        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) ==null);
        var unitOfWork = Mock.Of<IUnitOfWork>();
        var logger = Mock.Of<ILogger>();
        var mapper = Mock.Of<IMapper>();

        var service = new CategoryService(unitOfWork, tokenHandler, logger, mapper);

        var actual = service.GetAll("");

        Assert.ThrowsAsync<AuthenticationException>(() => actual).Wait();

    }

    #endregion

    #region GetByNameMethodTest

    [Fact]
    public void GetByNameMethodTest()
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
        var expected = new Category()
            {
                Id = 2,
                Name = "Second"
            };

        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == new User());
        var repository = Mock.Of<IRepository<CategoryEntity>>(x => x.GetAll() == categoryEntities);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var logger = Mock.Of<ILogger>();
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CategoryEntity,Category>();
        }).CreateMapper();

        var service = new CategoryService(unitOfWork, tokenHandler, logger, mapper);

        var actual = service.GetByName("","Second").Result;
        
        Assert.Equal(expected.Id,actual.Id);

    }

    #endregion
}