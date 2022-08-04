using System.Text.Json;
using AutoMapper;
using BLL.Objects;
using BLL.Util.Interface;
using BLL.Util.Logger;
using DAL;

namespace BLL.Helpers.Token;

public class CustomTokenHandler : ITokenHandler
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CustomTokenHandler(IUnitOfWork unitOfWork, ILogger logger, IDomainMapperHandler domainMapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = domainMapper.GetMapper();
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
            var parseUser = _mapper.Map<User>(_unitOfWork.UserRepository.GetById(obj.Id));
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