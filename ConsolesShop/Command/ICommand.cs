using ConsolesShop.User;

namespace ConsolesShop.Command;

public interface ICommand
{
    public bool ItsMe(string commandName);
    public bool CanExecute(IUser user, string[] args = null);
    public void Execute(string[] args = null);
    public void GetHelp();
}