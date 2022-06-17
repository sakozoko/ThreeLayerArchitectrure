namespace BLL.Logger;

public interface ILogger
{
    public Task Log(string msg);
    public Task Log(Exception exception);
}