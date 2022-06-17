using System.Threading.Tasks;

namespace Shop.Command;

public interface ICommand
{
    public bool ItsMe(string commandName);
    public string Execute(string[] args);
    public string GetHelp();
}