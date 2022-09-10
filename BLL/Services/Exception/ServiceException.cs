using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace BLL.Services.Exception
{
    [Serializable]
    public class ServiceException : System.Exception
    {
        public ServiceException()
        {
        }

        public ServiceException(string message) : base(message)
        {
        }

        public ServiceException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public ServiceException(string message, Type serviceType, [CallerMemberName] string callerName = "") :
            base($"{serviceType.FullName}.{callerName}: {message}")
        {
        }


        protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}