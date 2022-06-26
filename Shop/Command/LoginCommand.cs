using BLL;
using BLL.Services.Factory;
using Entities;

namespace Shop.Command;

public class LoginCommand : BaseCommand
{
    private static readonly string[] Names = { "li", "login" };
    private static readonly string[] Parameters = { "-n", "-p" };
    private readonly IServiceContainer _serviceContainer;
    private string _name;
    private string _password;

    public LoginCommand(IServiceContainer serviceContainer) : base(Names, Parameters)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        if (!TryParseLoginAndPassword(args)) return GetHelp();
        var authenticateRequest = new AuthenticateRequest { Username = _name, Password = _password };
        var response = _serviceContainer.UserService.Authenticate(authenticateRequest);
        if (response is null) return "Name or password is incorrect";
        ConsoleUserInterface.AuthenticationData = response;
        return $"{response.Name}, hi!";
    }


    private bool TryParseLoginAndPassword(string[] args)
    {
        if (TryParseArgs(args, out var dictionary))
            return dictionary.TryGetValue(Parameters[0], out _name)
                   && dictionary.TryGetValue(Parameters[1], out _password);
        return false;
    }

    public override string GetHelp()
    {
        return "Login \t Login or li \t -n -p";
    }
}