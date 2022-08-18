using System;
using System.Collections.Generic;
using System.IO;
using Json.Net;
using MarketUI.Models;
using MarketUI.Util.Interface;

namespace MarketUI.Util;

public class CommandsInfoHandlerJson : ICommandsInfoHandler
{
    private Dictionary<string, CommandInfoModel> _dictionary;

    public CommandInfoModel GetCommandInfo(string commandName)
    {
        if (_dictionary is null || !_dictionary.ContainsKey(commandName))
            _dictionary =
                JsonNet.Deserialize<Dictionary<string, CommandInfoModel>>(
                    File.ReadAllText(@"Command/CommandsInfo.json"));

        if (_dictionary.ContainsKey(commandName))
            return _dictionary[commandName];
        throw new ArgumentException($"Json file doesnt have keys name {commandName}", nameof(commandName));
    }
}