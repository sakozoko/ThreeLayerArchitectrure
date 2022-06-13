using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BLL.Command;
using Xunit;

namespace ConsolesShopTest.Commands;

//Because BasicCommand is an abstract class,
//I will use the concrete realization BasicCommandTestClass,
//it only has a basic realization 
public class BasicCommandTestClass : BasicCommand
{
    public BasicCommandTestClass(string[] names = null!, string[] parameters = null!) : base(names, parameters)
    {
    }

    public override Task<string> Execute(string[] args = null)
    {
        throw new NotImplementedException();
    }

    public override Task<string> GetHelp()
    {
        throw new NotImplementedException();
    }
}

public class BasicCommandTest
{
    [Theory]
    [InlineData(new[] { "n", "na" }, new[] { "p", "pa" })]
    [InlineData(new[] { "n", "na" }, null)]
    public void ConstructorWorksCorrectly(string[] names, string[] parameters)
    {
        //arrange
        var command = new BasicCommandTestClass(names, parameters);
        var fieldInfoPrivateNames =
            typeof(BasicCommandTestClass).BaseType?
                .GetField("_names", BindingFlags.Instance | BindingFlags.NonPublic);
        var fieldInfoPrivateParams = typeof(BasicCommandTestClass).BaseType?
            .GetField("_params", BindingFlags.Instance | BindingFlags.NonPublic);
        //act
        var actualNames = fieldInfoPrivateNames?.GetValue(command);
        var actualParams = fieldInfoPrivateParams?.GetValue(command);
        //assert
        Assert.Same(names, actualNames);
        Assert.Same(parameters, actualParams);
    }

    [Theory]
    [InlineData(new[] { "sd", "-psdf" }, new[] { "sd", "sdddssd", "asdas", "-psdf", "asd" })]
    [InlineData(new[] { "-s" }, new[] { "asds", "-s", "asdas" })]
    [InlineData(new[] { "-sd", "-psdf", "asd", "asd" },
        new[] { "-sd", "sdddssd", "-psdf", "psdf", "asd", "asd", "asd", "das" })]
    [InlineData(new[] { "-g" }, new[] { "-g" })]
    public void ParseArgsTestReturnCorrectParams(string[] param, string[] args)
    {
        //arrange
        var command = new BasicCommandTestClass();
        var methodInfo =
            typeof(BasicCommandTestClass).GetMethod("ParseArgs", BindingFlags.NonPublic | BindingFlags.Instance);
        var fieldInfo =
            typeof(BasicCommandTestClass).BaseType?.GetField("_params", BindingFlags.NonPublic | BindingFlags.Instance);

        fieldInfo?.SetValue(command, param);
        //act
        var result = methodInfo?.Invoke(command, new object?[]
        {
            args
        });
        var actual = (Dictionary<string, string>)result!;
        //assert
        var allParamsParsed = param.All(p => actual.TryGetValue(p, out _));
        Assert.True(allParamsParsed);
    }

    [Theory]
    [InlineData(new[] { "sd", "-psdf" }, new[] { "sdddssd", "asdas", "asd" })]
    [InlineData(new[] { "" }, new[] { "asds", "-s", "asdas" })]
    public void TryParseArgsTestReturnCorrectParams(string[] param, string[] args)
    {
        //arrange
        var command = new BasicCommandTestClass();
        var methodInfo =
            typeof(BasicCommandTestClass).GetMethod("TryParseArgs", BindingFlags.NonPublic | BindingFlags.Instance);
        var fieldInfo =
            typeof(BasicCommandTestClass).BaseType?.GetField("_params", BindingFlags.NonPublic | BindingFlags.Instance);

        fieldInfo?.SetValue(command, param);
        //act
        var result = methodInfo?.Invoke(command, new object?[]
        {
            args,
            new Dictionary<string, string>()
        });
        //assert
        var actual = (bool)result!;
        Assert.False(actual);
    }

    [Theory]
    [InlineData(new[] { "sd", "dc" }, "sd")]
    [InlineData(new[] { "-sd", "dc" }, "-sd")]
    [InlineData(new[] { "sdds" }, "sdds")]
    [InlineData(new[] { "sd123 sd", "dcva 12" }, "dcva 12")]
    [InlineData(new[] { "sd123 sd", "dcva 123", "sd", "gkal", "jkas", "dcva 12" }, "dcva 12")]
    public void ItsMeCorrectlyReturnsTrue(string[] names, string commandName)
    {
        //arrange
        var command = new BasicCommandTestClass();
        var fieldInfo =
            typeof(BasicCommandTestClass).BaseType?.GetField("_names", BindingFlags.Instance | BindingFlags.NonPublic);
        fieldInfo?.SetValue(command, names);
        //act
        var actual = command.ItsMe(commandName);
        //assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData(new[] { "sd", "dc" }, "sddc")]
    [InlineData(new[] { "-sd", "dc" }, "-dc")]
    [InlineData(new[] { "sdds" }, "dd")]
    [InlineData(new[] { "sd123 sd", "dcva 12" }, "dcva  12")]
    [InlineData(new[] { "sd123 sd", "dcva 123", "sd", "gkal", "jkas" }, "dcva 12")]
    public void ItsMeCorrectlyReturnsFalse(string[] names, string commandName)
    {
        //arrange
        var command = new BasicCommandTestClass();
        var fieldInfo =
            typeof(BasicCommandTestClass).BaseType?.GetField("_names", BindingFlags.Instance | BindingFlags.NonPublic);
        fieldInfo?.SetValue(command, names);
        //act
        var actual = command.ItsMe(commandName);
        //assert
        Assert.False(actual);
    }
}