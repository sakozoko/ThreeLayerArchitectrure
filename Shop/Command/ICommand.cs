using System.Threading.Tasks;

namespace Shop.Command;

public interface ICommand
{
    public bool ItsMe(string commandName);
    public Task<string> Execute(string[] args);
    public string GetHelp();
}