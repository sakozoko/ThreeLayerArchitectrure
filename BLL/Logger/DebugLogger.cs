using System.Diagnostics;

namespace BLL.Logger;

public class DebugLogger : ILogger
{
    public Task Log(string msg)
    {
        return Task.Factory.StartNew(() => Debug.WriteLine(msg));
    }

    public Task LogException(string msg)
    {
        return Task.Factory.StartNew(() => Debug.WriteLine(msg, "WARN"));
    }
}