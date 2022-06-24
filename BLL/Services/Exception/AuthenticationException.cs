using System.Runtime.Serialization;

namespace BLL.Services.Exception;

[Serializable]
public class AuthenticationException : System.Exception
{
    public AuthenticationException(string msg) :
        base(msg)
    {
    }

    protected AuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}