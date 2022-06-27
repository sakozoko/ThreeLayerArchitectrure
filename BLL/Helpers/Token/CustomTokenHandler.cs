using System.Text.Json;
using BLL.Util.Helpers.Token;
using BLL.Util.Logger;
using DAL.Util;
using Entities;

namespace BLL.Helpers.Token;

public class CustomTokenHandler : ITokenHandler
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CustomTokenHandler(IUnitOfWork unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork;
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
            var parseUser = _unitOfWork.UserRepository.GetById(obj.Id);
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