using ConsolesShop.Command;
using ConsolesShop.User;

namespace ConsolesShop;

public class Shop
{
    private static readonly ICommand IncorrectCommand = new IncorrectCommand();
    private static readonly IUser GuestUser = new Guest();
    private readonly ICommand[] _commands;

    public Shop()
    {
        LoggedInUser = GuestUser;

        _commands = new []
        {
            new SearchByNameCommand(Assortment.Products),
            new ViewProductsCommand(Assortment.Products),
            new LoginCommand(Assortment.Users),
            new LogoutCommand(),
            IncorrectCommand
        };

        _commands[^1] = new HelpCommand(_commands);
        InitializeRegisteredUserEvents();
    }

   public IUser LoggedInUser { get; private set; }

    private void InitializeRegisteredUserEvents()
    {
        foreach (var user in Assortment.Users)
            if (user is RegisteredUser us)
                us.ChangeIsLoggedIn += OnLoggedInChanged;
    }

    private void OnLoggedInChanged(object sender, UserLogInEventArgs e)
    {
        if (e.IsLoggedIn)
            LoggedInUser = sender as IUser;
        else if (LoggedInUser is RegisteredUser { IsLoggedIn: true })
            LoggedInUser = GuestUser;
    }

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
        var args = commandString.Split(' ');
        var command = CorrectCommand(args[0]);

        if (args.Length > 1)
        {
            if (command.CanExecute(LoggedInUser, args[1..]))
                command.Execute(args);
            else
                IncorrectCommand.Execute();
        }
        else
        {
            ExecuteCommandWithoutArgs(command);
        }
    }

    private void ExecuteCommandWithoutArgs(ICommand command)
    {
        if (command.CanExecute(LoggedInUser))
            command.Execute();
        else
            IncorrectCommand.Execute();
    }
}