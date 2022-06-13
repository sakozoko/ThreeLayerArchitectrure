using System;
using System.IO;
using System.Text;

namespace ConsolesShopTest.Helpers;

public class TextWriterTest:TextWriter
{
    private readonly Action<string?> _action;
    public override Encoding Encoding { get; }

    public TextWriterTest(Action<string?> act, Encoding encoding)
    {
        _action = act;
        Encoding = encoding;
    }
    public override void WriteLine(string? value)
    {
        _action("\n"+value);
        base.WriteLine(value);
    }

    public override void WriteLine(object? value)
    {
        _action("\n"+value);
        base.WriteLine(value);
    }

    public override void Write(string? s)
    {
        _action(s);
        base.Write(s);
    }
}