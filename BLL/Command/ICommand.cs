using Entities.User;

namespace BLL.Command;

public interface ICommand
{
    public bool ItsMe(string commandName);
    public bool CanExecute(IUser user, string[] args = null);
    public Task<string> Execute(string[] args=null);
    public Task<string> GetHelp();
}