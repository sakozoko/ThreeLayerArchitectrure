namespace BLL.Services;

public class ServiceException : Exception
{
    public ServiceException(string serviceName, string message)
    {
        Message = $"{serviceName} {message}";
    }

    public override string Message { get; }
}