using System.Threading.Tasks;
using BLL;
using Entities;

namespace Shop.Command;

public class LoginCommand : BasicCommand
{
    private static readonly string[] Names = { "li", "login" };
    private static readonly string[] Parameters = { "-n", "-p" };
    private readonly Service _service;
    private string _name;
    private string _password;

    public LoginCommand(Service service) : base(Names, Parameters)
    {
        _service = service;
    }

    public override string Execute(string[] args)
    {
        if (!TryParseLoginAndPassword(args)) return GetHelp();
        var authenticateRequest = new AuthenticateRequest { Username = _name, Password = _password };
        var response = _service.Factory.UserService.Authenticate(authenticateRequest);
        if (response is null) return "Name or password is incorrect";
        Shop.AuthenticationData = response;
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