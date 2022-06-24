using System.Diagnostics;
using System.Text;

namespace BLL.Logger;

public class DebugLogger : ILogger
{
    public Task Log(string msg)
    {
        return Task.Factory.StartNew(() => Debug.WriteLine(msg));
    }

    public Task Log(Exception exception)
    {
        return Task.Factory.StartNew(() =>
        {
            StringBuilder sb = new();
            if (exception.Data.Contains("type"))
            {
                var type = exception.Data["type"] as Type;
                sb.Append(type.FullName);
            }

            if (exception.Data.Contains("callerName"))
            {
                var callerName = exception.Data["callerName"] as string;
                sb.Append($".{callerName}");
            }

            sb.Append($": {exception.Message} - {exception.GetType().FullName}");
            Debug.WriteLine(sb.ToString(), "Exception");
        });
    }
}