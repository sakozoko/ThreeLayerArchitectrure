using System.Collections.Generic;
using BLL;
using BLL.Objects;
using MarketUI.Command.Base;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command.User;

public class LoginCommand : BaseCommand
{
    private readonly IServiceContainer _serviceContainer;
    private Dictionary<string, string> _dict;

    public LoginCommand(IServiceContainer serviceContainer, IUserInterfaceMapperHandler mapperHandler,
        ICommandsInfoHandler cih) :
        base(mapperHandler, cih)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        var authenticateRequest = new AuthenticateRequestModel();
        if (!TryCreateDictionary(args) || !TryParseAndSaveName(authenticateRequest) ||
            !TryParseAndSavePsw(authenticateRequest))
            return GetHelp();

        var response = Mapper.Map<AuthenticateResponseModel>(
            _serviceContainer.UserService.Authenticate(Mapper.Map<AuthenticateRequest>(authenticateRequest)));
        if (response is null) return "Name or password is incorrect";
        ConsoleUserInterface.AuthenticationData = response;
        return $"{response.Name}, hi!";
    }

    private bool TryCreateDictionary(string[] args)
    {
        return TryParseArgs(args, out _dict);
    }

    private bool TryParseAndSaveName(AuthenticateRequestModel requestModel)
    {
        if (_dict.TryGetValue(Parameters[0], out var name))
        {
            requestModel.Name = name;
            return true;
        }

        return false;
    }

    private bool TryParseAndSavePsw(AuthenticateRequestModel requestModel)
    {
        if (_dict.TryGetValue(Parameters[1], out var psw))
        {
            requestModel.Password = psw;
            return true;
        }

        return false;
    }
}