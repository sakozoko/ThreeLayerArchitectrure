using BLL;
using BLL.Objects;

namespace MarketUI.Command;

public class RegistrationCommand : BaseCommand
{
    private static readonly string[] Parameters = { "-n", "-p" };
    private readonly IServiceContainer _serviceContainer;
    private string _name;
    private string _password;

    public RegistrationCommand(IServiceContainer serviceContainer) : base(Parameters)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        if (!TryParseParameters(args)) return GetHelp();
        var request = new AuthenticateRequest { Username = _name, Password = _password };
        var response = _serviceContainer.UserService.Registration(request);
        if (response is null) return "Args value incorrect";
        ConsoleUserInterface.AuthenticationData = response;
        return $"{response.Name} welcome!";
    }

    private bool TryParseParameters(string[] args)
    {
        return TryParseArgs(args, out var dict) &&
               dict.TryGetValue(Parameters[0], out _name) && dict.TryGetValue(Parameters[1], out _password);
    }

    public override string GetHelp()
    {
        return "Registration \t registration, r \t -n -p ";
    }
}