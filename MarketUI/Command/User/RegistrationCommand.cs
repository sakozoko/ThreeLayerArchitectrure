using System.Collections.Generic;
using BLL;
using BLL.Objects;
using MarketUI.Command.Base;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command.User;

public class RegistrationCommand : BaseCommand
{
    private readonly IServiceManager _serviceManager;
    private Dictionary<string, string> _dict;

    public RegistrationCommand(IServiceManager serviceManager, IUserInterfaceMapperHandler mapperHandler,
        ICommandsInfoHandler cih) :
        base(mapperHandler, cih)
    {
        _serviceManager = serviceManager;
    }

    public override string Execute(string[] args)
    {
        if (ConsoleUserInterface.AuthenticationData is not null)
            return "You cannot register when you are already logged in";
        var request = new AuthenticateRequestModel();
        if (!TryCreateDictionary(args) || !TryParseAndSaveName(request) ||
            (!TryParseAndSavePsw(request) && !TryParseAndSaveSurname(request)))
            return GetHelp();
        var response =
            Mapper.Map<AuthenticateResponseModel>(
                _serviceManager.UserService.Registration(Mapper.Map<AuthenticateRequest>(request)));
        if (response is null) return "Args value incorrect";
        ConsoleUserInterface.AuthenticationData = response;
        return $"{response.Name} welcome!";
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

    private bool TryParseAndSaveSurname(AuthenticateRequestModel requestModel)
    {
        if (_dict.TryGetValue(Parameters[1], out var surname))
        {
            requestModel.Surname = surname;
            return true;
        }

        return false;
    }

    private bool TryParseAndSavePsw(AuthenticateRequestModel requestModel)
    {
        if (_dict.TryGetValue(Parameters[2], out var psw))
        {
            requestModel.Password = psw;
            return true;
        }

        return false;
    }
}