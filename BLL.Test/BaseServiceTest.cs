using System;
using AutoMapper;
using BLL.Helpers.Token;
using BLL.Logger;
using BLL.Services.Exception;
using BLL.Test.Helpers;
using DAL;
using Entities;
using Moq;
using Xunit;

namespace BLL.Test;

public class BaseServiceTest
{
    [Fact]
    public void ConstructorTest()
    {
        CreateDependencies(out var moqLogger, out var unitOfWork, out var tokenHandler, out var mapper);

        var actual = new BaseServiceTester(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        Assert.Same(moqLogger.Object, actual.LoggerTest);
        Assert.Same(unitOfWork, actual.UnitOfWorkTest);
        Assert.Same(tokenHandler, actual.TokenHandlerTest);
        Assert.Same(mapper, actual.MapperTest);
    }

    [Fact]
    public void LogAndThrowAuthenticationExceptionTest()
    {
        const string expectedMsg = "new Msg";
        CreateDependencies(out var moqLogger, out var unitOfWork, out var tokenHandler, out var mapper);

        var baseServiceTest = new BaseServiceTester(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        var actual =
            Assert.Throws<AuthenticationException>(() =>
                baseServiceTest.LogAndThrowAuthenticationExceptionTest(expectedMsg));

        Assert.Equal(expectedMsg, actual.Message);
        moqLogger.Verify(x => x.Log(actual), Times.Once);
    }

    [Fact]
    public void ThrowAuthenticationExceptionIfUserIsNotAdminTestUserIsNotAdmin()
    {
        CreateDependencies(out var moqLogger, out var unitOfWork, out var tokenHandler, out var mapper);

        var baseServiceTest = new BaseServiceTester(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        var actual = Assert.Throws<AuthenticationException>(() =>
            baseServiceTest.ThrowAuthenticationExceptionIfUserIsNotAdminTest(
                new UserEntity
                {
                    IsAdmin = false
                }));

        moqLogger.Verify(x => x.Log(actual), Times.Once);
    }

    [Fact]
    public void ThrowAuthenticationExceptionIfUserIsNullTestUserIsNull()
    {
        CreateDependencies(out var moqLogger, out var unitOfWork, out var tokenHandler, out var mapper);

        var baseServiceTest = new BaseServiceTester(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        var actual =
            Assert.Throws<AuthenticationException>(() =>
                baseServiceTest.ThrowAuthenticationExceptionIfUserIsNullTest(null!));

        moqLogger.Verify(x => x.Log(actual), Times.Once);
    }

    [Fact]
    public void ThrowAuthenticationExceptionIfUserIsNullOrNotAdminTestUserIsNull()
    {
        CreateDependencies(out var moqLogger, out var unitOfWork, out var tokenHandler, out var mapper);

        var baseServiceTest = new BaseServiceTester(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        var actual = Assert.Throws<AuthenticationException>(() =>
            baseServiceTest.ThrowAuthenticationExceptionIfUserIsNullOrNotAdminTest(null!));

        moqLogger.Verify(x => x.Log(actual), Times.Once);
    }

    [Fact]
    public void ThrowAuthenticationExceptionIfUserIsNullOrNotAdminTestUserIsNotAdmin()
    {
        CreateDependencies(out var moqLogger, out var unitOfWork, out var tokenHandler, out var mapper);

        var baseServiceTest = new BaseServiceTester(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        var actual = Assert.Throws<AuthenticationException>(() =>
            baseServiceTest.ThrowAuthenticationExceptionIfUserIsNullOrNotAdminTest(
                new UserEntity
                {
                    IsAdmin = false
                }));

        moqLogger.Verify(x => x.Log(actual), Times.Once);
    }

    [Fact]
    public void ThrowAuthenticationExceptionIfUserIsNullOrNotAdminTestUserIsAdmin()
    {
        CreateDependencies(out var moqLogger, out var unitOfWork, out var tokenHandler, out var mapper);

        var baseServiceTest = new BaseServiceTester(unitOfWork, tokenHandler, moqLogger.Object, mapper);

        baseServiceTest.ThrowAuthenticationExceptionIfUserIsNullOrNotAdminTest(
            new UserEntity
            {
                IsAdmin = true
            });

        moqLogger.Verify(x => x.Log(It.IsAny<Exception>()), Times.Never);
    }

    private void CreateDependencies(out Mock<ILogger> moqLogger, out IUnitOfWork unitOfWork,
        out ITokenHandler tokenHandler,
        out IMapper mapper)
    {
        moqLogger = new Mock<ILogger>();
        moqLogger.Setup(x => x.Log(It.IsAny<Exception>())).Verifiable();
        unitOfWork = Mock.Of<IUnitOfWork>();
        tokenHandler = Mock.Of<ITokenHandler>();
        mapper = Mock.Of<IMapper>();
    }
}