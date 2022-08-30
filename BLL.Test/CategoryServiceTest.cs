using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public void CreateCategoryAndLoggingCorrectlyWork(string categoryName, bool expected)
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
        MethodsTestReturnedException(null, categoryService => categoryService.Create("",""));
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
        MethodsTestReturnedException(new User{IsAdmin = false},categoryService=> categoryService.ChangeName("", "New Name",1));
    }
    
    [Fact]
    public void ChangeNameAndLoggingTestWhenUserIsNullReturnedException()
    {
        MethodsTestReturnedException(null, categoryService => categoryService.ChangeName("","New Name",1));
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
        MethodsTestReturnedException(null, categoryService => categoryService.GetAll(""));
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
    [Fact]
    public void GetByNameTestUserIsNullReturnedException()
    {
        MethodsTestReturnedException(null, categoryService => categoryService.GetByName("",""));
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

        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == new User());
        var repository = Mock.Of<IRepository<CategoryEntity>>(x => x.GetById(expectedId) == categoryEntities[expectedId-1]);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var logger = Mock.Of<ILogger>();
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CategoryEntity,Category>();
        }).CreateMapper();

        var service = new CategoryService(unitOfWork, tokenHandler, logger, mapper);

        var actual = service.GetById("", expectedId).Result;
        var expected = mapper.Map<Category>(categoryEntities[expectedId - 1]);
        
        Assert.Equal(expected.Id,actual.Id);
        Assert.Equal(expected.Name,actual.Name);
    }
    
    [Fact]
    public void GetByIdTestUserIsNullReturnedException()
    {
        MethodsTestReturnedException(null, categoryService => categoryService.GetById("",0));
    }

    #endregion

    #region RemoveMethodTest

    [Theory]
    [InlineData(0,0,true)]
    [InlineData(1,0,false)]
    [InlineData(0,1,false)]
    [InlineData(0,-1,false)]
    [InlineData(1,1,true)]
    public void RemoveTestCorrectlyWork(int targetId, int removedId, bool expected)
    {
        var categoryEntity = new CategoryEntity{ Id = targetId };
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == new User(){IsAdmin = true});
        var repository = Mock.Of<IRepository<CategoryEntity>>(x => 
            x.Delete(It.Is<CategoryEntity>(c=>c.Id==targetId)) == true &&
            x.GetById(targetId)==categoryEntity);
        var unitOfWork = Mock.Of<IUnitOfWork>(x => x.CategoryRepository == repository);
        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CategoryEntity,Category>();
        }).CreateMapper();

        var service = new CategoryService(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        var actual = service.Remove("", removedId).Result;
        var times = expected ? Times.Once() : Times.Never();
        
        Assert.Equal(expected, actual);
        moqLogger.Verify(x=>x.Log(It.IsAny<string>()), times);
    }

    [Fact]
    public void RemoveTestUserIsNotAdminReturnedException()
    {
        MethodsTestReturnedException(new User{IsAdmin = false},categoryService=>categoryService.Remove("",0));
    }

    [Fact]
    public void RemoveTestUserIsNullReturnedException()
    {
        MethodsTestReturnedException(null, categoryService => categoryService.Remove("",0));
    }

    #endregion
    
    private static void MethodsTestReturnedException(User tokenHandlerReturnedValue, Func<CategoryService, Task> testMethod)
    {
        var tokenHandler = Mock.Of<ITokenHandler>(x => x.GetUser(It.IsAny<string>()) == tokenHandlerReturnedValue);
        var unitOfWork = Mock.Of<IUnitOfWork>();
        var mapper = Mock.Of<IMapper>();
        var moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<Exception>())).Verifiable();
        moqLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();

        var service = new CategoryService(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        Assert.ThrowsAsync<AuthenticationException>(() => testMethod.Invoke(service)).Wait();
        moqLogger.Verify(x=>x.Log(It.IsAny<Exception>()),Times.Once);
        moqLogger.Verify(x=>x.Log(It.IsAny<string>()),Times.Never);
    }
}