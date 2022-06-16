﻿using System.Text.Json;
using BLL.Logger;
using DAL.Repositories;
using Entities;

namespace BLL.Helpers;

public class CustomTokenHandler
{
    private readonly ILogger _logger;
    private readonly IRepository<User> _userRepository;

    public CustomTokenHandler(IRepository<User> userRepository, ILogger logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public string GenerateToken(User user)
    {
        var serialize = JsonSerializer.Serialize(user);
        return serialize;
    }

    public bool ValidateToken(string token)
    {
        var parseUser = GetUser(token);
        return parseUser is not null;
    }

    public User GetUser(string token)
    {
        try
        {
            var obj = JsonSerializer.Deserialize<User>(token);
            var parseUser = _userRepository.GetById(obj.Id);
            return parseUser;
        }
        catch (JsonException e)
        {
            _logger.Log("JsonException in GetUser, message: " + e.Message);
            return null;
        }
        catch (ArgumentNullException)
        {
            _logger.Log("Token is null");
            return null;
        }
    }
}