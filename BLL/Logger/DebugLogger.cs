using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Logger
{
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
                var sb = new StringBuilder();
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
}