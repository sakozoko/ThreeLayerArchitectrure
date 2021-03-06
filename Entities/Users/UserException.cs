using System.Runtime.Serialization;

namespace Entities.Users;

[Serializable]
public class UserException : Exception
{
    public UserException(string msg) : base(msg)
    {
    }

    protected UserException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }
}