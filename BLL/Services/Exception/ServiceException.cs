using System.Runtime.CompilerServices;

namespace BLL.Services.Exception;

public class ServiceException : System.Exception
{
    public ServiceException(string message, Type serviceType, [CallerMemberName] string callerName = ""):
        base($"{serviceType.FullName}.{callerName}: {message}")
    {
    }
}