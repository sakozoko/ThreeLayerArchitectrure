using System.Collections.Generic;

namespace MarketUI.Extension;

public static class StringParserExtension
{
    public static string[] SplitStringWithQuotes(this string str)
    {
        str = str.Replace("  ", " ").Trim();
        if (str == "")
            return new[] { "" };
        var strToSplit = str;
        var parts = new List<string>();
        while (strToSplit.Contains('"'))
        {
            var indexOfFirstQuote = strToSplit.IndexOf('"');
            var indexOfSecondQuote = strToSplit[(indexOfFirstQuote + 1)..].IndexOf('"') + indexOfFirstQuote + 1;
            if (indexOfSecondQuote <= indexOfFirstQuote + 1)
            {
                strToSplit = strToSplit.Remove(indexOfSecondQuote, 1);
                continue;
            }

            parts.Add(strToSplit[(indexOfFirstQuote + 1)..indexOfSecondQuote]);

            strToSplit = strToSplit[..indexOfFirstQuote] + strToSplit[(indexOfSecondQuote + 1)..];
        }

        var arr = strToSplit.Split(' ');
        var k = 0;

        for (var i = 0; i < arr.Length; i++)
            if (arr[i] == "")
                arr[i] = parts[k++];

        return arr;
    }
}