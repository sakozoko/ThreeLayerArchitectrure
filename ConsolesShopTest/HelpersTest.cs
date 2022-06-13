using System;
using System.Reflection;
using Xunit;

namespace ConsolesShopTest;

public class HelpersTest
{
    [Theory]
    [InlineData("li -n \"Nona 228\" -p 123",
        new[]
        {
            "li",
            "-n",
            "Nona 228",
            "-p",
            "123"
        })]
    [InlineData("li -n  \"Nona 228\" -p 123",
        new[]
        {
            "li",
            "-n",
            "Nona 228",
            "-p",
            "123"
        })]
    [InlineData("li -n \"Nona 228\" -p \"123 123\"",
        new[]
        {
            "li",
            "-n",
            "Nona 228",
            "-p",
            "123 123"
        })]
    [InlineData("  \"li -n\" Nona 228\" -p 123",
        new[]
        {
            "li -n",
            "Nona",
            "228",
            "-p",
            "123"
        })]
    [InlineData("",
        new[]
        {
            ""
        })]
    public void ParseStringWithQuotesTest(string str, string[]? expected)
    {
        //arrange
        var methodInfo = Type.GetType("ConsolesShop.Helpers.ParseStringHelper, ConsolesShop")?.GetMethod(
            "SplitStringWithQuotes",
            BindingFlags.Public | BindingFlags.Static);
        //act
        var actual = methodInfo?.Invoke(null, new object[] { str }) as string[];
        //assert
        Assert.Equal(expected, actual);
        Assert.NotNull(actual);
    }
}