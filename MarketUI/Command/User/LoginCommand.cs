﻿using System.Collections.Generic;
using BLL;
using BLL.Objects;
using MarketUI.Command.Base;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Command.User
{
    public class LoginCommand : BaseParameterizedCommand
    {
        private readonly IServiceManager _serviceManager;
        private Dictionary<string, string> _dict;

        public LoginCommand(IServiceManager serviceManager, IUserInterfaceMapperHandler mapperHandler,
            ICommandsInfoHandler cih) :
            base(mapperHandler, cih)
        {
            _serviceManager = serviceManager;
        }

        public override string Execute(string[] args)
        {
            if (!(ConsoleUserInterface.AuthenticationData is null))
                return "You cannot log in when you are already logged in";
            var authenticateRequest = new AuthenticateRequestModel();
            if (!TryCreateDictionary(args) || !TryParseAndSaveName(authenticateRequest) ||
                !TryParseAndSavePsw(authenticateRequest))
                return GetHelp();

            var response = Mapper.Map<AuthenticateResponseModel>(
                _serviceManager.UserService.Authenticate(Mapper.Map<AuthenticateRequest>(authenticateRequest)));
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
}