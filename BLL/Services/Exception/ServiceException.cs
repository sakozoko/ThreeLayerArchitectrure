using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace BLL.Util.Services.Exception;

[Serializable]
public class ServiceException : System.Exception
{
    public ServiceException(string message, Type serviceType, [CallerMemberName] string callerName = "") :
        base($"{serviceType.FullName}.{callerName}: {message}")
    {
    }

    protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}