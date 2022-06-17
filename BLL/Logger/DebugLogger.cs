using System.Diagnostics;

namespace BLL.Logger;

public class DebugLogger : ILogger
{
    public Task Log(string msg) => 
        Task.Factory.StartNew(() => Debug.WriteLine(msg));
    
    public Task Log(Exception exception) =>
        Task.Factory.StartNew(() => Debug.WriteLine($"{exception.Message} - {exception.GetType().FullName}", "Exception"));
}