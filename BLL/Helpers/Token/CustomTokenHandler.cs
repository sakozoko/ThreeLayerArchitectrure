using System.Text.Json;
using BLL.Extension;
using BLL.Util.Logger;
using DAL;
using BLL.Objects;
using Entities;
using User = BLL.Objects.User;

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

    public string GenerateToken(User userEntity)
    {
        var serialize = JsonSerializer.Serialize(userEntity);
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
            var parseUser = _unitOfWork.UserRepository.GetById(obj.Id).ToDomain();
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