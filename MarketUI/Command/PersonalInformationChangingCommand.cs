using System.Collections.Generic;
using BLL;
using BLL.Objects;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command;

public class PersonalInformationChangingCommand : BaseCommand
{
    private static readonly string[] Parameters = { "-id", "-n", "-s", "-p", "-op" };
    private readonly IServiceContainer _serviceContainer;
    private Dictionary<string, string> _dict;

    public PersonalInformationChangingCommand(IServiceContainer serviceContainer,
        IUserInterfaceMapperHandler mapperHandler) : base(mapperHandler, Parameters)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        if (!ArgumentsAreValid(args))
            return GetHelp();
        _dict.TryGetValue(Parameters[0], out var stringUserId);
        int.TryParse(stringUserId, out var userId);
        UserModel targetUser = null;
        if (userId != 0)
        {
            targetUser = Mapper.Map<UserModel>(_serviceContainer.UserService
                .GetById(ConsoleUserInterface.AuthenticationData.Token, userId).Result);
            if (targetUser is null)
                return "User not found";
        }

        if (TryChangeName(targetUser) | TryChangeSurname(targetUser) | TryChangePsw(targetUser))
        {
            if (targetUser is null)
            {
                
            }
            return "Updated information saved";
        }

        return "Something is wrong";
    }

    private bool TryChangeName(UserModel targetUser)
    {
        if (_dict.TryGetValue(Parameters[1], out var newName) && !string.IsNullOrEmpty(newName))
            return _serviceContainer.UserService.ChangeName(ConsoleUserInterface.AuthenticationData.Token, newName,
                Mapper.Map<User>(targetUser)).Result;
        return false;
    }

    private bool TryChangeSurname(UserModel targetUser)
    {
        if (_dict.TryGetValue(Parameters[2], out var newSurname) && !string.IsNullOrEmpty(newSurname))
            return _serviceContainer.UserService.ChangeSurname(ConsoleUserInterface.AuthenticationData.Token, newSurname,
                Mapper.Map<User>(targetUser)).Result;
        return false;
    }

    private bool TryChangePsw(UserModel targetUser)
    {
        if (_dict.TryGetValue(Parameters[3], out var newPsw) && newPsw.Length > 5 &&
            _dict.TryGetValue(Parameters[4], out var oldPsw))
            return _serviceContainer.UserService.ChangePassword(ConsoleUserInterface.AuthenticationData.Token, newPsw, oldPsw,
                Mapper.Map<User>(targetUser)).Result;
        return false;
    }

    private bool ArgumentsAreValid(string[] args)
    {
        return TrySetDictionary(args)  && ((_dict.ContainsKey(Parameters[0])
                                            && (_dict.ContainsKey(Parameters[1]) || 
                                                _dict.ContainsKey(Parameters[2]) ||
                                                _dict.ContainsKey(Parameters[3]))) || 
                                           _dict.ContainsKey(Parameters[1]) || _dict.ContainsKey(Parameters[2])) || 
               (_dict.ContainsKey(Parameters[3]) && _dict.ContainsKey(Parameters[4]));
    }
    private bool TrySetDictionary(string[] args)
    {
        return TryParseArgs(args, out _dict);
    }
    public override string GetHelp()
    {
        return "Not implement";
    }
}