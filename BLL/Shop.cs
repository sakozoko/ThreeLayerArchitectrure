using BLL.Command;
using BLL.Helpers;
using DAL;
using Entities.User;

namespace BLL;

public class Shop
{
    private static readonly ICommand IncorrectCommand = new IncorrectCommand();
    private static readonly IUser GuestUser = null;
    private readonly ICommand[] _commands;
    private readonly UnitOfWork _unitOfWork = new();
    

    public Shop()
    {
        LoggedInUser = GuestUser;
        #region SetCommands
        _commands = new[]
        {
            new SearchByNameCommand(_unitOfWork.ProductRepository),
            new ViewProductsCommand(_unitOfWork.ProductRepository),
            new LoginCommand(_unitOfWork.UserRepository,
                x => { LoggedInUser = x; }),
            new LogoutCommand(
                () => { LoggedInUser = GuestUser; }),
            new CreateNewOrderCommand(),
            new ViewOrderHistoryCommand(),
            IncorrectCommand
        };

        _commands[^1] = new HelpCommand(_commands);
        #endregion
        
    }
    
    public Shop(ICommand[] commands, IUser user = null)
    {
        _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        LoggedInUser = user;
    }
    
    public IUser LoggedInUser { get; set; }

    private ICommand CorrectCommand(string name)
    {
        var correctCommand = IncorrectCommand;
        foreach (var command in _commands)
        {
            if (!command.ItsMe(name)) continue;
            correctCommand = command;
            break;
        }

        return correctCommand;
    }

    public void ExecuteCommand(string commandString)
    {
        var args = SplitString(commandString);

        var command = CorrectCommand(args[0]);

        if (args.Length > 1)
        {
            if (command.CanExecute(LoggedInUser, args[1..]))
                command.Execute();
            else
                IncorrectCommand.Execute();
        }
        else
        {
            ExecuteCommandWithoutArgs(command);
        }
    }

    private string[] SplitString(string str)
    {
        return ParseStringHelper.SplitStringWithQuotes(str);
    }


    private void ExecuteCommandWithoutArgs(ICommand command)
    {
        if (command.CanExecute(LoggedInUser))
            command.Execute();
        else
            IncorrectCommand.Execute();
    }
    
}