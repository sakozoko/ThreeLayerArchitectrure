using System;
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
        var mapper = Mock.Of<IMapper>(x =>
            x.Map<CategoryEntity>(It.Is<Category>(c => c.Name == categoryName)) ==
            new CategoryEntity{
             Name = categoryName });

        var categoryService = new CategoryService(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        var resultId = categoryService.Create("", categoryName).Result;
        var actual = resultId == resultCategoryId;
        var expectedTimes = actual ? Times.Once() : Times.Never();
        
        moqLogger.Verify(x=>x.Log(It.IsAny<string>()), expectedTimes);
        Assert.Equal(expected,actual);
    }
    [Fact]
    public void CreateCategoryAndLoggingTokenTestUserIsNotAdminReturnedException()
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
    public void CreateCategoryAndLoggingTokenTestUserIsNullReturnedException()
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
}