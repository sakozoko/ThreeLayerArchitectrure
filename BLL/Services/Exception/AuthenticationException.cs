using System;
using System.Runtime.Serialization;

namespace BLL.Services.Exception
{
    [Serializable]
    public class AuthenticationException : System.Exception
    {
        public AuthenticationException()
        {
        }

        public AuthenticationException(string msg) : base(msg)
        {
        }

        public AuthenticationException(string msg, System.Exception innerException) : base(msg, innerException)
        {
        }


        protected AuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}