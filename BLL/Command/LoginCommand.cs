using DAL.Repositories;
using Entities.User;
namespace BLL.Command;

public class LoginCommand : BasicCommand
{
    private static readonly string[] Names = { "li", "login" };
    private static readonly string[] Parameters = { "-n", "-p" };
    private readonly Action<IUser> _saveSession;
    private readonly IRepository<IUser> _userRepository;
    private string _name;
    private string _password;

    public LoginCommand(IRepository<IUser> userRepository, Action<IUser> act) : base(Names, Parameters)
    {
        _userRepository = userRepository;
        _saveSession = act;
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        return user is null && TryParseLoginAndPassword(args);
    }

    public override Task<string> Execute(string[] args = null)
    {
        return Task<string>.Factory.StartNew(() =>
        {
            var user = TryFindUserByName();
            switch (user)
            {
                case null:
                    return "Name incorrect";
                default:
                    if (!user.Login(_password)) return "Password incorrect";
                    _saveSession.Invoke(user);
                    return "Login successful";
            }
        });
        
    }
    private IUser TryFindUserByName()
    {
        return _userRepository.GetAll().ToList().Find(x=>x.Name==_name);
    }

    private bool TryParseLoginAndPassword(string[] args)
    {
        if (TryParseArgs(args, out var dictionary))
            return dictionary.TryGetValue(Parameters[0], out _name)
                   && dictionary.TryGetValue(Parameters[1], out _password);
        return false;
    }

    public override Task<string> GetHelp()
    {
        return Task<string>.Factory.StartNew(() => "Login \t Login or li \t -n -p");
    }
}