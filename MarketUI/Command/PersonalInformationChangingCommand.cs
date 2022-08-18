﻿using System.Collections.Generic;
using BLL;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command;

public class PersonalInformationChangingCommand : BaseCommand
{
    private readonly IServiceContainer _serviceContainer;
    private Dictionary<string, string> _dict;

    public PersonalInformationChangingCommand(IServiceContainer serviceContainer,
        IUserInterfaceMapperHandler mapperHandler, ICommandsInfoHandler cih) : base(mapperHandler, cih)
    {
        _serviceContainer = serviceContainer;
    }

    public override string Execute(string[] args)
    {
        if (!ArgumentsAreValid(args))
            return GetHelp();
        _dict.TryGetValue(Parameters[0], out var stringUserId);
        int.TryParse(stringUserId, out var userId);
        if (userId != 0)
        {
            var targetUser = Mapper.Map<UserModel>(_serviceContainer.UserService
                .GetById(ConsoleUserInterface.AuthenticationData.Token, userId).Result);
            if (targetUser is null)
                return "User not found";
        }

        if (TryChangeName(userId) | TryChangeSurname(userId) | TryChangePsw(userId))
        {
            if (userId == 0)
                ConsoleUserInterface.AuthenticationData =
                    Mapper.Map<AuthenticateResponseModel>(_serviceContainer.UserService.GetAuthenticateResponse(
                        ConsoleUserInterface.AuthenticationData
                            .Token));
            return "Updated information saved";
        }

        return "Something is wrong";
    }

    private bool TryChangeName(int userId)
    {
        if (_dict.TryGetValue(Parameters[1], out var newName) && !string.IsNullOrEmpty(newName))
            return _serviceContainer.UserService.ChangeName(ConsoleUserInterface.AuthenticationData.Token, newName,
                userId).Result;
        return false;
    }

    private bool TryChangeSurname(int userId)
    {
        if (_dict.TryGetValue(Parameters[2], out var newSurname) && !string.IsNullOrEmpty(newSurname))
            return _serviceContainer.UserService.ChangeSurname(ConsoleUserInterface.AuthenticationData.Token,
                newSurname,
                userId).Result;
        return false;
    }

    private bool TryChangePsw(int userId)
    {
        if (_dict.TryGetValue(Parameters[3], out var newPsw) && newPsw.Length > 5 &&
            _dict.TryGetValue(Parameters[4], out var oldPsw))
            return _serviceContainer.UserService.ChangePassword(ConsoleUserInterface.AuthenticationData.Token, newPsw,
                oldPsw,
                userId).Result;
        return false;
    }

    private bool ArgumentsAreValid(string[] args)
    {
        return (TrySetDictionary(args) && ((_dict.ContainsKey(Parameters[0])
                                            && (_dict.ContainsKey(Parameters[1]) ||
                                                _dict.ContainsKey(Parameters[2]) ||
                                                _dict.ContainsKey(Parameters[3]))) ||
                                           _dict.ContainsKey(Parameters[1]) || _dict.ContainsKey(Parameters[2]))) ||
               (_dict.ContainsKey(Parameters[3]) && _dict.ContainsKey(Parameters[4]));
    }

    private bool TrySetDictionary(string[] args)
    {
        return TryParseArgs(args, out _dict);
    }
}