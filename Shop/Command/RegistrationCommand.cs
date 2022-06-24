using BLL;
using Entities;

namespace Shop.Command;

public class RegistrationCommand : BaseCommand
{
    private static readonly string[] Names = { "reg", "r", "rg" };
    private static readonly string[] Parameters = { "-n", "-p" };
    private readonly Service _service;
    private string _name;
    private string _password;

    public RegistrationCommand(Service service) : base(Names, Parameters)
    {
        _service = service;
    }

    public override string Execute(string[] args)
    {
        if (!TryParseParameters(args)) return GetHelp();
        var request = new AuthenticateRequest { Username = _name, Password = _password };
        var response = _service.Factory.UserService.Registration(request);
        if (response is null) return "Args value incorrect";
        Shop.AuthenticationData = response;
        return $"{response.Name} welcome!";
    }

    private bool TryParseParameters(string[] args)
    {
        return TryParseArgs(args, out var dict) &&
               dict.TryGetValue(Parameters[0], out _name) && dict.TryGetValue(Parameters[1], out _password);
    }

    public override string GetHelp()
    {
        return "Registration \t reg, rg, r \t -n -p ";
    }
}